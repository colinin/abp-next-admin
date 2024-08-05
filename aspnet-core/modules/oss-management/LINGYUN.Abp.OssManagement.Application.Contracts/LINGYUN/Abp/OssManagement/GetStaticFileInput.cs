using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.OssManagement;

public class GetStaticFileInput : GetFileMultiTenancyInput
{
    [Required]
    public string Name { get; set; }

    public string Path { get; set; }

    public string Bucket { get; set; }

    public string Process { get; set; }
}
