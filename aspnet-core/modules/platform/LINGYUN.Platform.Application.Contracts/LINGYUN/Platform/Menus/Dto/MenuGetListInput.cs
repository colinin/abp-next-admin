using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Menus
{
    public class MenuGetListInput : PagedAndSortedResultRequestDto
    {
        public PlatformType? PlatformType { get; set; }

        public string Filter { get; set; }

        public bool Reverse { get; set; }

        public Guid? ParentId { get; set; }

        public Guid? LayoutId { get; set; }
    }
}
