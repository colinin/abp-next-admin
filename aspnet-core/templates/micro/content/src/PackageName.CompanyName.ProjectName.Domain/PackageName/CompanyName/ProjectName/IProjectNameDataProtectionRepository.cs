using LINGYUN.Abp.DataProtection;
using Volo.Abp.Domain.Entities;

namespace PackageName.CompanyName.ProjectName;
/// <summary>
/// 受保护实体保护仓储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">实体主键类型</typeparam>
public interface IProjectNameDataProtectionRepository<TEntity, TKey> :
    IProjectNameBasicRepository<TEntity, TKey>,
    IDataProtectionRepository<TEntity>
    where TEntity : class, IEntity<TKey>
{
}
