using LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;
using LINGYUN.Abp.OperationRateLimiting.Algorithms.TokenBucket;
using LINGYUN.Abp.OperationRateLimiting.Dtos;
using LINGYUN.Abp.OperationRateLimiting.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.OperationRateLimiting;

[Authorize(AbpOperationRateLimitingPermissions.Policies.Default)]
public class OperationRateLimitingPolicyAppService :
    ApplicationService,
    IOperationRateLimitingPolicyAppService
{
    protected IOperationRateLimitingPolicyRecordRepository Repository { get; }

    protected IDynamicOperationRateLimitingPolicyStore DynamicPolicyStore { get; }

    protected IDistributedEventBus DistributedEventBus { get; }

    protected AbpOperationRateLimitingOptions RateLimitingOptions { get; }

    public OperationRateLimitingPolicyAppService(
        IOperationRateLimitingPolicyRecordRepository repository,
        IDynamicOperationRateLimitingPolicyStore dynamicPolicyStore,
        IDistributedEventBus distributedEventBus,
        IOptions<AbpOperationRateLimitingOptions> rateLimitingOptions)
    {
        Repository = repository;
        DynamicPolicyStore = dynamicPolicyStore;
        DistributedEventBus = distributedEventBus;
        RateLimitingOptions = rateLimitingOptions.Value;
    }

    public virtual async Task<OperationRateLimitingPolicyDto> GetAsync(Guid id)
    {
        var record = await Repository.GetAsync(id);

        return MapToDto(record);
    }

    public virtual async Task<OperationRateLimitingPolicyDto> GetByNameAsync(string name)
    {
        var record = await Repository.FindByNameAsync(name);

        if (record == null)
        {
            throw new EntityNotFoundException(typeof(OperationRateLimitingPolicyRecord), name);
        }

        return MapToDto(record);
    }

    public virtual async Task<PagedResultDto<OperationRateLimitingPolicyDto>> GetListAsync(
        OperationRateLimitingPolicyGetListInput input)
    {
        var specification = new OperationRateLimitingPolicyListSpecification(input);

        var totalCount = await Repository.GetCountAsync(specification);
        var records = await Repository.GetListAsync(
            specification,
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount);

        return new PagedResultDto<OperationRateLimitingPolicyDto>(
            totalCount,
            records.Select(MapToDto).ToList());
    }

    [Authorize(AbpOperationRateLimitingPermissions.Policies.Create)]
    public virtual async Task<OperationRateLimitingPolicyDto> CreateAsync(
        CreateOperationRateLimitingPolicyDto input)
    {
        var name = input.Name.Trim();

        if (await Repository.FindByNameAsync(name) != null)
        {
            throw new BusinessException(AbpOperationRateLimitingManagementErrorCodes.PolicyNameAlreadyExists)
                .WithData("Name", name);
        }

        var record = new OperationRateLimitingPolicyRecord(GuidGenerator.Create(), name, CurrentTenant.Id)
        {
            DisplayName = input.DisplayName,
            ErrorCode = input.ErrorCode,
            IsEnabled = input.IsEnabled
        };
        record.SetRules(CreateRuleRecords(record.Id, input.Rules));

        await Repository.InsertAsync(record, autoSave: true);
        await DynamicPolicyStore.InvalidateCacheAsync();

        return MapToDto(record);
    }

    [Authorize(AbpOperationRateLimitingPermissions.Policies.Update)]
    public virtual async Task<OperationRateLimitingPolicyDto> UpdateAsync(
        Guid id,
        UpdateOperationRateLimitingPolicyDto input)
    {
        var record = await Repository.GetAsync(id);

        record.DisplayName = input.DisplayName;
        record.ErrorCode = input.ErrorCode;
        record.IsEnabled = input.IsEnabled;
        record.SetRules(CreateRuleRecords(record.Id, input.Rules));

        await Repository.UpdateAsync(record, autoSave: true);
        await DynamicPolicyStore.InvalidateCacheAsync();

        return MapToDto(record);
    }

    [Authorize(AbpOperationRateLimitingPermissions.Policies.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var record = await Repository.GetAsync(id);

        await Repository.DeleteAsync(record, autoSave: true);
        await DynamicPolicyStore.InvalidateCacheAsync();
    }

    protected virtual List<OperationRateLimitingRuleDefinition> CreateRuleDefinitions(
        List<OperationRateLimitingRuleDto> rules)
    {
        if (rules == null || rules.Count == 0)
        {
            throw InvalidPolicy("A policy must contain at least one rule.");
        }

        var definitions = new List<OperationRateLimitingRuleDefinition>(rules.Count);

        foreach (var rule in rules)
        {
            if (RateLimitingOptions.GetAlgorithmType(rule.Algorithm) == null)
            {
                throw InvalidPolicy($"Undefined operation rate limiting algorithm: {rule.Algorithm}.");
            }

            if (rule.PartitionType == OperationRateLimitingPartitionType.Custom &&
                (string.IsNullOrWhiteSpace(rule.PartitionResolverName) ||
                 RateLimitingOptions.GetPartitionKeyResolverOrNull(rule.PartitionResolverName!) == null))
            {
                throw InvalidPolicy($"Undefined partition key resolver: {rule.PartitionResolverName}.");
            }

            var definition = new OperationRateLimitingRuleDefinition
            {
                Name = rule.Name,
                Algorithm = rule.Algorithm,
                Duration = TimeSpan.FromSeconds(rule.DurationSeconds),
                MaxCount = rule.MaxCount,
                PartitionType = rule.PartitionType,
                PartitionResolverName = rule.PartitionResolverName,
                IsMultiTenancyEnabled = rule.IsMultiTenancyEnabled,
                ErrorCode = rule.ErrorCode,
                Properties = new Dictionary<string, string>(rule.Properties, StringComparer.Ordinal)
            };

            SetAlgorithmSpecificProperties(definition, rule);

            definitions.Add(definition);
        }

        return definitions;
    }

    protected virtual List<OperationRateLimitingRuleRecord> CreateRuleRecords(
        Guid policyId,
        List<OperationRateLimitingRuleDto> rules)
    {
        return CreateRuleDefinitions(rules)
            .Select((definition, index) => definition.ToRecord(GuidGenerator.Create(), policyId, index))
            .ToList();
    }

    protected virtual void SetAlgorithmSpecificProperties(
        OperationRateLimitingRuleDefinition definition,
        OperationRateLimitingRuleDto rule)
    {
        if (string.Equals(definition.Algorithm, OperationRateLimitingAlgorithms.SlidingWindow, StringComparison.OrdinalIgnoreCase))
        {
            OperationRateLimitingRuleDefinitionSlidingWindowExtensions.SetSegments(definition, rule.Segments);
        }
        else if (string.Equals(definition.Algorithm, OperationRateLimitingAlgorithms.TokenBucket, StringComparison.OrdinalIgnoreCase))
        {
            OperationRateLimitingRuleDefinitionTokenBucketExtensions.SetTokensPerPeriod(definition, rule.TokensPerPeriod);
        }
    }

    protected virtual OperationRateLimitingPolicyDto MapToDto(OperationRateLimitingPolicyRecord record)
    {
        return new OperationRateLimitingPolicyDto
        {
            Id = record.Id,
            Name = record.Name,
            DisplayName = record.DisplayName,
            IsEnabled = record.IsEnabled,
            ErrorCode = record.ErrorCode,
            Rules = record.Rules
                .OrderBy(rule => rule.SortOrder)
                .Select(rule => MapToDto(rule.ToDefinition()))
                .ToList(),
            CreationTime = record.CreationTime,
            LastModificationTime = record.LastModificationTime
        };
    }

    protected virtual OperationRateLimitingRuleDto MapToDto(OperationRateLimitingRuleDefinition rule)
    {
        var dto = new OperationRateLimitingRuleDto
        {
            Name = rule.Name,
            Algorithm = rule.Algorithm,
            DurationSeconds = (int)Math.Ceiling(rule.Duration.TotalSeconds),
            MaxCount = rule.MaxCount,
            PartitionType = rule.PartitionType,
            PartitionResolverName = rule.PartitionResolverName,
            IsMultiTenancyEnabled = rule.IsMultiTenancyEnabled,
            ErrorCode = rule.ErrorCode,
            Properties = new Dictionary<string, string>(rule.Properties, StringComparer.Ordinal)
        };

        FillAlgorithmSpecificProperties(dto, rule);

        return dto;
    }

    protected virtual void FillAlgorithmSpecificProperties(
        OperationRateLimitingRuleDto dto,
        OperationRateLimitingRuleDefinition rule)
    {
        if (string.Equals(rule.Algorithm, OperationRateLimitingAlgorithms.SlidingWindow, StringComparison.OrdinalIgnoreCase))
        {
            dto.Segments = OperationRateLimitingRuleDefinitionSlidingWindowExtensions.GetSegments(rule);
        }
        else if (string.Equals(rule.Algorithm, OperationRateLimitingAlgorithms.TokenBucket, StringComparison.OrdinalIgnoreCase))
        {
            dto.TokensPerPeriod = OperationRateLimitingRuleDefinitionTokenBucketExtensions.GetTokensPerPeriod(rule);
        }
    }

    protected virtual BusinessException InvalidPolicy(string reason)
    {
        return new BusinessException(AbpOperationRateLimitingManagementErrorCodes.InvalidPolicy)
            .WithData("Reason", reason);
    }

    private class OperationRateLimitingPolicyListSpecification : Specification<OperationRateLimitingPolicyRecord>
    {
        private readonly OperationRateLimitingPolicyGetListInput _input;

        public OperationRateLimitingPolicyListSpecification(OperationRateLimitingPolicyGetListInput input)
        {
            _input = input;
        }

        public override Expression<Func<OperationRateLimitingPolicyRecord, bool>> ToExpression()
        {
            return policy =>
                (string.IsNullOrWhiteSpace(_input.Filter) ||
                 policy.Name.Contains(_input.Filter!) ||
                 (policy.DisplayName != null && policy.DisplayName.Contains(_input.Filter!))) &&
                (!_input.IsEnabled.HasValue || policy.IsEnabled == _input.IsEnabled.Value) &&
                (string.IsNullOrWhiteSpace(_input.Algorithm) ||
                 policy.Rules.Any(rule => rule.Algorithm == _input.Algorithm));
        }
    }
}
