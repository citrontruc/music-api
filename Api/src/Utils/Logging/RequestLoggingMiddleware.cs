/*
Middleware to log all incoming calls as information.
*/

internal class RequestLoggingMiddleware : IloggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;

        _logger.LogInformation(
            "Incoming request: {Method} {Path} at {Time}",
            requestMethod,
            requestPath,
            startTime
        );

        await _next(context);

        var duration = DateTime.UtcNow - startTime;
        _logger.LogInformation(
            "Completed request: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms",
            requestMethod,
            requestPath,
            context.Response.StatusCode,
            duration.TotalMilliseconds
        );
    }
}
