using System.Net;
using System.Text.Json;
namespace Beer_StoreOrder.Api.Middleware;
public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;
    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var errorResponse = new ErrorResponse();
        switch (exception)
        {
            case ApplicationException ex:
                if (ex.Message == "Bad Request")
                {
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                }
                else if (ex.Message == "Same Id already exists")
                {
                    errorResponse.StatusCode = (int)StatusCodes.Status422UnprocessableEntity;
                    errorResponse.Message = ex.Message;
                }
                else
                {
                    errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = ex.Message;
                }

                break;
            default:
                errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = exception.Message;
                break;
        }
        _logger.LogError($"{exception} Request failed with Status Code {context.Response.StatusCode}");
        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
    }
}

