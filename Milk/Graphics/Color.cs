using System;

namespace Milk.Graphics
{
    public struct Color : IEquatable<Color>
    {
        public Color(byte r = 0x00, byte b = 0x00, byte g = 0x00, byte a = 0xFF)
        {
            this.r = r;
            this.b = b;
            this.g = g;
            this.a = a;
        }

        public byte r;
        public byte b;
        public byte g;
        public byte a;

        public static Color Transparent => new Color(0x00, 0x00, 0x00, 0x00);
        public static Color Black => new Color(0x00, 0x00, 0x00, 0xFF);

        public bool Equals(Color other)
        {
            return r == other.r && b == other.b && g == other.g && a == other.a;
        }
    }
}
