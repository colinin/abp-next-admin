using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.Identity
{
    public class OrganizationUnitUpdateDto : ExtensibleObject
    {
        public string DisplayName { get; set; }
    }
}
