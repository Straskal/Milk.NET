using Milk.Graphics.OpenGL;
using Milk.Platform;
using System;
using System.Runtime.InteropServices;

namespace Milk.Graphics
{
    /// <summary>
    /// The GraphicsAdapter provides high level methods of communicating with the GPU.
    /// </summary>
    public class GraphicsAdapter : IDisposable
    {
        private readonly Window _window;
        private readonly GLFW.framebuffersizefun _onFrameBufferSizeChanged;
        private readonly BufferObject<float> _primitiveBufferObject;

        private bool _isVsyncEnabled;

        internal GraphicsAdapter(Window window)
        {
            _window = window;

            GL.Init(GLFW.GetProcAddress);
            GL.Enable(GL.BLENDING);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

            _isVsyncEnabled = false;
            _onFrameBufferSizeChanged = (IntPtr win, int w, int h) => GL.Viewport(0, 0, w, h);
            GLFW.SetFramebufferSizeCallback(window.Handle, Marshal.GetFunctionPointerForDelegate(_onFrameBufferSizeChanged));

            _primitiveBufferObject = new BufferObject<float>(
                new BufferObjectAttribute[]
                    {
                        new BufferObjectAttribute<float>(2),
                        new BufferObjectAttribute<float>(4)
                    });

            DefaultShaderProgram = ShaderProgram.LoadDefaultShader();
        }

        public ShaderProgram DefaultShaderProgram { get; }

        public bool IsVsyncEnabled
        {
            get => _isVsyncEnabled;
            set
            {
                if (value != _isVsyncEnabled)
                {
                    _isVsyncEnabled = value;
                    GLFW.SwapInterval(value ? 1 : 0);
                }
            }
        }

        /// <summary>
        /// Creates a new BufferObject with the given size and attributes.
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="size"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public BufferObject<TVertex> CreateBufferObject<TVertex>(BufferObjectAttribute[] attributes)
            where TVertex : unmanaged
        {
            return new BufferObject<TVertex>(attributes);
        }

        /// <summary>
        /// Clears the framebuffer.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="alpha"></param>
        public void Clear(float red, float green, float blue, float alpha)
        {
            GL.ClearColor(red, green, blue, alpha);
            GL.Clear(GL.COLOR_BUFFER_BIT);
        }

        /// <summary>
        /// Swap the front and back buffer.
        /// </summary>
        public void SwapFramebuffer()
        {
            GLFW.SwapBuffers(_window.Handle);
        }

        // TODO: Convert pixel coordinates to NDC.
        public void DrawFilledRectangle(float x, float y, float w, float h, float r, float g, float b, float a)
        {
            int frameBufferWidth = 0;
            int frameBufferHeight = 0;
            GLFW.GetFramebufferSize(_window.Handle, ref frameBufferWidth, ref frameBufferHeight);

            _primitiveBufferObject.Clear();
            _primitiveBufferObject.AddVertices(
                x, y, r, g, b, a,           // Top left
                x + w, y, r, g, b, a,       // Top right
                x + w, y - h, r, g, b, a,   // Bottom right
                x, y, r, g, b, a,           // Top left
                x, y - h, r, g, b, a,       // Bottom left
                x + w, y - h, r, g, b, a    // Bottom right
            );

            DrawBufferObject(DefaultShaderProgram, _primitiveBufferObject, BufferDrawMode.Triangles);
        }

        /// <summary>
        /// Draw a BufferObject with the given shader program and draw mode.
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="shaderProgram"></param>
        /// <param name="buffer"></param>
        /// <param name="mode"></param>
        public void DrawBufferObject<TVertex>(ShaderProgram shaderProgram, BufferObject<TVertex> buffer, BufferDrawMode mode = BufferDrawMode.Points)
            where TVertex : unmanaged
        {
            shaderProgram.Use();
            buffer.Draw(mode);
        }

        public void Dispose()
        {
            _primitiveBufferObject.Dispose();
            DefaultShaderProgram.Dispose();
        }
    }
}
