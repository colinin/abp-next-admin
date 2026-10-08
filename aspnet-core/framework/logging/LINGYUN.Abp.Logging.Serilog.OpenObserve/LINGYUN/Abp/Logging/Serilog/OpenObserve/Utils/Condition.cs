namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Utils;

public record Condition(string Field, ComparisonOp Op, object? Value);
