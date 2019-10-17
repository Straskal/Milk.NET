using System;
using Milk.Graphics;

namespace Milk
{
    public class Program
    {
        // Triangle vertices
        private static float[] vertices = {
            -0.5f, -0.5f,
            0.5f, -0.5f,
            0.0f,  0.5f,
        };

        public static void Main(string[] args)
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
                window.OnCloseRequested += () => isRunning = false;
                Keyboard.OnKeyPressed += (Keys key) => { if (key == Keys.Escape) isRunning = false; };

                do
                {
                    window.Renderer.Clear(0, 0, 0, 0);
                    window.Renderer.DrawTriangle();
                    window.SwapBuffers();
                    window.PollEvents();
                } while (isRunning);
            }
        }
    }
}
