using System;

namespace Milk.Math
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public Vector2(float value)
        {
            x = value;
            y = value;
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float x;
        public float y;

        public float Length()
            => (float)System.Math.Sqrt(x * x + y * y);

        public Vector2 Normalized()
            => this / Length();

        public bool Equals(Vector2 other)
            => this == other;        

        public static bool operator ==(Vector2 v1, Vector2 v2)
            => v1.x == v2.x && v1.y == v2.y;        

        public static bool operator !=(Vector2 v1, Vector2 v2)
            => v1.x != v2.x || v1.y != v2.y;        

        public static Vector2 operator -(Vector2 v)
            => new Vector2(-v.x, -v.y);        

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
            => new Vector2(v1.x + v2.x, v1.y + v2.y);        

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
            => new Vector2(v1.x - v2.x, v1.y - v2.y);        

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
            => new Vector2(v1.x * v2.x, v1.y * v2.y);        

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
            => new Vector2(v1.x / v2.x, v1.y / v2.y);        

        public static Vector2 operator +(Vector2 v, float scalar)
            => new Vector2(v.x + scalar, v.y + scalar);        

        public static Vector2 operator -(Vector2 v, float scalar)
            => new Vector2(v.x - scalar, v.y - scalar);        

        public static Vector2 operator *(Vector2 v, float scalar)
            => new Vector2(v.x * scalar, v.y * scalar);        

        public static Vector2 operator /(Vector2 v, float scalar)
            => new Vector2(v.x / scalar, v.y / scalar);        
    }
}
