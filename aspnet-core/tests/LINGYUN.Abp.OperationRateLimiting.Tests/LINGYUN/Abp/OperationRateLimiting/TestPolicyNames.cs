namespace LINGYUN.Abp.OperationRateLimiting;

public static class TestPolicyNames
{
    public const string Fixed = "Test.Fixed";
    public const string Sliding = "Test.Sliding";
    public const string TokenBucket = "Test.TokenBucket";
    public const string Concurrency = "Test.Concurrency";
    public const string Email = "Test.Email";
    public const string PhoneNumber = "Test.PhoneNumber";
    public const string MultiRule = "Test.MultiRule";
    public const string Combined = "Test.Combined";
    public const string Ban = "Test.Ban";
    public const string CustomError = "Test.CustomError";
    public const string CustomPartition = "Test.CustomPartition";
    public const string Tenant = "Test.Tenant";
    public const string DisabledPolicy = "Test.DisabledPolicy";
    public const string Merged = "Test.Merged";
    public const string DynamicOnly = "Test.DynamicOnly";
    public const string Attribute = "Test.Attribute";
    public const string ClassLevel = "Test.ClassLevel";
    public const string CustomAlgorithm = "Test.CustomAlgorithm";
}
