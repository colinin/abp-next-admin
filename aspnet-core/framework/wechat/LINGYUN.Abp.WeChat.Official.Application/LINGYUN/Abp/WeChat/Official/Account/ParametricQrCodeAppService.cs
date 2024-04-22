using LINGYUN.Abp.WeChat.Official.Account.Models;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.WeChat.Official.Account;
public class ParametricQrCodeAppService : ApplicationService, IParametricQrCodeAppService
{
    private readonly IParametricQrCodeGenerator _qrCodeGenerator;

    public ParametricQrCodeAppService(IParametricQrCodeGenerator qrCodeGenerator)
    {
        _qrCodeGenerator = qrCodeGenerator;
    }

    public async virtual Task<IRemoteStreamContent> GenerateAsync(ParametricQrCodeGenerateInput input)
    {
        var createTicketModel = CreateTicketModel.EnumScene(input.SceneEnum);
        var ticketModel = await _qrCodeGenerator.CreateTicketAsync(createTicketModel);
        var stream = await _qrCodeGenerator.ShowQrCodeAsync(ticketModel.Ticket);

        return new RemoteStreamContent(stream);
    }
}
