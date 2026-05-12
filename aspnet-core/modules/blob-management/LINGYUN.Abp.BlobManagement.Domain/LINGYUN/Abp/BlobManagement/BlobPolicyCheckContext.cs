using System;

namespace LINGYUN.Abp.BlobManagement;

public class BlobPolicyCheckContext
{
    public IServiceProvider ServiceProvider { get; }
    public string PolicyName { get; }
    public Blob Blob { get; }
    public BlobPolicyCheckContext(
        IServiceProvider serviceProvider, 
        string policyName,
        Blob blob)
    {
        ServiceProvider = serviceProvider;
        PolicyName = policyName;
        Blob = blob;
    }
}
