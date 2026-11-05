namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Implemented by a method parameter (usually an input DTO) that provides the
/// <see cref="OperationRateLimitingContext.Parameter"/> value.
/// </summary>
public interface IHasOperationRateLimitingParameter
{
    string? GetPartitionParameter();
}
