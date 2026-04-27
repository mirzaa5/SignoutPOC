
using StackExchange.Redis;
namespace ApiGateway.Infrastructure;
public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    public RedisService(IConnectionMultiplexer db)
    {
        _db = db.GetDatabase();
    }

    public async Task<bool> IsTokenBlackListedASync(string token)
    {
        return await _db.KeyExistsAsync(token);          
    }
}