using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
        private readonly IAuthService _authService;

        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        // [EnableRateLimiting("LoginPolicy")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {

                var ipAddress = HttpContext.GetClientIP();
                var loginReqDto = request.ToDto(ipAddress);
                var result = await _authService.LoginAsync(loginReqDto, cancellationToken);
                if (result == null)
                {
                    return Unauthorized();
                }
                return Ok(result.ToResponse());
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Login cancelled by client.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Login");
                var pd = new ProblemDetails
                {
                    Title = "An unexpected error occurred while processing the request.",
                    Status = (int)HttpStatusCode.InternalServerError,
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, pd);
            }
        }

        [HttpPost("token")]
        [AllowAnonymous]
        [EnableRateLimiting("LoginPolicy")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<LoginResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {

                var ipAddress = HttpContext.GetClientIP();
                var refreshTokenDto = request.ToDto(ipAddress);
                var result = await _authService.GetRefreshToken(refreshTokenDto, cancellationToken);
                if (result == null)
                {
                    return Unauthorized();
                }
                return Ok(result.ToResponse());
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Refresh token request cancelled by client.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during RefreshToken");
                var pd = new ProblemDetails
                {
                    Title = "An unexpected error occurred while processing the request.",
                    Status = (int)HttpStatusCode.InternalServerError,
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, pd);
            }
        }
    }
}
