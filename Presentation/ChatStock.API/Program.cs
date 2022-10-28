using ChatStock.Application.IoC.HttpClients;
using ChatStock.Application.IoC.Services;
using ChatStock.Application.Services;
using ChatStock.Common.Models;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using ChatStock.API.RabbitMQ;
using ChatStock.Application.IoC.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog((ctx, lc) => lc
//    .WriteTo.File("logs-.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IStockService, StockService>();

builder.Services.ApplicationHttpClientsIoC();
builder.Services.ApplicationServicesIoC();
builder.Services.ApplicationRabbitMQIoC();
//builder.Services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


