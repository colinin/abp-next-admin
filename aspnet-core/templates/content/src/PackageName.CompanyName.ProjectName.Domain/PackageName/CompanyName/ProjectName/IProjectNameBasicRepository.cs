using LINGYUN.Abp.DataProtection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Specifications;

namespace PackageName.CompanyName.ProjectName;
/// <summary>
/// 基本仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">实体主键类型</typeparam>
public interface IProjectNameBasicRepository<TEntity, TKey> : IDataProtectionRepository<TEntity>
    where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// 获取过滤后的实体数量
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetCountAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取过滤后的实体列表(分页)
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync(
        ISpecification<TEntity> specification,
        string sorting = nameof(IEntity<TKey>.Id),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取过滤后的实体列表
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetListAsync(
        ISpecification<TEntity> specification,
        string sorting = nameof(IEntity<TKey>.Id),
        int maxResultCount = 10,
        CancellationToken cancellationToken = default);
}
