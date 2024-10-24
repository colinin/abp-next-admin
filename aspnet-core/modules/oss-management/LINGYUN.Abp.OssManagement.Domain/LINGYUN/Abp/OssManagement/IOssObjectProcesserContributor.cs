using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement;

public interface IOssObjectProcesserContributor
{
    Task ProcessAsync(OssObjectProcesserContext context);
}
