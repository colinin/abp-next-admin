using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace LINGYUN.Abp.EntityFrameworkCore.Tests
{
    public class EfCoreLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EfCoreLogger(categoryName);//创建EFLogger类的实例
        }

        public void Dispose()
        {

        }
    }

    public class EfCoreLogger : ILogger
    {
        private readonly string _categoryName;

        public EfCoreLogger(string categoryName) => this._categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logContent = formatter(state, exception);

            Debug.WriteLine("");
            // Console.ForegroundColor = logLevel == LogLevel.Warning || logLevel == LogLevel.Error ? ConsoleColor.Red : ConsoleColor.Green;
            Debug.WriteLine(logContent);
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }

    public static class EFCoreLoggerExtensions
    {
        public static DbContextOptionsBuilder UseEFCoreLogger(this DbContextOptionsBuilder builder)
        {
            return builder.UseLoggerFactory(new EfCoreLoggerFactory());
        }
    }
}
