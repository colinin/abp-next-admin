using System;
using System.ComponentModel;

namespace LINGYUN.Platform.Datas;

public class DataMoveDto
{

    [DisplayName("DisplayName:ParentData")]
    public Guid? ParentId { get; set; }
}
