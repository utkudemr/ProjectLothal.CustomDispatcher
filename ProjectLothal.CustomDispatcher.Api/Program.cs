using System.Reflection;
using Lothal.Mediator.Core;
using Lothal.Mediator.Core.Dispatchers;
using Lothal.Mediator.Core.Pipelines;
using ProjectLothal.CustomDispatcher.Api.Decorators;
using ProjectLothal.CustomDispatcher.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITestBusinessService, TestBusinessService>();
builder.Services.AddSingleton<Mediator>();
builder.Services.AddHandlers(Assembly.GetExecutingAssembly());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditLogBehavior<,>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
