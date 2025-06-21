using Shouldly;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.IO;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace LINGYUN.Abp.Exporter.Pdf;
public abstract class ExcelToPdfProvider_Tests<TStartupModule> : AbpExporterPdfTestBase<TStartupModule>
        where TStartupModule : IAbpModule
{
    private readonly IExcelToPdfProvider _excelToPdfProvider;
    private readonly IVirtualFileProvider _virtualFileProvider;
    public ExcelToPdfProvider_Tests()
    {
        _excelToPdfProvider = GetRequiredService<IExcelToPdfProvider>();
        _virtualFileProvider = GetRequiredService<IVirtualFileProvider>();
    }

    [Fact]
    public async virtual Task Should_Parsed()
    {
        var excelFileInfo = _virtualFileProvider.GetFileInfo("/LINGYUN/Abp/Exporter/Pdf/Resources/test.xlsx");
        using (var excelStream = excelFileInfo.CreateReadStream())
        {
            using (var pdfStream = await _excelToPdfProvider.ParseAsync(excelStream))
            {
                pdfStream.Seek(0, System.IO.SeekOrigin.Begin);
                pdfStream.Length.ShouldBePositive();
            }
        }
    }
}
