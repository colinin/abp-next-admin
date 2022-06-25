using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateContentContributor : ITemplateContentContributor, ITransientDependency
{
    public async virtual Task<string> GetOrNullAsync(TemplateContentContributorContext context)
    {
        var repository = context.ServiceProvider.GetRequiredService<ITextTemplateRepository>();
        var template = await repository.FindByNameAsync(context.TemplateDefinition.Name, context.Culture);

        return template?.Content;
    }
}
