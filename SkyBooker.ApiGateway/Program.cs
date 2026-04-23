using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load Ocelot config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// CORS (for React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add Ocelot
builder.Services.AddOcelot();

var app = builder.Build();

// Enable CORS BEFORE Ocelot
app.UseCors("AllowFrontend");

// Start gateway
await app.UseOcelot();

app.Run();