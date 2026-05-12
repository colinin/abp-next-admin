using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobManagement;

public interface IBlobPolicyCheckProvider
{
    string Name { get; }

    Task CheckAsync(BlobPolicyCheckContext context);
}
