using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    public static class PlatformEfCoreQueryableExtensions
    {

        public static IQueryable<Layout> IncludeDetails(this IQueryable<Layout> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable;
        }

        public static IQueryable<Menu> IncludeDetails(this IQueryable<Menu> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable;
        }

        public static IQueryable<Data> IncludeDetails(this IQueryable<Data> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .AsSplitQuery()
                .Include(x => x.Items);
        }
    }
}
