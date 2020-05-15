using LINGYUN.ApiGateway.Data.Filter;
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
            query = ApplyDataFilters(query);
            return query;
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <typeparam name="TQueryable"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        protected override TQueryable ApplyDataFilters<TQueryable>(TQueryable query)
        {
            query = base.ApplyDataFilters(query);

            if (typeof(IActivation).IsAssignableFrom(typeof(TEntity)))
            {
                var enabledActivation = DataFilter.IsEnabled<IActivation>();
                if (enabledActivation)
                {
                    query = (TQueryable)query.Where(e => ((IActivation)e).IsActive);
                }
            }

            return query;
        }
    }
}
