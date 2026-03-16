using LINGYUN.Abp.AIManagement.Chats.Dtos;
using LINGYUN.Abp.AIManagement.Localization;
using LINGYUN.Abp.AIManagement.Permissions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.AIManagement.Chats;
public class ConversationAppService :
    CrudAppService<
        ConversationRecord,
        ConversationDto,
        Guid,
        ConversationGetListInput,
        ConversationCreateDto,
        ConversationUpdateDto>,
    IConversationAppService
{
    private readonly ConversationCleanupOptions _cleanupOptions;
    public ConversationAppService(
        IRepository<ConversationRecord, Guid> repository,
        IOptions<ConversationCleanupOptions> cleanupOptions) 
        : base(repository)
    {
        _cleanupOptions = cleanupOptions.Value;

        LocalizationResource = typeof(AIManagementResource);
        ObjectMapperContext = typeof(AbpAIManagementApplicationModule);

        CreatePolicyName = AIManagementPermissionNames.Conversation.Create;
        UpdatePolicyName = AIManagementPermissionNames.Conversation.Update;
        DeletePolicyName = AIManagementPermissionNames.Conversation.Delete;
        GetListPolicyName = AIManagementPermissionNames.Conversation.Default;
        GetPolicyName = AIManagementPermissionNames.Conversation.Default;
    }

    protected async override Task<IQueryable<ConversationRecord>> CreateFilteredQueryAsync(ConversationGetListInput input)
    {
        var queryable = await base.CreateFilteredQueryAsync(input);

        return queryable
            .Where(x => x.CreatorId == CurrentUser.Id)
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter!));
    }

    protected override ConversationRecord MapToEntity(ConversationCreateDto createInput)
    {
        var createdAt = Clock.Now;
        var expiredTime = createdAt.Add(_cleanupOptions.ExpiredTime);
        var conversationName = createInput.Name ?? L["NewConversation"];
        return new ConversationRecord(
            GuidGenerator.Create(),
            conversationName,
            createInput.Workspace,
            createdAt,
            expiredTime,
            CurrentTenant.Id);
    }

    protected override void MapToEntity(ConversationUpdateDto updateInput, ConversationRecord entity)
    {
        if (!string.Equals(entity.Name, updateInput.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            entity.SetName(updateInput.Name);
        }

        entity.ChangeTime(Clock.Now);
    }

    protected override ConversationDto MapToGetOutputDto(ConversationRecord entity)
    {
        return new ConversationDto
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            ExpiredAt = entity.ExpiredAt,
            CreationTime = entity.CreationTime,
            CreatorId = entity.CreatorId,
            LastModificationTime = entity.LastModificationTime,
            LastModifierId = entity.LastModifierId,
            Name = entity.Name,
            UpdateAt = entity.UpdateAt,
            Workspace = entity.Workspace,
        };
    }

    protected override ConversationDto MapToGetListOutputDto(ConversationRecord entity)
    {
        return MapToGetOutputDto(entity);
    }
}
