using BusinessLayer.Interfaces;
using DataTypes.Requests;
using DataTypes.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<ActionResult<AuthenticationRes>> Login([FromBody] AuthenticationReq request)
        {
            var ipAddress = "";
            var result = await _authService.LoginAsync(request.UserName, request.Password, ipAddress);
            if (result == null)
            {
                return StatusCode((int)System.Net.HttpStatusCode.Unauthorized);
            }
            return Ok(result);
        }

        [HttpPost("token")]
        public async Task<ActionResult<AuthenticationRes>> Refresh([FromBody] RefreshTokenReq request)
        {

            // var token  = await HttpContext.GetTokenAsync("access_token");
            if (request == null)
                throw new UnauthorizedAccessException();
            var ipAddress = "";
            var result = await _authService.GetRefreshToken(request.Token, request.RefreshToken, ipAddress);
            if (result == null)
            {
                return StatusCode((int)System.Net.HttpStatusCode.Unauthorized);
            }
            return Ok(result);
        }
    }
}
