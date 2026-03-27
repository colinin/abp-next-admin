using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.AIManagement.Tools.Dtos;
public class AIToolPropertyDescriptorDto
{
    public string Name { get; set; }
    public string ValueType { get; set; }
    public List<NameValue<object>> Options { get; set; }
    public string DisplayName { get; set; }
    public string? Description { get; set; }
}
