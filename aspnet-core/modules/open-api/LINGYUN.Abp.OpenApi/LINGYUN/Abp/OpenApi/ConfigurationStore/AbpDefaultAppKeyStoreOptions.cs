namespace LINGYUN.Abp.OpenApi.ConfigurationStore
{
    public class AbpDefaultAppKeyStoreOptions
    {
        public AppDescriptor[] Apps { get; set; }

        public AbpDefaultAppKeyStoreOptions()
        {
            Apps = new AppDescriptor[0];
        }
    }
}
