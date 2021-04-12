using Dapr.Client;

namespace LINGYUN.Abp.Dapr.Client
{
    public interface IDaprClientFactory
    {
        DaprClient Create();
    }
}
