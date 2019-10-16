using Milk.Platform;
using System;

namespace Milk
{
    public class Window
    {
        internal IntPtr Handle { get; private set; }

        public void Initialize(WindowParameters parameters)
        {
            GLFW.Init();
            GLFW.WindowHint(GLFW.CONTEXT_VERSION_MAJOR, 4);
            GLFW.WindowHint(GLFW.CONTEXT_VERSION_MINOR, 5);
            GLFW.WindowHint(GLFW.OPENGL_PROFILE, GLFW.GLFW_OPENGL_CORE_PROFILE);
            GLFW.WindowHint(GLFW.FOCUSED, GLFW.TRUE);
            GLFW.WindowHint(GLFW.FOCUS_ON_SHOW, GLFW.TRUE);
            GLFW.WindowHint(GLFW.DECORATED, parameters.IsBordered ? GLFW.TRUE : GLFW.FALSE);
            GLFW.WindowHint(GLFW.RESIZABLE, parameters.IsResizable ? GLFW.TRUE : GLFW.FALSE);

            IntPtr monitorPtr = parameters.IsFullscreen ? GLFW.GetPrimaryMonitor() : IntPtr.Zero;
            Handle = GLFW.CreateWindow(parameters.Width, parameters.Height, parameters.Title, monitorPtr, IntPtr.Zero);

            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Could not create Glfw window!");

            GLFW.MakeContextCurrent(Handle);
        }

        public bool ShouldClose => GLFW.WindowShouldClose(Handle) == 1;

        public void PollEvents()
        {
            GLFW.PollEvents();
        }

        public void SwapBuffers()
        {
            GLFW.SwapBuffers(Handle);
        }

        public void Close()
        {
            GLFW.Terminate();
        }
    }
}
