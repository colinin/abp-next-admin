using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Messages;

[Authorize(PlatformPermissions.SmsMessage.Default)]
[Area(PlatformRemoteServiceConsts.ModuleName)]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{PlatformRemoteServiceConsts.ModuleName}/messages/sms")]
public class SmsMessageController : AbpControllerBase, ISmsMessageAppService
{
    private readonly ISmsMessageAppService _service;
    public SmsMessageController(ISmsMessageAppService service)
    {
        _service = service;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(PlatformPermissions.SmsMessage.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<SmsMessageDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<SmsMessageDto>> GetListAsync(SmsMessageGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPost]
    [Route("{id}/send")]
    [Authorize(PlatformPermissions.SmsMessage.SendMessage)]
    public virtual Task SendAsync(Guid id)
    {
        return _service.SendAsync(id);
    }
}
