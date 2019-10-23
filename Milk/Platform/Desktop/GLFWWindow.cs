using Milk.Gfx;
using Milk.Input;
using Milk.Platform.Events;
using System;
using System.Runtime.InteropServices;

namespace Milk.Platform.Desktop
{
    internal class GLFWWindow : Window
    {
        private readonly GLFW.windowclosefun _closeRequested;
        private readonly GLFW.windowsizefun _sizeChanged;
        private readonly GLFW.framebuffersizefun _framebufferSizeChanged;

        private int _frameBufferWidth;
        private int _frameBufferHeight;

        internal GLFWWindow(WindowParameters parameters)
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

            _frameBufferWidth = parameters.Width;
            _frameBufferHeight = parameters.Height;
            Width = parameters.Width;
            Height = parameters.Height;

            // Must be called before creating our graphics object.
            GLFW.MakeContextCurrent(Handle);
            Graphics = new Graphics(this);

            _closeRequested = (IntPtr window) =>
            {
                PublishCloseRequestedEvent(new WindowCloseRequestedEventArgs());
            };

            _sizeChanged = (IntPtr window, int w, int h) =>
            {
                Width = w;
                Height = h;
                PublishResizedEvent(new WindowResizedEventArgs(w, h));
            };

            _framebufferSizeChanged = (IntPtr window, int w, int h) =>
            {
                _frameBufferWidth = w;
                _frameBufferHeight = h;
                PublishFramebufferSizeChangedEvent(new FramebufferSizeChangedEventArgs(w, h));
            };

            GLFW.SetWindowCloseCallback(Handle, Marshal.GetFunctionPointerForDelegate(_closeRequested));
            GLFW.SetWindowSizeCallback(Handle, Marshal.GetFunctionPointerForDelegate(_sizeChanged));
            GLFW.SetFramebufferSizeCallback(Handle, Marshal.GetFunctionPointerForDelegate(_framebufferSizeChanged));

            // TODO: Remove this. Eventually, Milk will have an InputDeviceManager.
            Keyboard.Init(Handle);
        }

        internal override IntPtr Handle { get; }

        internal override int FramebufferWidth => _frameBufferWidth;
        internal override int FramebufferHeight => _frameBufferHeight;

        public override int Width { get; protected set; }
        public override int Height { get; protected set; }

        /// <summary>
        /// The window's graphics adapter, which allows for high level communication with the GPU.
        /// </summary>
        public override Graphics Graphics { get; }

        /// <summary>
        /// Polls all currently queued input events.
        /// </summary>
        public override void PollEvents()
        {
            GLFW.PollEvents();
        }

        public override void Dispose()
        {
            Graphics.Dispose();
            GLFW.Terminate();
        }

        internal override void ToggleVsync(bool toggle)
        {
            GLFW.SwapInterval(toggle ? 1 : 0);
        }

        internal override void SwapFramebuffers()
        {
            GLFW.SwapBuffers(Handle);
        }

        internal override void MakeGLContextCurrent()
        {
            GLFW.MakeContextCurrent(Handle);
        }

        internal override IntPtr GetProcAddress(string name)
        {
            return GLFW.GetProcAddress(name);
        }
    }
}
