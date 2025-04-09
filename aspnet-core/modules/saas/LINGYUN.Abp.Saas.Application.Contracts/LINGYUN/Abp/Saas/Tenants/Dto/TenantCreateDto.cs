using LINGYUN.Abp.Saas.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantCreateDto : TenantCreateOrUpdateBase
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string AdminEmailAddress { get; set; }

    [Required]
    [MaxLength(128)]
    public string AdminPassword { get; set; }

    /// <summary>
    /// 使用共享数据库
    /// </summary>
    public bool UseSharedDatabase { get; set; } = true;

    /// <summary>
    /// 默认数据库连接字符串
    /// </summary>
    [DynamicStringLength(typeof(TenantConnectionStringConsts), nameof(TenantConnectionStringConsts.MaxValueLength))]
    public string DefaultConnectionString { get; set; }

    /// <summary>
    /// 其他数据库连接
    /// </summary>
    public List<TenantConnectionStringSetInput> ConnectionStrings { get; set; } = new List<TenantConnectionStringSetInput>();

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = base.Validate(validationContext);

        if (!UseSharedDatabase && DefaultConnectionString.IsNullOrWhiteSpace())
        {
            var saasResource = validationContext.GetRequiredService<IStringLocalizer<AbpSaasResource>>();

            var errors = new ValidationResult[1]
            {
                new ValidationResult(
                    saasResource["IfUseCustomDataBaseDefaultConnectionStringIsRequiredMessage"],
                    new string[1]{ nameof(DefaultConnectionString) })
            };
            return validationResults.Union(errors);
        }

        return validationResults;
    }
}