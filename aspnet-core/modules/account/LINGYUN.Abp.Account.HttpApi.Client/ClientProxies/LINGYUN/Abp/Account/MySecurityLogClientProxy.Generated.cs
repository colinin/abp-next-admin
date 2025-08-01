// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using LINGYUN.Abp.Account;
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
namespace LINGYUN.Abp.Account;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IMySecurityLogAppService), typeof(MySecurityLogClientProxy))]
public partial class MySecurityLogClientProxy : ClientProxyBase<IMySecurityLogAppService>, IMySecurityLogAppService
{
    public virtual async Task DeleteAsync(Guid id)
    {
        await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }

    public virtual async Task<SecurityLogDto> GetAsync(Guid id)
    {
        return await RequestAsync<SecurityLogDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }

    public virtual async Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetListInput input)
    {
        return await RequestAsync<PagedResultDto<SecurityLogDto>>(nameof(GetListAsync), new ClientProxyRequestTypeValue
        {
            { typeof(SecurityLogGetListInput), input }
        });
    }
}
