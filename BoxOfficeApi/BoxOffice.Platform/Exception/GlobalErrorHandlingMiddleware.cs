using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BoxOffice.Platform.Exception
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context,
            System.Exception exception)
        {
            var exceptionResult = JsonSerializer.Serialize(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            switch (exception)
            {
                case ResourceNotFoundException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    }
                default:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    }
            }

            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
