using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OneUron.BLL.ExceptionHandle;

namespace OneUron.BLL.ExceptionHandle
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // chạy request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ApiResponse<object> response;
            int statusCode = (int)HttpStatusCode.InternalServerError;

            switch (exception)
            {
                case ApiException.ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.UnprocessableEntity;
                    response = ApiResponse<object>.FailValidation(
                        validationEx.Message,
                        "VALIDATION_ERROR",
                        validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    );
                    break;

                case ApiException.NotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = ApiResponse<object>.FailResponse("NOT_FOUND", notFoundEx.Message);
                    break;

                case ApiException.BadRequestException badRequestEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.FailResponse("BAD_REQUEST", badRequestEx.Message);
                    break;

                case ApiException.UnauthorizedException unauthorizedEx:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    response = ApiResponse<object>.FailResponse("UNAUTHORIZED", unauthorizedEx.Message);
                    break;

                case ApiException.ForbiddenException forbiddenEx:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    response = ApiResponse<object>.FailResponse("FORBIDDEN", forbiddenEx.Message);
                    break;

                case ApiException.BussinessException businessEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.FailResponse("BUSINESS_ERROR", businessEx.Message);
                    break;

                default:
                    response = ApiResponse<object>.FailResponse("INTERNAL_SERVER_ERROR", exception.Message);
                    break;
            }

            context.Response.StatusCode = statusCode;
            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }
    }
}
