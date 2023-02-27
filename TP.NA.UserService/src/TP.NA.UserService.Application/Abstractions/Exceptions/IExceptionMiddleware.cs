using Microsoft.AspNetCore.Http;

namespace TP.NA.UserService.Application.Abstractions.Exceptions
{
    internal interface IExceptionMiddleware
    {
        Task InvokeAsync(HttpContext httpContext);

        Task HandleExceptionAsync(HttpContext context, Exception exception);
    }
}