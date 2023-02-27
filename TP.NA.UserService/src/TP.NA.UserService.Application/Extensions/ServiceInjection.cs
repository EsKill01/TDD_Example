using Carter;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using TP.NA.UserService.Application.Abstractions.Loggers;
using TP.NA.UserService.Application.Commands.User;
using TP.NA.UserService.Application.Commands.User.Request;
using TP.NA.UserService.Application.Configs;

namespace TP.NA.UserService.Application.Extensions
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Use this if you want to use validation exception behavior
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddLogging();

            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>));
            services.AddTransient<IValidator<UpdateUserCommand.Command>, UpdateUserCommand.Validation>();
            services.AddTransient<IValidator<CreateUserRequest>, CreateUser.ValidationCommandCreateUser>();
            services.AddCarter();
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        /// <summary>
        /// Method to configure application
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigApp(this IServiceCollection builder)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            builder.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}