using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.InMemory;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Specifications;
using Volo.Abp.Testing;
using Xunit;

namespace LINGYUN.Abp.Logging;

public abstract class LoggingManager_Tests<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly string _context;
    private readonly Microsoft.Extensions.Logging.ILogger _logger;
    private readonly ILoggingManager _manager;

    public LoggingManager_Tests()
    {
        _manager = GetRequiredService<ILoggingManager>();

        _context = GetType().FullName!;
        var loggerFactory = GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger(_context);
    }

    protected override void BeforeAddApplication(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithUniqueId()
            .WriteTo.InMemory()
            .CreateLogger();

        services.AddLogging(logging =>
        {
            logging.AddSerilog();
        });
    }

    [Fact]
    public async Task Should_Get_List()
    {
        _logger.LogDebug("xunit test debug log");
        _logger.LogInformation("xunit test information log");
        _logger.LogWarning("xunit test warning log");
        _logger.LogError("xunit test error log");

        await Log.CloseAndFlushAsync();

        await Task.Delay(5000);

        (await _manager.GetCountAsync(context: _context)).ShouldBe(4);

        (await _manager.GetCountAsync(level: LogLevel.Information, context: _context)).ShouldBe(1);

        var logs = await _manager.GetListAsync(level: LogLevel.Information, context: _context);
        logs.Count.ShouldBe(1);
        logs[0].Level.ShouldBe(LogLevel.Information);
        logs[0].Message.ShouldBe("xunit test information log");
        logs[0].Fields.ShouldNotBeNull();
        logs[0].Fields.Id.ShouldNotBeNullOrWhiteSpace();
        logs[0].Fields.Context.ShouldBe(_context);

        var log = await _manager.GetAsync(logs[0].Fields.Id);
        log.Message.ShouldBe("xunit test information log");
        log.Fields.ShouldNotBeNull();
        log.Fields.Id.ShouldNotBeNullOrWhiteSpace();
        log.Fields.Id.ShouldBe(logs[0].Fields.Id);
        log.Fields.Context.ShouldBe(_context);
    }

    [Fact]
    public async Task Should_Get_List_With_Specification()
    {
        _logger.LogDebug("xunit test debug log");
        _logger.LogInformation("xunit test information log");
        _logger.LogWarning("xunit test warning log");
        _logger.LogError("xunit test error log");

        await Log.CloseAndFlushAsync();

        await Task.Delay(5000);

        var specification = new ExpressionSpecification<LogInfo>(
            x => x.Level == LogLevel.Information && x.Fields.Context == _context);

        var logs = await _manager.GetListAsync(specification);
        logs.Count.ShouldBe(1);
        logs[0].Level.ShouldBe(LogLevel.Information);
        logs[0].Message.ShouldBe("xunit test information log");
        logs[0].Fields.ShouldNotBeNull();
        logs[0].Fields.Id.ShouldNotBeNullOrWhiteSpace();
        logs[0].Fields.Context.ShouldBe(_context);

        var log = await _manager.GetAsync(logs[0].Fields.Id);
        log.Message.ShouldBe("xunit test information log");
        log.Fields.ShouldNotBeNull();
        log.Fields.Id.ShouldNotBeNullOrWhiteSpace();
        log.Fields.Id.ShouldBe(logs[0].Fields.Id);
        log.Fields.Context.ShouldBe(_context);
    }
}
