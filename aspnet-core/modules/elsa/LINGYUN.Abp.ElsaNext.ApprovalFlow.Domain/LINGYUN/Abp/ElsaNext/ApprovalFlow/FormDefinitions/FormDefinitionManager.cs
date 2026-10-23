using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

public class FormDefinitionManager : DomainService
{
	protected IFormDefinitionRepository Repository { get; }

	public FormDefinitionManager(IFormDefinitionRepository repository)
	{
		Repository = repository;
	}

	public virtual async Task<FormDefinition> CreateAsync(string name, string code, int version, string? description = null, CancellationToken cancellationToken = default)
	{
		if (await Repository.FindByCodeAsync(code, version, cancellationToken: cancellationToken) != null)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormDefinition.CodeAlreadyExists, logLevel: LogLevel.Warning);
		}
		var definition = new FormDefinition(GuidGenerator.Create(), name, code, version, description);
		return await Repository.InsertAsync(definition, true, cancellationToken);
	}

	public virtual async Task<FormDefinition> UpdateAsync(Guid id, string name, string? description, CancellationToken cancellationToken = default)
	{
		var definition = await Repository.GetAsync(id, cancellationToken: cancellationToken);
		definition.Update(name, description);
		return await Repository.UpdateAsync(definition, true, cancellationToken);
	}

	public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var definition = await Repository.GetAsync(id, cancellationToken: cancellationToken);
		await Repository.DeleteAsync(definition, cancellationToken: cancellationToken);
	}

	public virtual async Task<FormDefinition> PublishAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var definition = await Repository.GetAsync(id, cancellationToken: cancellationToken);
		definition.Publish(new DateTimeOffset(Clock.Now));
		return await Repository.UpdateAsync(definition, true, cancellationToken);
	}

	public virtual async Task<FormDefinition> AddFieldAsync(
        Guid id, 
        string key,
        string label,
        string type,
        string permission, 
        int order,
        bool isRequired, 
        string? hint = null,
        string? optionsJson = null, 
        CancellationToken cancellationToken = default)
	{
		var definition = await Repository.GetAsync(id, cancellationToken: cancellationToken);
		definition.AddField(GuidGenerator.Create(), key, label, type, permission, order, isRequired, hint, optionsJson);
		return await Repository.UpdateAsync(definition, true, cancellationToken);
	}

	public virtual async Task<FormDefinition> UpdateFieldAsync(
        Guid id, 
        Guid fieldId, 
        string label,
        string permission,
        int order, 
        bool isRequired, 
        string? hint = null,
        string? optionsJson = null,
        CancellationToken cancellationToken = default)
	{
		var definition = await Repository.GetAsync(id, cancellationToken: cancellationToken);
		definition.UpdateField(fieldId, label, permission, order, isRequired, hint, optionsJson);
		return await Repository.UpdateAsync(definition, true, cancellationToken);
	}

	public virtual async Task<FormDefinition> RemoveFieldAsync(Guid id, Guid fieldId, CancellationToken cancellationToken = default)
	{
		var definition = await Repository.GetAsync(id, cancellationToken: cancellationToken);
		definition.RemoveField(fieldId);
		return await Repository.UpdateAsync(definition, true, cancellationToken);
	}
}
