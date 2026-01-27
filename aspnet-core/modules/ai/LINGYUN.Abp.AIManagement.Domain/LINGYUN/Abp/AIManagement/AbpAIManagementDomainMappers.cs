using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AIManagement.Chats;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.AIManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class TextChatMessageRecordToUserTextMessageMapper : MapperBase<TextChatMessageRecord, TextChatMessage>
{
    public override partial TextChatMessage Map(TextChatMessageRecord source);
    public override partial void Map(TextChatMessageRecord source, TextChatMessage destination);
}
