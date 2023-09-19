using System;
using System.Linq.Expressions;
using Volo.Abp.Identity;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Identity;
public class OrganizationUnitGetListSpecification : Specification<OrganizationUnit>
{
    protected OrganizationUnitGetByPagedDto Input { get; }
    public OrganizationUnitGetListSpecification(OrganizationUnitGetByPagedDto input)
    {
        Input = input;
    }

    public override Expression<Func<OrganizationUnit, bool>> ToExpression()
    {
        Expression<Func<OrganizationUnit, bool>> expression = _ => true;

        return expression
            .AndIf(!Input.Filter.IsNullOrWhiteSpace(), x =>
                x.DisplayName.Contains(Input.Filter) || x.Code.Contains(Input.Filter));
    }
}
