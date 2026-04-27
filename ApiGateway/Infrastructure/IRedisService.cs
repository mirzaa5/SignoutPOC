
namespace ApiGateway.Infrastructure
{
    public interface IRedisService
    {
        Task<bool> IsTokenBlackListedASync(string token); 
    }
}