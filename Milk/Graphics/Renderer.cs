using System;
using System.IO;
using System.Reflection;

namespace Milk.Graphics
{
    public class Renderer
    {
        private readonly uint _vertexArrayObject;
        private readonly uint _vertexBufferObject;

        private string _vertexShaderCode;
        private string _fragmentShaderCode;

        private float[] _vertices = {
            -0.5f, -0.5f,
            0.5f, -0.5f,
            0.0f,  0.5f,
        };

        internal Renderer()
        {
            GL.GenVertexArrays(1, ref _vertexArrayObject);
            GL.BindVertexArray(_vertexArrayObject);
            GL.GenBuffers(1, ref _vertexBufferObject);
            GL.BindBuffer(GL.ARRAY_BUFFER, _vertexBufferObject);
            GL.BufferData(GL.ARRAY_BUFFER, new IntPtr(sizeof(float) * _vertices.Length), _vertices, GL.STATIC_DRAW);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, GL.FLOAT, false, 0, IntPtr.Zero);
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            GL.BindVertexArray(0);

            LoadShaders();
        }

        public void Clear(float red, float green, float blue, float alpha)
        {
            GL.ClearColor(red, green, blue, alpha);
            GL.Clear(GL.COLOR_BUFFER_BIT);
        }

        public void DrawTriangle()
        {
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(GL.TRIANGLES, 0, 3);
        }

        private void LoadShaders()
        {
            Assembly assembly = typeof(GL).Assembly;

            using (Stream shaderStream = assembly.GetManifestResourceStream("Milk.Graphics.Shaders.DefaultVertexShader.glsl"))
            using (StreamReader streamReader = new StreamReader(shaderStream))
                _vertexShaderCode = streamReader.ReadToEnd();

            using (Stream shaderStream = assembly.GetManifestResourceStream("Milk.Graphics.Shaders.DefaultFragmentShader.glsl"))
            using (StreamReader streamReader = new StreamReader(shaderStream))
                _fragmentShaderCode = streamReader.ReadToEnd();
        }
    }
}
