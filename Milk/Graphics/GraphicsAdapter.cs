using Milk.Graphics.OpenGL;
using Milk.Platform;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using ShaderSources = Milk.Constants.ShaderSource;

namespace Milk.Graphics
{
    /// <summary>
    /// The GraphicsAdapter provides high level methods of communicating with the GPU.
    /// </summary>
    public class GraphicsAdapter : IDisposable
    {
        private readonly Window _window;
        private readonly GLFW.framebuffersizefun _onFrameBufferSizeChanged;
        private readonly BufferObject<Vertex2f1Rgba> _primitiveBufferObject;

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

            _primitiveBufferObject = new BufferObject<Vertex2f1Rgba>(512, BufferObjectAttribute.DefaultAttributes);

            DefaultShaderProgram = LoadEmbeddedShader<Vertex2f1Rgba>(
                ShaderSources.DefaultVertex,
                ShaderSources.DefaultFragment
            );
        }

        public ShaderProgram<Vertex2f1Rgba> DefaultShaderProgram { get; }

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
        public BufferObject<TVertex> CreateBufferObject<TVertex>(int size, BufferObjectAttribute[] attributes) 
            where TVertex : unmanaged
        {
            return new BufferObject<TVertex>(size, attributes);
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

        public void SwapFramebuffer()
        {
            GLFW.SwapBuffers(_window.Handle);
        }

        /// <summary>
        /// Draw a BufferObject with the given shader program and draw mode.
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="shaderProgram"></param>
        /// <param name="buffer"></param>
        /// <param name="mode"></param>
        public void DrawBufferObject<TVertex>(ShaderProgram<TVertex> shaderProgram, BufferObject<TVertex> buffer, BufferDrawMode mode = BufferDrawMode.Points)
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

        private ShaderProgram<TVertex> LoadEmbeddedShader<TVertex>(string vertexResourcePath, string fragmentResourcePath)
        {
            Assembly assembly = typeof(GL).Assembly;
            string vertexShaderCode = string.Empty;
            string fragmentShaderCode = string.Empty;

            using (Stream shaderStream = assembly.GetManifestResourceStream(vertexResourcePath))
            using (StreamReader streamReader = new StreamReader(shaderStream))
                vertexShaderCode = streamReader.ReadToEnd();

            using (Stream shaderStream = assembly.GetManifestResourceStream(fragmentResourcePath))
            using (StreamReader streamReader = new StreamReader(shaderStream))
                fragmentShaderCode = streamReader.ReadToEnd();

            return new ShaderProgram<TVertex>(vertexShaderCode, fragmentShaderCode);
        }
    }
}
