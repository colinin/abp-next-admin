using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WxPusher.QrCode;

public interface IWxPusherQrCodeProvider
{
    Task<CreateQrcodeResult> CreateQrcodeAsync(
        [NotNull] string extra,
        int validTime = 1800,
        CancellationToken cancellationToken = default);

    Task<GetScanQrCodeResult> GetScanQrCodeAsync(
        [NotNull] string code,
        CancellationToken cancellationToken = default);
}
