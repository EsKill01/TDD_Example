using NLog;
using TP.NA.UserService.Application.Abstractions.Loggers;

namespace TP.NA.UserService.Application.Configs
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message) => logger.Debug(message);

        public void LogError(string message) => logger.Error(message);

        public void LogInfo(string message) => logger.Info(message);

        public void LogWarn(string message) => logger.Warn(message);
    }
}