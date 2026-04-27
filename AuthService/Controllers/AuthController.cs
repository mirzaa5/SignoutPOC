
using AuthService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signout")]
        public async Task<IActionResult> Signout()
        {
             var authHeader = Request.Headers[HeaderNames.Authorization].ToString();
             

            if( string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                return  Unauthorized(new { message = "Missing or invalid token"});
            }
            var accessToken = authHeader["Bearer ".Length..].ToString();
            var response  =await _authService.SignOutAsync(accessToken);
                return response ?  Ok(new {message = "Successfully Signed out"}) 
                                   : StatusCode(500, new { message = "Sign out failed"});
                
            
        }
    }
}