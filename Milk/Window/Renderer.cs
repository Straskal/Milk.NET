using SDL2;
using System;

namespace Milk.Window
{
    public sealed class Renderer : IDisposable
    {
        private bool isInitialized;

        public Renderer()
        {
            isInitialized = false;

            Handle = IntPtr.Zero;
            ResolutionWidth = 0;
            ResolutionHeight = 0;
        }

        public IntPtr Handle { get; private set; }
        public int ResolutionWidth { get; private set; }
        public int ResolutionHeight { get; private set; }

        public bool Initialize(IntPtr windowHandle, int resolutionWidth, int resolutionHeight)
        {
            if (isInitialized)
                return true;

            ResolutionWidth = resolutionWidth;
            ResolutionHeight = resolutionHeight;

            Handle = SDL.SDL_CreateRenderer(
                windowHandle, 
                -1, 
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (Handle == IntPtr.Zero)
            {
                // Log error.
                return false;
            }

            SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "nearest");
            SDL.SDL_RenderSetLogicalSize(Handle, ResolutionWidth, ResolutionHeight);
            SDL.SDL_SetRenderDrawBlendMode(Handle, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            isInitialized = true;
            return true;
        }

        public void BeginDraw()
        {
            SDL.SDL_SetRenderDrawColor(Handle, 0x00, 0x00, 0x00, 0xFF);
            SDL.SDL_RenderClear(Handle);
        }

        public void EndDraw()
        {
            SDL.SDL_RenderPresent(Handle);
        }

        public void Dispose()
        {
            SDL.SDL_DestroyRenderer(Handle);
            Handle = IntPtr.Zero;
        }
    }
}
