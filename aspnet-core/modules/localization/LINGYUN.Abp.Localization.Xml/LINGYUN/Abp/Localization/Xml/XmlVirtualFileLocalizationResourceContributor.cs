using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Localization.Xml
{
    public class XmlVirtualFileLocalizationResourceContributor : XmlFileLocalizationResourceContributorBase
    {
        public XmlVirtualFileLocalizationResourceContributor(string filePath) 
            : base(filePath)
        {
        }

        protected override IFileProvider BuildFileProvider(LocalizationResourceInitializationContext context)
        {
            return context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();
        }
    }
}
