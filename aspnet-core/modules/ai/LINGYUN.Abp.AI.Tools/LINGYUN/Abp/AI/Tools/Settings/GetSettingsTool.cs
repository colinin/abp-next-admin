using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.AI.Tools.Settings;
public class GetSettingsTool : ITransientDependency
{
    public const string Name = "GetSettings";

    private readonly ISettingProvider _settingProvider;
    public GetSettingsTool(ISettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
    }

    public async Task<object?> InvokeAsync()
    {
        return await _settingProvider.GetAllAsync();
    }
}
