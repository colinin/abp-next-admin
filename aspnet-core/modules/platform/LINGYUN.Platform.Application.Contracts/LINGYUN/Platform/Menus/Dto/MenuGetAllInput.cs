using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Menus
{
    public class MenuGetAllInput : ISortedResultRequest
    {
        public PlatformType? PlatformType { get; set; }

        public string Filter { get; set; }

        public bool Reverse { get; set; }

        public Guid? ParentId { get; set; }

        public string Sorting { get; set; }

        public Guid? LayoutId { get; set; }
    }
}
