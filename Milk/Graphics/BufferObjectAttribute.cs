using Milk.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace Milk.Graphics
{
    public class BufferObjectAttribute
    {
        private static readonly Dictionary<Type, uint> _supportedAttributeTypes = new Dictionary<Type, uint>
        {
            { typeof(float), GL.FLOAT }
        };

        public static BufferObjectAttribute[] DefaultAttributes => new BufferObjectAttribute[]
        {
            new BufferObjectAttribute<float>(2),
            new BufferObjectAttribute<float>(4)
        };

        public BufferObjectAttribute(Type type, int numComponents)
        {
            if (_supportedAttributeTypes.TryGetValue(type, out uint glEnum))
            {
                TypeEnum = glEnum;
                Type = type;
                NumComponents = numComponents;
                return;
            }

            throw new InvalidOperationException($"Type {type.Name} is not supported by BufferObjectAttribute.");
        }

        internal uint TypeEnum { get; }
        public Type Type { get; }
        public int NumComponents { get; }
    }

    public class BufferObjectAttribute<T> : BufferObjectAttribute
    {
        public BufferObjectAttribute(int numComponents) : base(typeof(T), numComponents)
        {
        }
    }
}
