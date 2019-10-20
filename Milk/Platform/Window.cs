using Milk.Graphics;
using Milk.Input;
using System;
using System.Runtime.InteropServices;

namespace Milk.Platform
{
    public class Window : IDisposable
    {
        private readonly GLFW.windowclosefun _closeRequested;
        private readonly GLFW.windowsizefun _sizeChanged;
        private readonly GLFW.framebuffersizefun _framebufferSizeChanged;

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

            Width = parameters.Width;
            Height = parameters.Height;

            _closeRequested = (IntPtr window) => CloseRequested?.Invoke(this, new WindowCloseRequestedEventArgs());
            _sizeChanged = (IntPtr window, int w, int h) =>
            {
                Width = w;
                Height = h;
                Resized?.Invoke(this, new WindowResizedEventArgs(w, h));
            };
            _framebufferSizeChanged = (IntPtr window, int w, int h) => FramebufferSizeChanged?.Invoke(this, new FramebufferSizeChangedEventArgs(w, h));

            GLFW.SetWindowCloseCallback(Handle, Marshal.GetFunctionPointerForDelegate(_closeRequested));
            GLFW.SetWindowSizeCallback(Handle, Marshal.GetFunctionPointerForDelegate(_sizeChanged));
            GLFW.SetFramebufferSizeCallback(Handle, Marshal.GetFunctionPointerForDelegate(_framebufferSizeChanged));

            GLFW.MakeContextCurrent(Handle);

            // TODO: Remove this. Eventually, Milk will have an InputDeviceManager.
            Keyboard.Init(Handle);

            Graphics = new GraphicsAdapter(this);
        }

        internal IntPtr Handle { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// The window's graphics adapter, which allows for high level communication with the GPU.
        /// </summary>
        public GraphicsAdapter Graphics { get; private set; }

        #region Events

        /// <summary>
        /// Called when the user tries closing the window.
        /// </summary>
        public event EventHandler<WindowCloseRequestedEventArgs> CloseRequested;

        /// <summary>
        /// Called when the window is resized.
        /// </summary>
        public event EventHandler<WindowResizedEventArgs> Resized;

        /// <summary>
        /// Called when the window's content framebuffer's size changes.
        /// </summary>
        internal event EventHandler<FramebufferSizeChangedEventArgs> FramebufferSizeChanged;

        #endregion

        /// <summary>
        /// Polls all currently queued input events.
        /// </summary>
        public void PollEvents()
        {
            GLFW.PollEvents();
        }

        public void Dispose()
        {
            Graphics.Dispose();
            GLFW.Terminate();
        }
    }
}
