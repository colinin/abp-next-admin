using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AIManagement.Messages;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.AIManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class UserTextMessageRecordToUserTextMessageMapper : MapperBase<UserTextMessageRecord, TextChatMessage>
{
    public override partial TextChatMessage Map(UserTextMessageRecord source);
    public override partial void Map(UserTextMessageRecord source, TextChatMessage destination);
}
