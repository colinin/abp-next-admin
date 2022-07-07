using JetBrains.Annotations;
using System.Collections.Generic;

namespace LINGYUN.Abp.BackgroundTasks;

public interface IJobDefinitionManager
{
    [NotNull]
    JobDefinition Get([NotNull] string name);

    IReadOnlyList<JobDefinition> GetAll();

    JobDefinition GetOrNull(string name);
}
