using Milk.Graphics;
using SDL2;
using System;

namespace Milk.Asset
{
    // TODO: Set blank white placeholder texture for textures that cannot be loaded.
    public class TextureLoader : IAssetLoader
    {
        public bool Initialize()
        {
            var imageFlags = SDL_image.IMG_InitFlags.IMG_INIT_JPG | SDL_image.IMG_InitFlags.IMG_INIT_PNG;

            if (SDL_image.IMG_Init(imageFlags) < 0)
            {
                Logger.Log(LogLevel.Error, $"Error initializing SDL_image!");
                return false;
            }
            return true;
        }

        public object Load(string assetName)
        {
            IntPtr surfaceHandle = SDL_image.IMG_Load(assetName);
            if (surfaceHandle == IntPtr.Zero)
            {
                Logger.Log(LogLevel.Error, $"Error loading surface {assetName}");
                return null;
            }
            IntPtr textureHandle = SDL.SDL_CreateTextureFromSurface(Engine.Instance.Renderer.Handle, surfaceHandle);
            SDL.SDL_FreeSurface(surfaceHandle);
            SDL.SDL_QueryTexture(textureHandle, out uint format, out int access, out int w, out int h);
            var texture = new Texture();
            texture.Initialize(textureHandle, w, h);
            return texture;
        }
    }
}
