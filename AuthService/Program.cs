using AuthService.Services;
using AuthService.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddOpenApi();

// Get Redis URL from environment
var redisUrl = Environment.GetEnvironmentVariable("REDIS_URL");

if (string.IsNullOrEmpty(redisUrl))
{
    redisUrl = "localhost:6379"; // local fallback
}

// Parse properly
var options = ConfigurationOptions.Parse(redisUrl, true);
options.AbortOnConnectFail = false;

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(options));

builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IAuthService, AuthSignoutService>();
builder.Services.AddControllers();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();