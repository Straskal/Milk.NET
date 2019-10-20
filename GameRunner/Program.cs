using Milk.Input;
using Milk.Pltf;

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

            using (var window = Window.Create(windowParams))
            {
                window.CloseRequested += (object sender, WindowCloseRequestedEventArgs e) => isRunning = false;
                Keyboard.OnKeyPressed += (Keys key) => { if (key == Keys.Escape) isRunning = false; };

                var graphics = window.Graphics;
                graphics.IsVsyncEnabled = true;

                while (isRunning)
                {
                    window.PollEvents();
                    graphics.Clear(0, 0, 0, 0);
                    graphics.DrawFilledRectangle(100f, 200f, 100f, 100f, 1, 0, 0, 1);
                    graphics.SwapFramebuffer();
                }
            }
        }
    }
}
