using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement.FileSystem.ImageSharp
{
    public class ImageSharpProcesserContributor : IFileSystemOssObjectProcesserContributor
    {
        protected static readonly string[] ImageTypes = new string[]
        {
            "6677",// bmp
            "7173",// gif
            "13780",// png
            "255216"// jpg
        };
        
        public virtual async Task ProcessAsync(FileSystemOssObjectContext context)
        {
            var copyStream = context.OssObject.Content;
            var bytes = await copyStream.GetAllBytesAsync();

            if (IsImage(bytes))
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
            using var image = Image.Load(fileBytes, out var format);

            // 大小
            var width = GetInt32Prarm(args, "w_");
            var height = GetInt32Prarm(args, "h_");
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
            var encoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(format);
            image.Save(imageStream, encoder);
            imageStream.Seek(0, SeekOrigin.Begin);

            content = imageStream;
            return true;
        }

        private static bool IsImage(byte[] fileBytes)
        {
            if (fileBytes.IsNullOrEmpty())
            {
                return false;
            }

            string fileclass = "";
            for (int i = 0; i < 2; i++)
            {
                fileclass += fileBytes[i].ToString();
            }

            return ImageTypes.Any(type => type.Equals(fileclass));
        }

        private static string GetString(string[] args, string key)
        {
            if (!args.Any())
            {
                return null;
            }

            return args
                .Where(arg => arg.StartsWith(key))
                .Select(arg => arg.Substring(key.Length))
                .FirstOrDefault();
        }

        private static string GetInt32Prarm(string[] args, string key)
        {
            if (!args.Any())
            {
                return null;
            }

            return args
                .Where(arg => arg.StartsWith(key))
                .Select(arg => arg.Substring(key.Length))
                .FirstOrDefault(arg => int.TryParse(arg, out _));
        }
    }
}
