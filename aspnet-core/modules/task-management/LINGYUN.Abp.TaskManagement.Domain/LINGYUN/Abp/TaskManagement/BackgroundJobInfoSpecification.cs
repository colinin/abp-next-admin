using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.TaskManagement;
public class BackgroundJobInfoSpecification : Specification<BackgroundJobInfo>
{
    protected BackgroundJobInfoFilter Filter { get; }
    public BackgroundJobInfoSpecification(BackgroundJobInfoFilter filter)
    {
        Filter = filter;
    }

    public override Expression<Func<BackgroundJobInfo, bool>> ToExpression()
    {
        Expression<Func<BackgroundJobInfo, bool>> expression = _ => true;

        return expression
            .AndIf(!Filter.Type.IsNullOrWhiteSpace(), x => x.Type.Contains(Filter.Type))
            .AndIf(!Filter.Group.IsNullOrWhiteSpace(), x => x.Group.Equals(Filter.Group))
            .AndIf(!Filter.Name.IsNullOrWhiteSpace(), x => x.Name.Equals(Filter.Name))
            .AndIf(!Filter.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(Filter.Filter) ||
                x.Group.Contains(Filter.Filter) || x.Type.Contains(Filter.Filter) || x.Description.Contains(Filter.Filter))
            .AndIf(Filter.JobType.HasValue, x => x.JobType == Filter.JobType)
            .AndIf(Filter.Status.HasValue, x => x.Status == Filter.Status.Value)
            .AndIf(Filter.Priority.HasValue, x => x.Priority == Filter.Priority.Value)
            .AndIf(Filter.Source.HasValue, x => x.Source == Filter.Source.Value)
            .AndIf(Filter.IsAbandoned.HasValue, x => x.IsAbandoned == Filter.IsAbandoned.Value)
            .AndIf(Filter.BeginLastRunTime.HasValue, x => x.LastRunTime >= Filter.BeginLastRunTime)
            .AndIf(Filter.EndLastRunTime.HasValue, x => x.LastRunTime <= Filter.EndLastRunTime)
            .AndIf(Filter.BeginTime.HasValue, x => x.BeginTime >= Filter.BeginTime)
            .AndIf(Filter.EndTime.HasValue, x => x.EndTime <= Filter.EndTime)
            .AndIf(Filter.BeginCreationTime.HasValue, x => x.CreationTime >= Filter.BeginCreationTime)
            .AndIf(Filter.EndCreationTime.HasValue, x => x.CreationTime <= Filter.EndCreationTime);
    }
}
