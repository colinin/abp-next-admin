using System.Threading.Tasks;

namespace LINGYUN.Abp.AuditLogging;

public interface ISecurityLogInfoContributor
{
    Task ContributeAsync(SecurityLogInfoContributorContext context);
}
