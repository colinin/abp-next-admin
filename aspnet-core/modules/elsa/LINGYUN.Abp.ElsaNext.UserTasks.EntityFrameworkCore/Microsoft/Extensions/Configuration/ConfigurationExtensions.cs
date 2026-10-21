namespace Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static string? GetConnectionStringOrDefault(this IConfiguration configuration, string name, string defaultConnectionStringName = "Default")
    {
        return configuration?.GetConnectionString(name) ?? configuration?.GetConnectionString(defaultConnectionStringName);
    }
}
