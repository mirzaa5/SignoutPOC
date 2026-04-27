using ApiGateway.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis:6379"));
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


var app = builder.Build();
app.UseHttpsRedirection();

//Middleware
app.UseMiddleware<JwtAuthencticationMiddleware>();

app.MapReverseProxy();
app.Run();
