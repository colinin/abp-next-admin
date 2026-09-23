using LINGYUN.Abp.SettingManagement;
using LINGYUN.Abp.Tencent.Captcha;
using LINGYUN.Abp.Tencent.QQ;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace LINGYUN.Abp.Tencent.SettingManagement;

[DependsOn(
    typeof(AbpTencentCloudModule),
    typeof(AbpTencentCaptchaModule),
    typeof(AbpTencentQQModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpTencentCloudSettingManagementModule : AbpModule
{

}