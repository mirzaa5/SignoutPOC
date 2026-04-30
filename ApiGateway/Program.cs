using ApiGateway.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisUrl = Environment.GetEnvironmentVariable("REDIS_URL");

if (string.IsNullOrEmpty(redisUrl))
{
    redisUrl = "localhost:6379"; // fallback for local dev
}

var options = ConfigurationOptions.Parse(redisUrl, true);
options.AbortOnConnectFail = false;

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(options));

builder.Services.AddScoped<IRedisService, RedisService>();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseHttpsRedirection();

// Middleware
app.UseMiddleware<JwtAuthencticationMiddleware>();

app.MapReverseProxy();

app.Run();