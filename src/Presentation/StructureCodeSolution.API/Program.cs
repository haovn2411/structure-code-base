using StructureCodeSolution.API.Middlewares;
using StructureCodeSolution.Application.DependencyInjection.Extentions;
using StructureCodeSolution.Persistence.DependencyInjection.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////// ========================================
// #Persistence layer
builder.Services.AddSQLServerPersistence();
builder.Services.AddRepositoryPersistence();
builder.Services.AddDomainEventCollector();
builder.Services.AddInterceptorPersistence();
builder.Services.AddUserService();

////// ========================================
// #Application layer
builder.Services.AddConfigureMediatR();
builder.Services.AddConfigureAutoMapper();
builder.Services.AddDomainEventNotificationHandlers();

////// ========================================
/// #API layer
builder.Services.AddTransient<ExceptionHandlingMiddleware>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
