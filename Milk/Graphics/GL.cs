using System;
using System.Runtime.InteropServices;

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
        }

        [DllImport(OPENGL_DLL, EntryPoint = "glDrawArrays")]
        internal static extern void DrawArrays(int mode, int first, int count);

        // Not all OpenGL functions are exposed in the DLL. 
        // Some are function pointers that are retrieved via GetProcAddress.
        internal delegate void glGenBuffers(int n, ref uint buffers);
        internal delegate void glBindBuffer(uint target, uint buffer);
        internal delegate void glBufferData(uint target, IntPtr size, float[] data, uint usage);
        internal delegate void glEnableVertexAttribArray(uint index);
        internal delegate void glVertexAttribPointer(uint indx, int size, uint type, bool normalized, int stride, IntPtr ptr);
        internal delegate void glGenVertexArrays(int n, ref uint arrays);
        internal delegate void glBindVertexArray(uint array);
        internal delegate void glClearColor(float r, float g, float b, float a);
        internal delegate void glClear(int mask);

        internal static glGenBuffers GenBuffers { get; private set; }
        internal static glBindBuffer BindBuffer { get; private set; }
        internal static glBufferData BufferData { get; private set; }
        internal static glEnableVertexAttribArray EnableVertexAttribArray { get; private set; }
        internal static glVertexAttribPointer VertexAttribPointer { get; private set; }
        internal static glGenVertexArrays GenVertexArrays { get; private set; }
        internal static glBindVertexArray BindVertexArray { get; private set; }
        internal static glClearColor ClearColor { get; private set; }
        internal static glClear Clear { get; private set; }
    }
}
