namespace Milk.Asset
{
    public interface IAssetManager
    {
        T Load<T>(string assetName) where T : class;
    }
}
