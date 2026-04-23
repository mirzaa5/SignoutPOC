namespace AuthService.Services;
public interface IAuthService
{
    Task<bool> SignOutAsync(string token);
}