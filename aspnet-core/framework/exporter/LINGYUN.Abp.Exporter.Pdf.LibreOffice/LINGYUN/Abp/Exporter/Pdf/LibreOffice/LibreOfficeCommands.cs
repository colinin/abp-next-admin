using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Exporter.Pdf.LibreOffice;

public class LibreOfficeCommands
{
    public static string WindowsCli { get; set; } = "soffice.com";
    public static string WindowsCliDir { get; set; } = "C:\\Program Files\\LibreOffice\\program\\";

    public static string UnixCli { get; set; } = "libreoffice";
    public static string UnixCliDir { get; set; } = "";

    public static string GetCli()
    {
        if (OperatingSystem.IsWindows())
        {
            return Path.Combine(WindowsCliDir, WindowsCli);
        }

        // 详细的操作系统版本: https://zh-cn.libreoffice.org/get-help/system-requirements/
        if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
        {
            return Path.Combine(UnixCliDir, UnixCli);
        }

        throw new PlatformNotSupportedException($"The current platform {Environment.OSVersion.ToString()} does not support the libreoffice runtime library");
    }

    public static bool IsLibreOffliceInstalled()
    {
        return LibreOfficeCommands.IsLibreOfficeAvailable(GetCli());
    }
    /// <summary>
    /// LibreOffice是否可用
    /// </summary>
    /// <param name="commandFile"></param>
    /// <returns></returns>
    public static bool IsLibreOfficeAvailable(string commandFile)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = commandFile,
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return process.ExitCode == 0 && output.Contains("LibreOffice");
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// Excel转换为Pdf
    /// </summary>
    /// <param name="excelFile"></param>
    /// <param name="outputPath"></param>
    /// <exception cref="Exception"></exception>
    public async static Task ExcelToPdf(string excelFile, string outputPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var start = new ProcessStartInfo
        {
            FileName = GetCli(),
            Arguments = $"--headless --convert-to pdf \"{excelFile}\" --outdir \"{outputPath}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        var process = new Process
        {
            StartInfo = start,
        };
        process.Start();

        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0)
        {
            throw new Exception($"Excel failed to convert to PDF. Error code: {process.ExitCode}");
        }
    }
}
