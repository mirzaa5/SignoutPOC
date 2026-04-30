using ApiGateway.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisUrl = Environment.GetEnvironmentVariable("REDIS_URL");

ConfigurationOptions options;

if (string.IsNullOrEmpty(redisUrl))
{
    options = ConfigurationOptions.Parse("localhost:6379");
}
else if (redisUrl.StartsWith("redis://") || redisUrl.StartsWith("rediss://"))
{
    // ConfigurationOptions.Parse does not handle redis:// URI scheme correctly
    // and will duplicate the port number, so we parse the URI manually.
    var uri = new Uri(redisUrl);
    options = new ConfigurationOptions
    {
        EndPoints = { { uri.Host, uri.Port } },
        AbortOnConnectFail = false,
    };

    if (!string.IsNullOrEmpty(uri.UserInfo))
    {
        var parts = uri.UserInfo.Split(':', 2);
        if (parts.Length == 2)
        {
            // Railway sets the username to "default"; StackExchange.Redis uses Password only
            options.Password = Uri.UnescapeDataString(parts[1]);
        }
    }

    if (redisUrl.StartsWith("rediss://"))
    {
        options.Ssl = true;
    }
}
else
{
    // Plain StackExchange.Redis connection string (e.g. local dev override)
    options = ConfigurationOptions.Parse(redisUrl);
    options.AbortOnConnectFail = false;
}

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