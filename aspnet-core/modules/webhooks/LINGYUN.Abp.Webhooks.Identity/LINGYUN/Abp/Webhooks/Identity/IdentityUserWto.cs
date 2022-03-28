using System;

namespace LINGYUN.Abp.Webhooks.Identity;

[Serializable]
public class IdentityUserWto
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }
}
