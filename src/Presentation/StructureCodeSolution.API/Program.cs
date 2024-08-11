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


// #Application layer
builder.Services.AddConfigureMediatR();
builder.Services.AddConfigureAutoMapper();
builder.Services.AddDomainEventNotificationHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
