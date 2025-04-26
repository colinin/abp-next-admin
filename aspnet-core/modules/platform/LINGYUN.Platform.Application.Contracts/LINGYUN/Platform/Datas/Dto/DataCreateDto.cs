using System;
using System.ComponentModel;

namespace LINGYUN.Platform.Datas;

public class DataCreateDto : DataCreateOrUpdateDto
{
    [DisplayName("DisplayName:ParentData")]
    public Guid? ParentId { get; set; }
}
