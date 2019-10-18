using Milk.Graphics.OpenGL;
using System;
using System.IO;
using System.Reflection;

using ShaderSources = Milk.Constants.ShaderSource;

namespace Milk.Graphics
{
    public class Renderer : IDisposable
    {
        internal Renderer()
        {
            GL.Enable(GL.BLENDING);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

            DefaultShaderProgram = LoadEmbeddedShader(
                ShaderSources.DefaultVertex,
                ShaderSources.DefaultFragment
            );
        }

        public ShaderProgram DefaultShaderProgram { get; }

        public BufferObject CreateBufferObject(BufferObjectAttribute[] attributes, Vertex[] vertices)
        {
            var buff = new BufferObject(vertices.Length, attributes);
            buff.AddVertices(vertices);
            return buff;
        }

        public void Clear(float red, float green, float blue, float alpha)
        {
            GL.ClearColor(red, green, blue, alpha);
            GL.Clear(GL.COLOR_BUFFER_BIT);
        }

        public void DrawBuffer(BufferObject buffer, BufferDrawMode mode = BufferDrawMode.Points)
        {
            DefaultShaderProgram.Use();
            buffer.Draw(mode);
        }

        public void Dispose()
        {
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
