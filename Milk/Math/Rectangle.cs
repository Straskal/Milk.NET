using System;

namespace Milk.Math
{
    public struct Rectangle : IEquatable<Rectangle>
    {
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public int x;
        public int y;
        public int width;
        public int height;

        public int Top => y;
        public int Bottom => y + height;
        public int Left => x;
        public int Right => x + width;

        public bool IsEmpty => x == 0 && y == 0 && width == 0 && height == 0;

        public bool Overlaps(Rectangle other)
            => other.Left < Right
            && Left < other.Right
            && other.Top < Bottom
            && Top < other.Bottom;

        public bool Equals(Rectangle other)
            => x == other.x
            && y == other.y
            && width == other.width
            && height == other.height;
    }
}
