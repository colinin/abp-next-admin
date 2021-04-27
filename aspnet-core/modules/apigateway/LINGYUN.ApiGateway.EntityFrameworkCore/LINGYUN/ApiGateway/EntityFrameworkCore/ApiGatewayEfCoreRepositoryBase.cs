using System.Linq;
using System.Threading.Tasks;
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

        public override async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            var query = await base.GetQueryableAsync();
            query = base.ApplyDataFilters(query);
            return query;
        }

        /// <summary>
        /// 重写查询,添加过滤器
        /// </summary>
        /// <returns></returns>
        [System.Obsolete("将在abp框架移除之后删除")]
        protected override IQueryable<TEntity> GetQueryable()
        {
            var query = base.GetQueryable();
            query = base.ApplyDataFilters(query);
            return query;
        }
    }
}
