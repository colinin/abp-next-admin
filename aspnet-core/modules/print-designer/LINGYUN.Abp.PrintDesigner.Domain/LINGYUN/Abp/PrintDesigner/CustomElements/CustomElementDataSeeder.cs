using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.PrintDesigner.CustomElements;

public class CustomElementDataSeeder : ICustomElementDataSeeder, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICustomElementRepository _customElementRepository;
    public CustomElementDataSeeder(
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IJsonSerializer jsonSerializer,
        ICustomElementRepository customElementRepository)
    {
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _jsonSerializer = jsonSerializer;
        _customElementRepository = customElementRepository;
    }

    public async virtual Task SeedAsync(
        string name,
        string? designerData = null,
        string? extendedData = null,
        CancellationToken cancellationToken = default)
    {
        if (await _customElementRepository.FindByNameAsync(name, cancellationToken: cancellationToken) == null)
        {
            var customElement = new CustomElement(
                _guidGenerator.Create(),
                name,
                tenantId: _currentTenant.Id);
            customElement.SetData(designerData ?? "{}", extendedData ?? "{}");

            await _customElementRepository.InsertAsync(customElement, cancellationToken: cancellationToken);
        }
    }

    public async virtual Task SeedAsync(
        string name,
        Dictionary<string, object>? designerData = null,
        Dictionary<string, object>? extendedData = null,
        CancellationToken cancellationToken = default)
    {
        var data = _jsonSerializer.Serialize(designerData ?? new Dictionary<string, object>());
        var extData = _jsonSerializer.Serialize(extendedData ?? new Dictionary<string, object>());
        await SeedAsync(name, data, extData, cancellationToken);
    }
}
