using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using StandDigital.Core.Models;
using System.Collections.Concurrent;

namespace StandDigital.Extensions;

public static class ImageResize
{
    private static IImageEncoder jpgEncoder => new JpegEncoder() { Quality = 100 };
    private static IImageEncoder pngEncoder => new PngEncoder();

    public static async Task<(byte[] imageStream,bool isPrimary)[]> ResizePhotos(this List<ImageUpload> imagesAbsolutePaths,string? primaryPhoto)
    {
        ConcurrentBag<(byte[],bool)> resultDic = [];

        await Parallel.ForEachAsync(imagesAbsolutePaths, async (image, cancellation) =>
        {
            using (Image imageSharpImg = Image.Load(image.AbsoluteNameWithPath))
            {
                imageSharpImg.Mutate(x => x.Resize(600, 400));
                using (var ms = new MemoryStream())
                {
                    imageSharpImg.Save(ms, ImageEncoder(image.ImageName));
                    ms.Position = 0;

                    if(Equals(image.ImageName,primaryPhoto))
                        resultDic.Add((ms.ToArray(),true));
                }
            }
        });

        return [.. resultDic];
    }

    private static IImageEncoder ImageEncoder(string imageName) =>
        imageName.Substring(imageName.LastIndexOf('.')) switch
        {
            ".jpeg" or ".jpg" => jpgEncoder,
            ".png" => pngEncoder,
            _ => jpgEncoder
        };
}
