using System.Threading.Tasks;

namespace LINGYUN.Abp.AuditLogging;

public interface IAuditLogInfoContributor
{
    Task ContributeAsync(AuditLogInfoContributorContext context);
}
