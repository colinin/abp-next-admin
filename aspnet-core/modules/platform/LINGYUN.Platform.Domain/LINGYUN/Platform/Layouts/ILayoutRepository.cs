using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Layouts;

public interface ILayoutRepository : IBasicRepository<Layout, Guid>
{
    /// <summary>
    /// 根据名称查询布局
    /// </summary>
    /// <param name="name"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Layout> FindByNameAsync(
        string name,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        string framework = "",
        string filter = "",
        CancellationToken cancellationToken = default);

    Task<List<Layout>> GetPagedListAsync(
        string framework = "",
        string filter = "",
        string sorting = nameof(Layout.Name),
        bool includeDetails = false,
        int skipCount = 0,
        int maxResultCount = 10,
        CancellationToken cancellationToken = default);
}
