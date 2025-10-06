using System.Net;

namespace RESTWebApi.Exceptions
{
    public class GlobalCancellationException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Exception> _logger;
        // private readonly ILoggerManager _logger;
        public GlobalCancellationException(RequestDelegate next, ILogger<Exception> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            //catch (TaskCanceledException ex)
            //{
            //    Console.WriteLine("");
            //}
            //catch (AuthenticationException ex)
            //{
            //    Console.WriteLine(ex);
            //}
            catch (Exception ex)
            {
                // _logger.LogError($"Something went wrong: {ex}");
                _logger.LogError(ex, "An unhandled exception");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = $"Internal Server Error. {exception.Message}"
            }.ToString());
        }
    }
}
