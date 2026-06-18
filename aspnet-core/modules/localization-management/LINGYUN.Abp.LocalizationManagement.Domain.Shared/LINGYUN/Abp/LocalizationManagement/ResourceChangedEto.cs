using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.LocalizationManagement;

[Serializable]
public class ResourceChangedEto
{
    public List<string> Resources { get; set; }
    public List<string> Cultures { get; set; }
}
