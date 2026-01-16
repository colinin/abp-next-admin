using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.AI;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore.ValueConversions;
public class ChatRoleValueConverter(ConverterMappingHints? mappingHints = null) : ValueConverter<ChatRole, string>(
       value => value.Value,
       value => new ChatRole(value),
       mappingHints
       )
{
}
