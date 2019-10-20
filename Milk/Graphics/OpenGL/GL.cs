using System;
using System.Runtime.InteropServices;
using System.Text;

/*
    Milk already depends on OpenGL, so I don't want to also depend on a third party interop library, so we have our own.

    https://docs.factorcode.org/content/article-handbook.html is a great resource for constants/enum value lookups.
*/
namespace Milk.Graphics.OpenGL
{
    internal static class GL
    {
        private const string OPENGL_DLL = "opengl32.dll";

        #region Constants

        internal const int ARRAY_BUFFER = 0x8892;
        internal const int STATIC_DRAW = 0x88E4;
        internal const uint FLOAT = 0x1406;
        internal const int TRIANGLES = 0x0004;
        internal const int COLOR_BUFFER_BIT = 0x4000;
        internal const uint VERTEX_SHADER = 35633;
        internal const uint FRAGMENT_SHADER = 35632;
        internal const uint COMPILE_STATUS = 35713;
        internal const uint LINK_STATUS = 35714;
        internal const uint BLENDING = 3042;
        internal const uint SRC_ALPHA = 770;
        internal const uint ONE_MINUS_SRC_ALPHA = 771;

        #endregion

        internal static void Init(Func<string, IntPtr> getProcAddress)
        {
            /*
                Not all OpenGL functions are exposed in the DLL. 
                Some are function pointers that are retrieved via getProcAddress. 
            */
            T LoadOpenGLFunction<T>()
            {
                IntPtr functionPtr = getProcAddress(typeof(T).Name);

                return functionPtr != IntPtr.Zero
                    ? Marshal.GetDelegateForFunctionPointer<T>(functionPtr)
                    : throw new InvalidOperationException($"Unable to load Function Pointer: {typeof(T).Name}");
            }

            Viewport = LoadOpenGLFunction<glViewport>();
            Enable = LoadOpenGLFunction<glEnable>();
            BlendFunc = LoadOpenGLFunction<glBlendFunc>();
            GenBuffers = LoadOpenGLFunction<glGenBuffers>();
            BindBuffer = LoadOpenGLFunction<glBindBuffer>();
            BufferData = LoadOpenGLFunction<glBufferData>();
            DeleteVertexArrays = LoadOpenGLFunction<glDeleteVertexArrays>();
            DeleteBuffers = LoadOpenGLFunction<glDeleteBuffers>();
            EnableVertexAttribArray = LoadOpenGLFunction<glEnableVertexAttribArray>();
            VertexAttribPointer = LoadOpenGLFunction<glVertexAttribPointer>();
            GenVertexArrays = LoadOpenGLFunction<glGenVertexArrays>();
            BindVertexArray = LoadOpenGLFunction<glBindVertexArray>();
            ClearColor = LoadOpenGLFunction<glClearColor>();
            Clear = LoadOpenGLFunction<glClear>();
            CreateShader = LoadOpenGLFunction<glCreateShader>();
            ShaderSource = LoadOpenGLFunction<glShaderSource>();
            CompileShader = LoadOpenGLFunction<glCompileShader>();
            GetShaderIv = LoadOpenGLFunction<glGetShaderiv>();
            GetShaderInfoLog = LoadOpenGLFunction<glGetShaderInfoLog>();
            CreateProgram = LoadOpenGLFunction<glCreateProgram>();
            AttachShader = LoadOpenGLFunction<glAttachShader>();
            LinkProgram = LoadOpenGLFunction<glLinkProgram>();
            GetProgramIv = LoadOpenGLFunction<glGetProgramiv>();
            GetProgramInfoLog = LoadOpenGLFunction<glGetProgramInfoLog>();
            DeleteShader = LoadOpenGLFunction<glDeleteShader>();
            UseProgram = LoadOpenGLFunction<glUseProgram>();
            DeleteProgram = LoadOpenGLFunction<glDeleteProgram>();
            GetUniformLocation = LoadOpenGLFunction<glGetUniformLocation>();
            UniformMatrix4fv = LoadOpenGLFunction<glUniformMatrix4fv>();
        }

