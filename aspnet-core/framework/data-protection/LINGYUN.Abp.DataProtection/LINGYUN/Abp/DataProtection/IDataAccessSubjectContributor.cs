using System.Collections.Generic;

namespace LINGYUN.Abp.DataProtection;
public interface IDataAccessSubjectContributor
{
    string Name { get; }
    List<DataAccessFilterGroup> GetFilterGroups(DataAccessSubjectContributorContext context);
    List<string> GetAllowProperties(DataAccessSubjectContributorContext context);
}
