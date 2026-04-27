using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AuthService.Services;

public class AuthSignoutService : IAuthService
{
    private IRedisService _redis;
    public AuthSignoutService(IRedisService redis)
    {
        _redis = redis;
    }

    public async Task<bool> SignOutAsync(string token)
    {
        return await _redis.BlackListTokenAsync(token);
    }
}
