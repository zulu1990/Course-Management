using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CourseManagementProject.Infrastructure.Implementation
{
    public class ExceptionMiddleware : IMiddleware
    {
        
        public ExceptionMiddleware()
        {

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                //_logger.LogError(e, next.Method.Name);

                await HandleExceptionAsync(context, e);
            }
        }


        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = 500;
            var response = new
            {
                status = statusCode,
                detail = exception.Message,
                customMessage = "here is our custom message we want to show to user"
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
