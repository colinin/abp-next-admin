using System;

namespace LINGYUN.Abp.Gdpr;

/// <summary>
/// 用户账户删除请求事件传输对象
/// </summary>
[Serializable]
public class GdprUserAccountDeletionRequestedEto
{
    public Guid UserId { get; set; }
}
