using SDL2;
using System;

namespace Milk.Window
{
    public sealed class SDLRenderer : IDisposable
    {
        private bool isInitialized;

        public SDLRenderer()
        {
            isInitialized = false;
            ResolutionWidth = 0;
            ResolutionHeight = 0;
        }

        public IntPtr Renderer { get; private set; }
        public int ResolutionWidth { get; private set; }
        public int ResolutionHeight { get; private set; }

        public bool Initialize(IntPtr window, int resolutionWidth, int resolutionHeight)
        {
            if (isInitialized)
                return true;

            ResolutionWidth = resolutionWidth;
            ResolutionHeight = resolutionHeight;

            Renderer = SDL.SDL_CreateRenderer(
                window, 
                -1, 
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

            if (Renderer == IntPtr.Zero)
            {
                // Log error.
                return false;
            }

            SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "nearest");
            SDL.SDL_RenderSetLogicalSize(Renderer, ResolutionWidth, ResolutionHeight);
            SDL.SDL_SetRenderDrawBlendMode(Renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            isInitialized = true;
            return true;
        }

        public void BeginDraw()
        {
            SDL.SDL_SetRenderDrawColor(Renderer, 0x00, 0x00, 0x00, 0xFF);
            SDL.SDL_RenderClear(Renderer);
        }

        public void EndDraw()
        {
            SDL.SDL_RenderPresent(Renderer);
        }

        public void Free()
        {
            SDL.SDL_DestroyRenderer(Renderer);
        }

        public void Dispose()
        {
            SDL.SDL_DestroyRenderer(Renderer);
            Renderer = IntPtr.Zero;
        }
    }
}
