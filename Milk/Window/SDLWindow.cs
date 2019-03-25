using SDL2;
using System;

namespace Milk.Window
{
    public sealed class SDLWindow : IDisposable
    {
        private bool isInitialized;

        public SDLWindow()
        {
            isInitialized = false;

            Window = IntPtr.Zero;
            Title = "milk";
            Width = 0;
            Height = 0;
            IsFullscreen = false;
        }

        public IntPtr Window { get; private set; }
        public string Title { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool IsFullscreen { get; private set; }

        public bool Initialize(string title, int width, int height, bool fullscreen)
        {
            if (isInitialized)
                return true;

            Title = title;
            Width = width;
            Height = height;

            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                // Log error.
                return false;
            }

            Window = SDL.SDL_CreateWindow(
                Title, 
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED, 
                Width, 
                Height, 
                SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            if (Window == IntPtr.Zero)
            {
                // Log error.
                return false;
            }

            if (IsFullscreen)
                ToggleFullscreen();

            isInitialized = true;
            return true;
        }

        public void ToggleFullscreen()
        {
            IsFullscreen = !IsFullscreen;

            if (IsFullscreen)
                SDL.SDL_SetWindowFullscreen(Window, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
            else
                SDL.SDL_SetWindowFullscreen(Window, 0);
        }

        public void Free()
        {
            SDL.SDL_DestroyWindow(Window);
        }

        public void Dispose()
        {
            SDL.SDL_DestroyWindow(Window);
            Window = IntPtr.Zero;
        }
    }
}
