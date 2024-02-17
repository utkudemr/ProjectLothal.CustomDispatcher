using ProjectLothal.CustomDispatcher.Api;
using ProjectLothal.CustomDispatcher.Api.Decorators;
using ProjectLothal.CustomDispatcher.Api.Dispatchers;
using ProjectLothal.CustomDispatcher.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<ITestBusinessService, TestBusinessService>();
builder.Services.AddTransient<ILoggerService, LogBusinessService>();
builder.Services.AddSingleton<Mediator>();
builder.Services.AddHandlers();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
