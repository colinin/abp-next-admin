using System.ComponentModel.DataAnnotations;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;

public class FormDataSaveDto
{
    [Required]
    [DynamicStringLength(typeof(FormDataConsts), nameof(FormDataConsts.MaxDataJsonLength))]
    public string DataJson { get; set; } = "{}";
}
