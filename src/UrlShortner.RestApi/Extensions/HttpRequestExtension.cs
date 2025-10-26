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
                userId = int.Parse(identity.FindFirst(ClaimTypes.Sid).Value);
            }
            return userId;
        }
    }
}
