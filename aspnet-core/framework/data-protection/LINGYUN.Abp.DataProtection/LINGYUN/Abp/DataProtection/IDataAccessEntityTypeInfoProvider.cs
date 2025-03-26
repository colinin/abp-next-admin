using LINGYUN.Abp.DataProtection.Models;
using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtection;

public interface IDataAccessEntityTypeInfoProvider
{
    Task<EntityTypeInfoModel> GetEntitTypeInfoAsync(DataAccessEntitTypeInfoContext context);
}
