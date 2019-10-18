using System.IO;

namespace Milk.Asset
{
    public interface IStreamProvider
    {
        Stream GetStream(string path);
    }
}
