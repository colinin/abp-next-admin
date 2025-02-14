using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Messages;

[Authorize(PlatformPermissions.EmailMessage.Default)]
[Area(PlatformRemoteServiceConsts.ModuleName)]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{PlatformRemoteServiceConsts.ModuleName}/messages/email")]
public class EmailMessageController : AbpControllerBase, IEmailMessageAppService
{
    private readonly IEmailMessageAppService _service;
    public EmailMessageController(IEmailMessageAppService service)
    {
        _service = service;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(PlatformPermissions.EmailMessage.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<EmailMessageDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<EmailMessageDto>> GetListAsync(EmailMessageGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPost]
    [Route("{id}/send")]
    [Authorize(PlatformPermissions.EmailMessage.SendMessage)]
    public virtual Task SendAsync(Guid id)
    {
        return _service.SendAsync(id);
    }
}
