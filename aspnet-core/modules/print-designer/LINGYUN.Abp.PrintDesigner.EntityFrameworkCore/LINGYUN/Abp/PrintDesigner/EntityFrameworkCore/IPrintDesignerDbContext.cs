using LINGYUN.Abp.PrintDesigner.CustomElements;
using LINGYUN.Abp.PrintDesigner.Templates;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.PrintDesigner.EntityFrameworkCore;

[ConnectionStringName(PrintDesignerDbProperties.ConnectionStringName)]
public interface IPrintDesignerDbContext : IEfCoreDbContext
{
    DbSet<CustomElement> CustomElements { get; }
    DbSet<Template> Templates { get; }
}
