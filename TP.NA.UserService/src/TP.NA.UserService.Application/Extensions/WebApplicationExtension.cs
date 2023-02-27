using Carter;
using Microsoft.AspNetCore.Builder;
using TP.NA.Common.Repository.Extensions;
using TP.NA.UserService.Application.Exceptions;
using TP.NA.UserService.Domain.Entities;

namespace TP.NA.UserService.Application.Extensions
{
    public static class WebApplicationExtension
    {
        /// <summary>
        /// Configure application properties at application layer level
        /// </summary>
        /// <param name="app">Web application</param>
        public static void ConfigureApp(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.MapCarter();
        }

        /// <summary>
        /// Web application builder extension
        /// </summary>
        /// <param name="builder">Web application builder</param>
        public static void WebAppBuilder(this WebApplicationBuilder builder)
        {
            builder.AddCosmosDb<UserEntity>("users");
        }
    }
}