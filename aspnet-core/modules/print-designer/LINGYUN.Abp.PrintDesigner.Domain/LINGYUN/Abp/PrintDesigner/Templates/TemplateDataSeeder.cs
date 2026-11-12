using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.PrintDesigner.Templates;

public class TemplateDataSeeder : ITemplateDataSeeder, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ITemplateRepository _templateRepository;
    public TemplateDataSeeder(
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        IJsonSerializer jsonSerializer,
        ITemplateRepository templateRepository)
    {
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _jsonSerializer = jsonSerializer;
        _templateRepository = templateRepository;
    }

    public async virtual Task SeedAsync(
        string name,
        string? designerData = null,
        string? extendedData = null,
        CancellationToken cancellationToken = default)
    {
        if (await _templateRepository.FindByNameAsync(name, cancellationToken: cancellationToken) == null)
        {
            var template = new Template(
                _guidGenerator.Create(),
                name,
                tenantId: _currentTenant.Id);
            template.SetData(designerData ?? "{}", extendedData ?? "{}");

            await _templateRepository.InsertAsync(template, cancellationToken: cancellationToken);
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
