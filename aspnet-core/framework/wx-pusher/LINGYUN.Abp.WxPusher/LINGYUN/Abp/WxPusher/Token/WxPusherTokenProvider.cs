using LINGYUN.Abp.WxPusher.Settings;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.WxPusher.Token;

public class WxPusherTokenProvider : IWxPusherTokenProvider, ITransientDependency
{
    protected ISettingProvider SettingProvider { get; }

    public WxPusherTokenProvider(ISettingProvider settingProvider)
    {
        SettingProvider = settingProvider;
    }

    public async virtual Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        var appToken = await SettingProvider.GetOrNullAsync(WxPusherSettingNames.Security.AppToken);

        Check.NotNullOrWhiteSpace(appToken, nameof(appToken));

        return appToken;
    }
}
