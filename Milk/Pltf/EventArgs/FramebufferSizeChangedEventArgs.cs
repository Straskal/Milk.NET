using System;

namespace Milk.Pltf
{
    public class FramebufferSizeChangedEventArgs : EventArgs
    {
        public FramebufferSizeChangedEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
