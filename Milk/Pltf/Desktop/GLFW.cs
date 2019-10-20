using System;
using System.Runtime.InteropServices;

/*
    Milk already depends on GLFW, so I don't want to also depend on a third party interop library, so we have our own.
*/
namespace Milk.Pltf.Desktop
{
    internal static class GLFW
    {
        private const string GLFW_DLL = "glfw";

        #region Hints

        internal const int CONTEXT_VERSION_MAJOR = 0x00022002;
        internal const int CONTEXT_VERSION_MINOR = 0x00022003;
        internal const int OPENGL_PROFILE = 0x00022008;
        internal const int FOCUSED = 0x00020001;
        internal const int FOCUS_ON_SHOW = 0x0002000C;
        internal const int DECORATED = 0x00020005;
        internal const int RESIZABLE = 0x00020003;

        #endregion

        #region Constants

        internal const int TRUE = 1;
        internal const int FALSE = 0;
        internal const int RELEASE = 0;
        internal const int PRESS = 1;

        internal const int OPENGL_CORE_PROFILE = 0x00032001;

        #endregion

        [DllImport(GLFW_DLL, EntryPoint = "glfwInit", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Init();

        [DllImport(GLFW_DLL, EntryPoint = "glfwWindowHint", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool WindowHint(int hint, int value);

        [DllImport(GLFW_DLL, EntryPoint = "glfwGetPrimaryMonitor", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetPrimaryMonitor();

        [DllImport(GLFW_DLL, EntryPoint = "glfwTerminate", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Terminate();

        [DllImport(GLFW_DLL, EntryPoint = "glfwCreateWindow", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr CreateWindow(int width, int height, string title, IntPtr monitor, IntPtr share);

        [DllImport(GLFW_DLL, EntryPoint = "glfwMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void MakeContextCurrent(IntPtr window);

        [DllImport(GLFW_DLL, EntryPoint = "glfwGetFramebufferSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void GetFramebufferSize(IntPtr window, ref int width, ref int height);

        [DllImport(GLFW_DLL, EntryPoint = "glfwSwapInterval", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SwapInterval(int interval);

        [DllImport(GLFW_DLL, EntryPoint = "glfwSwapBuffers", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SwapBuffers(IntPtr window);

        [DllImport(GLFW_DLL, EntryPoint = "glfwGetProcAddress", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetProcAddress(string procname);

        [DllImport(GLFW_DLL, EntryPoint = "glfwPollEvents", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void PollEvents();

        #region Window Events


        [DllImport(GLFW_DLL, EntryPoint = "glfwSetWindowCloseCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetWindowCloseCallback(IntPtr window, IntPtr cbfun);
        internal delegate void windowclosefun(IntPtr window);

        [DllImport(GLFW_DLL, EntryPoint = "glfwSetWindowSizeCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetWindowSizeCallback(IntPtr window, IntPtr cbfun);
        internal delegate void windowsizefun(IntPtr window, int width, int height);

        [DllImport(GLFW_DLL, EntryPoint = "glfwSetFramebufferSizeCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetFramebufferSizeCallback(IntPtr window, IntPtr cbfun);
        internal delegate void framebuffersizefun(IntPtr window, int width, int height);

        #endregion

        #region Input Events

        [DllImport(GLFW_DLL, EntryPoint = "glfwSetKeyCallback", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SetKeyCallback(IntPtr window, IntPtr cbfun);
        internal delegate void keyfun(IntPtr window, int key, int scancode, int action, int mods);

        #endregion
    }
}
