using Spire.Xls;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Exporter.Pdf.SpireLib;

public class SpireExcelToPdfProvider : IExcelToPdfProvider
{
    public virtual Task<Stream> ParseAsync(Stream excelStream, CancellationToken cancellationToken = default)
    {
        using var workBook = new Workbook();
        Stream memoryStream = new MemoryStream();
        workBook.LoadFromStream(excelStream);
        var workSheet = workBook.Worksheets[0];
        workSheet.SaveToPdfStream(memoryStream);

        return Task.FromResult(memoryStream);
    }
}
