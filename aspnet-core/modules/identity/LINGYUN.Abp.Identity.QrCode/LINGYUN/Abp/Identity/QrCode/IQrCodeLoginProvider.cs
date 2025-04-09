using System.Threading.Tasks;

namespace LINGYUN.Abp.Identity.QrCode;

public interface IQrCodeLoginProvider
{
    Task<QrCodeInfo> GenerateAsync();

    Task<QrCodeInfo> GetCodeAsync(string key);

    Task<QrCodeInfo> ScanCodeAsync(string key, QrCodeScanParams @params);

    Task<QrCodeInfo> ConfirmCodeAsync(string key);

    Task RemoveAsync(string key);
}
