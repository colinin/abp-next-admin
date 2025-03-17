using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Xunit;

namespace LINGYUN.Abp.Emailing.Platform;
public class PlatformEmailingSender_Tests : AbpEmailingPlatformTestsBase
{
    private readonly IEmailSender _emailSender;

    public PlatformEmailingSender_Tests()
    {
        _emailSender = GetRequiredService<IEmailSender>();
    }

    [Fact]
    public async Task ShouldSendMailMessageAsync()
    {
        var mailMessage = new MailMessage("from_mail_address@asd.com", "to_mail_address@asd.com", "subject", "body")
        { 
            IsBodyHtml = true
        };

        await _emailSender.SendAsync(mailMessage);
    }

    [Fact]
    public async Task ShouldSendMailMessage()
    {
        var mailMessage = new MailMessage("from_mail_address@asd.com", "to_mail_address@asd.com", "subject", "body")
        {
            IsBodyHtml = true
        };

        await _emailSender.SendAsync(mailMessage);
    }
}
