using Serilog;
using Serilog.Events;
using System.Text;

namespace POC.API.Middleware
{
    public class RequestLogMiddleware
    {
        
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// Logs the incoming HTTP request and invokes the next middleware in the pipeline.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestLog = await BuildRequestLogAsync(context);
            Log.ForContext("IsRequestLog", true)
            .Information("Incoming HTTP Request {@RequestDetails}", requestLog);
            context.Request.Body.Position = 0;
            await _next(context);

        }


        private async Task<object> BuildRequestLogAsync(HttpContext context)
        {
           // throw new Exception("Geeting error onnmiddleware");
            var request = context.Request;

            // Read Body safely
            request.Body.Position = 0;
            string body = string.Empty;
            using (var reader = new StreamReader( request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
            }
            request.Body.Position = 0;
            return new
            {
                Method = request.Method,
                Scheme = request.Scheme,
                Host = request.Host.Value,
                Path = request.Path.Value,
                QueryString = request.QueryString.Value,
                ContentType = request.ContentType,
                ContentLength = request.ContentLength,
                Body = body,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
