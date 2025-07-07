using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.IO;

namespace LINGYUN.Abp.Exporter.Pdf.LibreOffice;
public class LibreOfficeExcelToPdfProvider : IExcelToPdfProvider
{
    public async virtual Task<Stream> ParseAsync(Stream excelStream, CancellationToken cancellationToken = default)
    {
        var outputPath = Path.Combine(Path.GetTempPath(), "excel2pdf");

        DirectoryHelper.CreateIfNotExists(outputPath);

        var templateFileId = Guid.NewGuid().ToString();
        var tempExcelFile = Path.Combine(outputPath, $"{templateFileId}.xlsx");
        var tempPdfFile = Path.Combine(outputPath, $"{templateFileId}.pdf");

        try
        {
            if (!File.Exists(tempExcelFile))
            {
                using (var excelFile = File.Create(tempExcelFile))
                {
                    await excelStream.CopyToAsync(excelFile, cancellationToken);
                }
            }

            await LibreOfficeCommands.ExcelToPdf(tempExcelFile, outputPath, cancellationToken);

            var pdfStream = new MemoryStream();

            using (var pdfFileStream = File.OpenRead(tempPdfFile))
            {
                await pdfFileStream.CopyToAsync(pdfStream, cancellationToken);
                pdfStream.Seek(0, SeekOrigin.Begin);
            }

            return pdfStream;
        }
        catch
        {
            throw;
        }
        finally
        {
            FileHelper.DeleteIfExists(tempExcelFile);
            FileHelper.DeleteIfExists(tempPdfFile);
        }
    }
}
