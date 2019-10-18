using Milk.Graphics;
using Milk.Graphics.OpenGL;
using Milk.Input;
using Milk.Platform;
using System;
using System.Runtime.InteropServices;

namespace Milk
{
    public class Window : IDisposable
    {
        private readonly GLFW.windowclosefun _onWindowClosed;

        public Window(WindowParameters parameters)
        {
            GLFW.Init();
            GLFW.WindowHint(GLFW.CONTEXT_VERSION_MAJOR, 4);
            GLFW.WindowHint(GLFW.CONTEXT_VERSION_MINOR, 5);
            GLFW.WindowHint(GLFW.OPENGL_PROFILE, GLFW.OPENGL_CORE_PROFILE);
            GLFW.WindowHint(GLFW.FOCUSED, GLFW.TRUE);
            GLFW.WindowHint(GLFW.FOCUS_ON_SHOW, GLFW.TRUE);
            GLFW.WindowHint(GLFW.DECORATED, parameters.IsBordered ? GLFW.TRUE : GLFW.FALSE);
            GLFW.WindowHint(GLFW.RESIZABLE, parameters.IsResizable ? GLFW.TRUE : GLFW.FALSE);

            IntPtr monitorPtr = parameters.IsFullscreen ? GLFW.GetPrimaryMonitor() : IntPtr.Zero;
            Handle = GLFW.CreateWindow(parameters.Width, parameters.Height, parameters.Title, monitorPtr, IntPtr.Zero);

            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Could not create Glfw window!");

            _onWindowClosed = (IntPtr window) => OnCloseRequested?.Invoke();
            GLFW.SetWindowCloseCallback(Handle, Marshal.GetFunctionPointerForDelegate(_onWindowClosed));

            Keyboard.Init(Handle);

            GLFW.MakeContextCurrent(Handle);
            GL.Init(GLFW.GetProcAddress);

            Renderer = new Renderer();
        }

        internal IntPtr Handle { get; private set; }

        public Renderer Renderer { get; private set; }

        #region Events

        public delegate void WindowClosedHandler();
        public event WindowClosedHandler OnCloseRequested;

        #endregion

        public void PollEvents()
        {
            GLFW.PollEvents();
        }

        public void SwapBuffers()
        {
            GLFW.SwapBuffers(Handle);
        }

        public void Dispose()
        {
            Renderer.Dispose();
            GLFW.Terminate();
        }
    }
}
