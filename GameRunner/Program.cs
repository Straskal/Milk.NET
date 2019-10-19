using Milk;
using Milk.Graphics;
using Milk.Input;

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
                var buffer = window.Graphics.CreateBufferObject(
                    BufferObjectAttribute.DefaultAttributes,
                    new Vertex2f1Rgba[]
                    {
                        new Vertex2f1Rgba(0.5f, 0.5f, 0f, 1f, 0f, 1f),
                        new Vertex2f1Rgba(0.5f, -0.5f, 0f, 1f, 0f, 1f),
                        new Vertex2f1Rgba(-0.5f, 0.5f, 0f, 1f, 0f, 1f),
                        new Vertex2f1Rgba(0.5f, -0.5f, 0f, 1f, 0f, 1f),
                        new Vertex2f1Rgba(-0.5f, -0.5f, 0f, 1f, 0f, 1f),
                        new Vertex2f1Rgba(-0.5f, 0.5f, 0f, 1f, 0f, 1f),
                    }
                );

                window.OnCloseRequested += () => isRunning = false;
                Keyboard.OnKeyPressed += (Keys key) => { if (key == Keys.Escape) isRunning = false; };

                var graphics = window.Graphics;

                do
                {
                    graphics.Clear(0, 0, 0, 0);
                    graphics.DrawBuffer(graphics.DefaultShaderProgram, buffer, BufferDrawMode.Triangles);

                    window.SwapBuffers();
                    window.PollEvents();
                } while (isRunning);

                buffer.Dispose();
            }
        }
    }
}
