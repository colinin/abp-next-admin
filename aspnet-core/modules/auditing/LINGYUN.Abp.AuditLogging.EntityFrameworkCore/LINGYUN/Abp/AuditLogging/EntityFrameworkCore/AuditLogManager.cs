using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore
{
    [Dependency(ReplaceServices = true)]
    public class AuditLogManager : IAuditLogManager, ISingletonDependency
    {
        protected IObjectMapper ObjectMapper { get; }
        protected IAuditLogRepository AuditLogRepository { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected AbpAuditingOptions Options { get; }
        protected IAuditLogInfoToAuditLogConverter Converter { get; }

        public ILogger<AuditLogManager> Logger { protected get; set; }

        public AuditLogManager(
            IObjectMapper objectMapper,
            IAuditLogRepository auditLogRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IOptions<AbpAuditingOptions> options,
            IAuditLogInfoToAuditLogConverter converter)
        {
            ObjectMapper = objectMapper;
            AuditLogRepository = auditLogRepository;
            UnitOfWorkManager = unitOfWorkManager;
            Converter = converter;
            Options = options.Value;

            Logger = NullLogger<AuditLogManager>.Instance;
        }


        public virtual async Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            Guid? userId = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            string clientId = null,
            string clientIpAddress = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await AuditLogRepository.GetCountAsync(
                startTime,
                endTime,
                httpMethod,
                url,
                userId,
                userName,
                applicationName,
                clientIpAddress,
                correlationId,
                maxExecutionDuration,
                minExecutionDuration,
                hasException,
                httpStatusCode,
                cancellationToken);
        }

        public virtual async Task<List<AuditLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string httpMethod = null,
            string url = null,
            Guid? userId = null,
            string userName = null,
            string applicationName = null,
            string correlationId = null,
            string clientId = null,
            string clientIpAddress = null,
            int? maxExecutionDuration = null,
            int? minExecutionDuration = null,
            bool? hasException = null,
            HttpStatusCode? httpStatusCode = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var auditLogs = await AuditLogRepository.GetListAsync(
                sorting,
                maxResultCount,
                skipCount,
                startTime,
                endTime,
                httpMethod,
                url,
                userId,
                userName,
                applicationName,
                clientIpAddress,
                correlationId,
                maxExecutionDuration,
                minExecutionDuration,
                hasException,
                httpStatusCode,
                includeDetails,
                cancellationToken);

            return ObjectMapper.Map<List<Volo.Abp.AuditLogging.AuditLog>, List<AuditLog>>(auditLogs);
        }

        public virtual async Task<AuditLog> GetAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var auditLog = await AuditLogRepository.GetAsync(id, includeDetails, cancellationToken);

            return ObjectMapper.Map<Volo.Abp.AuditLogging.AuditLog, AuditLog>(auditLog);
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using (var uow = UnitOfWorkManager.Begin(true))
            {
                await AuditLogRepository.DeleteAsync(id);
                await uow.CompleteAsync();
            }
        }

        public virtual async Task<string> SaveAsync(
            AuditLogInfo auditInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!Options.HideErrors)
            {
                return await SaveLogAsync(auditInfo, cancellationToken);
            }

            try
            {
                return await SaveLogAsync(auditInfo, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not save the audit log object: " + Environment.NewLine + auditInfo.ToString());
                Logger.LogException(ex, LogLevel.Error);
            }
            return "";
        }

        protected virtual async Task<string> SaveLogAsync(
            AuditLogInfo auditInfo,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var uow = UnitOfWorkManager.Begin(true))
            {
                var auditLog = await AuditLogRepository.InsertAsync(
                    await Converter.ConvertAsync(auditInfo),
                    false,
                    cancellationToken);
                await uow.CompleteAsync();

                return auditLog.Id.ToString();
            }
        }
    }
}
