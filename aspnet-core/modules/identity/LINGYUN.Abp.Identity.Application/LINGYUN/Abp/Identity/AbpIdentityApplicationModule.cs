using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity
{
    [DependsOn(
        typeof(Volo.Abp.Identity.AbpIdentityApplicationModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpIdentityDomainModule))]
    public class AbpIdentityApplicationModule : AbpModule
    {
        // private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpIdentityApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityApplicationModuleAutoMapperProfile>(validate: true);
            });
        }

        //public override void PostConfigureServices(ServiceConfigurationContext context)
        //{
        //    OneTimeRunner.Run(() =>
        //    {
        //        ObjectExtensionManager.Instance
        //            .AddOrUpdateProperty<string>(
        //                new[]
        //                {
        //                    typeof(IdentityUserDto),
        //                    typeof(IdentityUserCreateDto),
        //                    typeof(IdentityUserUpdateDto),
        //                    typeof(ProfileDto),
        //                    typeof(UpdateProfileDto)
        //                },
        //                ExtensionIdentityUserConsts.AvatarUrlField);
        //    });
        //}
    }
}
