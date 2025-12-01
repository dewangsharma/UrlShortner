using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTWebApi.Extensions;
using System.Net;
using UrlShortner.Application.Interfaces;
using UrlShortner.RestApi.Mappers;
using UrlShortner.RestApi.Models.Authentications;

namespace UrlShortner.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthService _authService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ipAddress = HttpContext.GetClientIP();
                var loginReqDto = request.ToDto(ipAddress);
                var result = await _authService.LoginAsync(loginReqDto, cancellationToken);

                if (result == null)
                {
                    _logger.LogWarning("Invalid login attempt for user {UserName} from IP {IPAddress}", request.UserName, ipAddress);
                    return Unauthorized("Invalid login attempt");
                }

                return Ok(result.ToResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the login.");
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var ipAddress = HttpContext.GetClientIP();
                var refreshTokenDto = request.ToDto(ipAddress);
                var result = await _authService.GetRefreshToken(refreshTokenDto, cancellationToken);

                if (result == null)
                {
                    _logger.LogWarning("Invalid refresh token attempt for token: {Token}", request.Token);
                    return Unauthorized("Invalid refresh token.");
                }

                return Ok(result.ToResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the login.");
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}