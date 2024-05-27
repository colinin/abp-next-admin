using System.Threading.Tasks;

namespace LINGYUN.Abp.TextTemplating;
public interface IStaticTemplateSaver
{
    Task SaveDefinitionTemplateAsync();

    Task SaveTemplateContentAsync();
}
