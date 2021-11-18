using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    [Authorize(IdentityPermissions.IdentityClaimType.Default)]
    public class IdentityClaimTypeAppService : IdentityAppServiceBase, IIdentityClaimTypeAppService
    {
        protected IdentityClaimTypeManager IdentityClaimTypeManager { get; }

        protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }

        public IdentityClaimTypeAppService(
            IdentityClaimTypeManager identityClaimTypeManager,
            IIdentityClaimTypeRepository identityClaimTypeRepository)
        {
            IdentityClaimTypeManager = identityClaimTypeManager;
            IdentityClaimTypeRepository = identityClaimTypeRepository;
        }

        [Authorize(IdentityPermissions.IdentityClaimType.Create)]
        public virtual async Task<IdentityClaimTypeDto> CreateAsync(IdentityClaimTypeCreateDto input)
        {
            if (await IdentityClaimTypeRepository.AnyAsync(input.Name))
            {
                throw new UserFriendlyException(L["IdentityClaimTypeAlreadyExists", input.Name]);
            }
            var identityClaimType = new IdentityClaimType(
                GuidGenerator.Create(),
                input.Name,
                input.Required,
                input.IsStatic,
                input.Regex,
                input.RegexDescription,
                input.Description,
                input.ValueType
            );
            identityClaimType = await IdentityClaimTypeManager.CreateAsync(identityClaimType);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityClaimType, IdentityClaimTypeDto>(identityClaimType);
        }

        [Authorize(IdentityPermissions.IdentityClaimType.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var identityClaimType = await IdentityClaimTypeRepository.FindAsync(id);
            if (identityClaimType == null)
            {
                return;
            }
            CheckDeletionClaimType(identityClaimType);
            await IdentityClaimTypeRepository.DeleteAsync(identityClaimType);
        }

        public virtual async Task<IdentityClaimTypeDto> GetAsync(Guid id)
        {
            var identityClaimType = await IdentityClaimTypeRepository.FindAsync(id);

            return ObjectMapper.Map<IdentityClaimType, IdentityClaimTypeDto>(identityClaimType);
        }

        public virtual async Task<ListResultDto<IdentityClaimTypeDto>> GetAllListAsync()
        {
            var identityClaimTypes = await IdentityClaimTypeRepository
                .GetListAsync();

            return new ListResultDto<IdentityClaimTypeDto>(
                ObjectMapper.Map<List<IdentityClaimType>, List<IdentityClaimTypeDto>>(identityClaimTypes));
        }

        public virtual async Task<PagedResultDto<IdentityClaimTypeDto>> GetListAsync(IdentityClaimTypeGetByPagedDto input)
        {
            var identityClaimTypeCount = await IdentityClaimTypeRepository.GetCountAsync(input.Filter);

            var identityClaimTypes = await IdentityClaimTypeRepository
                .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityClaimTypeDto>(identityClaimTypeCount,
                ObjectMapper.Map<List<IdentityClaimType>, List<IdentityClaimTypeDto>>(identityClaimTypes));
        }

        [Authorize(IdentityPermissions.IdentityClaimType.Update)]
        public virtual async Task<IdentityClaimTypeDto> UpdateAsync(Guid id, IdentityClaimTypeUpdateDto input)
        {
            var identityClaimType = await IdentityClaimTypeRepository.GetAsync(id);
            CheckChangingClaimType(identityClaimType);
            identityClaimType.Required = input.Required;
            if (!string.Equals(identityClaimType.Regex, input.Regex, StringComparison.InvariantCultureIgnoreCase))
            {
                identityClaimType.Regex = input.Regex;
            }
            if (!string.Equals(identityClaimType.RegexDescription, input.RegexDescription, StringComparison.InvariantCultureIgnoreCase))
            {
                identityClaimType.RegexDescription = input.RegexDescription;
            }
            if (!string.Equals(identityClaimType.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                identityClaimType.Description = input.Description;
            }

            identityClaimType = await IdentityClaimTypeManager.UpdateAsync(identityClaimType);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityClaimType, IdentityClaimTypeDto>(identityClaimType);
        }

        protected virtual void CheckChangingClaimType(IdentityClaimType claimType)
        {
            if (claimType.IsStatic)
            {
                throw new BusinessException(IdentityErrorCodes.StaticClaimTypeChange);
            }
        }

        protected virtual void CheckDeletionClaimType(IdentityClaimType claimType)
        {
            if (claimType.IsStatic)
            {
                throw new BusinessException(IdentityErrorCodes.StaticClaimTypeDeletion);
            }
        }
    }
}
