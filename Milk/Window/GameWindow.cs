using SDL2;
using System;

namespace Milk.Window
{
    public sealed class GameWindow : IDisposable
    {
        private bool isInitialized;

        public GameWindow()
        {
            isInitialized = false;

            Handle = IntPtr.Zero;
            Title = string.Empty;
            Width = 0;
            Height = 0;
            IsFullscreen = false;
        }

        public IntPtr Handle { get; private set; }
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

            SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");

            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Logger.Log(LogLevel.Error, $"Error initializing SDL: {SDL.SDL_GetError()}");
                return false;
            }

            Handle = SDL.SDL_CreateWindow(
                Title, 
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED, 
                Width, 
                Height, 
                SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

            if (Handle == IntPtr.Zero)
            {
                Logger.Log(LogLevel.Error, $"Error creating SDL window: {SDL.SDL_GetError()}");
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
                SDL.SDL_SetWindowFullscreen(Handle, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
            else
                SDL.SDL_SetWindowFullscreen(Handle, 0);
        }

        public void Dispose()
        {
            SDL.SDL_DestroyWindow(Handle);
            Handle = IntPtr.Zero;

            SDL.SDL_Quit();
        }
    }
}
