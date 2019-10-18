using System.IO;

namespace Milk.Asset
{
    public interface IDecoder
    {
        object Decode(Stream stream);
    }
}
