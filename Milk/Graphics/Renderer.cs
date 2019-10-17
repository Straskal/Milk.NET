using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Milk.Graphics
{
    public class Renderer
    {
        private readonly uint _vertexArrayObject;
        private readonly uint _vertexBufferObject;

        private readonly ShaderProgram _defaultShaderProgram;

        private readonly float[] _vertices = {
            -0.5f, -0.5f,
            0.5f, -0.5f,
            0.0f,  0.5f,
        };

        unsafe internal Renderer()
        {
            GL.GenVertexArrays(1, ref _vertexArrayObject);
            GL.BindVertexArray(_vertexArrayObject);
            GL.GenBuffers(1, ref _vertexBufferObject);
            GL.BindBuffer(GL.ARRAY_BUFFER, _vertexBufferObject);

            fixed (float* temp = &_vertices[0])
                GL.BufferData(
                    GL.ARRAY_BUFFER,
                    new IntPtr(sizeof(float) * _vertices.Length),
                    new IntPtr((void*)temp),
                    GL.STATIC_DRAW
                );

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, GL.FLOAT, false, 0, IntPtr.Zero);
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            GL.BindVertexArray(0);

            _defaultShaderProgram = LoadEmbeddedShader(
                "Milk.Graphics.Shaders.DefaultVertexShader.glsl",
                "Milk.Graphics.Shaders.DefaultFragmentShader.glsl"
            );
        }

        public void Clear(float red, float green, float blue, float alpha)
        {
            GL.ClearColor(red, green, blue, alpha);
            GL.Clear(GL.COLOR_BUFFER_BIT);
        }

        public void DrawTriangle()
        {
            _defaultShaderProgram.Use();
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(GL.TRIANGLES, 0, 3);
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
    }
}
