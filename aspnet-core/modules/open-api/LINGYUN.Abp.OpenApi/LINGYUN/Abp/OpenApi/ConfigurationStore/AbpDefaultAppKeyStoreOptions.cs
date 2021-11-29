namespace LINGYUN.Abp.OpenApi.ConfigurationStore
{
    public class AbpDefaultAppKeyStoreOptions
    {
        public AppDescriptor[] AppDescriptors { get; set; }

        public AbpDefaultAppKeyStoreOptions()
        {
            AppDescriptors = new AppDescriptor[0];
        }
    }
}
