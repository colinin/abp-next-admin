using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.AIManagement.Tools.Dtos;
public class AIToolPropertyDescriptorDto
{
    public string Name { get; set; } = default!;
    public bool Required { get; set; }
    public string ValueType { get; set; } = default!;
    public List<NameValue<object>> Options { get; set; } = new List<NameValue<object>>();
    public string DisplayName { get; set; } = default!;
    public string? Description { get; set; }
    public List<NameValue<object>> Dependencies { get; set; } = new List<NameValue<object>>();
}
