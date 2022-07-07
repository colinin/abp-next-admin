using System.Collections.Generic;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobDefinitionContext
{
    JobDefinition GetOrNull(string name);

    IReadOnlyList<JobDefinition> GetAll();

    void Add(params JobDefinition[] definitions);
}
