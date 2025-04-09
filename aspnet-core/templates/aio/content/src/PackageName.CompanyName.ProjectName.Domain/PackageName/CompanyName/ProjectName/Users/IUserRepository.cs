using System;
using Volo.Abp.Domain.Repositories;

namespace PackageName.CompanyName.ProjectName.Users
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}