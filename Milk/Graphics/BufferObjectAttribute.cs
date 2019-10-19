using System;

namespace Milk.Graphics
{
    public struct BufferObjectAttribute
    {
        public static BufferObjectAttribute[] DefaultAttributes => new BufferObjectAttribute[]
        {
            new BufferObjectAttribute(typeof(float), 2),
            new BufferObjectAttribute(typeof(float), 4)
        };

        public BufferObjectAttribute(Type type, int numComponents)
        {
            Type = type;
            NumComponents = numComponents;
        }

        public Type Type;
        public int NumComponents;
    }
}
