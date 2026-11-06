namespace LINGYUN.Abp.OperationRateLimiting;

public class TestRateLimitingInput : IHasOperationRateLimitingParameter
{
    public string? PhoneNumber { get; set; }

    public string? GetPartitionParameter()
    {
        return PhoneNumber;
    }
}
