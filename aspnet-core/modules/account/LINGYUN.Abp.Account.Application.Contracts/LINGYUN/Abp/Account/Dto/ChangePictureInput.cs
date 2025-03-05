using Volo.Abp.Auditing;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Account;

public class ChangePictureInput
{
    [DisableAuditing]
    public IRemoteStreamContent File { get; set; }
}
