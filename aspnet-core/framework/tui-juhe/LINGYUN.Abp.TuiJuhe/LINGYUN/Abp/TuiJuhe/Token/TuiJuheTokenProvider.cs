using LINGYUN.Abp.TuiJuhe.Settings;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.TuiJuhe.Token;

public class TuiJuheTokenProvider : ITuiJuheTokenProvider, ITransientDependency
{
    protected ISettingProvider SettingProvider { get; }

    public TuiJuheTokenProvider(ISettingProvider settingProvider)
    {
        SettingProvider = settingProvider;
    }

    public async virtual Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        return await SettingProvider.GetOrNullAsync(
            TuiJuheSettingNames.Security.Token);
    }
}
