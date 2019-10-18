using Milk.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Milk.Asset
{
    public class AssetCache
    {
        private readonly IStreamProvider _streamProvider;
        private readonly Dictionary<Type, IDecoder> _decoders;
        private readonly Dictionary<string, object> _loadedAssets;

        public AssetCache(IStreamProvider streamProvider)
        {
            _streamProvider = streamProvider;
            _decoders = new Dictionary<Type, IDecoder>()
            {
                { typeof(Texture), new TextureDecoder() }
            };

            _loadedAssets = new Dictionary<string, object>();
        }

        public T Load<T>(string path) where T : class
        {
            if (_loadedAssets.TryGetValue(path, out object existingAsset))
                if (existingAsset != null)
                {
                    if (existingAsset is T existingAssetAsT)
                        return existingAssetAsT;

                    throw new ArgumentException($"{path} cannot be loaded as a(n) {typeof(T).Name}");
                }

            if (_decoders.TryGetValue(typeof(T), out IDecoder decoder))
                using (Stream stream = _streamProvider.GetStream(path))
                {
                    var asset = decoder.Decode(stream);
                    if (asset is T requested)
                        return requested;
                }

            return null;
        }
    }
}
