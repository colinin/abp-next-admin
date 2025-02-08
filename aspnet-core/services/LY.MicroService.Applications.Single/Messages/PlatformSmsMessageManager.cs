using LINGYUN.Platform.Messages;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Sms;

namespace LY.MicroService.Applications.Single.Messages;


[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(ISmsMessageManager), typeof(SmsMessageManager))]
public class PlatformSmsMessageManager : SmsMessageManager
{
    public PlatformSmsMessageManager(
        ISmsMessageRepository repository) : base(repository)
    {
    }

    protected override ISmsSender GetSmsSender()
    {
        return LazyServiceProvider.GetRequiredKeyedService<ISmsSender>("DefaultSmsSender");
    }
}
