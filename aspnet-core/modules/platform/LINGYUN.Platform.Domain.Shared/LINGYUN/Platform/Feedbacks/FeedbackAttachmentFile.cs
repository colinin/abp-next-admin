namespace LINGYUN.Platform.Feedbacks;
public class FeedbackAttachmentFile(string name, long size)
{
    public string Name { get; } = name;
    public long Size { get; } = size;
}
