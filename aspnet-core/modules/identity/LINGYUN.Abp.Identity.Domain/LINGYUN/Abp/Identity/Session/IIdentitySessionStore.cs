using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity.Session;
/// <summary>
/// 用户会话持久化
/// </summary>
public interface IIdentitySessionStore
{
    /// <summary>
    /// 创建用户会话
    /// </summary>
    /// <param name="sessionId">会话id</param>
    /// <param name="device">设备</param>
    /// <param name="deviceInfo">设备信息</param>
    /// <param name="userId">用户id</param>
    /// <param name="clientId">客户端id</param>
    /// <param name="ipAddresses">ip地址</param>
    /// <param name="tenantId">租户id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>创建完成的 <seealso cref="IdentitySession"/></returns>
    Task<IdentitySession> CreateAsync(
        string sessionId,
        string device,
        string deviceInfo,
        Guid userId,
        string clientId,
        string ipAddresses,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 更新用户会话
    /// </summary>
    /// <param name="session">用户会话实体</param>
    /// <param name="cancellationToken"></param>
    /// <returns>a completed task.</returns>
    Task UpdateAsync(
        IdentitySession session,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询用户会话
    /// </summary>
    /// <param name="id">会话key</param>
    /// <param name="cancellationToken"></param>
    /// <returns>查询到的<seealso cref="IdentitySession"/></returns>
    /// <exception cref="Volo.Abp.Domain.Entities.EntityNotFoundException"></exception>
    Task<IdentitySession> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询用户会话
    /// </summary>
    /// <param name="id">会话key</param>
    /// <param name="cancellationToken"></param>
    /// <returns>如果存在返回 <seealso cref="IdentitySession"/> 的实例, 否则返回 null.</returns>
    Task<IdentitySession> FindAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询用户会话
    /// </summary>
    /// <param name="sessionId">会话id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>查询到的<seealso cref="IdentitySession"/></returns>
    /// <exception cref="Volo.Abp.Domain.Entities.EntityNotFoundException"></exception>
    Task<IdentitySession> GetAsync(
        string sessionId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询用户会话
    /// </summary>
    /// <param name="sessionId">会话id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>如果存在返回 <seealso cref="IdentitySession"/> 的实例, 否则返回 null.</returns>
    Task<IdentitySession> FindAsync(
        string sessionId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 查询用户最后一次会话
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="device">设备</param>
    /// <param name="cancellationToken"></param>
    /// <returns>如果存在返回 <seealso cref="IdentitySession"/> 的实例, 否则返回 null.</returns>
    Task<IdentitySession> FindLastAsync(
        Guid userId,
        string device,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 会话是否已存在
    /// </summary>
    /// <param name="sessionId">会话id</param>
    /// <param name="cancellationToken"></param>
    /// <returns>如果存在,返回true, 否则返回false.</returns>
    Task<bool> ExistAsync(
        string sessionId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 撤销用户会话
    /// </summary>
    /// <param name="id">会话key</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 撤销用户会话
    /// </summary>
    /// <param name="sessionId">会话id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAsync(
        string sessionId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 撤销全部会话
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="exceptSessionId">排除会话key</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAllAsync(
        Guid userId, 
        Guid? exceptSessionId = null,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 撤销全部会话
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="device">设备</param>
    /// <param name="exceptSessionId">排除会话key</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAllAsync(
        Guid userId, 
        string device,
        Guid? exceptSessionId = null,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 撤销不活跃会话
    /// </summary>
    /// <param name="inactiveTimeSpan">不活跃的时间间隔</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAllAsync(
        TimeSpan inactiveTimeSpan, 
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 撤销指定的会话
    /// </summary>
    /// <param name="userId">用户id</param>
    /// <param name="device">设备</param>
    /// <param name="exceptSessionId">排除会话id</param>
    /// <param name="maxCount">最大数量</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeWithAsync(
        Guid userId,
        string device = null,
        Guid? exceptSessionId = null,
        int maxCount = 0,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 撤销会话列表
    /// </summary>
    /// <param name="identitySessions">会话列表</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeManyAsync(
        IEnumerable<IdentitySession> identitySessions,
        CancellationToken cancellationToken = default);
}
