namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Determines how the partition (counter) key of a rate limiting rule is resolved.
/// </summary>
public enum OperationRateLimitingPartitionType : byte
{
    /// <summary>
    /// Uses the <see cref="OperationRateLimitingContext.Parameter"/> value as-is.
    /// </summary>
    Parameter = 0,

    /// <summary>
    /// Uses the identifier of the current authenticated user.
    /// </summary>
    CurrentUser = 10,

    /// <summary>
    /// Uses the identifier of the current tenant ("host" when no tenant is active).
    /// </summary>
    CurrentTenant = 20,

    /// <summary>
    /// Uses the client IP address resolved from the web request.
    /// </summary>
    ClientIp = 30,

    /// <summary>
    /// Resolves an email address from the parameter or the current user, normalized to upper case.
    /// </summary>
    Email = 40,

    /// <summary>
    /// Resolves a phone number from the parameter or the current user, normalized by removing formatting characters.
    /// </summary>
    PhoneNumber = 50,

    /// <summary>
    /// Uses a named custom partition key resolver registered in the options.
    /// </summary>
    Custom = 100
}
