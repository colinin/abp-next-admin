using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.UserTasks;

/// <summary>
/// Bridges <see cref="FormDefinition"/> into the Elsa User Tasks form pipeline: a user task can reference a form
/// definition by provider name and its data is validated against the definition before the workflow resumes.
/// </summary>
public class FormDefinitionUserTaskFormProvider : IUserTaskFormProvider, ITransientDependency
{
    public const string ProviderName = "form-definitions";

    public const string DefinitionIdMetadataKey = "definitionId";

    public string Name => ProviderName;

    protected IFormDefinitionRepository DefinitionRepository { get; }

    public FormDefinitionUserTaskFormProvider(IFormDefinitionRepository definitionRepository)
    {
        DefinitionRepository = definitionRepository;
    }

    public virtual async Task<ResolvedUserTaskForm?> ResolveAsync(UserTaskFormReference reference, CancellationToken cancellationToken = default)
    {
        if (!string.Equals(reference.ProviderName, Name, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var definition = await FindDefinitionAsync(reference.Key, reference.Version, cancellationToken);
        if (definition == null)
        {
            return null;
        }

        var fields = definition.Fields
            .OrderBy(x => x.Order)
            .Select(x => new UserTaskFormFieldDescriptor(
                x.Key,
                x.Label,
                x.Type,
                x.IsRequired,
                x.Permission == FormFieldPermissions.Hidden,
                false))
            .ToArray();

        return new ResolvedUserTaskForm(
            reference,
            definition.Version.ToString(),
            new Dictionary<string, object?>
            {
                [DefinitionIdMetadataKey] = definition.Id.ToString("D"),
                ["code"] = definition.Code,
                ["name"] = definition.Name,
            })
        {
            Fields = fields,
        };
    }

    public virtual async Task<UserTaskFormValidationResult> ValidateAndNormalizeAsync(
        ResolvedUserTaskForm form,
        string actionKey,
        JsonElement data,
        CancellationToken cancellationToken = default)
    {
        if (!form.Metadata.TryGetValue(DefinitionIdMetadataKey, out var definitionIdValue) ||
            definitionIdValue == null ||
            !Guid.TryParse(definitionIdValue.ToString(), out var definitionId))
        {
            return Failed("表单定义缺失。");
        }

        var definition = await DefinitionRepository.GetAsync(definitionId, cancellationToken: cancellationToken);
        if (data.ValueKind != JsonValueKind.Object)
        {
            return Failed("表单数据必须是 JSON 对象。");
        }

        var submitted = data;
        var errors = new List<string>();

        foreach (var property in submitted.EnumerateObject())
        {
            var field = definition.Fields.FirstOrDefault(x => x.Key == property.Name);
            if (field == null)
            {
                errors.Add($"未知字段：{property.Name}。");
                continue;
            }

            if (!FormFieldPermissions.CanRead(field.Permission))
            {
                errors.Add($"字段“{field.Label}”不可提交。");
            }
        }

        foreach (var field in definition.Fields.Where(x => x.IsRequired && FormFieldPermissions.CanWrite(x.Permission)))
        {
            if (!submitted.TryGetProperty(field.Key, out var value) || IsEmpty(value))
            {
                errors.Add($"字段“{field.Label}”为必填项。");
            }
        }

        if (errors.Count > 0)
        {
            return new UserTaskFormValidationResult(false, null, errors);
        }

        return new UserTaskFormValidationResult(true, submitted.Clone(), null);
    }

    protected virtual async Task<FormDefinition?> FindDefinitionAsync(string key, string? version, CancellationToken cancellationToken = default)
    {
        if (Guid.TryParse(key, out var definitionId))
        {
            try
            {
                return await DefinitionRepository.GetAsync(definitionId, cancellationToken: cancellationToken);
            }
            catch (EntityNotFoundException)
            {
                return null;
            }
        }

        int? parsedVersion = int.TryParse(version, out var value) ? value : null;
        return await DefinitionRepository.FindByCodeAsync(key, parsedVersion, cancellationToken: cancellationToken);
    }

    private static bool IsEmpty(JsonElement value)
    {
        return value.ValueKind switch
        {
            JsonValueKind.Null or JsonValueKind.Undefined => true,
            JsonValueKind.String => string.IsNullOrWhiteSpace(value.GetString()),
            JsonValueKind.Array => value.GetArrayLength() == 0,
            _ => false,
        };
    }

    private static UserTaskFormValidationResult Failed(string error)
    {
        return new UserTaskFormValidationResult(false, null, new[] { error });
    }
}
