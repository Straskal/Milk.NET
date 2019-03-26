using Milk.Asset;

namespace Milk.Scene.Callbacks
{
    public interface ILoadable
    {
        void OnLoad(IAssetManager assetManager);
    }
}
