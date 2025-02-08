using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Messages.Integration;

[Area(PlatformRemoteServiceConsts.ModuleName)]
[ControllerName("SmsMessageIntegration")]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route($"integration-api/{PlatformRemoteServiceConsts.ModuleName}/messages/sms")]
public class SmsMessageIntegrationController : AbpControllerBase, ISmsMessageIntegrationService
{
    private readonly ISmsMessageIntegrationService _service;
    public SmsMessageIntegrationController(ISmsMessageIntegrationService service)
    {
        _service = service;
    }

    [HttpPost]
    public virtual Task<SmsMessageDto> CreateAsync(SmsMessageCreateDto input)
    {
        return _service.CreateAsync(input);
    }
}
