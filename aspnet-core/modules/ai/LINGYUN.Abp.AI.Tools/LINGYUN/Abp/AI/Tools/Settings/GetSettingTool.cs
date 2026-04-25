using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.AI.Tools.Settings;
public class GetSettingTool : ITransientDependency
{
    public const string Name = "GetSetting";

    private readonly ISettingProvider _settingProvider;
    public GetSettingTool(ISettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
    }

    public async Task<object?> InvokeAsync(string name)
    {
        return await _settingProvider.GetOrNullAsync(name);
    }
}
