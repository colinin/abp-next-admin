using Microsoft.Extensions.Options;
using MiniSoftware;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Exporter.MiniSoftware;

public class MiniWordExporterProvider : IWordExporterProvider, ITransientDependency
{
    private readonly AbpExporterMiniWordOptions _exporterOptions;
    private readonly IVirtualFileProvider _virtualFileProvider;
    public MiniWordExporterProvider(
        IVirtualFileProvider virtualFileProvider, 
        IOptions<AbpExporterMiniWordOptions> exporterOptions)
    {
        _virtualFileProvider = virtualFileProvider;
        _exporterOptions = exporterOptions.Value;
    }

    public async virtual Task<Stream> ExportAsync(byte[] template, object data, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsByTemplateAsync(template, data, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream;
    }

    public async virtual Task<Stream> ExportAsync(object data, CancellationToken cancellationToken = default)
    {
        var dataType = data.GetType();
        if (dataType.IsGenericType)
        {
            dataType = dataType.GetGenericArguments()[0];
        }

        if (!_exporterOptions.ExportTemplateMapping.TryGetValue(dataType, out var filePath))
        {
            throw new AbpException($"No Word template file configuration of type {dataType} was found!");
        }

        byte[]? template = null;
        if (File.Exists(filePath))
        {
            using (var fs = File.OpenRead(filePath))
            {
                template = await fs.GetAllBytesAsync(cancellationToken);
            }
        }
        else
        {
            var templeFileInfo = _virtualFileProvider.GetFileInfo(filePath);
            if (templeFileInfo.Exists)
            {
                using (var fs = templeFileInfo.CreateReadStream())
                {
                    template = await fs.GetAllBytesAsync(cancellationToken);
                }
            }
        }

        if (template == null)
        {
            throw new AbpException($"The Word template file for the {dataType} type configuration {filePath} does not exist!");
        }

        return await ExportAsync(template, data, cancellationToken);
    }
}
