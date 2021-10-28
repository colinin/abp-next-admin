using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.SecurityLog;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore
{
    [Dependency(ReplaceServices = true)]
    public class SecurityLogManager : ISecurityLogManager, ISingletonDependency
    {
        public ILogger<SecurityLogManager> Logger { get; set; }

        protected IObjectMapper ObjectMapper { get; }
        protected AbpSecurityLogOptions SecurityLogOptions { get; }
        protected IIdentitySecurityLogRepository IdentitySecurityLogRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }

        public SecurityLogManager(
            IObjectMapper objectMapper,
            ILogger<SecurityLogManager> logger,
            IOptions<AbpSecurityLogOptions> securityLogOptions,
            IIdentitySecurityLogRepository identitySecurityLogRepository,
            IGuidGenerator guidGenerator,
            IUnitOfWorkManager unitOfWorkManager)
        {
            Logger = logger;
            ObjectMapper = objectMapper;
            SecurityLogOptions = securityLogOptions.Value;
            IdentitySecurityLogRepository = identitySecurityLogRepository;
            GuidGenerator = guidGenerator;
            UnitOfWorkManager = unitOfWorkManager;
        }

        public virtual async Task SaveAsync(
            SecurityLogInfo securityLogInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!SecurityLogOptions.IsEnabled)
            {
                return;
            }

            using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
            {
                await IdentitySecurityLogRepository.InsertAsync(
                    new IdentitySecurityLog(GuidGenerator, securityLogInfo),
                    false,
                    cancellationToken);
                await uow.CompleteAsync();
            }
        }

        public virtual async Task<SecurityLog> GetAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var securityLog = await IdentitySecurityLogRepository.GetAsync(id, includeDetails, cancellationToken);

            return ObjectMapper.Map<IdentitySecurityLog, SecurityLog>(securityLog);
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using (var uow = UnitOfWorkManager.Begin(true))
            {
                await IdentitySecurityLogRepository.DeleteAsync(id);
                await uow.CompleteAsync();
            }
        }

        public virtual async Task<List<SecurityLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null,
            string identity = null,
            string action = null,
            Guid? userId = null,
            string userName = null,
            string clientId = null,
            string clientIpAddress = null,
            string correlationId = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var securityLogs = await IdentitySecurityLogRepository.GetListAsync(
                sorting,
                maxResultCount,
                skipCount,
                startTime,
                endTime,
                applicationName,
                identity,
                action,
                userId,
                userName,
                clientId,
                correlationId,
                includeDetails,
                cancellationToken);

            return ObjectMapper.Map<List<IdentitySecurityLog>, List<SecurityLog>>(securityLogs);
        }


        public virtual async Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null,
            string identity = null,
            string action = null,
            Guid? userId = null,
            string userName = null,
            string clientId = null,
            string clientIpAddress = null,
            string correlationId = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await IdentitySecurityLogRepository.GetCountAsync(
                startTime,
                endTime,
                applicationName,
                identity,
                action,
                userId,
                userName,
                clientId,
                correlationId,
                cancellationToken);
        }
    }
}
