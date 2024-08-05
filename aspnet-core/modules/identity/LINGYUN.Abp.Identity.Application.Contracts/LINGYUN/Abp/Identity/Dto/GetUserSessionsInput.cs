using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity;
public class GetUserSessionsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 用户id
    /// </summary>
    public Guid? UserId { get; set; }
    /// <summary>
    /// 设备
    /// </summary>
    public string Device { get; set; }
    /// <summary>
    /// 客户端id
    /// </summary>
    public string ClientId { get; set; }
}
