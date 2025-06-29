// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using LINGYUN.Abp.OssManagement;
using LINGYUN.Abp.OssManagement.Integration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;

// ReSharper disable once CheckNamespace
namespace LINGYUN.Abp.OssManagement.Integration;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IOssObjectIntegrationService), typeof(OssObjectIntegrationClientProxy))]
[IntegrationService]
public partial class OssObjectIntegrationClientProxy : ClientProxyBase<IOssObjectIntegrationService>, IOssObjectIntegrationService
{
    public virtual async Task<OssObjectDto> CreateAsync(CreateOssObjectInput input)
    {
        return await RequestAsync<OssObjectDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(CreateOssObjectInput), input }
        });
    }

    public virtual async Task DeleteAsync(GetOssObjectInput input)
    {
        await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
        {
            { typeof(GetOssObjectInput), input }
        });
    }

    public virtual async Task<GetOssObjectExistsResult> ExistsAsync(GetOssObjectInput input)
    {
        return await RequestAsync<GetOssObjectExistsResult>(nameof(ExistsAsync), new ClientProxyRequestTypeValue
        {
            { typeof(GetOssObjectInput), input }
        });
    }

    public virtual async Task<IRemoteStreamContent> GetAsync(GetOssObjectInput input)
    {
        return await RequestAsync<IRemoteStreamContent>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(GetOssObjectInput), input }
        });
    }
}
