using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Datas
{
    public interface IDataRepository : IBasicRepository<Data, Guid>
    {
        Task<Data> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        Task<List<Data>> GetChildrenAsync(
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<int> GetCountAsync(
            string filter = "",
            CancellationToken cancellationToken = default);

        Task<List<Data>> GetPagedListAsync(
            string filter = "",
            string sorting = nameof(Data.Code),
            bool includeDetails = false,
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
    }
}
