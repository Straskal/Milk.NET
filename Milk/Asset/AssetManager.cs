using Milk.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Milk.Asset
{
    public class AssetManager : IDisposable
    {
        private readonly Dictionary<Type, IAssetLoader> assetLoaders;
        private readonly Dictionary<string, object> loadedAssets;

        public AssetManager()
        {
            assetLoaders = new Dictionary<Type, IAssetLoader>
            {
                { typeof(Texture), new TextureLoader() }
            };

            loadedAssets = new Dictionary<string, object>();
        }

        public bool Initialize()
        {
            return assetLoaders.All(loader => loader.Value.Initialize());
        }

        public void Dispose()
        {
            foreach (var asset in loadedAssets)
            {
                if (asset.Value is IDisposable disposable)
                    disposable.Dispose();
            }

            loadedAssets.Clear();
        }

        public T Load<T>(string assetName) where T : class
        {
            if (loadedAssets.TryGetValue(assetName, out object existingAsset))
            {
                if (existingAsset != null)
                {
                     if (existingAsset is T existingAssetAsT)
                        return existingAssetAsT;

                    Logger.Log(LogLevel.Warning, $"Failed to load {assetName} as {typeof(T)}");
                    return null;
                }
            }

            if (assetLoaders.TryGetValue(typeof(T), out IAssetLoader loader))
            {
                var loadedAsset = loader.Load(assetName);

                if (loadedAsset is T assetAsT)
                {
                    loadedAssets.Add(assetName, loadedAsset);
                    return assetAsT;
                }

                // TODO: The initial load of an asset does not dispose of it if it wasn't loaded as the correct type.
                //       For now, this is fine. The asset will be cleaned up eventually. But it might warrant a more
                //       descriptive message and disposable of "failed" asset.
                Logger.Log(LogLevel.Warning, $"Failed to load {assetName} as {typeof(T)}");
                return null;
            }

            return null;
        }
    }
}
