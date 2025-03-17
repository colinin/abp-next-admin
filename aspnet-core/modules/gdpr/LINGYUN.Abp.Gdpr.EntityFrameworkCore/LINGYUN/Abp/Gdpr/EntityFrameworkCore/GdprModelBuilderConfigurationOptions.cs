using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.Gdpr.EntityFrameworkCore;
public class GdprModelBuilderConfigurationOptions(
    [NotNull] string tablePrefix = "",
    [CanBeNull] string? schema = null) : 
    AbpModelBuilderConfigurationOptions(tablePrefix, schema)
{
}
