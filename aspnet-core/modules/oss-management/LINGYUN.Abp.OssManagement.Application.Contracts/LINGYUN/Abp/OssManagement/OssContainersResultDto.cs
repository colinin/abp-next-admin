using System.Collections.Generic;

namespace LINGYUN.Abp.OssManagement;

public class OssContainersResultDto
{
    public string Prefix { get; set; }
    public string Marker { get; set; }
    public string NextMarker { get; set; }
    public int MaxKeys { get; set; }
    public List<OssContainerDto> Containers { get; set; } = new List<OssContainerDto>();
}
