using System;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Portal;

public abstract class EnterpriseCreateOrUpdateDto 
{
    /// <summary>
    /// 关联租户
    /// </summary>
    public Guid? TenantId { get; set; }
    /// <summary>
    /// 英文名称
    /// </summary>
    [DynamicStringLength(typeof(EnterpriseConsts), nameof(EnterpriseConsts.MaxEnglishNameLength))]
    public string EnglishName { get; set; }
    /// <summary>
    /// Logo
    /// </summary>
    [DynamicStringLength(typeof(EnterpriseConsts), nameof(EnterpriseConsts.MaxLogoLength))]
    public string Logo { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    [DynamicStringLength(typeof(EnterpriseConsts), nameof(EnterpriseConsts.MaxAddressLength))]
    public string Address { get; set; }
    /// <summary>
    /// 法人代表
    /// </summary>
    [DynamicStringLength(typeof(EnterpriseConsts), nameof(EnterpriseConsts.MaxLegalManLength))]
    public string LegalMan { get; set; }
    /// <summary>
    /// 税务登记号
    /// </summary>
    [DynamicStringLength(typeof(EnterpriseConsts), nameof(EnterpriseConsts.MaxTaxCodeLength))]
    public string TaxCode { get; set; }
    /// <summary>
    /// 组织机构代码
    /// </summary>
    [DynamicStringLength(typeof(EnterpriseConsts), nameof(EnterpriseConsts.MaxOrganizationCodeLength))]
    public string OrganizationCode { get; set; }
    /// <summary>
    /// 注册代码
    /// </summary>
    [DynamicStringLength(typeof(EnterpriseConsts), nameof(EnterpriseConsts.MaxRegistrationCodeLength))]
    public string RegistrationCode { get; set; }
    /// <summary>
    /// 注册日期
    /// </summary>
    public DateTime? RegistrationDate { get; set; }
    /// <summary>
    /// 过期日期
    /// </summary>
    public DateTime? ExpirationDate { get; set; }
}
