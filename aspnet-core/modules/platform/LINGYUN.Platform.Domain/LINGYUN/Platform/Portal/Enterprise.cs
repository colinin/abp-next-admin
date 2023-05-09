using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Platform.Portal;
/// <summary>
/// 企业信息
/// </summary>
public class Enterprise : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 租户标识
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; protected set; }
    /// <summary>
    /// 英文名称
    /// </summary>
    public virtual string EnglishName { get; set; }
    /// <summary>
    /// Logo
    /// </summary>
    public virtual string Logo { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    public virtual string Address { get; set; }
    /// <summary>
    /// 法人代表
    /// </summary>
    public virtual string LegalMan { get; set; }
    /// <summary>
    /// 税务登记号
    /// </summary>
    public virtual string TaxCode { get; set; }
    /// <summary>
    /// 组织机构代码
    /// </summary>
    public virtual string OrganizationCode { get; protected set; }
    /// <summary>
    /// 注册代码
    /// </summary>
    public virtual string RegistrationCode { get; protected set; }
    /// <summary>
    /// 注册日期
    /// </summary>
    public virtual DateTime? RegistrationDate { get; protected set; }
    /// <summary>
    /// 过期日期
    /// </summary>
    public virtual DateTime? ExpirationDate { get; protected set; }
    protected Enterprise() 
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public Enterprise(
        string name,
        string address, 
        string taxCode, 
        string organizationCode = null, 
        string registrationCode = null, 
        DateTime? registrationDate = null, 
        DateTime? expirationDate = null,
        Guid? tenantId = null)
    {
        Address = Check.Length(address, nameof(address), EnterpriseConsts.MaxAddressLength);
        TaxCode = Check.Length(taxCode, nameof(taxCode), EnterpriseConsts.MaxTaxCodeLength);
        TenantId = tenantId;

        SetName(name);
        SetOrganization(organizationCode);
        SetRegistration(registrationCode, registrationDate, expirationDate);

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties(); 
    }

    public void SetTenantId(Guid? tenantId = null)
    {
        TenantId = tenantId;
    }

    public void SetName(string name, string englishName = null)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), EnterpriseConsts.MaxNameLength);
        EnglishName = Check.Length(englishName, nameof(englishName), EnterpriseConsts.MaxEnglishNameLength);
    }

    public void SetOrganization(string organizationCode)
    {
        OrganizationCode = Check.Length(organizationCode, nameof(organizationCode), EnterpriseConsts.MaxOrganizationCodeLength);
    }

    public void SetRegistration(
        string registrationCode,
        DateTime? registrationDate = null,
        DateTime? expirationDate = null)
    {
        RegistrationCode = Check.Length(registrationCode, nameof(registrationCode), EnterpriseConsts.MaxRegistrationCodeLength);
        RegistrationDate = registrationDate;
        ExpirationDate = expirationDate;
    }
}
