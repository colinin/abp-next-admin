using Elsa.Workflows;
using Volo.Abp.Guids;

namespace LINGYUN.Abp.ElsaNext;

public class AbpElsaIdentityGenerator : IIdentityGenerator
{
    private readonly IGuidGenerator _guidGenerator;

    public AbpElsaIdentityGenerator(IGuidGenerator guidGenerator)
    {
        _guidGenerator = guidGenerator;
    }

    public string GenerateId()
    {
        return _guidGenerator.Create().ToString("N");
    }
}
