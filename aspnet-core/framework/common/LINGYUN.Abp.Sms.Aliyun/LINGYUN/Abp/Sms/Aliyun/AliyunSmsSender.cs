﻿using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using LINGYUN.Abp.Aliyun;
using LINGYUN.Abp.Aliyun.Features;
using LINGYUN.Abp.Aliyun.Settings;
using LINGYUN.Abp.Features.LimitValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Json;
using Volo.Abp.Settings;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Sms.Aliyun
{
    [Dependency(ServiceLifetime.Singleton)]
    [ExposeServices(typeof(ISmsSender), typeof(AliyunSmsSender))]
    [RequiresFeature(AliyunFeatureNames.Sms.Enable)]
    public class AliyunSmsSender : ISmsSender
    {
        protected IJsonSerializer JsonSerializer { get; }
        protected ISettingProvider SettingProvider { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected IAcsClientFactory AcsClientFactory { get; }
        public AliyunSmsSender(
            IJsonSerializer jsonSerializer,
            ISettingProvider settingProvider,
            IServiceProvider serviceProvider,
            IAcsClientFactory acsClientFactory)
        {
            JsonSerializer = jsonSerializer;
            SettingProvider = settingProvider;
            ServiceProvider = serviceProvider;
            AcsClientFactory = acsClientFactory;
        }

        [RequiresLimitFeature(
            AliyunFeatureNames.Sms.SendLimit,
            AliyunFeatureNames.Sms.SendLimitInterval,
            LimitPolicy.Month,
            AliyunFeatureNames.Sms.DefaultSendLimit,
            AliyunFeatureNames.Sms.DefaultSendLimitInterval)]
        public async virtual Task SendAsync(SmsMessage smsMessage)
        {
            var domain = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.Domain);
            var action = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.ActionName);
            var version = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.Version);

            Check.NotNullOrWhiteSpace(domain, AliyunSettingNames.Sms.Domain);
            Check.NotNullOrWhiteSpace(action, AliyunSettingNames.Sms.ActionName);
            Check.NotNullOrWhiteSpace(version, AliyunSettingNames.Sms.Version);

            CommonRequest request = new CommonRequest
            {
                Method = MethodType.POST,
                Domain = domain,
                Action = action,
                Version = version
            };
            await TryAddTemplateCodeAsync(request, smsMessage);
            await TryAddSignNameAsync(request, smsMessage);
            await TryAddSendPhoneAsync(request, smsMessage);

            TryAddTemplateParam(request, smsMessage);

            try
            {
                var client = await AcsClientFactory.CreateAsync();
                CommonResponse response = client.GetCommonResponse(request);
                var responseContent = Encoding.Default.GetString(response.HttpResponse.Content);
                var aliyunResponse = JsonSerializer.Deserialize<AliyunSmsResponse>(responseContent);
                if (!aliyunResponse.IsSuccess())
                {
                    if (await SettingProvider.IsTrueAsync(AliyunSettingNames.Sms.VisableErrorToClient))
                    {
                        throw new UserFriendlyException(aliyunResponse.Code, aliyunResponse.Message);
                    }
                    throw new AliyunSmsException(aliyunResponse.Code, $"Text message sending failed, code:{aliyunResponse.Code}, message:{aliyunResponse.Message}!");
                }
            }
            catch(ServerException se)
            {
                throw new AliyunSmsException(se.ErrorCode, $"Sending text messages to aliyun server is abnormal,type: {se.ErrorType}, error: {se.ErrorMessage}");
            }
            catch(ClientException ce)
            {
                throw new AliyunSmsException(ce.ErrorCode, $"A client exception occurred in sending SMS messages,type: {ce.ErrorType}, error: {ce.ErrorMessage}");
            }
        }

        private async Task TryAddTemplateCodeAsync(CommonRequest request, SmsMessage smsMessage)
        {
            if (smsMessage.Properties.TryGetValue("TemplateCode", out object template) && template != null)
            {
                request.AddQueryParameters("TemplateCode", template.ToString());
                smsMessage.Properties.Remove("TemplateCode");
            }
            else
            {
                var defaultTemplateCode = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.DefaultTemplateCode);
                Check.NotNullOrWhiteSpace(defaultTemplateCode, "TemplateCode");
                request.AddQueryParameters("TemplateCode", defaultTemplateCode);
            }
        }

        private async Task TryAddSignNameAsync(CommonRequest request, SmsMessage smsMessage)
        {
            if (smsMessage.Properties.TryGetValue("SignName", out object signName) && signName != null)
            {
                request.AddQueryParameters("SignName", signName.ToString());
                smsMessage.Properties.Remove("SignName");
            }
            else
            {
                var defaultSignName = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.DefaultSignName);
                Check.NotNullOrWhiteSpace(defaultSignName, "SignName");
                request.AddQueryParameters("SignName", defaultSignName);
            }
        }

        private async Task TryAddSendPhoneAsync(CommonRequest request, SmsMessage smsMessage)
        {
            if (smsMessage.PhoneNumber.IsNullOrWhiteSpace())
            {
                var defaultPhoneNumber = await SettingProvider.GetOrNullAsync(AliyunSettingNames.Sms.DefaultPhoneNumber);
                // check phone number length...
                Check.NotNullOrWhiteSpace(
                    defaultPhoneNumber,
                    AliyunSettingNames.Sms.DefaultPhoneNumber, 
                    maxLength: 11, minLength: 11);
                request.AddQueryParameters("PhoneNumbers", defaultPhoneNumber);
            }
            else
            {
                request.AddQueryParameters("PhoneNumbers", smsMessage.PhoneNumber);
            }
        }

        private void TryAddTemplateParam(CommonRequest request, SmsMessage smsMessage)
        {
            if (smsMessage.Properties.Any())
            {
                var queryParamJson = JsonSerializer.Serialize(smsMessage.Properties);
                request.AddQueryParameters("TemplateParam", queryParamJson);
            }
        }
    }
}