        [DllImport(OPENGL_DLL, EntryPoint = "glDrawArrays")]
        internal static extern void DrawArrays(int mode, int first, int count);

        #region Function pointers

        internal delegate void glViewport(int x, int y, int w, int h);
        internal static glViewport Viewport { get; private set; }

        internal delegate void glEnable(uint cap);
        internal static glEnable Enable { get; private set; }

        internal delegate void glBlendFunc(uint sfactor, uint dfactor);
        internal static glBlendFunc BlendFunc { get; private set; }

        internal delegate void glGenBuffers(int n, ref uint buffers);
        internal static glGenBuffers GenBuffers { get; private set; }

        internal delegate void glBindBuffer(uint target, uint buffer);
        internal static glBindBuffer BindBuffer { get; private set; }

        internal delegate void glBufferData(uint target, IntPtr size, IntPtr data, uint usage);
        internal static glBufferData BufferData { get; private set; }

        internal delegate void glDeleteVertexArrays(uint n, ref uint buffers);
        internal static glDeleteVertexArrays DeleteVertexArrays { get; private set; }

        internal delegate void glDeleteBuffers(int n, ref uint buffers);
        internal static glDeleteBuffers DeleteBuffers { get; private set; }

        internal delegate void glEnableVertexAttribArray(uint index);
        internal static glEnableVertexAttribArray EnableVertexAttribArray { get; private set; }

        internal delegate void glVertexAttribPointer(uint indx, int size, uint type, bool normalized, int stride, IntPtr ptr);
        internal static glVertexAttribPointer VertexAttribPointer { get; private set; }

        internal delegate void glGenVertexArrays(int n, ref uint arrays);
        internal static glGenVertexArrays GenVertexArrays { get; private set; }

        internal delegate void glBindVertexArray(uint array);
        internal static glBindVertexArray BindVertexArray { get; private set; }

        internal delegate void glClearColor(float r, float g, float b, float a);
        internal static glClearColor ClearColor { get; private set; }

        internal delegate void glClear(int mask);
        internal static glClear Clear { get; private set; }

        internal delegate uint glCreateShader(uint type);
        internal static glCreateShader CreateShader { get; private set; }

        internal delegate void glShaderSource(uint shader, int count, ref StringBuilder str, int[] length);
        internal static glShaderSource ShaderSource { get; private set; }

        internal delegate void glCompileShader(uint shader);
        internal static glCompileShader CompileShader { get; private set; }

        internal delegate void glGetShaderiv(uint shader, uint name, ref int success);
        internal static glGetShaderiv GetShaderIv { get; private set; }

        internal delegate void glGetShaderInfoLog(uint shader, int maxLength, ref int length, StringBuilder infoLog);
        internal static glGetShaderInfoLog GetShaderInfoLog { get; private set; }

        internal delegate uint glCreateProgram();
        internal static glCreateProgram CreateProgram { get; private set; }

        internal delegate void glAttachShader(uint shader, uint part);
        internal static glAttachShader AttachShader { get; private set; }

        internal delegate void glLinkProgram(uint shader);
        internal static glLinkProgram LinkProgram { get; private set; }

        internal delegate void glGetProgramiv(uint program, uint name, ref int success);
        internal static glGetProgramiv GetProgramIv { get; private set; }

        internal delegate void glGetProgramInfoLog(uint program, int maxLength, ref int length, StringBuilder infoLog);
        internal static glGetProgramInfoLog GetProgramInfoLog { get; private set; }

        internal delegate void glDeleteShader(uint shader);
        internal static glDeleteShader DeleteShader { get; private set; }

        internal delegate void glUseProgram(uint program);
        internal static glUseProgram UseProgram { get; private set; }

        internal delegate void glDeleteProgram(uint program);
        internal static glDeleteProgram DeleteProgram { get; private set; }

        internal delegate int glGetUniformLocation(uint program, string name);
        internal static glGetUniformLocation GetUniformLocation { get; private set; }

        internal delegate void glUniformMatrix4fv(int location, int count, int transpose, IntPtr value);
        internal static glUniformMatrix4fv UniformMatrix4fv { get; private set; }

        #endregion
    }
}
