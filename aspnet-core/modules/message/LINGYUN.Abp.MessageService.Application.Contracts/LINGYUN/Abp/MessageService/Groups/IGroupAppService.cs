using LINGYUN.Abp.IM.Groups;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Groups
{
    public interface IGroupAppService : IApplicationService
    {
        /// <summary>
        /// 搜索群组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<Group>> SearchAsync(GroupSearchInput input);
        /// <summary>
        /// 获取群组信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<Group> GetAsync(string groupId);
    }
}
