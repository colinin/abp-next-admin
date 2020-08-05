using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace LINGYUN.ApiGateway.EntityFrameworkCore
{
    public class ApiGatewayEfCoreRepositoryBase<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity, TKey>
        where TDbContext : AbpDbContext<TDbContext>
        where TEntity : class, IEntity<TKey>
    {
        public ICurrentUser CurrentUser { get; set; }

        public ApiGatewayEfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
        /// <summary>
        /// 重写查询,添加过滤器
        /// </summary>
        /// <returns></returns>
        protected override IQueryable<TEntity> GetQueryable()
        {
            var query = base.GetQueryable();
            query = base.ApplyDataFilters(query);
            return query;
        }
    }
}
