﻿using Milk.Window;
using SDL2;

namespace Milk
{
    /// <summary>
    /// As of right now. this is a bs class for testing.
    /// </summary>
    public sealed class Game
    {
        private GameWindow window;
        private Renderer renderer;
        private bool isRunning;

        public bool Initialize()
        {
            window = new GameWindow();

            if (!window.Initialize("milk", 800, 600, false))
                return false;

            renderer = new Renderer();

            if (!renderer.Initialize(window.Handle, 640, 360))
                return false;

            return true;
        }

        public void Run()
        {
            isRunning = true;

            while (isRunning)
            {
                while (SDL.SDL_PollEvent(out var e) != 0)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        isRunning = false;

                    if (e.type == SDL.SDL_EventType.SDL_KEYUP)
                    {
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                            isRunning = false;

                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_f)
                            window.ToggleFullscreen();
                    }
                }

                renderer.BeginDraw();
                renderer.EndDraw();
            }

            renderer.Dispose();
            window.Dispose();
        }
    }
}
