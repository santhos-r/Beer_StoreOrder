using Beer_StoreOrder.Api.Logger;
using Beer_StoreOrder.Service.Services;
using Beer_StoreOrder.Service.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Northwind.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
var environmentName = builder.Environment.EnvironmentName;

builder.Configuration
    .SetBasePath(currentDirectory)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{environmentName}.json", true, true)
    .AddEnvironmentVariables();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Added New
builder.Services.AddDbContext<NorthwindContext>
    (options => options.UseSqlite("Name=NorthwindDB"));

builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddScoped<IBeerService, BeerService>();
builder.Services.AddScoped<IBreweryService, BreweryService>();
builder.Services.AddScoped<IBarService, BarService>();
builder.Services.AddScoped<IBreweryBeerService, BreweryBeerService>();
builder.Services.AddScoped<IBarBeerStockService, BarBeerStockService>();
var app = builder.Build();

app.UseMiddleware<ExceptionHandling>();

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
