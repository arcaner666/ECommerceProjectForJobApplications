namespace ECommerce.Domain.Contracts;

public interface ILoggerManager
{
    bool IsDebugEnabled { get; }
    bool IsErrorEnabled { get; }
    bool IsInfoEnabled { get; }
    bool IsWarnEnabled { get; }
    bool IsFatalEnabled { get; }
    void LogDebug(object message);
    void LogDebug(object message, object value);
    void LogError(object message);
    void LogError(object message, object value);
    void LogInfo(object message);
    void LogInfo(object message, object value);
    void LogWarn(object message);
    void LogWarn(object message, object value);
    void LogFatal(object message);
    void LogFatal(object message, object value);
}