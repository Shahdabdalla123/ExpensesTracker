using System.Diagnostics;

namespace BankProject.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);
            await _next(context);
            stopwatch.Stop();
            _logger.LogInformation("Request processed: {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms",
            context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }

    //// Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
