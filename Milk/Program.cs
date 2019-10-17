using System;
using Milk.Platform;

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
                IsBordered = false,
                IsResizable = false,
                IsFullscreen = false
            };

            using (var window = new Window(windowParams))
            {
                window.OnCloseRequested += () => isRunning = false;

                // Create the VAO
                uint vao = 0;
                GL.GenVertexArrays(1, ref vao);
                GL.BindVertexArray(vao);

                // Create the VBO
                uint vbo = 0;
                GL.GenBuffers(1, ref vbo);
                GL.BindBuffer(GL.ARRAY_BUFFER, vbo);
                GL.BufferData(GL.ARRAY_BUFFER, new IntPtr(sizeof(float) * vertices.Length), vertices, GL.STATIC_DRAW);

                // Draw the Triangle
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 2, GL.FLOAT, false, 0, IntPtr.Zero);

                do
                {
                    GL.ClearColor(0.0F, 0.0F, 0.0F, 1.0F);
                    GL.Clear(GL.COLOR_BUFFER_BIT);
                    GL.DrawArrays(GL.TRIANGLES, 0, 3);

                    window.SwapBuffers();
                    window.PollEvents();
                } while (isRunning);
            }
        }
    }
}
