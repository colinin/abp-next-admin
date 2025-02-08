using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Messages;
public interface IEmailMessageAppService : IApplicationService
{
    /// <summary>
    /// 获取邮件消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmailMessageDto> GetAsync(Guid id);
    /// <summary>
    /// 删除邮件消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
    /// <summary>
    /// 发送邮件消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task SendAsync(Guid id);
    /// <summary>
    /// 获取邮件消息列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<EmailMessageDto>> GetListAsync(EmailMessageGetListInput input);
}
