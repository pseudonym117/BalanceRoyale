namespace BalanceRoyale.Middleware
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            this.LogRequest(context);
            await this.next(context);
        }

        private void LogRequest(HttpContext context)
        {
            this.logger.LogInformation("{0:MM/dd/yy H:mm:ss} ({}) [{}] {}", DateTime.Now, context.TraceIdentifier, context.Request.Method, context.Request.Path);
        }
    }
}
