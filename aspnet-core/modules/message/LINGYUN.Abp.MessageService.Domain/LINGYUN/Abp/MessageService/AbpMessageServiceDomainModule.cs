using LINGYUN.Abp.MessageService.Mapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpMessageServiceDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MessageServiceDomainAutoMapperProfile>(validate: true);
            });
        }
    }
}
