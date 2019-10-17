using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Milk.Graphics
{
    internal static class GL
    {
        private const string OPENGL_DLL = "opengl32.dll";

        #region Constants
        internal const int ARRAY_BUFFER = 0x8892;
        internal const int STATIC_DRAW = 0x88E4;
        internal const int FLOAT = 0x1406;
        internal const int TRIANGLES = 0x0004;
        internal const int COLOR_BUFFER_BIT = 0x4000;
        internal const uint VERTEX_SHADER = 35633;
        internal const uint FRAGMENT_SHADER = 35632;
        internal const uint COMPILE_STATUS = 35713;
        internal const uint LINK_STATUS = 35714;
        #endregion

        internal static void Init(Func<string, IntPtr> getProcAddress)
        {
            T LoadOpenGLFunction<T>()
            {
                IntPtr functionPtr = getProcAddress(typeof(T).Name);

                return functionPtr != IntPtr.Zero
                    ? Marshal.GetDelegateForFunctionPointer<T>(functionPtr)
                    : throw new InvalidOperationException($"Unable to load Function Pointer: {typeof(T).Name}");
            }

            GenBuffers = LoadOpenGLFunction<glGenBuffers>();
            BindBuffer = LoadOpenGLFunction<glBindBuffer>();
            BufferData = LoadOpenGLFunction<glBufferData>();
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
        }

        [DllImport(OPENGL_DLL, EntryPoint = "glDrawArrays")]
        internal static extern void DrawArrays(int mode, int first, int count);

        // Not all OpenGL functions are exposed in the DLL. 
        // Some are function pointers that are retrieved via GetProcAddress.
        #region Function pointers

        internal delegate void glGenBuffers(int n, ref uint buffers);
        internal delegate void glBindBuffer(uint target, uint buffer);
        internal delegate void glBufferData(uint target, IntPtr size, IntPtr data, uint usage);
        internal delegate void glEnableVertexAttribArray(uint index);
        internal delegate void glVertexAttribPointer(uint indx, int size, uint type, bool normalized, int stride, IntPtr ptr);
        internal delegate void glGenVertexArrays(int n, ref uint arrays);
        internal delegate void glBindVertexArray(uint array);
        internal delegate void glClearColor(float r, float g, float b, float a);
        internal delegate void glClear(int mask);
        internal delegate uint glCreateShader(uint type);
        internal delegate void glShaderSource(uint shader, int count, ref StringBuilder str, int[] length);
        internal delegate void glCompileShader(uint shader);
        internal delegate void glGetShaderiv(uint shader, uint name, ref int success);
        internal delegate void glGetShaderInfoLog(uint shader, int maxLength, ref int length, StringBuilder infoLog);
        internal delegate uint glCreateProgram();
        internal delegate void glAttachShader(uint shader, uint part);
        internal delegate void glLinkProgram(uint shader);
        internal delegate void glGetProgramiv(uint program, uint name, ref int success);
        internal delegate void glGetProgramInfoLog(uint program, int maxLength, ref int length, StringBuilder infoLog);
        internal delegate void glDeleteShader(uint shader);
        internal delegate void glUseProgram(uint program);
        internal delegate void glDeleteProgram(uint program);

        internal static glGenBuffers GenBuffers { get; private set; }
        internal static glBindBuffer BindBuffer { get; private set; }
        internal static glBufferData BufferData { get; private set; }
        internal static glEnableVertexAttribArray EnableVertexAttribArray { get; private set; }
        internal static glVertexAttribPointer VertexAttribPointer { get; private set; }
        internal static glGenVertexArrays GenVertexArrays { get; private set; }
        internal static glBindVertexArray BindVertexArray { get; private set; }
        internal static glClearColor ClearColor { get; private set; }
        internal static glClear Clear { get; private set; }
        internal static glCreateShader CreateShader{ get; private set; }
        internal static glShaderSource ShaderSource { get; private set; }
        internal static glCompileShader CompileShader { get; private set; }
        internal static glGetShaderiv GetShaderIv { get; private set; }
        internal static glGetShaderInfoLog GetShaderInfoLog { get; private set; }
        internal static glCreateProgram CreateProgram { get; private set; }
        internal static glAttachShader AttachShader { get; private set; }
        internal static glLinkProgram LinkProgram { get; private set; }
        internal static glGetProgramiv GetProgramIv { get; private set; }
        internal static glGetProgramInfoLog GetProgramInfoLog { get; private set; }
        internal static glDeleteShader DeleteShader { get; private set; }
        internal static glUseProgram UseProgram { get; private set; }
        internal static glDeleteProgram DeleteProgram { get; private set; }

        #endregion
    }
}
