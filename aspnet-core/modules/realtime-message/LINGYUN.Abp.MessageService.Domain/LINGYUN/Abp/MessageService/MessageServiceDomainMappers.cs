using LINGYUN.Abp.IM.Groups;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Chat;
using LINGYUN.Abp.MessageService.Groups;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.MessageService;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class GroupMessageToChatMessageMapper : MapperBase<GroupMessage, ChatMessage>
{
    [MapProperty(nameof(GroupMessage.GroupId), nameof(ChatMessage.GroupId))]
    [MapProperty(nameof(GroupMessage.CreatorId), nameof(ChatMessage.FormUserId))]
    [MapProperty(nameof(GroupMessage.SendUserName), nameof(ChatMessage.FormUserName))]
    [MapProperty(nameof(GroupMessage.CreationTime), nameof(ChatMessage.SendTime))]
    [MapProperty(nameof(GroupMessage.Type), nameof(ChatMessage.MessageType))]
    [MapperIgnoreTarget(nameof(ChatMessage.ToUserId))]
    [MapperIgnoreTarget(nameof(ChatMessage.IsAnonymous))]
    public override partial ChatMessage Map(GroupMessage source);

    [MapProperty(nameof(GroupMessage.GroupId), nameof(ChatMessage.GroupId))]
    [MapProperty(nameof(GroupMessage.CreatorId), nameof(ChatMessage.FormUserId))]
    [MapProperty(nameof(GroupMessage.SendUserName), nameof(ChatMessage.FormUserName))]
    [MapProperty(nameof(GroupMessage.CreationTime), nameof(ChatMessage.SendTime))]
    [MapProperty(nameof(GroupMessage.Type), nameof(ChatMessage.MessageType))]
    [MapperIgnoreTarget(nameof(ChatMessage.ToUserId))]
    [MapperIgnoreTarget(nameof(ChatMessage.IsAnonymous))]
    public override partial void Map(GroupMessage source, ChatMessage destination);

    public override void AfterMap(GroupMessage source, ChatMessage destination)
    {
        destination.IsAnonymous = source.GetProperty(nameof(ChatMessage.IsAnonymous), false);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class UserMessageToChatMessageMapper : MapperBase<UserMessage, ChatMessage>
{
    [MapProperty(nameof(UserMessage.ReceiveUserId), nameof(ChatMessage.ToUserId))]
    [MapProperty(nameof(UserMessage.CreatorId), nameof(ChatMessage.FormUserId))]
    [MapProperty(nameof(UserMessage.SendUserName), nameof(ChatMessage.FormUserName))]
    [MapProperty(nameof(UserMessage.CreationTime), nameof(ChatMessage.SendTime))]
    [MapProperty(nameof(UserMessage.Type), nameof(ChatMessage.MessageType))]
    [MapperIgnoreTarget(nameof(ChatMessage.GroupId))]
    [MapperIgnoreTarget(nameof(ChatMessage.IsAnonymous))]
    public override partial ChatMessage Map(UserMessage source);

    [MapProperty(nameof(UserMessage.ReceiveUserId), nameof(ChatMessage.ToUserId))]
    [MapProperty(nameof(UserMessage.CreatorId), nameof(ChatMessage.FormUserId))]
    [MapProperty(nameof(UserMessage.SendUserName), nameof(ChatMessage.FormUserName))]
    [MapProperty(nameof(UserMessage.CreationTime), nameof(ChatMessage.SendTime))]
    [MapProperty(nameof(UserMessage.Type), nameof(ChatMessage.MessageType))]
    [MapperIgnoreTarget(nameof(ChatMessage.GroupId))]
    [MapperIgnoreTarget(nameof(ChatMessage.IsAnonymous))]
    public override partial void Map(UserMessage source, ChatMessage destination);
    public override void AfterMap(UserMessage source, ChatMessage destination)
    {
        destination.IsAnonymous = source.GetProperty(nameof(ChatMessage.IsAnonymous), false);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ChatGroupToGroupMapper : MapperBase<ChatGroup, Group>
{
    [MapProperty(nameof(ChatGroup.GroupId), nameof(Group.Id))]
    [MapProperty(nameof(ChatGroup.MaxUserCount), nameof(Group.MaxUserLength))]
    [MapperIgnoreTarget(nameof(Group.GroupUserCount))]
    public override partial Group Map(ChatGroup source);

    [MapProperty(nameof(ChatGroup.GroupId), nameof(Group.Id))]
    [MapProperty(nameof(ChatGroup.MaxUserCount), nameof(Group.MaxUserLength))]
    [MapperIgnoreTarget(nameof(Group.GroupUserCount))]
    public override partial void Map(ChatGroup source, Group destination);
}
