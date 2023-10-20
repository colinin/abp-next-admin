namespace Dapr.Client;

public static class IDaprClientFactoryExtensions
{
    public static DaprClient CreateClient(
        this IDaprClientFactory clientFactory)
    {
        return clientFactory.CreateClient("Default");
    }
}
