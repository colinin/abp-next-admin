using System.Collections.Generic;

namespace LINGYUN.Abp.Account;
public class ExternalLoginResultDto
{
    public List<UserLoginInfoDto> UserLogins { get; set; } = new List<UserLoginInfoDto>();
    public List<ExternalLoginInfoDto> ExternalLogins { get; set; } = new List<ExternalLoginInfoDto>();
}
