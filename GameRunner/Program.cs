using Milk;
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
                window.OnCloseRequested += () => isRunning = false;
                Keyboard.OnKeyPressed += (Keys key) => { if (key == Keys.Escape) isRunning = false; };

                var graphics = window.Graphics;
                graphics.IsVsyncEnabled = true;

                while (isRunning)
                {
                    window.PollEvents();
                    graphics.Clear(0, 0, 0, 0);
                    graphics.DrawFilledRectangle(0f, 0f, 0.5f, 0.5f, 1, 0, 0, 1);
                    graphics.SwapFramebuffer();
                }
            }
        }
    }
}
