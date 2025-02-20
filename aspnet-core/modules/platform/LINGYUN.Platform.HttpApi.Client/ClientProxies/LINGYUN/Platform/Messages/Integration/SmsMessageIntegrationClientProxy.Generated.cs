// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using LINGYUN.Platform.Messages;
using LINGYUN.Platform.Messages.Integration;
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
namespace LINGYUN.Platform.Messages.Integration;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(ISmsMessageIntegrationService), typeof(SmsMessageIntegrationClientProxy))]
[IntegrationService]
public partial class SmsMessageIntegrationClientProxy : ClientProxyBase<ISmsMessageIntegrationService>, ISmsMessageIntegrationService
{
    public virtual async Task<SmsMessageDto> CreateAsync(SmsMessageCreateDto input)
    {
        return await RequestAsync<SmsMessageDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(SmsMessageCreateDto), input }
        });
    }
}
