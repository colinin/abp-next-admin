using LINGYUN.Platform.Messages;
using LINGYUN.Platform.Messages.Integration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Sms.Platform;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
public class PlatformSmsSender : ISmsSender
{
    private readonly ISmsMessageIntegrationService _service;
    public PlatformSmsSender(ISmsMessageIntegrationService service)
    {
        _service = service;
    }

    public async virtual Task SendAsync(SmsMessage smsMessage)
    {
        var createInput = new SmsMessageCreateDto(
            smsMessage.PhoneNumber,
            smsMessage.Text);

        if (smsMessage.Properties != null)
        {
            createInput.ExtraProperties = new ExtraPropertyDictionary();
            foreach (var property in smsMessage.Properties)
            {
                createInput.ExtraProperties[property.Key] = property.Value;
            }
        }
        
        await _service.CreateAsync(createInput);
    }
}
