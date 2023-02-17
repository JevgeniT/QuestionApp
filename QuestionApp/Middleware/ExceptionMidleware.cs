using System.Net;
using Newtonsoft.Json;

namespace QuestionApp.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IWebHostEnvironment webHostEnvironment)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = ex switch
        {
            ArgumentException _ => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        var result = JsonConvert.SerializeObject(new { error = ex.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
