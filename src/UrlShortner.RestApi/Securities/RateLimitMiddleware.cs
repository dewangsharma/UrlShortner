using Microsoft.Extensions.Caching.Distributed;
using RESTWebApi.Extensions;
using System.Text.Json;
using UrlShortner.RestApi.Models.Authentications;

namespace UrlShortner.RestApi.Securities
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public RateLimitMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Apply rate limit only on /authentication
            if (!path.Contains("authentication"))
            {
                await _next(context);
                return;
            }

            string ip = context.GetClientIP();

            string username = "";
            if (context.Request.Method == "POST" &&
                context.Request.ContentType?.Contains("application/json") == true)
            {
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();

                context.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(body));

                // dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(body);
                // email = json?.email ?? "";

                var dto = JsonSerializer.Deserialize<LoginRequest>(body);
                username = dto?.UserName ?? "";
            }

            var key = $"authentication:{username}:{ip}";

            var attempts = await _cache.GetStringAsync(key);

            int count = string.IsNullOrEmpty(attempts) ? 0 : int.Parse(attempts);

            if (count >= 5)
            {
                context.Response.StatusCode = 429; // Too Many Requests
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Too many login attempts. Try again after sometime"
                });
                return;
            }

            count++;

            await _cache.SetStringAsync(
                key,
                count.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });

            await _next(context);
        }
    }
}
