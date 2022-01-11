using System.Threading.Tasks;
using Volo.Abp.Emailing;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class SendEmailJob : IJobRunnable
{
    public const string PropertyFrom = "from";
    public const string PropertyTo = "to";
    public const string PropertySubject = "subject";
    public const string PropertyBody = "body";
    public const string PropertyIsBodyHtml = "isBodyHtml";

    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        context.TryGetString(PropertyFrom, out var from);
        var to = context.GetString(PropertyTo);
        var subject = context.GetString(PropertySubject);
        var body = context.GetString(PropertyBody);
        context.TryGetJobData<bool>(PropertyIsBodyHtml, out var isBodyHtml);

        var emailSender = context.GetRequiredService<IEmailSender>();

        await emailSender.QueueAsync(from, to, subject, body, isBodyHtml);
    }
}
