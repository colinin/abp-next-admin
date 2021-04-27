using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.ProjectManagement.Projects
{
    public interface IProjectLogRepository : IRepository<ProductBuildLog, int>
    {
        Task<List<ProductBuildLog>> GetListAsync(
            Guid projectId,
            LogLevel? level = null);
    }
}
