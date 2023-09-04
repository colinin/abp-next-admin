using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Imaging;

namespace LINGYUN.Abp.OssManagement.FileSystem.Imaging;
public class AbpImagingProcesserContributor : IFileSystemOssObjectProcesserContributor
{
    public async virtual Task ProcessAsync(FileSystemOssObjectContext context)
    {
        Stream processSreeam = new MemoryStream();
        var copyStream = context.OssObject.Content;
        await copyStream.CopyToAsync(processSreeam);
        copyStream.Seek(0, SeekOrigin.Begin);
        var bytes = await copyStream.GetAllBytesAsync();

        if (bytes.IsImage())
        {
            var args = context.Process.Split(',');

            if (Resize(args, out var resizeArgs))
            {
                var imageResizer = context.ServiceProvider.GetRequiredService<IImageResizer>();
                var resizeResult = await imageResizer.ResizeAsync(processSreeam, resizeArgs);
                if (resizeResult.State == ImageProcessState.Done)
                {
                    processSreeam = resizeResult.Result;
                }
            }
            if (Compress(args))
            {
                var imageCompressor = context.ServiceProvider.GetRequiredService<IImageCompressor>();
                var compressResult = await imageCompressor.CompressAsync(processSreeam);
                if (compressResult.State == ImageProcessState.Done)
                {
                    processSreeam = compressResult.Result;
                }
            }
            if (processSreeam.Length != copyStream.Length)
            {
                context.SetContent(processSreeam);
                // 释放原图形流数据
                await copyStream.DisposeAsync();
            }
            else
            {
                await processSreeam.DisposeAsync();
            }
        }
    }

    protected virtual bool Resize(string[] args, out ImageResizeArgs resizeArgs)
    {
        // 大小
        var width = args.GetInt32Prarm("w_");
        var height = args.GetInt32Prarm("h_");
        if (!width.IsNullOrWhiteSpace() &&
            !height.IsNullOrWhiteSpace())
        {
            resizeArgs = new ImageResizeArgs(
                int.Parse(width),
                int.Parse(height));
            return true;
        }
        resizeArgs = null;
        return false;
    }

    protected virtual bool Compress(string[] args)
    {
        return args.Contains("cm");
    }
}
