using Milk.Graphics.OpenGL;
using Milk.Platform;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using ShaderSources = Milk.Constants.ShaderSource;

namespace Milk.Graphics
{
    public class GraphicsAdapter : IDisposable
    {
        private readonly GLFW.framebuffersizefun _onFrameBufferSizeChanged;

        private readonly BufferObject<Vertex2f1Rgba> _primitiveBufferObject;

        internal GraphicsAdapter(Window window)
        {
            GL.Init(GLFW.GetProcAddress);
            GL.Enable(GL.BLENDING);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

            GLFW.SwapInterval(1);

            _onFrameBufferSizeChanged = (IntPtr win, int w, int h) => GL.Viewport(0, 0, w, h);
            GLFW.SetFramebufferSizeCallback(window.Handle, Marshal.GetFunctionPointerForDelegate(_onFrameBufferSizeChanged));

            _primitiveBufferObject = new BufferObject<Vertex2f1Rgba>(512, BufferObjectAttribute.DefaultAttributes);

            DefaultShaderProgram = LoadEmbeddedShader(
                ShaderSources.DefaultVertex,
                ShaderSources.DefaultFragment
            );
        }

        public ShaderProgram DefaultShaderProgram { get; }

        public BufferObject<TVertex> CreateBufferObject<TVertex>(int size, BufferObjectAttribute[] attributes) 
            where TVertex : unmanaged
        {
            return new BufferObject<TVertex>(size, attributes);
        }

        public BufferObject<TVertex> CreateBufferObject<TVertex>(BufferObjectAttribute[] attributes, TVertex[] vertices)
            where TVertex : unmanaged
        {
            var buffer = new BufferObject<TVertex>(vertices.Length, attributes);
            buffer.AddVertices(vertices);
            return buffer;
        }

        public void Clear(float red, float green, float blue, float alpha)
        {
            GL.ClearColor(red, green, blue, alpha);
            GL.Clear(GL.COLOR_BUFFER_BIT);
        }

        public void DrawBuffer<TVertex>(
            ShaderProgram shaderProgram, 
            BufferObject<TVertex> buffer, 
            BufferDrawMode mode = BufferDrawMode.Points)
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

        private ShaderProgram LoadEmbeddedShader(string vertexResourcePath, string fragmentResourcePath)
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

            return new ShaderProgram(vertexShaderCode, fragmentShaderCode);
        }
    }
}
