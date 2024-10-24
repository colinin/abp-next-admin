using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement.ImageSharp;

public class ImageSharpOssObjectProcesser : IOssObjectProcesserContributor
{
    public async virtual Task ProcessAsync(OssObjectProcesserContext context)
    {
        var copyStream = context.OssObject.Content;
        var bytes = await copyStream.GetAllBytesAsync();

        if (bytes.IsImage())
        {
            var args = context.Process.Split(',');
            if (DrawGraphics(bytes, args, out var content))
            {
                context.SetContent(content);

                // 释放原图形流数据
                await copyStream.DisposeAsync();
            }
        }
    }

    protected virtual bool DrawGraphics(byte[] fileBytes, string[] args, out Stream content)
    {
        using var image = Image.Load(fileBytes);

        // 大小
        var width = args.GetInt32Prarm("w_");
        var height = args.GetInt32Prarm("h_");
        if (!width.IsNullOrWhiteSpace() &&
            !height.IsNullOrWhiteSpace())
        {
            image.Mutate(x => x.Resize(int.Parse(width), int.Parse(height)));
        }

        // 水印
        //var txt = GetString(args, "t_");
        //if (!txt.IsNullOrWhiteSpace())
        //{
        //    FontCollection fonts = new FontCollection();
        //    FontFamily fontfamily = fonts.Install("本地字体.TTF");
        //    var font = new Font(fontfamily, 20, FontStyle.Bold);
        //    var size = TextMeasurer.Measure(txt, new RendererOptions(font));

        //    image.Mutate(x => x.DrawText(txt, font, Color.WhiteSmoke,
        //        new PointF(image.Width - size.Width - 3, image.Height - size.Height - 3)));
        //}

        // TODO: 其他处理参数及现有的优化

        var imageStream = new MemoryStream();
        image.Save(imageStream, image.Metadata.DecodedImageFormat);
        imageStream.Seek(0, SeekOrigin.Begin);

        content = imageStream;
        return true;
    }
}
