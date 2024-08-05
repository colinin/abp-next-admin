using System.Collections.Generic;

namespace LINGYUN.Abp.OssManagement;

public class OssObjectsResultDto
{
    public string Bucket { get; set; }
    public string Prefix { get; set; }
    public string Delimiter { get; set; }
    public string Marker { get; set; }
    public string NextMarker { get; set; }
    public int MaxKeys { get; set; }
    public List<OssObjectDto> Objects { get; set; } = new List<OssObjectDto>();
}
