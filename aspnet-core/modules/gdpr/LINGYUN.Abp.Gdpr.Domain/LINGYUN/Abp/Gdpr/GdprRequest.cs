using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;

namespace LINGYUN.Abp.Gdpr;

/// <summary>
/// 表示用户发出的 GDPR 请求
/// See: https://abp.io/docs/latest/modules/gdpr#gdprrequest
/// </summary>
public class GdprRequest : AggregateRoot<Guid>, IHasCreationTime
{
    ///// TODO: Abp Framework GDPR模块不支持多租户
    ///// <summary>
    ///// 租户Id
    ///// </summary>
    ///// public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 发出请求的用户的 ID
    /// </summary>
    public virtual Guid UserId { get; protected set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public virtual DateTime CreationTime { get; protected set; }
    /// <summary>
    /// 数据准备过程的结束时间
    /// </summary>
    public virtual DateTime ReadyTime { get; protected set; }
    /// <summary>
    /// 收集的用户个人数据
    /// </summary>
    public virtual ICollection<GdprInfo> Infos { get; protected set; }
    protected GdprRequest()
    {
        Infos = new Collection<GdprInfo>();
    }

    public GdprRequest(
        Guid id, 
        Guid userId,
        DateTime readyTime)
      : base(id)
    {
        UserId = userId;
        ReadyTime = readyTime;
        Infos = new Collection<GdprInfo>();
    }

    public void AddData(
        IGuidGenerator guidGenerator, 
        string data, 
        string provider)
    {
        Infos.Add(
            new GdprInfo(
                guidGenerator.Create(), 
                Id, 
                data,
                provider));
    }
}
