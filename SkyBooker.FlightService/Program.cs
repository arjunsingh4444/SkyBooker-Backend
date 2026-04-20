using Microsoft.EntityFrameworkCore;
using SkyBooker.FlightService.Data;
using SkyBooker.FlightService.Interfaces;
using SkyBooker.FlightService.Repositories;
using SkyBooker.FlightService.Services;
using SkyBooker.FlightService.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB Context
builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IFlightService, FlightService>();

var app = builder.Build();

// Custom Middlewares (Optional but recommended)
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS
app.UseHttpsRedirection();

// Authorization (future use)
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();