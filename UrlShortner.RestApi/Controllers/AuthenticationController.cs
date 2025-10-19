using Microsoft.AspNetCore.Mvc;
using System.Net;
using UrlShortner.Application.Interfaces;
using UrlShortner.RestApi.Mappers;
using UrlShortner.RestApi.Models.Authentications;

namespace RESTWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IConfiguration config, IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var ipAddress = "";
            var loginReqDto = request.ToDto(ipAddress);
            var result = await _authService.LoginAsync(loginReqDto);
            if (result == null)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }
            return Ok(result);
        }

        [HttpPost("token")]
        public async Task<ActionResult<LoginResponse>> Refresh([FromBody] RefreshTokenRequest request)
        {
            // var token  = await HttpContext.GetTokenAsync("access_token");
            if (request == null)
            {
                throw new UnauthorizedAccessException();
            }

            var ipAddress = "";
            var refreshTokenDto = request.ToDto(ipAddress);
            var result = await _authService.GetRefreshToken(refreshTokenDto);
            if (result == null)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }
            return Ok(result);
        }
    }
}
