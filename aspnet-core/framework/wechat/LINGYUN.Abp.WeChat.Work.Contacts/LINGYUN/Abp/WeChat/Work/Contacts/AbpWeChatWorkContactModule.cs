using LINGYUN.Abp.WeChat.Common;
using LINGYUN.Abp.WeChat.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Common.Messages;
using LINGYUN.Abp.WeChat.Work.Contacts.Messages.Models;
using LINGYUN.Abp.WeChat.Work.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WeChat.Work.Contacts;
/// <summary>
/// 企业微信通讯录模块
/// </summary>
[DependsOn(typeof(AbpWeChatWorkModule))]
public class AbpWeChatWorkContactModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpWeChatWorkMessageResolveOptions>(options =>
        {
            options.MapEvent("change_contact", context =>
            {
                var changeType = context.GetMessageData("ChangeType");
                return changeType switch
                {
                    "create_user" => context.GetWeChatMessage<CreateUserEvent>(),
                    "update_user" => context.GetWeChatMessage<UpdateUserEvent>(),
                    "delete_user" => context.GetWeChatMessage<DeleteUserEvent>(),
                    "create_party" => context.GetWeChatMessage<CreateDepartmentEvent>(),
                    "update_party" => context.GetWeChatMessage<UpdateDepartmentEvent>(),
                    "delete_party" => context.GetWeChatMessage<DeleteDepartmentEvent>(),
                    "update_tag" => context.GetWeChatMessage<UserTagChangeEvent>(),
                    _ => throw new AbpWeChatException($"Contact change event {changeType} is not mounted!"),
                };
            });
            options.MapEvent("batch_job_result", context => context.GetWeChatMessage<BatchJobResultEvent>());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpWeChatWorkContactModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WeChatWorkResource>()
                .AddVirtualJson("/LINGYUN/Abp/WeChat/Work/Contacts/Localization/Resources");
        });
    }
}
