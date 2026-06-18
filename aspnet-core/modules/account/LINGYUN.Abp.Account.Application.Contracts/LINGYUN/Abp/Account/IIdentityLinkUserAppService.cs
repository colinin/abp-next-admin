using LINGYUN.Abp.Account.Dto;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Account;
/// <summary>
/// 关联用户应用服务接口
/// </summary>
public interface IIdentityLinkUserAppService : IApplicationService
{
    /// <summary>
    /// 关联用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task LinkAsync(LinkUserInput input);
    /// <summary>
    /// 取消关联用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UnlinkAsync(UnLinkUserInput input);
    /// <summary>
    /// 生成关联用户Token
    /// </summary>
    /// <returns></returns>
    Task<string> GenerateLinkTokenAsync();
    /// <summary>
    /// 生成关联用户登录Token
    /// </summary>
    /// <returns></returns>
    Task<string> GenerateLinkLoginTokenAsync();
    /// <summary>
    /// 获取已关联用户列表
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<LinkUserDto>> GetListAsync();
    /// <summary>
    /// 验证关联用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<VerifyLinkUserDto> VerifyLinkUserAsync(VerifyLinkUserInput input);
    /// <summary>
    /// 验证关联用户Token
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> VerifyLinkTokenAsync(VerifyLinkTokenInput input);
    /// <summary>
    /// 验证关联用户登录Token
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> VerifyLinkLoginTokenAsync(VerifyLinkTokenInput input);
}
