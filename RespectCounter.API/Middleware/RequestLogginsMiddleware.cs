using System.Text;

namespace RespectCounter.API.Middleware;
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            context.Request.EnableBuffering();

            var bodyAsText = "";
            if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
            {
                using (var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true))
                {
                    bodyAsText = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            _logger.LogInformation(
                "Request {method} {path} | Query: {query} | Body: {body}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                bodyAsText);
        }

        await _next(context);
    }
}