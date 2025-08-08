using System;
using NLog;

namespace HikvisionTimeLapse.Core.Services;

public interface ILoggingService
{
    void Info(string message);
    void Warn(string message);
    void Error(string message);
}

public sealed class LoggingService : ILoggingService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public void Info(string message) => _logger.Info(message);
    public void Warn(string message) => _logger.Warn(message);
    public void Error(string message) => _logger.Error(message);
}



