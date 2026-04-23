using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public interface IRedisService
    {
        Task<bool> BlackListTokenAsync(string token);
        Task<bool> IsTokenBlackListedASync(string token);
    }
}