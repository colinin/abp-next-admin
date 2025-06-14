using System.Globalization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LY.MicroService.Applications.Single.Branding;

[Dependency(ReplaceServices = true)]
public class SingleBrandingProvider(IConfiguration configuration) : DefaultBrandingProvider
{
    public override string AppName => configuration[GetConfigKey("App:Name")] ?? base.AppName;
    public override string LogoUrl => configuration["App:LogoUrl"] ?? base.LogoUrl;
    public override string LogoReverseUrl => configuration["App:LogoReverseUrl"] ?? base.LogoUrl;
    private static string GetConfigKey(string key)
    {
        return $"{key}:{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}";
    }
}
