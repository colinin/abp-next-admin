using System.Threading.Tasks;
using Volo.Abp.Data;

namespace LINGYUN.Platform.Messages.Integration;
public class SmsMessageIntegrationService : PlatformApplicationServiceBase, ISmsMessageIntegrationService
{
    private readonly ISmsMessageRepository _smsMessageRepository;

    public SmsMessageIntegrationService(ISmsMessageRepository smsMessageRepository)
    {
        _smsMessageRepository = smsMessageRepository;
    }


    public async virtual Task<SmsMessageDto> CreateAsync(SmsMessageCreateDto input)
    {
        var smsMessage = new SmsMessage(
            GuidGenerator.Create(),
            input.PhoneNumber,
            input.Text,
            CurrentUser.Id,
            CurrentUser.UserName
        );
        if (input.ExtraProperties != null)
        {
            foreach (var prop in input.ExtraProperties)
            {
                smsMessage.SetProperty(prop.Key, prop.Value);
            }
        }

        smsMessage = await _smsMessageRepository.InsertAsync(smsMessage);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<SmsMessage, SmsMessageDto>(smsMessage);
    }
}
