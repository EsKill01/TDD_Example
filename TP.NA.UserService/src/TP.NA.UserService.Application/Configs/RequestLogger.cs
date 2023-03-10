using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using TP.NA.UserService.Application.Abstractions.Loggers;

namespace TP.NA.UserService.Application.Configs
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> logger;
        private readonly ILoggerManager _loggerManager;

        public RequestLogger(ILogger<TRequest> logger, ILoggerManager loggerManager)
        {
            this._loggerManager = loggerManager;
            this.logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            logger.LogInformation("User service request: {Name} {@Request}",
                name, request);

            _loggerManager.LogInfo($"Service request: {request}");

            return Task.CompletedTask;
        }
    }
}