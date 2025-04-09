using System;
using Volo.Abp.Application.Dtos;

namespace PackageName.CompanyName.ProjectName.Users.Dtos
{
    [Serializable]
    public class UserPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string NickName { get; set; }
    }
}