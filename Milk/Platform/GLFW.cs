using System;
using System.Runtime.InteropServices;

namespace Milk.Platform
{
    internal static class GLFW
    {
        private const string GLFW_DLL = "glfw";

        #region Hints
        internal const int CONTEXT_VERSION_MAJOR    = 0x00022002;
        internal const int CONTEXT_VERSION_MINOR    = 0x00022003;
        internal const int OPENGL_PROFILE           = 0x00022008;
        internal const int FOCUSED                  = 0x00020001;
        internal const int FOCUS_ON_SHOW            = 0x0002000C;
        internal const int DECORATED                = 0x00020005;
        internal const int RESIZABLE                = 0x00020003;
        #endregion

        #region Constants
        internal const int TRUE = 1;
        internal const int FALSE = 0;

        internal const int GLFW_OPENGL_CORE_PROFILE = 0x00032001;
        #endregion

        [DllImport(GLFW_DLL, EntryPoint = "glfwInit", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Init();

        [DllImport(GLFW_DLL, EntryPoint = "glfwWindowHint", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool WindowHint(int hint, int value);

        [DllImport(GLFW_DLL, EntryPoint = "glfwGetPrimaryMonitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetPrimaryMonitor();

        [DllImport(GLFW_DLL, EntryPoint = "glfwTerminate", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Terminate();

        [DllImport(GLFW_DLL, EntryPoint = "glfwCreateWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);

        [DllImport(GLFW_DLL, EntryPoint = "glfwMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MakeContextCurrent(IntPtr window);

        [DllImport(GLFW_DLL, EntryPoint = "glfwSwapBuffers", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SwapBuffers(IntPtr window);

        [DllImport(GLFW_DLL, EntryPoint = "glfwGetProcAddress", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetProcAddress(string procname);

        [DllImport(GLFW_DLL, EntryPoint = "glfwPollEvents", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PollEvents();

        [DllImport(GLFW_DLL, EntryPoint = "glfwWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WindowShouldClose(IntPtr window);
    }
}
