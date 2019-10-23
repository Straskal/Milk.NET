using Milk.Gfx;
using Milk.Platform.Desktop;
using Milk.Platform.Events;
using System;

namespace Milk.Platform
{
    public abstract class Window : IDisposable
    {
        /// <summary>
        /// Returns a Window for the current platform.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Window Create(WindowParameters parameters)
        {
            // Right now, milk only officially supports desktop.
            return new GLFWWindow(parameters);
        }

        internal abstract IntPtr Handle { get; }

        internal abstract int FramebufferWidth { get; }
        internal abstract int FramebufferHeight { get; }

        public abstract int Width { get; protected set; }
        public abstract int Height { get; protected set; }

        /// <summary>
        /// The window's graphics adapter, which allows for high level communication with the GPU.
        /// </summary>
        public abstract Graphics Graphics { get; }

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

        protected void PublishCloseRequestedEvent(WindowCloseRequestedEventArgs args)
        {
            // Temporary copy in case of unsubscription during null check.
            EventHandler<WindowCloseRequestedEventArgs> e = CloseRequested;
            e?.Invoke(this, args);
        }

        protected void PublishResizedEvent(WindowResizedEventArgs args)
        {
            // Temporary copy in case of unsubscription during null check.
            EventHandler<WindowResizedEventArgs> e = Resized;
            e?.Invoke(this, args);
        }

        protected void PublishFramebufferSizeChangedEvent(FramebufferSizeChangedEventArgs args)
        {
            // Temporary copy in case of unsubscription during null check.
            EventHandler<FramebufferSizeChangedEventArgs> e = FramebufferSizeChanged;
            e?.Invoke(this, args);
        }

        #endregion

        internal abstract void MakeGLContextCurrent();
        internal abstract IntPtr GetProcAddress(string name);
        internal abstract void ToggleVsync(bool toggle);
        internal abstract void SwapFramebuffers();

        /// <summary>
        /// Polls all currently queued input events.
        /// </summary>
        public abstract void PollEvents();

        public abstract void Dispose();
    }
}
