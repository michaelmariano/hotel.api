using Domain.Exceptions;
using System.Text.Json;

namespace Hotel.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException cx)
            {
                var response = context.Response;

                response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { message = cx.Message });

                response.StatusCode = (int)cx.StatusCode;

                await response.WriteAsync(result);

                //TODO LOG
            }
            catch (Exception ex)
            {
                var response = context.Response;

                response.StatusCode = 500;

                response.ContentType = "application/json";

                await response.WriteAsync(JsonSerializer.Serialize(new { message = ex.Message }));

                //TODO LOG
            }
        }
    }
}
