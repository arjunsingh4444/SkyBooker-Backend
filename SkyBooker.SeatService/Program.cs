using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using SkyBooker.SeatService.Data;
using SkyBooker.SeatService.Interfaces;
using SkyBooker.SeatService.Repositories;
using SkyBooker.SeatService.Services;
using SkyBooker.SeatService.Middleware;
using SkyBooker.SeatService.Background;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Seat Service API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enter JWT token only (no Bearer needed)"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// DB
builder.Services.AddDbContext<SeatDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

//  Background service (seat hold TTL)
builder.Services.AddHostedService<SeatHoldCleanupService>();

// JWT AUTH (same as AuthService)
var jwt = builder.Configuration.GetSection("Jwt");
var secret = jwt["Secret"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret!)),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Pipeline
app.UseHttpsRedirection();

//  Logging (optional)
app.UseMiddleware<RequestLoggingMiddleware>();

//  Global exception handler
app.UseMiddleware<ExceptionMiddleware>();

//  Auto add Bearer
app.UseMiddleware<JwtMiddleware>();

//  Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();