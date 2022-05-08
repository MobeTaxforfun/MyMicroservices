using IdentityModel.Client;

namespace WebMVC.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope);
    }
}
