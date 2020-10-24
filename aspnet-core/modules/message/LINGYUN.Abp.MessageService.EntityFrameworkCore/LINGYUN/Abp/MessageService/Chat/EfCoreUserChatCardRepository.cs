using LINGYUN.Abp.IM;
using LINGYUN.Abp.MessageService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class EfCoreUserChatCardRepository : EfCoreRepository<IMessageServiceDbContext, UserChatCard, long>, IUserChatCardRepository
    {
        public EfCoreUserChatCardRepository(
            IDbContextProvider<IMessageServiceDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> CheckUserIdExistsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AnyAsync(ucc => ucc.UserId == userId,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<UserCard> GetMemberAsync(Guid findUserId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(ucc => ucc.UserId == findUserId)
                .Select(ucc => ucc.ToUserCard())
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetMemberCountAsync(string findUserName = "", int? startAge = null, int? endAge = null, Sex? sex = null, CancellationToken cancellationToken = default)
        {
            return await DbSet
                  .WhereIf(!findUserName.IsNullOrWhiteSpace(), ucc => ucc.UserName.Contains(findUserName))
                  .WhereIf(startAge.HasValue, ucc => ucc.Age >= startAge.Value)
                  .WhereIf(endAge.HasValue, ucc => ucc.Age <= endAge.Value)
                  .WhereIf(sex.HasValue, ucc => ucc.Sex  == sex)
                  .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<UserCard>> GetMembersAsync(string findUserName = "", int? startAge = null, int? endAge = null, Sex? sex = null, string sorting = nameof(UserChatCard.UserId), bool reverse = false, int skipCount = 0, int maxResultCount = 10, CancellationToken cancellationToken = default)
        {
            sorting ??= nameof(UserChatCard.UserId);
            sorting = reverse ? sorting + " desc" : sorting;
            return await DbSet
                  .WhereIf(!findUserName.IsNullOrWhiteSpace(), ucc => ucc.UserName.Contains(findUserName))
                  .WhereIf(startAge.HasValue, ucc => ucc.Age >= startAge.Value)
                  .WhereIf(endAge.HasValue, ucc => ucc.Age <= endAge.Value)
                  .WhereIf(sex.HasValue, ucc => ucc.Sex == sex)
                  .OrderBy(sorting)
                  .PageBy(skipCount, maxResultCount)
                  .Select(ucc => ucc.ToUserCard())
                  .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
