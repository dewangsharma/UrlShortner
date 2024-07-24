using DataTypes;
using DataTypes.Requests;
using DataTypes.Responses;

namespace BusinessLayer.Interfaces
{
    public interface IAuthService
    {
        (string Username,string Password) EncryptCredentials(string username, string password);
        Task<AuthenticationRes> LoginAsync(string username, string password, string ipAddress);
        Task<AuthenticationRes> GetRefreshToken(string token, string refreshToken, string ipAddress);

    }
}
