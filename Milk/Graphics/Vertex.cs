using System.Runtime.InteropServices;

namespace Milk.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X;
        public float Y;
    }
}
