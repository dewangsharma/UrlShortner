using System.Security.Claims;

namespace RESTWebApi.Extensions
{
    public static class HttpRequestExtension
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            int userId = 0;
            if (httpContext.User.Identity is ClaimsIdentity identity)
            {
                var sidClaim = identity.FindFirst(ClaimTypes.Sid);
                if (sidClaim != null && int.TryParse(sidClaim.Value, out int parsedUserId))
                {
                    userId = parsedUserId;
                }
            }
            return userId;
        }

        public static string GetClientIP(this HttpContext httpContext)
        {
            var xForwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                return xForwardedFor.Split(',').First().Trim();
            }

            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
}
