using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;

public class FormData : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public Guid FormDefinitionId { get; private set; }

    public string FormCode { get; private set; }

    public int FormVersion { get; private set; }

    public string InstanceId { get; private set; }

    public string DataJson { get; private set; } = default!;

    private FormData()
    {
        FormCode = "";
        InstanceId = "";
        DataJson = "{}";
    }

    public FormData(
        Guid id,
        Guid formDefinitionId,
        string formCode,
        int formVersion,
        string instanceId,
        string dataJson)
        : base(id)
    {
        FormDefinitionId = formDefinitionId;
        FormCode = Check.NotNullOrWhiteSpace(
            formCode,
            nameof(formCode),
            FormDataConsts.MaxFormCodeLength);
        FormVersion = Check.Positive(formVersion, nameof(formVersion));
        InstanceId = Check.NotNullOrWhiteSpace(
            instanceId,
            nameof(instanceId),
            FormDataConsts.MaxInstanceIdLength);
        SetData(dataJson);
    }

    public void SetData(string dataJson)
    {
        DataJson = Check.NotNullOrWhiteSpace(
            dataJson,
            nameof(dataJson),
            FormDataConsts.MaxDataJsonLength);
    }
}
