using CaseAPI.Exceptions.Common;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace CaseAPI.Middlewares;

public sealed class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        context.Response.StatusCode = ex switch
        {
            BadRequestException => context.Response.StatusCode = (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => context.Response.StatusCode = (int)HttpStatusCode.Unauthorized,
            NotFoundException => context.Response.StatusCode = (int)HttpStatusCode.NotFound,
            _ => context.Response.StatusCode = (int)HttpStatusCode.InternalServerError
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            ex.Message,
        }, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        }));
    }
}
