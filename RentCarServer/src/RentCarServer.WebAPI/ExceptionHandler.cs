using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using RentCarServer.Application.Behaviors;
using RentCarServer.WebAPI.Middlewares;
using TS.Result;

namespace RentCarServer.WebAPI;

public sealed class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Result<string> errorResult;

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 500;

        var actualException = exception is AggregateException agg && agg.InnerException != null
        ? agg.InnerException
        : exception;

        var exceptionType = actualException.GetType();
        var validationExceptionType = typeof(ValidationException);
        var authorizationExceptionType = typeof(AuthorizationException);
        var tokenException = typeof(TokenException);

        if (exceptionType == validationExceptionType)
        {
            httpContext.Response.StatusCode = 422;

            errorResult = Result<string>.Failure(422, ((ValidationException)exception).Errors.Select(s => s.PropertyName).ToList());

            await httpContext.Response.WriteAsJsonAsync(errorResult);

            return true;
        }

        if (exceptionType == authorizationExceptionType)
        {
            httpContext.Response.StatusCode = 403;
            errorResult = Result<string>.Failure(403, "Bu işlem için yetkiniz yok");
            await httpContext.Response.WriteAsJsonAsync(errorResult);
            return true;
        }

        if (exceptionType == tokenException)
        {
            httpContext.Response.StatusCode = 401;
            errorResult = Result<string>.Failure(401, "Token geçeriz");
            await httpContext.Response.WriteAsJsonAsync(errorResult);
            return true;
        }

        errorResult = Result<string>.Failure(exception.Message);

        await httpContext.Response.WriteAsJsonAsync(errorResult);

        return true;
    }
}
