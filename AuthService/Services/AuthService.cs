using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        public AuthService()
        {
            
        }

        public Task<bool> SignOutAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}