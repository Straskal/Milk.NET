namespace Milk.Asset
{
    public interface IAssetLoader
    {
        bool Initialize();

        object Load(string assetName);
    }
}
