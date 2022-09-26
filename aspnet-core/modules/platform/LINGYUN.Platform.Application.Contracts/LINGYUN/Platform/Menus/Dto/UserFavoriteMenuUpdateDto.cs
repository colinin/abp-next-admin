using Volo.Abp.Domain.Entities;

namespace LINGYUN.Platform.Menus;

public class UserFavoriteMenuUpdateDto : UserFavoriteMenuCreateOrUpdateDto, IHasConcurrencyStamp
{

    public string ConcurrencyStamp { get; set; }
}
