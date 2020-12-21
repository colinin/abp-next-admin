using System;

namespace LINGYUN.Platform.Datas
{
    public class DataCreateDto : DataCreateOrUpdateDto
    {
        public Guid? ParentId { get; set; }
    }
}
