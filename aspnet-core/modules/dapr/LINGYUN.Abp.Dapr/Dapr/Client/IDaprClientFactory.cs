namespace Dapr.Client
{
    public interface IDaprClientFactory
    {
        DaprClient CreateClient(string name);
    }
}
