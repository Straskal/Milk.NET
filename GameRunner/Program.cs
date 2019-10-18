using Milk;
using Milk.Graphics;

namespace GameRunner
{
    class Program
    {
        static void Main()
        {
            bool isRunning = true;

            var windowParams = new WindowParameters
            {
                Title = "Whatep!",
                Width = 800,
                Height = 600,
                IsBordered = true,
                IsResizable = true,
                IsFullscreen = false
            };

            using (var window = new Window(windowParams))
            {
                var buffer = window.Renderer.CreateBufferObject(
                    new Vertex(0.5f, 0.5f),
                    new Vertex(0.5f, -0.5f),
                    new Vertex(-0.5f, 0.5f),
                    new Vertex(0.5f, -0.5f),
                    new Vertex(-0.5f, -0.5f),
                    new Vertex(-0.5f, 0.5f)
                );

                window.OnCloseRequested += () => isRunning = false;
                Keyboard.OnKeyPressed += (Keys key) => { if (key == Keys.Escape) isRunning = false; };

                do
                {
                    window.Renderer.Clear(0, 0, 0, 0);
                    window.Renderer.DrawBuffer(buffer);
                    window.SwapBuffers();
                    window.PollEvents();
                } while (isRunning);
            }
        }
    }
}
