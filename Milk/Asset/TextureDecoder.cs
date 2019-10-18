using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

namespace Milk.Asset
{
    public class TextureDecoder : IDecoder
    {
        public object Decode(Stream stream)
        {
            var image = Image.Load<Rgba32>(stream);
            var span = image.GetPixelSpan();

            throw new NotImplementedException();
        }
    }
}
