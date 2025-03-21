using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace PackageName.CompanyName.ProjectName.TreeCodes
{
    /// <summary>
    /// 树形编码生成器
    /// </summary>
    public class TreeCodeGenerator : ITreeCodeGenerator, ISingletonDependency
    {
        /// <summary>
        /// 生成树形编码
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="repository">仓储</param>
        /// <param name="parentId">父级Id</param>
        /// <returns>生成的树形编码</returns>
        public async virtual Task<string> GenerateAsync<TEntity>(
            IRepository<TEntity, Guid> repository,
            Guid? parentId)
            where TEntity : class, IEntity<Guid>, IHaveTreeCode
        {
            if (!parentId.HasValue)
            {
                // 生成根节点编码
                return await GenerateRootCodeAsync(repository);
            }
            else
            {
                // 生成子节点编码
                return await GenerateChildCodeAsync(repository, parentId.Value);
            }
        }

        private async Task<string> GenerateRootCodeAsync<TEntity>(
            IRepository<TEntity, Guid> repository)
            where TEntity : class, IEntity<Guid>, IHaveTreeCode
        {
            var query = await repository.GetQueryableAsync();
            query = query.Where(e => e.ParentId == null).OrderByDescending(e => e.CreationTime);
            var maxCodeEntity = await repository.AsyncExecuter.FirstOrDefaultAsync(query);

            if (maxCodeEntity == null)
            {
                return "0001";
            }

            int maxCode = int.Parse(maxCodeEntity.TreeCode.Split('.').LastOrDefault("0"));
            return (maxCode + 1).ToString("D4");
        }

        private async Task<string> GenerateChildCodeAsync<TEntity>(
            IRepository<TEntity, Guid> repository,
            Guid parentId)
            where TEntity : class, IEntity<Guid>, IHaveTreeCode
        {
            var parent = await repository.GetAsync(parentId);
            if (parent == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), parentId);
            }

            var query = await repository.GetQueryableAsync();
            query = query.Where(e => e.ParentId == parentId).OrderByDescending(e => e.CreationTime);
            var maxCodeEntity = await repository.AsyncExecuter.FirstOrDefaultAsync(query);

            string newCode;
            if (maxCodeEntity == null)
            {
                newCode = "0001";
            }
            else
            {
                int maxCode = int.Parse(maxCodeEntity.TreeCode.Split('.').Last());
                newCode = (maxCode + 1).ToString("D4");
            }

            // 构建完整的TreeCode: 父TreeCode.新编码
            return $"{parent.TreeCode}.{newCode}";
        }

        /// <summary>
        /// 更新节点及其所有子节点的TreeCode
        /// </summary>
        public async virtual Task UpdateTreeCodesAsync<TEntity>(
            IRepository<TEntity, Guid> repository,
            Guid entityId)
            where TEntity : class, IEntity<Guid>, IHaveTreeCode
        {
            var entity = await repository.GetAsync(entityId);
            var query = await repository.GetQueryableAsync();
            var children = await repository.AsyncExecuter.ToListAsync(
                query.Where(e => e.ParentId == entityId));

            foreach (var child in children)
            {
                // 获取子节点编码（TreeCode最后一部分）或生成新编码
                string childCode = child.TreeCode.Contains('.')
                    ? child.TreeCode.Split('.').Last()
                    : child.TreeCode;

                child.TreeCode = $"{entity.TreeCode}.{childCode}";
                await repository.UpdateAsync(child);

                // 递归更新子节点
                await UpdateTreeCodesAsync(repository, child.Id);
            }
        }
    }
}