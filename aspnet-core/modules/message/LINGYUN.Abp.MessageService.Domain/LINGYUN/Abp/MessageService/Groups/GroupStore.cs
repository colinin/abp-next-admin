using LINGYUN.Abp.IM.Groups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace LINGYUN.Abp.MessageService.Groups
{
    public class GroupStore : IGroupStore, ITransientDependency
    {
        private readonly IObjectMapper _objectMapper;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGroupRepository _groupRepository;

        public GroupStore(
            IObjectMapper objectMapper,
            ICurrentTenant currentTenant,
            IGroupRepository groupRepository)
        {
            _objectMapper = objectMapper;
            _currentTenant = currentTenant;
            _groupRepository = groupRepository;
        }

        public virtual async Task<Group> GetAsync(
            Guid? tenantId,
            string groupId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var group = await _groupRepository.FindByIdAsync(long.Parse(groupId), cancellationToken);
                return _objectMapper.Map<ChatGroup, Group>(group);
            }
        }

        public virtual async Task<int> GetCountAsync(
            Guid? tenantId,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _groupRepository.GetCountAsync(filter, cancellationToken);
            }
        }

        public virtual async Task<List<Group>> GetListAsync(
            Guid? tenantId,
            string filter = null,
            string sorting = nameof(Group.Name),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var groups = await _groupRepository.GetListAsync(
                    filter,
                    sorting,
                    skipCount,
                    maxResultCount,
                    cancellationToken);

                return _objectMapper.Map<List<ChatGroup>, List<Group>>(groups);
            }
        }
    }
}
