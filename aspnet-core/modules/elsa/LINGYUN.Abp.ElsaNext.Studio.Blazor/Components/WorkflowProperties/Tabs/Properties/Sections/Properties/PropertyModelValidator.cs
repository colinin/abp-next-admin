using Elsa.Api.Client.Resources.WorkflowDefinitions.Models;
using Elsa.Studio.Localization;
using FluentValidation;

namespace LINGYUN.Abp.ElsaNext.Studio.Blazor.Components.WorkflowProperties.Tabs.Properties.Sections.Properties;

public class PropertyModelValidator : AbstractValidator<PropertyModel>
{
    public PropertyModelValidator(WorkflowDefinition workflowDefinition, ILocalizer localizer)
    {
        RuleFor(x => x.Key).NotEmpty().WithMessage(localizer["Please enter a key for the property."]);

        RuleFor(x => x.Value)
            .Must((context, name, cancellationToken) =>
            {
                var existingKey = workflowDefinition.CustomProperties.Where(x => x.Key == name).Select(x => x.Key).FirstOrDefault();
                return existingKey == null || existingKey == context.Key;
            })
            .WithMessage(localizer["A property with this key already exists in the current scope."]);
    }
}
