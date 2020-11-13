using System.Threading.Tasks;

namespace LINGYUN.Abp.EventBus.CAP
{
    public interface IFailedThresholdCallbackNotifier
    {
        Task NotifyAsync(AbpCAPExecutionFailedException exception);
    }
}
