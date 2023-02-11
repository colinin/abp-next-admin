using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement;
public interface IOssObjectExpireor
{
    Task ExpireAsync(ExprieOssObjectRequest request);
}
