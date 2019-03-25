using SDL2;
using System;

namespace Milk.Graphics
{
    public class SDLTexture : IDisposable
    {
        public IntPtr Texture { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public void Set(IntPtr texture)
        {
            SDL.SDL_QueryTexture(texture, out uint format, out int access, out int width, out int height);

            Texture = texture;
            Width = width;
            Height = height;
        }

        public void Dispose()
        {
            SDL.SDL_DestroyTexture(Texture);
            Texture = IntPtr.Zero;

            Width = 0;
            Height = 0;
        }
    }
}
