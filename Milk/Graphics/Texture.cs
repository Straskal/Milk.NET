using SDL2;
using System;

namespace Milk.Graphics
{
    public class Texture : IDisposable
    {
        public Texture()
        {
            Handle = IntPtr.Zero;
            Width = 0;
            Height = 0;
        }

        public IntPtr Handle { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public void Initialize(IntPtr handle, int width, int height)
        {
            Handle = handle;
            Width = width;
            Height = height;
        }

        public void Dispose()
        {
            SDL.SDL_DestroyTexture(Handle);
            Handle = IntPtr.Zero;

            Width = 0;
            Height = 0;
        }
    }
}
