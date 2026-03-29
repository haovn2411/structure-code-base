# ??? Structure Code Base - Clean Architecture + DDD

## ?? **Architecture Overview**

```
Domain (Core Business Logic)
    ? depends on nothing
Application (Use Cases - CQRS)
    ? depends on Domain
Infrastructure (EF Core, Repositories)
    ? implements Application + Domain interfaces
Presentation (API)
    ? depends on Application
```

---

## ?? **Key Patterns**

### **1. DDD Aggregate**
- **Course** = Aggregate Root
- **Video** = Child Entity
- **Money, Rating** = Value Objects

### **2. CQRS**
- **Commands** ? Modify state
- **Queries** ? Read data

### **3. Result Pattern**
- Domain throws exceptions
- Application catches & converts to Result<T>

### **4. Domain Exceptions**
- Typed exceptions in Domain
- Convert to Error in Application

---

## ?? **Documentation**

**Single comprehensive guide:**

? **[docs/DDD_CLEAN_ARCHITECTURE.md](./docs/DDD_CLEAN_ARCHITECTURE.md)**

**Covers:**
- DDD patterns (Aggregate, Value Objects, Domain Events)
- Clean Architecture layers
- CQRS + Result Pattern
- EF Core integration (Shadow Properties, Explicit Loading)
- Best practices
- Code templates
- Common issues & fixes

---

## ?? **Quick Start**

1. Read **[docs/DDD_CLEAN_ARCHITECTURE.md](./docs/DDD_CLEAN_ARCHITECTURE.md)**
2. Review Course Aggregate implementation
3. Follow established patterns

**Philosophy:** Architecture patterns over implementation details.
