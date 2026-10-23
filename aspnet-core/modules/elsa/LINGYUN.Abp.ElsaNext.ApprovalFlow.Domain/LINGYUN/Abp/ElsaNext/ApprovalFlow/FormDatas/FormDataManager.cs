using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;

public class FormDataManager : DomainService
{
	protected IFormDefinitionRepository FormDefinitionRepository { get; }

	protected IFormDataRepository FormDataRepository { get; }

	public FormDataManager(IFormDefinitionRepository formDefinitionRepository, IFormDataRepository formDataRepository)
	{
		FormDefinitionRepository = formDefinitionRepository;
		FormDataRepository = formDataRepository;
	}

	public virtual async Task<FormData?> FindAsync(Guid formDefinitionId, string instanceId, CancellationToken cancellationToken = default)
	{
		return await FormDataRepository.FindByInstanceAsync(formDefinitionId, instanceId, cancellationToken);
	}

	public virtual async Task<FormData> SaveAsync(Guid formDefinitionId, string instanceId, string dataJson, CancellationToken cancellationToken = default)
	{
		var definition = await FormDefinitionRepository.GetAsync(formDefinitionId, cancellationToken: cancellationToken);
		if (!definition.IsPublished)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormData.DefinitionNotPublished);
		}
		var submitted = ParseObject(dataJson);
		var data = await FormDataRepository.FindByInstanceAsync(formDefinitionId, instanceId, cancellationToken);
		var merged = ((data == null) ? new JsonObject() : ParseObject(data.DataJson));
		foreach (var property in submitted)
		{
			var field = definition.Fields.FirstOrDefault(x => x.Key == property.Key) 
                ?? throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormField.NotFound);
			if (FormFieldPermissions.CanWrite(field.Permission))
			{
				merged[property.Key] = property.Value?.DeepClone();
			}
			else if (!FormFieldPermissions.CanRead(field.Permission))
			{
				throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormData.FieldHidden);
			}
		}
		EnsureRequiredFieldsFilled(definition, merged);
		var json = merged.ToJsonString();
		if (data == null)
		{
			data = new FormData(GuidGenerator.Create(), definition.Id, definition.Code, definition.Version, instanceId, json);
			await FormDataRepository.InsertAsync(data, true, cancellationToken);
		}
		else
		{
			data.SetData(json);
			await FormDataRepository.UpdateAsync(data, true, cancellationToken);
		}
		return data;
	}

	public virtual string GetVisibleDataJson(FormDefinition definition, string dataJson)
	{
		var jsonObject = ParseObject(dataJson);
		foreach (var item in definition.Fields.Where(x => !FormFieldPermissions.CanRead(x.Permission)))
		{
			jsonObject.Remove(item.Key);
		}
		return jsonObject.ToJsonString();
	}

	private static void EnsureRequiredFieldsFilled(FormDefinition definition, JsonObject data)
	{
		foreach (var item in definition.Fields.Where(x => x.IsRequired && FormFieldPermissions.CanWrite(x.Permission)))
		{
			if (IsEmpty(data[item.Key]))
			{
				throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormData.RequiredFieldMissing);
			}
		}
	}

	private static bool IsEmpty(JsonNode? node)
	{
		if (false)
		{
		}
		var result = node == null || ((node is JsonArray jsonArray) 
            ? (jsonArray.Count == 0) 
            : (node is JsonValue jsonValue && jsonValue.TryGetValue<string>(out var value) && string.IsNullOrWhiteSpace(value)));
		if (false)
		{
		}
		return result;
	}

	private static JsonObject ParseObject(string json)
	{
		if (json.IsNullOrWhiteSpace())
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormData.InvalidJson);
		}
		JsonNode jsonNode;
		try
		{
			jsonNode = JsonNode.Parse(json)!;
		}
		catch (JsonException)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormData.InvalidJson);
		}
        if (jsonNode is not JsonObject result)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.FormData.InvalidJson);
		}
		return result;
	}
}
