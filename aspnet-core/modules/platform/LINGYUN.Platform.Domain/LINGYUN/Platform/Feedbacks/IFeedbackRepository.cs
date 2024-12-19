using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Platform.Feedbacks;
public interface IFeedbackRepository : IBasicRepository<Feedback, Guid>
{
    Task<int> GetCountAsync(
        ISpecification<Feedback> specification,
        CancellationToken cancellationToken = default);

    Task<List<Feedback>> GetListAsync(
        ISpecification<Feedback> specification,
        string sorting = $"{nameof(Feedback.CreationTime)} DESC",
        int maxResultCount = 25,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
