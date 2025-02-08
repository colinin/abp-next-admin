using System.Threading.Tasks;
using Volo.Abp.Sms;
using Xunit;

namespace LINGYUN.Abp.Sms.Platform;
public class PlatformSmsSender_Tests : AbpSmsPlatformTestsBase
{
    private readonly ISmsSender _smsSender;

    public PlatformSmsSender_Tests()
    {
        _smsSender = GetRequiredService<ISmsSender>();
    }

    [Fact]
    public async Task SendSms_Test()
    {
        var msg = new SmsMessage("13800138000", "Platform Sms Sender Test");
        await _smsSender.SendAsync(msg);
    }
}
