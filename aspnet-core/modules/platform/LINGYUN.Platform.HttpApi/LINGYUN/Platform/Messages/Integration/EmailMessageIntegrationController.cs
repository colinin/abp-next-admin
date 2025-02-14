using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Messages.Integration;

[Area(PlatformRemoteServiceConsts.ModuleName)]
[ControllerName("EmailMessageIntegration")]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route($"integration-api/{PlatformRemoteServiceConsts.ModuleName}/messages/email")]
public class EmailMessageIntegrationController : AbpControllerBase, IEmailMessageIntegrationService
{
    private readonly IEmailMessageIntegrationService _service;
    public EmailMessageIntegrationController(IEmailMessageIntegrationService service)
    {
        _service = service;
    }


    [HttpPost]
    public virtual Task<EmailMessageDto> CreateAsync([FromForm] EmailMessageCreateDto input)
    {
        return _service.CreateAsync(input);
    }
}
