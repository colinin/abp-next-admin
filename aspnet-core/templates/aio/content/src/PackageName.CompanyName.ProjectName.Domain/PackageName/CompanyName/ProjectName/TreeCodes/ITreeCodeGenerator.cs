using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace PackageName.CompanyName.ProjectName.TreeCodes
{
    /// <summary>
    /// 树形编码生成器接口
    /// </summary>
    public interface ITreeCodeGenerator
    {
        /// <summary>
        /// 生成树形编码
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="parentId">父级Id</param>
        /// <returns>生成的树形编码</returns>
        Task<string> GenerateAsync<TEntity>(
            IRepository<TEntity, Guid> repository,
            Guid? parentId)
            where TEntity : class, IEntity<Guid>, IHaveTreeCode;

        /// <summary>
        /// 更新节点及其所有子节点的TreeCode
        /// </summary>
        Task UpdateTreeCodesAsync<TEntity>(
            IRepository<TEntity, Guid> repository,
            Guid entityId)
            where TEntity : class, IEntity<Guid>, IHaveTreeCode;
    }
}
