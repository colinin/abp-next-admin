using LINGYUN.Abp.DataProtection;

namespace Volo.Abp.Uow;
public static class IUnitOfWorkDataAccessExtensions
{
    private const string DataAccessRuleKey = "LINGYUN.Abp.DataProtection.DataAccess";

    public static IUnitOfWork SetAccessRuleInfo(
        this IUnitOfWork unitOfWork,
        DataAccessRuleInfo dataAccessRuleInfo)
    {
        unitOfWork.RemoveItem(DataAccessRuleKey);
        unitOfWork.AddItem(DataAccessRuleKey, dataAccessRuleInfo);

        return unitOfWork;
    }

    public static DataAccessRuleInfo GetAccessRuleInfo(
        this IUnitOfWork unitOfWork)
    {
        return unitOfWork.GetItemOrDefault<DataAccessRuleInfo>(DataAccessRuleKey)
            ?? new DataAccessRuleInfo(null);
    }
}
