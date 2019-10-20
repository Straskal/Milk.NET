using System;

namespace Milk.Pltf
{
    public class WindowResizedEventArgs : EventArgs
    {
        public WindowResizedEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
