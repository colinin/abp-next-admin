using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.DataProtectionManagement;

public interface ISubjectStrategyRepository : IBasicRepository<SubjectStrategy, Guid>
{
    Task<SubjectStrategy> FindBySubjectAsync(
        string subjectName,
        string subjectId,
        CancellationToken cancellationToken = default);
}
