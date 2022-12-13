using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace LINGYUN.Platform.Packages;

public class PackageSpecification : Specification<Package>
{
    protected PackageFilter Filter { get; }

    public PackageSpecification(PackageFilter filter)
    {
        Filter = filter;
    }

    public override Expression<Func<Package, bool>> ToExpression()
    {
        Expression<Func<Package, bool>> expression = (p) => true;

        return expression
            .AndIf(!Filter.Name.IsNullOrWhiteSpace(), x => x.Name == Filter.Name)
            .AndIf(!Filter.Note.IsNullOrWhiteSpace(), x => x.Note == Filter.Note)
            .AndIf(!Filter.Version.IsNullOrWhiteSpace(), x => x.Version == Filter.Version)
            .AndIf(!Filter.Description.IsNullOrWhiteSpace(), x => x.Description == Filter.Description)
            .AndIf(!Filter.Authors.IsNullOrWhiteSpace(), x => x.Authors == Filter.Authors)
            .AndIf(Filter.ForceUpdate.HasValue, x => x.ForceUpdate == Filter.ForceUpdate)
            .AndIf(!Filter.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(Filter.Filter) ||
                x.Note.Contains(Filter.Filter) || x.Version.Contains(Filter.Filter) ||
                x.Description.Contains(Filter.Filter) || x.Authors.Contains(Filter.Filter));
    }
}
