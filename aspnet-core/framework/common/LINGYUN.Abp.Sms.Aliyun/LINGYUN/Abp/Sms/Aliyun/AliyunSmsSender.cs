using AlibabaCloud.SDK.Dypnsapi20170525.Models;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using LINGYUN.Abp.Aliyun.Features;
using LINGYUN.Abp.Aliyun.Settings;
using LINGYUN.Abp.Features.LimitValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Json;
using Volo.Abp.Settings;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Sms.Aliyun;

[Dependency(ServiceLifetime.Singleton)]
[ExposeServices(
    typeof(ISmsSender), 
    typeof(IAliyunSmsVerifyCodeSender),
    typeof(AliyunSmsSender))]
[RequiresFeature(AliyunFeatureNames.Sms.Enable)]
public class AliyunSmsSender : ISmsSender, IAliyunSmsVerifyCodeSender
{
    protected IJsonSerializer JsonSerializer { get; }
    protected ISettingProvider SettingProvider { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected IDypnsClientFactory DypnsClientFactory { get; }
    protected IDysmsClientFactory DysmsClientFactory { get; }
    public AliyunSmsSender(
        IJsonSerializer jsonSerializer,
        ISettingProvider settingProvider,
        IServiceProvider serviceProvider,
        IDypnsClientFactory dypnsClientFactory,
        IDysmsClientFactory dysmsClientFactory)
    {
        JsonSerializer = jsonSerializer;
        SettingProvider = settingProvider;
        ServiceProvider = serviceProvider;
        DypnsClientFactory = dypnsClientFactory;
        DysmsClientFactory = dysmsClientFactory;
    }

    [RequiresLimitFeature(
        AliyunFeatureNames.Sms.SendLimit,
        AliyunFeatureNames.Sms.SendLimitInterval,
        LimitPolicy.Month,
        AliyunFeatureNames.Sms.DefaultSendLimit,
        AliyunFeatureNames.Sms.DefaultSendLimitInterval)]
    public async virtual Task SendAsync(SmsMessage smsMessage)
    {
        if (smsMessage.Properties.ContainsKey("SmsVerifyCode") &&
            smsMessage.Properties.TryGetValue("code", out var code))
        {
            smsMessage.Properties.TryGetValue("SignName", out var signName);
            smsMessage.Properties.TryGetValue("TemplateCode", out var templateCode);
            // 调用短信验证码服务
            await SendAsync(
                new SmsVerifyCodeMessage(
                    smsMessage.PhoneNumber,
                    new SmsVerifyCodeMessageParam(code.ToString(), "5"),
                    signName?.ToString(),
                    templateCode?.ToString()));
            return;
        }

        var domain = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.Domain);
        var action = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.ActionName);
        var version = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.Version);

        Check.NotNullOrWhiteSpace(domain, AliyunSettingNames.Sms.Domain);
        Check.NotNullOrWhiteSpace(action, AliyunSettingNames.Sms.ActionName);
        Check.NotNullOrWhiteSpace(version, AliyunSettingNames.Sms.Version);

        var request = new SendSmsRequest();

        await TryAddTemplateCodeAsync(request, smsMessage);
        await TryAddSignNameAsync(request, smsMessage);
        await TryAddSendPhoneAsync(request, smsMessage);
        TryAddTemplateParam(request, smsMessage);


        var dysmsClient = await DysmsClientFactory.CreateAsync();
        var response = await dysmsClient.SendSmsAsync(request);
        if (!string.Equals(response.Body.Code, "OK", StringComparison.CurrentCultureIgnoreCase))
        {
            if (await SettingProvider.IsTrueAsync(AliyunSettingNames.Sms.VisableErrorToClient))
            {
                throw new UserFriendlyException(response.Body.Code, response.Body.Message);
            }
            throw new AliyunSmsException(response.Body.Code, $"Text message sending failed, code:{response.Body.Code}, message:{response.Body.Message}!");
        }
    }

    [RequiresLimitFeature(
        AliyunFeatureNames.Sms.SendLimit,
        AliyunFeatureNames.Sms.SendLimitInterval,
        LimitPolicy.Month,
        AliyunFeatureNames.Sms.DefaultSendLimit,
        AliyunFeatureNames.Sms.DefaultSendLimitInterval)]
    public async virtual Task SendAsync(SmsVerifyCodeMessage message)
    {
        var version = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.Version);
        var domain = await SettingProvider.GetOrNullAsync(AliyunSettingNames.SmsVerifyCode.Domain);
        var signName = message.SignName ??
            await SettingProvider.GetOrNullAsync(AliyunSettingNames.SmsVerifyCode.DefaultSignName);
        var templateCode = message.TemplateCode ?? 
            await SettingProvider.GetOrNullAsync(AliyunSettingNames.SmsVerifyCode.DefaultTemplateCode);

        Check.NotNullOrWhiteSpace(version, AliyunSettingNames.Sms.Version);
        Check.NotNullOrWhiteSpace(domain, AliyunSettingNames.SmsVerifyCode.Domain);
        Check.NotNullOrWhiteSpace(signName, AliyunSettingNames.SmsVerifyCode.DefaultSignName);
        Check.NotNullOrWhiteSpace(templateCode, AliyunSettingNames.SmsVerifyCode.DefaultTemplateCode);

        var dypnsClient = await DypnsClientFactory.CreateAsync();

        var request = new SendSmsVerifyCodeRequest
        {
            PhoneNumber = message.PhoneNumber,
            SignName = signName,
            TemplateCode = templateCode,
            TemplateParam = JsonSerializer.Serialize(message.TemplateParam),
        };

        var response = await dypnsClient.SendSmsVerifyCodeAsync(request);
        if (response.Body.Success == false)
        {
            if (await SettingProvider.IsTrueAsync(AliyunSettingNames.Sms.VisableErrorToClient))
            {
                throw new UserFriendlyException(response.Body.Code, response.Body.Message);
            }
            throw new AliyunSmsException(response.Body.Code, $"Text message sending failed, code:{response.Body.Code}, message:{response.Body.Message}!");
        }
    }

    private async Task TryAddTemplateCodeAsync(SendSmsRequest request, SmsMessage smsMessage)
    {
        if (smsMessage.Properties.TryGetValue("TemplateCode", out var template) && template != null)
        {
            request.TemplateCode = template.ToString();
            smsMessage.Properties.Remove("TemplateCode");
        }
        else
        {
            var defaultTemplateCode = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.DefaultTemplateCode);
            Check.NotNullOrWhiteSpace(defaultTemplateCode, "TemplateCode");
            request.TemplateCode = defaultTemplateCode;
        }
    }

    private async Task TryAddSignNameAsync(SendSmsRequest request, SmsMessage smsMessage)
    {
        if (smsMessage.Properties.TryGetValue("SignName", out var signName) && signName != null)
        {
            request.SignName = signName.ToString();
            smsMessage.Properties.Remove("SignName");
        }
        else
        {
            var defaultSignName = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.DefaultSignName);
            Check.NotNullOrWhiteSpace(defaultSignName, "SignName");
            request.SignName = defaultSignName;
        }
    }

    private async Task TryAddSendPhoneAsync(SendSmsRequest request, SmsMessage smsMessage)
    {
        if (smsMessage.PhoneNumber.IsNullOrWhiteSpace())
        {
            var defaultPhoneNumber = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.DefaultPhoneNumber);
            // check phone number length...
            Check.NotNullOrWhiteSpace(
                defaultPhoneNumber,
                AliyunSettingNames.Sms.DefaultPhoneNumber, 
                maxLength: 11, minLength: 11);
            request.PhoneNumbers = defaultPhoneNumber;
        }
        else
        {
            request.PhoneNumbers = smsMessage.PhoneNumber;
        }
    }

    private void TryAddTemplateParam(SendSmsRequest request, SmsMessage smsMessage)
    {
        if (smsMessage.Properties.Any())
        {
            var queryParamJson = JsonSerializer.Serialize(smsMessage.Properties);
            request.TemplateParam = queryParamJson;
        }
    }
}
