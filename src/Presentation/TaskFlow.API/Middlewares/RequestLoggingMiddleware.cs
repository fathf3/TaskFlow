namespace TaskFlow.API.Middlewares
{
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
            var request = context.Request;
            var user = context.User.Identity?.Name ?? "Anonymous";

            _logger.LogInformation("HTTP {Method} {Path} by {User} from {IP}",
                request.Method,
                request.Path,
                user,
                context.Connection.RemoteIpAddress);

            await _next(context);
        }
    }
}
