# ??? DDD + Clean Architecture Guide

> **Focus:** Architecture patterns, not implementation details.

---

## ?? **PROJECT STRUCTURE**

```
src/
??? Core/
?   ??? Domain/              # Pure business logic, no dependencies
?   ??? Application/         # Use cases, CQRS, Result pattern
??? Infrastructure/
?   ??? Persistence/         # EF Core, repositories, database
??? Presentation/
    ??? API/                 # Controllers, HTTP, Swagger
```

**Dependency Flow:** Presentation ? Application ? Domain ? Infrastructure

---

## ?? **CORE PATTERNS**

### **1. DDD Aggregate**

```csharp
// Aggregate Root
public class Course : AggregateAuditedRoot<Guid>
{
    private readonly List<Video> _videos = new();  // Child entities (private)
    public IReadOnlyCollection<Video> Videos => _videos.AsReadOnly();  // Read-only
    
    public Money Price { get; private set; }  // Value object
    public Rating Rating { get; private set; }
    
    // ? All modifications via methods
    public void AddVideo(string title, string? description, TimeSpan duration, int? order = null)
    {
        if (_videos.Any(v => v.Order == order))
            throw new VideoException.DuplicateVideoOrderException(order.Value);
        
        var video = Video.Create(title, description, duration, order ?? _videos.Count);
        _videos.Add(video);
        Raise(new VideoAddedDomainEvent(Id, video.Id, title, ...));
    }
}

// Value Object
public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    
    public static Money Create(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new CourseException.InvalidPriceException();
        return new Money(amount, currency);
    }
}
```

**Rules:**
- Course = Root (entry point)
- Video = Child (accessed via Course only)
- Money/Rating = Value Objects (immutable)
- Private collections, read-only exposure
- Modifications via methods only

---

### **2. Domain Exceptions**

```csharp
// Domain/Abstractions/Exceptions/
public abstract class DomainException : Exception { }
public abstract class NotFoundException : DomainException { }
public abstract class BadRequestException : DomainException { }
public abstract class ConflictException : DomainException { }

// Domain/Aggregates/Courses/Exceptions/
public static class CourseException
{
    public class CourseNotFoundException : NotFoundException
    {
        public CourseNotFoundException(Guid id)
            : base($"Course with id '{id}' was not found") { }
    }
    
    public class InvalidCourseNameException : BadRequestException
    {
        public InvalidCourseNameException()
            : base("Course name cannot be empty") { }
    }
}

public static class VideoException
{
    public class DuplicateVideoOrderException : ConflictException
    {
        public DuplicateVideoOrderException(int order)
            : base($"A video with order {order} already exists") { }
    }
}
```

**Pattern:** Domain throws typed exceptions ? Application catches & converts to Result.

---

### **3. CQRS + Result Pattern**

```csharp
// Command
public record CreateCourseCommand(string Name, decimal Price, string Currency, ...) : ICommand<Guid>;

// Handler
public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCourseCommand request, ...)
    {
        try
        {
            var price = Money.Create(request.Price, request.Currency);
            var course = Course.Create(request.Name, ..., price, ...);
            
            _repository.Add(course);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success(course.Id);
        }
        catch (DomainException ex)
        {
            return Result.Failure<Guid>(new Error(ex.GetType().Name, ex.Message));
        }
    }
}

// Query
public record GetCourseByIdQuery(Guid Id) : IQuery<CourseResponse>;

public class GetCourseByIdQueryHandler : IQueryHandler<GetCourseByIdQuery, CourseResponse>
{
    public async Task<Result<CourseResponse>> Handle(...)
    {
        var course = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (course is null)
            return Result.Failure<CourseResponse>(new Error("Course.NotFound", "..."));
        
        var response = _mapper.Map<CourseResponse>(course);
        return Result.Success(response);
    }
}
```

**Flow:**
```
API ? MediatR ? Validation ? Handler
    ? Try { Domain Logic } 
    ? Catch { DomainException ? Result.Failure }
    ? Return Result<T>
```

---

### **4. EF Core Integration**

#### **Shadow Properties (Foreign Keys)**
```csharp
// Domain - NO CourseId in Video
public class Video : EntityAuditBase<Guid>
{
    public string Title { get; private set; }  // ? No CourseId!
}

// EF Configuration
public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.Property<Guid>("CourseId").IsRequired();  // ? Shadow property
        builder.HasIndex("CourseId", nameof(Video.Order)).IsUnique();
    }
}
```

#### **Private Collection Mapping**
```csharp
// EF Configuration
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Ignore(x => x.Videos);  // ? Ignore public property
        
        builder.HasMany<Video>("_videos")  // ? Map private field
            .WithOne()
            .HasForeignKey("CourseId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

#### **Explicit Loading (No Lazy Loading)**
```csharp
// Remove
// builder.UseLazyLoadingProxies(true);  ?

// Repository - Explicit includes
public async Task<Course?> GetByIdAsync(Guid id, ...)
{
    return await _dbContext.Courses
        .Include("_videos")  // ? Explicit load
        .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
}

// Query Handler
var courses = await query
    .Include("_videos")  // ? When needed
    .ToListAsync(cancellationToken);
