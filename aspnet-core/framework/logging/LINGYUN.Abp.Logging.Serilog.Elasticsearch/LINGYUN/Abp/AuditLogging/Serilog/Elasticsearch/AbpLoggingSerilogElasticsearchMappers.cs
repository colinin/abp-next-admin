using LINGYUN.Abp.Logging;
using LINGYUN.Abp.Logging.Serilog.Elasticsearch;
using Riok.Mapperly.Abstractions;
using Serilog.Events;
using System.Collections.Generic;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.AuditLogging.Serilog.Elasticsearch;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SerilogExceptionToLogExceptionMapper : MapperBase<SerilogException, LogException>
{
    public override partial LogException Map(SerilogException source);
    public override partial void Map(SerilogException source, LogException destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SerilogFieldToLogFieldMapper : MapperBase<SerilogField, LogField>
{
    [MapperIgnoreTarget(nameof(LogField.Id))]
    public override partial LogField Map(SerilogField source);

    [MapperIgnoreTarget(nameof(LogField.Id))]
    public override partial void Map(SerilogField source, LogField destination);

    public override void AfterMap(SerilogField source, LogField destination)
    {
        destination.Id = source.UniqueId.ToString();
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SerilogInfoToLogInfoMapper : MapperBase<SerilogInfo, LogInfo>
{
    private readonly SerilogFieldToLogFieldMapper _logFieldMapper;
    private readonly SerilogExceptionToLogExceptionMapper _logExceptionMapper;
    public SerilogInfoToLogInfoMapper(
        SerilogFieldToLogFieldMapper logFieldMapper,
        SerilogExceptionToLogExceptionMapper logExceptionMapper)
    {
        _logFieldMapper = logFieldMapper;
        _logExceptionMapper = logExceptionMapper;
    }

    [MapperIgnoreTarget(nameof(LogInfo.Fields))]
    [MapPropertyFromSource(nameof(SerilogInfo.Level), Use = nameof(GetLogLevel))]
    public override partial LogInfo Map(SerilogInfo source);

    [MapperIgnoreTarget(nameof(LogInfo.Fields))]
    [MapPropertyFromSource(nameof(SerilogInfo.Level), Use = nameof(GetLogLevel))]
    public override partial void Map(SerilogInfo source, LogInfo destination);

    public override void AfterMap(SerilogInfo source, LogInfo destination)
    {
        if (source.Exceptions != null)
        {
            destination.Exceptions ??= new List<LogException>();
            destination.Exceptions.Clear();

            foreach (var exception in source.Exceptions)
            {
                var logException = _logExceptionMapper.Map(exception);
                _logExceptionMapper.AfterMap(exception, logException);
                destination.Exceptions.Add(logException);
            }
        }

        if (source.Fields != null)
        {
            var logFields = _logFieldMapper.Map(source.Fields);
            _logFieldMapper.AfterMap(source.Fields, logFields);
            destination.Fields = logFields;
        }
    }

    [UserMapping(Default = false)]
    private static Microsoft.Extensions.Logging.LogLevel GetLogLevel(SerilogInfo source)
    {
        return source.Level switch
        {
            LogEventLevel.Fatal => Microsoft.Extensions.Logging.LogLevel.Critical,
            LogEventLevel.Error => Microsoft.Extensions.Logging.LogLevel.Error,
            LogEventLevel.Warning => Microsoft.Extensions.Logging.LogLevel.Warning,
            LogEventLevel.Information => Microsoft.Extensions.Logging.LogLevel.Information,
            LogEventLevel.Debug => Microsoft.Extensions.Logging.LogLevel.Debug,
            _ => Microsoft.Extensions.Logging.LogLevel.Trace,
        };
    }
}
