using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;

namespace LINGYUN.Abp.MessageService.Groups
{
    public static class IGroupRepositoryExtensions
    {
        public static async Task<ChatGroup> GetByIdAsync(
            this IGroupRepository repository,
            long id,
            CancellationToken cancellationToken = default)
        {
            var group = await repository.FindByIdAsync(id, cancellationToken);

            return group ?? throw new BusinessException(MessageServiceErrorCodes.GroupNotFount);
        }
    }
}
