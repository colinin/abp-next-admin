using LINGYUN.Abp.WeChat.Official.Account.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Official.Account;
/// <summary>
/// 生成带参数的二维码接口
/// </summary>
/// <remarks>
/// 详情见: https://developers.weixin.qq.com/doc/offiaccount/Account_Management/Generating_a_Parametric_QR_Code.html
/// </remarks>
public interface IParametricQrCodeGenerator
{
    /// <summary>
    /// 创建二维码ticket
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TicketModel> CreateTicketAsync(CreateTicketModel model, CancellationToken cancellationToken = default);
    /// <summary>
    /// 通过ticket换取二维码
    /// </summary>
    /// <param name="ticket"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Stream> ShowQrCodeAsync(string ticket, CancellationToken cancellationToken = default);
}
