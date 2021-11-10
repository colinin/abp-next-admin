using LINGYUN.Abp.IM.Groups;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Groups
{
    public interface IUserGroupAppService : IApplicationService
    {
        /// <summary>
        /// 申请加入群组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ApplyJoinGroupAsync(UserJoinGroupDto input);
        /// <summary>
        /// 获取我的群组
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<Group>> GetMyGroupsAsync();
        /// <summary>
        /// 获取群组用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<GroupUserCard>> GetGroupUsersAsync(GroupUserGetByPagedDto input);
        /// <summary>
        /// 处理用户群组申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task GroupAcceptUserAsync(GroupAcceptUserDto input);
        /// <summary>
        /// 群组移除用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task GroupRemoveUserAsync(GroupRemoveUserDto input);
    }
}
