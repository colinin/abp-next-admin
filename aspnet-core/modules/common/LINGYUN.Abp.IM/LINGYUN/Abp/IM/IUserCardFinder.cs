using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM
{
    /// <summary>
    /// IM用户资料查找接口
    /// </summary>
    public interface IUserCardFinder
    {
        /// <summary>
        /// 查询IM用户数量
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="findUserName">用户名称</param>
        /// <param name="startAge">起止年龄</param>
        /// <param name="endAge">起止年龄</param>
        /// <param name="sex">性别</param>
        /// <returns></returns>
        Task<int> GetCountAsync(
            Guid? tenantId,
            string findUserName = "",
            int? startAge = null,
            int? endAge = null,
            Sex? sex = null);
        /// <summary>
        /// 查询IM用户列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="findUserName">用户名称</param>
        /// <param name="startAge">起止年龄</param>
        /// <param name="endAge">起止年龄</param>
        /// <param name="sex">性别</param>
        /// <param name="sorting">排序字段</param>
        /// <param name="skipCount">起始记录位置</param>
        /// <param name="maxResultCount">最大返回数量</param>
        /// <returns></returns>
        Task<List<UserCard>> GetListAsync(
            Guid? tenantId,
            string findUserName = "",
            int? startAge = null,
            int? endAge = null,
            Sex? sex = null,
            string sorting = nameof(UserCard.UserId),
            int skipCount = 0,
            int maxResultCount = 10);
        /// <summary>
        /// 获取IM用户信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="findUserId"></param>
        /// <returns></returns>
        Task<UserCard> GetMemberAsync(
            Guid? tenantId,
            Guid findUserId);
    }
}
