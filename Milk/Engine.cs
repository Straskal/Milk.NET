using Milk.Asset;
using Milk.Graphics;
using Milk.Window;
using SDL2;

namespace Milk
{
    /// <summary>
    /// As of right now. this is a bs class for testing.
    /// </summary>
    public sealed class Engine
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
                            Window.ToggleFullscreen();
                    }
                }

                Renderer.BeginDraw();
                Renderer.Draw(texture, new Math.Vector2(10, 10), new Math.Rectangle(0, 0, 64, 64));
                Renderer.EndDraw();
            }

            Renderer.Dispose();
            Window.Dispose();
        }
    }
}
