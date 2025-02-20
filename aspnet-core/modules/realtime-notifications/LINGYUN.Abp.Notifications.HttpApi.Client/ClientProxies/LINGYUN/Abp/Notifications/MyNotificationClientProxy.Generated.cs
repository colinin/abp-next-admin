// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using LINGYUN.Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;

// ReSharper disable once CheckNamespace
namespace LINGYUN.Abp.Notifications;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IMyNotificationAppService), typeof(MyNotificationClientProxy))]
public partial class MyNotificationClientProxy : ClientProxyBase<IMyNotificationAppService>, IMyNotificationAppService
{
    public virtual async Task MarkReadStateAsync(NotificationMarkReadStateInput input)
    {
        await RequestAsync(nameof(MarkReadStateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(NotificationMarkReadStateInput), input }
        });
    }

    public virtual async Task DeleteAsync(long id)
    {
        await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
        {
            { typeof(long), id }
        });
    }

    public virtual async Task<UserNotificationDto> GetAsync(long id)
    {
        return await RequestAsync<UserNotificationDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(long), id }
        });
    }

    public virtual async Task<PagedResultDto<UserNotificationDto>> GetListAsync(UserNotificationGetByPagedDto input)
    {
        return await RequestAsync<PagedResultDto<UserNotificationDto>>(nameof(GetListAsync), new ClientProxyRequestTypeValue
        {
            { typeof(UserNotificationGetByPagedDto), input }
        });
    }
}
