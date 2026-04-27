using ApiGateway.Infrastructure;
using Microsoft.Net.Http.Headers;
using StackExchange.Redis;


public class JwtAuthencticationMiddleware
{
    private RequestDelegate _next;
    public JwtAuthencticationMiddleware(RequestDelegate requestDelegate)
    {
        _next = requestDelegate;
    }
    public async Task InvokeAsync(HttpContext context, IRedisService redisService ){
        
        var authHeader = context.Request.Headers[HeaderNames.Authorization].ToString();
        if(string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Missing or invalid token"});
            return;
        }
        var accessToken = authHeader["Bearer ".Length..].ToString();

        //Check redis if token is blacklisted
        var blacklisted = await redisService.IsTokenBlackListedASync(accessToken);
        if(blacklisted)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Token has been revoked"});
            return;
        }
        await _next(context);
    }
}