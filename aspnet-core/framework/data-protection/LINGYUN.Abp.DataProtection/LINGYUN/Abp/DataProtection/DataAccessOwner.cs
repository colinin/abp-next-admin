using System;
using System.Linq;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.DataProtection;
/// <summary>
/// 数据拥有者
/// </summary>
public class DataAccessOwner : IMayHaveCreator
{
    public Guid? CreatorId { get; set; }
    public string[] Roles { get; set; }
    public string[] OrganizaztionUnits { get; set; }
    protected DataAccessOwner()
    {
    }

    public DataAccessOwner(Guid? creatorId, string[] roles, string[] organizaztionUnits)
    {
        CreatorId = creatorId;
        Roles = roles;
        OrganizaztionUnits = organizaztionUnits;
    }

    public bool IsInRole(string[] roles)
    {
        return Roles != null && Roles.Any(r => roles.Contains(r));
    }

    public bool IsInOrganizaztionUnit(string[] organizaztionUnits)
    {
        return OrganizaztionUnits != null && OrganizaztionUnits.Any(ou => organizaztionUnits.Contains(ou));
    }
}
