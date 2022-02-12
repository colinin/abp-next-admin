using System;

namespace LINGYUN.Abp.OpenApi.ConfigurationStore
{
    public class AbpDefaultAppKeyStoreOptions
    {
        public AppDescriptor[] AppDescriptors { get; set; }

        public AbpDefaultAppKeyStoreOptions()
        {
            AppDescriptors = Array.Empty<AppDescriptor>();
        }
    }
}
