using Volo.Abp.Auditing;
using Volo.Abp.Content;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.BlobManagement.Integration.Dtos;

public class BlobFileCreateIntegrationDto : BlobFileGetByNameIntegrationDto
{
    public bool? Overwrite { get; set; }

    [DisableAuditing]
    [DisableValidation]
    public IRemoteStreamContent File { get; set; }
}
