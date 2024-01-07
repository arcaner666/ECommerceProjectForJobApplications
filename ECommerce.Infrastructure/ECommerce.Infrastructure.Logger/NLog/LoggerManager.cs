using ECommerce.Domain.Contracts;
using NLog;

namespace ECommerce.Infrastructure.Logger.NLog;

public class LoggerManager : ILoggerManager
{
    private static ILogger _logger = LogManager.GetCurrentClassLogger();

    public LoggerManager()
    {
    }

    public bool IsDebugEnabled => _logger.IsDebugEnabled;
    public bool IsErrorEnabled => _logger.IsErrorEnabled;
    public bool IsInfoEnabled => _logger.IsInfoEnabled;
    public bool IsWarnEnabled => _logger.IsWarnEnabled;
    public bool IsFatalEnabled => _logger.IsFatalEnabled;

    public void LogDebug(object message)
    {
        if (IsDebugEnabled)
            _logger.Debug("{@message}", message);
    }

    public void LogDebug(object message, object value)
    {
        if (IsDebugEnabled)
            _logger.Info("{message}, {@value}", message, value);
    }

    public void LogError(object message)
    {
        if (IsErrorEnabled)
            _logger.Error("{@message}", message);
    }

    public void LogError(object message, object value)
    {
        if (IsErrorEnabled)
            _logger.Info("{message}, {@value}", message, value);
    }

    public void LogInfo(object message)
    {
        if (IsInfoEnabled)
            _logger.Info("{@message}", message);
    }

    public void LogInfo(object message, object value)
    {
        if (IsInfoEnabled)
            _logger.Info("{message}, {@value}", message, value);
    }

    public void LogWarn(object message)
    {
        if (IsWarnEnabled)
            _logger.Warn("{@message}", message);
    }

    public void LogWarn(object message, object value)
    {
        if (IsWarnEnabled)
            _logger.Info("{message}, {@value}", message, value);
    }

    public void LogFatal(object message)
    {
        if (IsFatalEnabled)
            _logger.Fatal("{@message}", message);
    }

    public void LogFatal(object message, object value)
    {
        if (IsFatalEnabled)
            _logger.Info("{message}, {@value}", message, value);
    }
}