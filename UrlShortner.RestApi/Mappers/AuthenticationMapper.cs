using UrlShortner.Application.Models.Authentications;
using UrlShortner.RestApi.Models.Authentications;

namespace UrlShortner.RestApi.Mappers
{
    public static class AuthenticationMapper
    {
        public static LoginDto ToDto(this LoginRequest loginRequest, string ipAddress)
        {
            return new LoginDto
            {
                UserName = loginRequest.UserName,
                Password = loginRequest.Password,
                IPAddress = ipAddress
            };
        }


        public static RefreshTokenDto ToDto(this RefreshTokenRequest loginRequest, string ipAddress)
        {
            return new RefreshTokenDto
            {
                RefreshToken = loginRequest.RefreshToken,
                Token = loginRequest.Token,
                IPAddress = ipAddress
            };
        }
    }

}
