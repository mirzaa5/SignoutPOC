using AuthService.Services;
using StackExchange.Redis;
namespace AuthService.Infrastructure;
public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    public RedisService(IConnectionMultiplexer db)
    {
        _db = db.GetDatabase();
    }

    public async Task<bool> BlackListTokenAsync(string token)
    {
        try
        {
            await _db.StringSetAsync(key:token, value: "revoked");   
        }
        catch(Exception)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> IsTokenBlackListedASync(string token)
    {
        return await _db.KeyExistsAsync(token);          
    }
}
