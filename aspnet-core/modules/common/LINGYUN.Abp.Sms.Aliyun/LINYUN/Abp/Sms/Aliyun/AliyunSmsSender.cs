using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
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
using Volo.Abp.Localization;
using Volo.Abp.Sms;

namespace LINYUN.Abp.Sms.Aliyun
{
    [Dependency(ServiceLifetime.Singleton)]
    [ExposeServices(typeof(ISmsSender), typeof(AliyunSmsSender))]
    public class AliyunSmsSender : ISmsSender
    {
        protected AliyunSmsOptions Options { get; }
        protected IJsonSerializer JsonSerializer { get; }
        protected IHostEnvironment Environment { get; }
        protected IServiceProvider ServiceProvider { get; }
        public AliyunSmsSender(
            IHostEnvironment environment,
            IJsonSerializer jsonSerializer,
            IServiceProvider serviceProvider,
            IOptions<AliyunSmsOptions> options)
        {
            Options = options.Value;
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
                Version = Options.DefaultVersion
            };
            if (smsMessage.Properties.TryGetValue("TemplateCode", out object template))
            {
                request.AddQueryParameters("TemplateCode", template.ToString());
                smsMessage.Properties.Remove("TemplateCode");
            }
            else
            {
                Check.NotNullOrWhiteSpace(Options.DefaultTemplateCode, nameof(Options.DefaultTemplateCode));
                request.AddQueryParameters("TemplateCode", Options.DefaultTemplateCode);
            }

            if (smsMessage.Properties.TryGetValue("SignName", out object signName))
            {
                request.AddQueryParameters("SignName", signName.ToString());
                smsMessage.Properties.Remove("SignName");
            }
            else
            {
                Check.NotNullOrWhiteSpace(Options.DefaultSignName, nameof(Options.DefaultSignName));
                request.AddQueryParameters("SignName", Options.DefaultSignName);
            }
            if (Environment.IsDevelopment())
            {
                Check.NotNullOrWhiteSpace(Options.DeveloperPhoneNumber, nameof(Options.DeveloperPhoneNumber));
                request.AddQueryParameters("PhoneNumbers", Options.DeveloperPhoneNumber);
            }
            else
            {
                request.AddQueryParameters("PhoneNumbers", smsMessage.PhoneNumber);
            }

            var queryParamJson = JsonSerializer.Serialize(smsMessage.Properties);
            request.AddQueryParameters("TemplateParam", queryParamJson);
            try
            {
                IClientProfile profile = DefaultProfile.GetProfile(Options.RegionId, Options.AccessKeyId, Options.AccessKeySecret);
                IAcsClient client = new DefaultAcsClient(profile);
                CommonResponse response = client.GetCommonResponse(request);
                var responseContent = Encoding.Default.GetString(response.HttpResponse.Content);
                var aliyunResponse = JsonSerializer.Deserialize<AliyunSmsResponse>(responseContent);
                if (!aliyunResponse.IsSuccess())
                {
                    var localizerFactory = ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
                    var localizerError = aliyunResponse.GetErrorMessage().Localize(localizerFactory);
                    if (Options.VisableErrorToClient)
                    {
                        var localizer = ServiceProvider.GetRequiredService<IStringLocalizer<AliyunSmsResource>>();
                        localizerError = localizer["SendMessageFailed", localizerError];
                        throw new UserFriendlyException(localizerError);
                    }
                    throw new AbpException($"Text message sending failed:{localizerError}!");
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
    }
}
