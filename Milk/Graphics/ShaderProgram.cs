using Milk.Graphics.OpenGL;
using System;
using System.IO;
using System.Reflection;
using System.Text;

using ShaderSources = Milk.Constants.ShaderSource;

namespace Milk.Graphics
{
    /// <summary>
    /// Shaders are required in order to draw vertices.
    /// </summary>
    public class ShaderProgram : IDisposable
    {
        internal static ShaderProgram LoadDefaultShader()
        {
            Assembly assembly = typeof(ShaderProgram).Assembly;

            using (Stream vertStream = assembly.GetManifestResourceStream(ShaderSources.DefaultVertex))
            using (Stream fragStream = assembly.GetManifestResourceStream(ShaderSources.DefaultFragment))
            using (StreamReader vertReader = new StreamReader(vertStream))
            using (StreamReader fragReader = new StreamReader(fragStream))
                return new ShaderProgram(vertReader.ReadToEnd(), fragReader.ReadToEnd());
        }

        internal ShaderProgram(string vertexCode, string fragmentCode)
        {
            uint vertexShaderId = GL.CreateShader(GL.VERTEX_SHADER);
            uint fragmentShaderId = GL.CreateShader(GL.FRAGMENT_SHADER);

            var vertSb = new StringBuilder(vertexCode);
            var fragSb = new StringBuilder(fragmentCode);

            GL.ShaderSource(vertexShaderId, 1, ref vertSb, null);
            GL.ShaderSource(fragmentShaderId, 1, ref fragSb, null);
            GL.CompileShader(vertexShaderId);
            GL.CompileShader(fragmentShaderId);

            AssertNoCompileErrors(vertexShaderId, "VERTEX");
            AssertNoCompileErrors(fragmentShaderId, "FRAGMENT");

            Id = GL.CreateProgram();
            GL.AttachShader(Id, vertexShaderId);
            GL.AttachShader(Id, fragmentShaderId);
            GL.LinkProgram(Id);

            AssertNoLinkErrors(Id);

            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(fragmentShaderId);
        }

        internal uint Id { get; }

        public void Dispose()
        {
            GL.DeleteProgram(Id);
        }

        internal void Use()
        {
            GL.UseProgram(Id);
        }

        private void AssertNoCompileErrors(uint shaderId, string type)
        {
            int compiledSuccessfully = 0;
            var infoBuffer = new StringBuilder(1024);

            GL.GetShaderIv(shaderId, GL.COMPILE_STATUS, ref compiledSuccessfully);
            if (compiledSuccessfully == 0)
            {
                int _ = -1;
                GL.GetShaderInfoLog(shaderId, 1024, ref _, infoBuffer);
                throw new ArgumentException($"Error compiling {type} shader: {infoBuffer.ToString()}");
            }
        }

        private void AssertNoLinkErrors(uint programId)
        {
            int linkedSuccessfully = 0;
            var infoBuffer = new StringBuilder(1024);

            GL.GetProgramIv(programId, GL.LINK_STATUS, ref linkedSuccessfully);
            if (linkedSuccessfully == 0)
            {
                int _ = -1;
                GL.GetProgramInfoLog(programId, 1024, ref _, infoBuffer);
                throw new ArgumentException($"Error linking shader program: {infoBuffer.ToString()}");
            }
        }
    }
}
