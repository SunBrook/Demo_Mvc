/*
 * @author: S 2024/9/29 19:24:39
 */

using Demo_Mvc.Common.Models;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Demo_Mvc.Common.Tools
{
    /// <summary>
    /// 图片处理
    /// </summary>
    public class ImageSharpModule
    {
        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="fullPath">图片路径</param>
        /// <param name="prefix">图片后缀名</param>
        /// <param name="stream">图片流</param>
        /// <param name="imageSharpSetting">压缩设置</param>
        /// <returns>处理结果</returns>
        public static DisposeResult Compress(string fullPath, string? prefix = null,
            Stream? stream = null, ImageSharpSetting? imageSharpSetting = null)
        {
            try
            {
                if (!File.Exists(fullPath) && stream == null)
                {
                    // 文件不存在
                    return new DisposeResult { Result = false, ErrorMsg = $"文件不存在: {fullPath}" };
                }

                if (imageSharpSetting == null)
                {
                    imageSharpSetting = new ImageSharpSetting
                    {
                        Quality = 75,
                        IsResize = true,
                        SizeWidth = 1920,
                        SizeHeight = 1080
                    };
                }

                // 临时文件夹，方便比较压缩文件
                var tempDir = Path.Combine(Path.GetTempPath(), "RESULT FILE");
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                // 常用图片后缀名过滤
                var picSuffixs = new string[] { ".bmp", ".gif", ".jpeg", ".jpg", ".ppm", ".png", ".tif", ".tiff", ".tga", ".webp" };

                string suffix;

                if (stream == null)
                {
                    var file = new FileInfo(fullPath);
                    suffix = file.Extension;
                }
                else
                {
                    suffix = prefix ?? string.Empty;
                }

                // 图片后缀检查
                if (!picSuffixs.Contains(suffix))
                {
                    return new DisposeResult { Result = false, ErrorMsg = $"图片格式不支持：{suffix}" };
                }

                SixLabors.ImageSharp.Image image;

                if (stream == null)
                {
                    image = SixLabors.ImageSharp.Image.Load(fullPath);
                }
                else
                {
                    image = SixLabors.ImageSharp.Image.Load(stream);
                }

                // 超出桌面分辨率，按照最大分辨率去调整尺寸
                if (imageSharpSetting.IsResize)
                {
                    if (image.Width > imageSharpSetting.SizeWidth || image.Height > imageSharpSetting.SizeHeight)
                    {
                        var widthRate = (imageSharpSetting.SizeWidth / image.Width).ToDecimal(2);
                        var heightRate = (imageSharpSetting.SizeHeight / image.Width).ToDecimal(2);
                        var resizeRate = widthRate > heightRate ? widthRate : heightRate;
                        image.Mutate(x => x.Resize((int)(image.Width * resizeRate), (int)(image.Height * resizeRate)));
                    }
                }

                var fileName = Path.GetFileName(fullPath);
                var outFilePath = Path.Combine(tempDir, fileName);

                if (suffix == ".gif")
                {
                    // gif 特殊处理
                    using (var fs = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        image.Save(fs, new GifEncoder
                        {
                            Quantizer = KnownQuantizers.Wu
                        });
                    }
                }
                else
                {
                    // 其他静态文件，存储为jpg格式
                    using (var fs = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        image.Save(fs, new JpegEncoder
                        {
                            Quality = imageSharpSetting.Quality
                        });
                    }
                }

                image.Dispose();

                // 比较压缩完的图片 和 未压缩图片的大小，如果小于源文件，则进行移动覆盖操作，否则直接删除源文件
                var outPathFile = new FileInfo(outFilePath);
                if (File.Exists(fullPath))
                {
                    var originalPathFile = new FileInfo(fullPath);
                    if (outPathFile.Length < originalPathFile.Length)
                    {
                        File.Copy(outFilePath, fullPath, overwrite: true);
                    }
                }
                else
                {
                    File.Copy(outFilePath, fullPath, overwrite: true);
                }

                outPathFile.Delete();

                return new DisposeResult { Result = true };
            }
            catch (Exception ex)
            {
                return new DisposeResult { Result = false, ErrorMsg = ex.Message };
            }
        }
    }

    /// <summary>
    /// 图片压缩设置
    /// </summary>
    public class ImageSharpSetting
    {
        /// <summary>
        /// 图片质量
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// 图片超出预设的宽高后，是否等比例缩放
        /// </summary>
        public bool IsResize { get; set; }

        /// <summary>
        /// 预设宽
        /// </summary>
        public decimal SizeWidth { get; set; }

        /// <summary>
        /// 预设高
        /// </summary>
        public decimal SizeHeight { get; set; }
    }
}
