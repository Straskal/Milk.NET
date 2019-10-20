using GlmNet;
using Milk.Gfx.OpenGL;
using Milk.Platform;
using Milk.Platform.Events;
using System;

namespace Milk.Gfx
{
    /// <summary>
    /// The GraphicsAdapter provides high level methods of communicating with the GPU.
    /// </summary>
    public class Graphics : IDisposable
    {
        private readonly Window _window;
        private readonly ShaderProgram _defaultShaderProgram;
        private readonly BufferObject<float> _defaultBufferObject;

        private bool _isVsyncEnabled;
        private mat4 _orthoProjection;

        internal Graphics(Window window)
        {
            _window = window;

            GL.Init(_window.GetProcAddress);
            GL.Enable(GL.BLENDING);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

            _isVsyncEnabled = false;
            _orthoProjection = glm.ortho(0f, _window.Width, _window.Height, 0f, 0f, 1f);

            _defaultBufferObject = new BufferObject<float>(
                new BufferObjectAttribute[]
                    {
                        new BufferObjectAttribute<float>(2),
                        new BufferObjectAttribute<float>(4)
                    });

            _defaultShaderProgram = ShaderProgram.LoadDefaultShader();
            _window.FramebufferSizeChanged += OnFramebufferSizeChanged;
        }

        public bool IsVsyncEnabled
        {
            get => _isVsyncEnabled;
            set
            {
                if (value != _isVsyncEnabled)
                {
                    _isVsyncEnabled = value;
                    _window.ToggleVsync(_isVsyncEnabled);
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
            _window.SwapFramebuffers();
        }

        public void DrawFilledRectangle(float x, float y, float w, float h, float r, float g, float b, float a)
        {
            _defaultBufferObject.Clear();
            _defaultBufferObject.AddVertices(
                x, y, r, g, b, a,           // Top left
                x + w, y, r, g, b, a,       // Top right
                x + w, y - h, r, g, b, a,   // Bottom right
                x, y, r, g, b, a,           // Top left
                x, y - h, r, g, b, a,       // Bottom left
                x + w, y - h, r, g, b, a    // Bottom right
            );

            _defaultShaderProgram.SetMatrix4x4Uniform("projectionMatrix", _orthoProjection);
            DrawBufferObject(_defaultShaderProgram, _defaultBufferObject, BufferDrawMode.Triangles);
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
            _window.FramebufferSizeChanged -= OnFramebufferSizeChanged;
            _defaultBufferObject.Dispose();
            _defaultShaderProgram.Dispose();
        }

        private void OnFramebufferSizeChanged(object sender, FramebufferSizeChangedEventArgs eventArgs)
        {
            GL.Viewport(0, 0, eventArgs.Width, eventArgs.Height);
            _orthoProjection = glm.ortho(0f, eventArgs.Width, eventArgs.Height, 0f, 0f, 1f);
        }
    }
}