```

---

## ?? **KEY PATTERNS SUMMARY**

### **Encapsulation**
```csharp
private readonly List<T> _field = new();
public IReadOnlyCollection<T> Property => _field.AsReadOnly();
public void Add(...) { _field.Add(...); }  // Controlled access
```

### **Domain Events**
```csharp
public void DoSomething()
{
    // Business logic
    Raise(new SomethingHappenedEvent(...));
}
```

### **Value Objects**
```csharp
public sealed class ValueObject : ValueObject
{
    public Type Property { get; private set; }  // Immutable
    public static ValueObject Create(...) { }   // Factory
    protected override IEnumerable<object> GetEqualityComponents() { }
}
```

### **Validation**
```csharp
// Layer 1: FluentValidation (Application)
public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

// Layer 2: Domain Validation (Domain)
public void UpdateCourse(string name, ...)
{
    if (string.IsNullOrWhiteSpace(name))
        throw new InvalidCourseNameException();
}
```

---

## ?? **ARCHITECTURE FLOW**

```
???????????????????????????????????????
?         API Controller              ?  HTTP requests
???????????????????????????????????????
               ? Commands/Queries
???????????????????????????????????????
?         MediatR Pipeline            ?  Validation
???????????????????????????????????????
               ?
???????????????????????????????????????
?     Command/Query Handlers          ?  Try-catch
?  (Application Layer)                ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?       Domain Models                 ?  Throw exceptions
?  (Domain Layer - Pure Logic)        ?
???????????????????????????????????????
               ?
???????????????????????????????????????
?     Result<T> Conversion            ?  Success/Failure
???????????????????????????????????????
               ?
???????????????????????????????????????
?       HTTP Response                 ?  200/400/404/500
???????????????????????????????????????
```

---

## ?? **BEST PRACTICES**

### **Domain Layer**
- ? Pure business logic, zero dependencies
- ? Throw typed domain exceptions
- ? Private setters, encapsulation
- ? Value objects for concepts (Money, Rating)
- ? Domain events for side effects

### **Application Layer**
- ? CQRS pattern (Commands/Queries separation)
- ? Result pattern (no exceptions leak to API)
- ? Catch domain exceptions, convert to Result
- ? FluentValidation for input
- ? AutoMapper for DTOs

### **Infrastructure Layer**
- ? Shadow properties for foreign keys
- ? Explicit includes (no lazy loading)
- ? Private collection mapping with `Ignore()`
- ? Repository pattern

### **Presentation Layer**
- ? Thin controllers (delegate to MediatR)
- ? Handle Result pattern properly
- ? Return proper HTTP status codes

---

## ?? **COMMON ISSUES & FIXES**

| Issue | Fix |
|-------|-----|
| "Property not virtual" error | Remove `UseLazyLoadingProxies()`, use explicit `Include()` |
| "Cannot use field '_videos'" error | Add `builder.Ignore(x => x.Videos)` in EF config |
| Domain exceptions not caught | Wrap handler in `try-catch (DomainException ex)` |
| Foreign key in domain | Use shadow properties in EF config |
| N+1 queries | Explicit `Include()` in repository/queries |

---

## ?? **CODE TEMPLATES**

### **Create Command**
```csharp
// 1. Command
public record CreateXCommand(...) : ICommand<Guid>;

// 2. Validator
public class CreateXCommandValidator : AbstractValidator<CreateXCommand> { }

// 3. Handler
public class CreateXCommandHandler : ICommandHandler<CreateXCommand, Guid>
{
    public async Task<Result<Guid>> Handle(...)
    {
        try
        {
            var entity = X.Create(...);
            _repository.Add(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(entity.Id);
        }
        catch (DomainException ex)
        {
            return Result.Failure<Guid>(new Error(ex.GetType().Name, ex.Message));
        }
    }
}
```

### **Query**
```csharp
// 1. Query
public record GetXByIdQuery(Guid Id) : IQuery<XResponse>;

// 2. Handler
public class GetXByIdQueryHandler : IQueryHandler<GetXByIdQuery, XResponse>
{
    public async Task<Result<XResponse>> Handle(...)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity is null)
            return Result.Failure<XResponse>(XErrors.NotFound(request.Id));
        
        return Result.Success(_mapper.Map<XResponse>(entity));
    }
}
```

### **Domain Entity**
```csharp
public class X : AggregateAuditedRoot<Guid>
{
    private readonly List<Y> _items = new();
    public IReadOnlyCollection<Y> Items => _items.AsReadOnly();
    
    public ValueObject Value { get; private set; }
    
    private X() { }
    
    public static X Create(...)
    {
        var x = new X(...);
        x.Raise(new XCreatedEvent(...));
        return x;
    }
    
    public void DoSomething(...)
    {
        if (businessRule)
            throw new XException.SomeException();
        
        // Logic
        Raise(new SomethingHappenedEvent(...));
    }
}
```

---

## ?? **PRINCIPLES**

1. **Aggregate Pattern** - One root, controlled access
2. **Encapsulation** - Private fields, read-only properties
3. **Domain Exceptions** - Typed exceptions in domain
4. **Result Pattern** - Application converts exceptions to Results
5. **Shadow Properties** - Foreign keys outside domain
6. **Explicit Loading** - No lazy loading, explicit includes
7. **CQRS** - Separate commands and queries
8. **Clean Architecture** - Dependencies flow inward

---

## ?? **REFERENCES**

- Eric Evans - Domain-Driven Design (Blue Book)
- Vaughn Vernon - Implementing Domain-Driven Design (Red Book)
- Robert C. Martin - Clean Architecture
- Microsoft Docs - EF Core Shadow Properties
- Martin Fowler - CQRS Pattern

---

**This architecture ensures:**
- ? Clean separation of concerns
- ? Testable business logic
- ? Maintainable codebase
- ? DDD compliance
- ? Performance with explicit queries
