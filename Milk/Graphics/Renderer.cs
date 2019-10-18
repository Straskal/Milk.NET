using Milk.OpenGL;
using System;
using System.IO;
using System.Reflection;

namespace Milk.Graphics
{
    public class Renderer : IDisposable
    {
        private readonly ShaderProgram _defaultShaderProgram;

        unsafe internal Renderer()
        {
            _defaultShaderProgram = LoadEmbeddedShader(
                "Milk.Graphics.Shaders.DefaultVertexShader.glsl",
                "Milk.Graphics.Shaders.DefaultFragmentShader.glsl"
            );
        }

        public BufferObject CreateBufferObject(BufferObjectAttribute[] attributes, int size)
        {
            return new BufferObject(attributes, size);
        }

        public BufferObject CreateBufferObject(BufferObjectAttribute[] attributes, Vertex[] vertices)
        {
            return new BufferObject(attributes, vertices);
        }

        public void Clear(float red, float green, float blue, float alpha)
        {
            GL.ClearColor(red, green, blue, alpha);
            GL.Clear(GL.COLOR_BUFFER_BIT);
        }

        public void DrawBuffer(BufferObject buffer, BufferDrawMode mode = BufferDrawMode.Points)
        {
            _defaultShaderProgram.Use();
            buffer.Draw(mode);
        }

        private ShaderProgram LoadEmbeddedShader(string vertexResourcePath, string fragmentResourcePath)
        {
            string vertexShaderCode = string.Empty;
            string fragmentShaderCode = string.Empty;
            Assembly assembly = typeof(GL).Assembly;

            using (Stream shaderStream = assembly.GetManifestResourceStream(vertexResourcePath))
            using (StreamReader streamReader = new StreamReader(shaderStream))
                vertexShaderCode = streamReader.ReadToEnd();

            using (Stream shaderStream = assembly.GetManifestResourceStream(fragmentResourcePath))
            using (StreamReader streamReader = new StreamReader(shaderStream))
                fragmentShaderCode = streamReader.ReadToEnd();

            return new ShaderProgram(vertexShaderCode, fragmentShaderCode);
        }

        public void Dispose()
        {
            _defaultShaderProgram.Dispose();
        }
    }
}
