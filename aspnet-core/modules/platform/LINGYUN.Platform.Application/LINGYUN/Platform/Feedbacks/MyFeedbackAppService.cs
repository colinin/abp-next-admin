using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;
using static LINGYUN.Platform.Feedbacks.FeedbackAppService;

namespace LINGYUN.Platform.Feedbacks;

[Authorize]
public class MyFeedbackAppService : PlatformApplicationServiceBase, IMyFeedbackAppService
{
    private readonly IFeedbackRepository _feedbackRepository;
    public MyFeedbackAppService(IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public async virtual Task<PagedResultDto<FeedbackDto>> GetMyFeedbacksAsync(FeedbackGetListInput input)
    {
        var specification = new FeedbackGetListByUserSpecification(CurrentUser.GetId(), input);

        var totalCount = await _feedbackRepository.GetCountAsync(specification);
        var feedbacks = await _feedbackRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<FeedbackDto>(totalCount,
            ObjectMapper.Map<List<Feedback>, List<FeedbackDto>>(feedbacks));
    }

    private class FeedbackGetListByUserSpecification : FeedbackGetListSpecification
    {
        protected Guid UserId { get; }
        public FeedbackGetListByUserSpecification(
            Guid userId,
            FeedbackGetListInput input)
            : base(input)
        {
            UserId = userId;
        }

        public override Expression<Func<Feedback, bool>> ToExpression()
        {
            var expression = base.ToExpression();

            return expression.And(x => x.CreatorId == UserId);
        }
    }
}
