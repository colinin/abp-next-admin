using LINGYUN.Abp.Sms.Tencent.Settings;
using LINGYUN.Abp.Tencent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Settings;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Sms.Tencent
{
    [Dependency(ReplaceServices = true)]
    public class TencentCloudSmsSender : ISmsSender, ITransientDependency
    {
        public ILogger<TencentCloudSmsSender> Logger { protected get; set; }

        protected IJsonSerializer JsonSerializer { get; }
        protected ISettingProvider SettingProvider { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected TencentCloudClientFactory<SmsClient> TencentCloudClientFactory { get; }
        public TencentCloudSmsSender(
            IJsonSerializer jsonSerializer,
            ISettingProvider settingProvider,
            IServiceProvider serviceProvider,
            TencentCloudClientFactory<SmsClient> tencentCloudClientFactory)
        {
            JsonSerializer = jsonSerializer;
            SettingProvider = settingProvider;
            ServiceProvider = serviceProvider;
            TencentCloudClientFactory = tencentCloudClientFactory;

            Logger = NullLogger<TencentCloudSmsSender>.Instance;
        }

        public virtual async Task SendAsync(SmsMessage smsMessage)
        {
            var appId = await SettingProvider.GetOrNullAsync(TencentCloudSmsSettingNames.AppId);

            Check.NotNullOrWhiteSpace(appId, TencentCloudSmsSettingNames.AppId);

            // 统一使用 TemplateCode作为模板参数, 解决不一样的sms提供商参数差异
            if (!smsMessage.Properties.TryGetValue("TemplateCode", out var templateId))
            {
                templateId = await SettingProvider.GetOrNullAsync(TencentCloudSmsSettingNames.DefaultTemplateId);
            }

            if (!smsMessage.Properties.TryGetValue("SignName", out var signName))
            {
                signName = await SettingProvider.GetOrNullAsync(TencentCloudSmsSettingNames.DefaultSignName);
            }

            if (!smsMessage.Properties.TryGetValue("TemplateParam", out var templateParam))
            {
                templateParam = await SettingProvider.GetOrNullAsync(TencentCloudSmsSettingNames.DefaultSignName);
            }

            var smsClient = await TencentCloudClientFactory.CreateAsync();

            var request = new SendSmsRequest
            {
                SmsSdkAppId = appId,
                SignName = signName?.ToString(),
                TemplateId = templateId?.ToString(),
                PhoneNumberSet = smsMessage.PhoneNumber.Split(';'),
                TemplateParamSet = templateParam?.ToString()?.Split(';'),
            };

            var response = await smsClient.SendSms(request);

            var warningMessage = response.SendStatusSet
                    .Where(x => !"ok".Equals(x.Code, StringComparison.InvariantCultureIgnoreCase))
                    // 记录流水号, 手机号, 错误代码, 错误信息
                    .Select(x => $"SerialNo: {x.SerialNo}, PhoneNumber: {x.PhoneNumber}, Status: {x.Code}, Error: {x.Message}");
            // 所有短信发送失败, 抛出错误
            // 只要一条发送成功，即视为成功
            if (!response.SendStatusSet.Any(x => "ok".Equals(x.Code, StringComparison.InvariantCultureIgnoreCase)))
            {
                var errorMessage = warningMessage.JoinAsString(Environment.NewLine);

                throw new AbpException($"Send tencent cloud sms error: {errorMessage}");
            }

            if (warningMessage.Any())
            {
                Logger.LogWarning("Some failed send sms messages, error info:");
                Logger.LogWarning(warningMessage.JoinAsString(Environment.NewLine));
            }
        }
    }
}
