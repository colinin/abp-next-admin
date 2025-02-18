using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.Gdpr;

/// <summary>
/// 表示用户的个人数据
/// See: https://abp.io/docs/latest/modules/gdpr#gdprinfo
/// </summary>
public class GdprInfo : Entity<Guid>
{
    /// <summary>
    /// GDPR 请求的 ID
    /// </summary>
    public virtual Guid RequestId { get; protected set; }

    /// <summary>
    /// 用于存储个人数据
    /// </summary>
    public virtual string Data { get; protected set; }

    /// <summary>
    /// 表示收集个人数据的模块
    /// </summary>
    public virtual string Provider { get; protected set; }

    protected GdprInfo()
    {
    }

    public GdprInfo(
        Guid id,
        Guid requestId,
        string data, 
        string provider)
      : base(id)
    {
        RequestId = requestId;
        Data = Check.NotNullOrWhiteSpace(data, nameof(data));
        Provider = Check.NotNullOrWhiteSpace(provider, nameof(provider), GdprInfoConsts.MaxProviderLength);
    }
}
