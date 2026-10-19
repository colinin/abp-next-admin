#nullable enable
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 测试用日志提供器(收集日志以进行断言)
/// </summary>
public class TestLoggerProvider : ILoggerProvider
{
    public static readonly ConcurrentQueue<TestLogEntry> Entries = new();

    public ILogger CreateLogger(string categoryName)
    {
        return new TestLogger(categoryName);
    }

    public void Dispose()
    {
    }

    public static void Clear()
    {
        while (Entries.TryDequeue(out _))
        {
        }
    }

    private sealed class TestLogger : ILogger
    {
        private readonly string _categoryName;

        public TestLogger(string categoryName)
        {
            _categoryName = categoryName;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Enqueue(new TestLogEntry(_categoryName, logLevel, formatter(state, exception)));
        }
    }
}

public record TestLogEntry(string Category, LogLevel Level, string Message);
