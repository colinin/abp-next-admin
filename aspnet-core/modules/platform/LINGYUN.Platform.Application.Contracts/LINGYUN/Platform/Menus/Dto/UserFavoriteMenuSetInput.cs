using System;
using System.Collections.Generic;

namespace LINGYUN.Platform.Menus;

public class UserFavoriteMenuSetInput
{
    public string Framework { get; set; }

    public List<Guid> MenuIds { get; set; } = new List<Guid>();
}
