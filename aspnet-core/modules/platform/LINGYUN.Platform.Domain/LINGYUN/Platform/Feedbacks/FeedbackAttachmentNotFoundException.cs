using Volo.Abp;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackAttachmentNotFoundException : BusinessException
{
    public string Name { get; protected set; }
    public FeedbackAttachmentNotFoundException(string name)
        : base(
            PlatformErrorCodes.FeedackAttachmentNotFound, 
            $"User feedback: Attachment named {name} not found!")
    {
        Name = name;

        WithData("Name", name);
    }
}
