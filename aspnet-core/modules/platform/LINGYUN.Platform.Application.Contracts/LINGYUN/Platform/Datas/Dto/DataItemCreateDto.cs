using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Datas
{
    public class DataItemCreateDto : DataItemCreateOrUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(DataItemConsts), nameof(DataItemConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}
