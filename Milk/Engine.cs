using Milk.Asset;
using Milk.Graphics;
using Milk.Window;
using SDL2;
using System;

namespace Milk
{
    public sealed class Engine : IDisposable
    {
        private static Engine instance;

        public static Engine Instance
        {
            get
            {
                if (instance == null)
                    instance = new Engine();

                return instance;
            }
        }

        private Engine() { }

        public GameWindow Window { get; private set; }
        public Renderer Renderer { get; private set; }
        public AssetManager AssetManager { get; private set; }

        private Texture texture;

        private bool isRunning;

        public bool Initialize()
        {
            Window = new GameWindow();
            if (!Window.Initialize("milk", 1280, 720, false))
                return false;

            Renderer = new Renderer();
            if (!Renderer.Initialize(Window.Handle, 640, 360))
                return false;

            AssetManager = new AssetManager();
            if (!AssetManager.Initialize())
                return false;

            texture = AssetManager.Load<Texture>("res/player.png");

            return true;
        }

        public bool Run()
        {
            try
            {
                isRunning = true;

                const float MILLISECONDS_PER_FRAME = 1000 / 60; // = 16

                while (isRunning)
                {
                    uint startTicks = SDL.SDL_GetTicks();

                    HandleEvents();
                    Update(MILLISECONDS_PER_FRAME);
                    Render();

                    uint endTicks = SDL.SDL_GetTicks() - startTicks;
                    if (endTicks < MILLISECONDS_PER_FRAME)
                        SDL.SDL_Delay((uint)MILLISECONDS_PER_FRAME - endTicks);
                }
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error, e.Message);
                return false;
            }

            return true;
        }

        private void HandleEvents()
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
                        Window.ToggleFullscreen();
                }
            }
        }

        private void Update(float deltaTime)
        {
        }

        private void Render()
        {
            Renderer.BeginDraw();
            Renderer.Draw(texture, new Math.Vector2(10, 10), new Math.Rectangle(0, 0, 64, 64));
            Renderer.EndDraw();
        }

        public void Dispose()
        {
            Renderer.Dispose();
            Window.Dispose();
        }
    }
}
