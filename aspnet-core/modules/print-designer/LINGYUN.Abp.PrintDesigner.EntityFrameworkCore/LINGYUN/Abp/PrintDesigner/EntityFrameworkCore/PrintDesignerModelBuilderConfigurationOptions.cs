using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.PrintDesigner.EntityFrameworkCore;

public class PrintDesignerModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
{
    public PrintDesignerModelBuilderConfigurationOptions(
        [NotNull] string tablePrefix = "",
        [CanBeNull] string? schema = null)
        : base(
            tablePrefix,
            schema)
    {

    }
}
