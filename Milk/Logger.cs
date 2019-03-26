using System;

namespace Milk
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public static class Logger
    {
        public static event Action<LogLevel, string> OnLog;

        public static void Log(LogLevel level, string message)
        {
            OnLog?.Invoke(level, message);
        }
    }
}
