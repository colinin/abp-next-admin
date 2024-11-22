using LINGYUN.Platform.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackStatusException : BusinessException, ILocalizeErrorMessage
{
    public FeedbackStatus Status { get; protected set; }
    public FeedbackStatusException(string code, FeedbackStatus status)
        : base(code, $"Unable to comment on issues in {status} status")
    {
        Status = status;

        WithData("Status", status.ToString());
    }

    public string LocalizeMessage(LocalizationContext context)
    {
        var localizer = context.LocalizerFactory.Create<PlatformResource>();

        return localizer[PlatformErrorCodes.UnableFeedbackCommentInStatus, localizer[$"FeedbackStatus:{Status}"]];
    }
}
