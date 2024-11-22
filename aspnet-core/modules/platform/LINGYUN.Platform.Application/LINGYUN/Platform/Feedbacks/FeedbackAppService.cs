using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Feedbacks;

[Authorize(PlatformPermissions.Feedback.Default)]
public class FeedbackAppService : PlatformApplicationServiceBase, IFeedbackAppService
{
    private readonly FeedbackAttachmentManager _attachmentManager;
    private readonly IFeedbackRepository _feedbackRepository;

    public FeedbackAppService(
        FeedbackAttachmentManager attachmentManager, 
        IFeedbackRepository feedbackRepository)
    {
        _attachmentManager = attachmentManager;
        _feedbackRepository = feedbackRepository;
    }

    [Authorize(PlatformPermissions.Feedback.Create)]
    public async virtual Task<FeedbackDto> CreateAsync(FeedbackCreateDto input)
    {
        var feedback = new Feedback(
            GuidGenerator.Create(),
            input.Category,
            input.Content,
            FeedbackStatus.Created,
            CurrentTenant.Id);

        if (input.Attachments != null)
        {
            foreach (var attachment in input.Attachments)
            {
                var attachmentFile = await _attachmentManager.CopyFromTempAsync(
                    feedback,
                    attachment.Path,
                    attachment.Id);

                feedback.AddAttachment(
                    GuidGenerator,
                    attachmentFile.Name,
                    $"/api/platform/feedbacks/{feedback.Id}/attachments/{attachmentFile.Name}",
                    attachmentFile.Size);
            }
        }

        feedback = await _feedbackRepository.InsertAsync(feedback);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<Feedback, FeedbackDto>(feedback);
    }

    [Authorize(PlatformPermissions.Feedback.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var feedback = await _feedbackRepository.GetAsync(id);

        await _feedbackRepository.DeleteAsync(feedback);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(PlatformPermissions.Feedback.Default)]
    public async virtual Task<FeedbackDto> GetAsync(Guid id)
    {
        var feedback = await _feedbackRepository.GetAsync(id);

        return ObjectMapper.Map<Feedback, FeedbackDto>(feedback);
    }

    public async virtual Task<PagedResultDto<FeedbackDto>> GetListAsync(FeedbackGetListInput input)
    {
        var specification = new FeedbackGetListSpecification(input);

        var totalCount = await _feedbackRepository.GetCountAsync(specification);
        var feedbacks = await _feedbackRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<FeedbackDto>(totalCount,
            ObjectMapper.Map<List<Feedback>, List<FeedbackDto>>(feedbacks));
    }

    internal class FeedbackGetListSpecification : Volo.Abp.Specifications.Specification<Feedback>
    {
        protected FeedbackGetListInput Input { get; }
        public FeedbackGetListSpecification(FeedbackGetListInput input)
        {
            Input = input;
        }
        public override Expression<Func<Feedback, bool>> ToExpression()
        {
            Expression<Func<Feedback, bool>> expression = _ => true;

            return expression
                .AndIf(Input.Status.HasValue, x => x.Status == Input.Status)
                .AndIf(!Input.Category.IsNullOrWhiteSpace(), x => x.Category == Input.Category)
                .AndIf(!Input.Filter.IsNullOrWhiteSpace(), x => x.Category.Contains(Input.Filter) ||
                    x.Content.Contains(Input.Filter));
        }
    }
}
