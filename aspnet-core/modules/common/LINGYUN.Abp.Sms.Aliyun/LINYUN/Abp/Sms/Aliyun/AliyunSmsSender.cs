using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using LINGYUN.Abp.Aliyun.Authorization;
using LINYUN.Abp.Sms.Aliyun.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Sms;

namespace LINYUN.Abp.Sms.Aliyun
{
    [Dependency(ServiceLifetime.Singleton)]
    [ExposeServices(typeof(ISmsSender), typeof(AliyunSmsSender))]
    public class AliyunSmsSender : ISmsSender
    {
        protected AbpAliyunOptions AuthOptions { get; }
        protected AliyunSmsOptions Options { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected IHostEnvironment Environment { get; }
        protected IServiceProvider ServiceProvider { get; }
        public AliyunSmsSender(
            IHostEnvironment environment,
            IJsonSerializer jsonSerializer,
            IServiceProvider serviceProvider,
            IOptions<AliyunSmsOptions> options,
            IOptions<AbpAliyunOptions> authOptions)
        {
            Options = options.Value;
            AuthOptions = authOptions.Value;

            Environment = environment;
            JsonSerializer = jsonSerializer;
            ServiceProvider = serviceProvider;
        }

        public Task SendAsync(SmsMessage smsMessage)
        {
            CommonRequest request = new CommonRequest
            {
                Method = MethodType.POST,
                Domain = Options.Domain,
                Action = Options.ActionName,
                Version = Options.Version
            };
            TryAddTemplateCode(request, smsMessage);
            TryAddSignName(request, smsMessage);
            TryAddSendPhone(request, smsMessage);
            TryAddTemplateParam(request, smsMessage);

            try
            {
                IClientProfile profile = DefaultProfile.GetProfile(Options.RegionId, AuthOptions.AccessKeyId, AuthOptions.AccessKeySecret);
                IAcsClient client = new DefaultAcsClient(profile);
                CommonResponse response = client.GetCommonResponse(request);
                var responseContent = Encoding.Default.GetString(response.HttpResponse.Content);
                var aliyunResponse = JsonSerializer.Deserialize<AliyunSmsResponse>(responseContent);
                if (!aliyunResponse.IsSuccess())
                {
                    if (Options.VisableErrorToClient)
                    {
                        throw new AliyunSmsException(aliyunResponse.Code, aliyunResponse.Message);
                    }
                    throw new AbpException($"Text message sending failed, code:{aliyunResponse.Code}, message:{aliyunResponse.Message}!");
                }
            }
            catch(ServerException se)
            {
                throw new AbpException("Sending text messages to aliyun server is abnormal", se);
            }
            catch(ClientException ce)
            {
                throw new AbpException("A client exception occurred in sending SMS messages", ce);
            }

            return Task.CompletedTask;
        }

        private void TryAddTemplateCode(CommonRequest request, SmsMessage smsMessage)
        {
            if (smsMessage.Properties.TryGetValue("TemplateCode", out object template) && template != null)
            {
                request.AddQueryParameters("TemplateCode", template.ToString());
                smsMessage.Properties.Remove("TemplateCode");
            }
            else
            {
                Check.NotNullOrWhiteSpace(Options.DefaultTemplateCode, nameof(Options.DefaultTemplateCode));
                request.AddQueryParameters("TemplateCode", Options.DefaultTemplateCode);
            }
        }

        private void TryAddSignName(CommonRequest request, SmsMessage smsMessage)
        {
            if (smsMessage.Properties.TryGetValue("SignName", out object signName) && signName != null)
            {
                request.AddQueryParameters("SignName", signName.ToString());
                smsMessage.Properties.Remove("SignName");
            }
            else
            {
                Check.NotNullOrWhiteSpace(Options.DefaultSignName, nameof(Options.DefaultSignName));
                request.AddQueryParameters("SignName", Options.DefaultSignName);
            }
        }

        private void TryAddSendPhone(CommonRequest request, SmsMessage smsMessage)
        {
            if (Environment.IsDevelopment())
            {
                // check phone number length...
                Check.NotNullOrWhiteSpace(
                    Options.DeveloperPhoneNumber,
                    nameof(Options.DeveloperPhoneNumber), 
                    maxLength: 11, minLength: 11);
                request.AddQueryParameters("PhoneNumbers", Options.DeveloperPhoneNumber);
            }
            else
            {
                request.AddQueryParameters("PhoneNumbers", smsMessage.PhoneNumber);
            }
        }

        private void TryAddTemplateParam(CommonRequest request, SmsMessage smsMessage)
        {
            if (smsMessage.Properties.Count > 0)
            {
                var queryParamJson = JsonSerializer.Serialize(smsMessage.Properties);
                request.AddQueryParameters("TemplateParam", queryParamJson);
            }
        }
    }
}
