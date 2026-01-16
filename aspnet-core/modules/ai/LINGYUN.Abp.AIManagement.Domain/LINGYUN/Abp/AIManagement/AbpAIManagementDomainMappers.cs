using LINGYUN.Abp.AI.Models;
using LINGYUN.Abp.AIManagement.Messages;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.AIManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class UserTextMessageRecordToUserTextMessageMapper : MapperBase<UserTextMessageRecord, UserTextMessage>
{
    public override partial UserTextMessage Map(UserTextMessageRecord source);
    public override partial void Map(UserTextMessageRecord source, UserTextMessage destination);
}
