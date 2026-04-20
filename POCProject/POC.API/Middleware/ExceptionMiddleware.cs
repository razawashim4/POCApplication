using Serilog;
using Serilog.Events;
using System.Net;

namespace POC.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Exception log for the entire application.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                var errorDetails = new
                {
                    Message = ex.Message,
                    ExceptionType = ex.GetType().FullName,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.Message,
                    Source = ex.Source,
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    Timestamp = DateTime.UtcNow
                };

                Log.ForContext("IsExceptionLog", true)
                .ForContext("ErrorDetails", errorDetails, destructureObjects: true)
                .Error(ex.Message+"/n"+ex.StackTrace);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Internal Server Error");
            }

        }
    }
}
