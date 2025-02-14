using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Messages;
public interface ISmsMessageAppService : IApplicationService
{
    /// <summary>
    /// 获取短信消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SmsMessageDto> GetAsync(Guid id);
    /// <summary>
    /// 删除短信消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
    /// <summary>
    /// 重新发送短信消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task SendAsync(Guid id);
    /// <summary>
    /// 获取短信消息列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<SmsMessageDto>> GetListAsync(SmsMessageGetListInput input);
}
