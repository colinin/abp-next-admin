using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;
using Xunit;

namespace LINGYUN.Abp.ExceptionHandling.Emailing
{
    public class AbpEmailingExceptionSubscriber_Test : AbpExceptionHandlingEmailingTestBase
    {
        private readonly IExceptionNotifier _notifier;
        public AbpEmailingExceptionSubscriber_Test()
        {
            _notifier = GetRequiredService<IExceptionNotifier>();
        }

        [Fact]
        public async Task Send_Error_Notifier_Test()
        {
            try
            {
                int x = 10;
                int y = 0;
                int zeroDiv = x / y;
            }
            catch(Exception ex)
            {
                await _notifier.NotifyAsync(
                new ExceptionNotificationContext(
                    new TestSendEmailException(
                        "Test exception notufy with en", ex), LogLevel.Warning));
                using (CultureHelper.Use("zh-Hans"))
                {
                    await _notifier.NotifyAsync(
                        new ExceptionNotificationContext(
                            new TestSendEmailException(
                                "测试中文异常模板推送", ex), LogLevel.Warning));
                }
            }
        }
    }
}
