using Milk.Gfx.OpenGL;
using System;
using System.Collections.Generic;

namespace Milk.Gfx
{
    /// <summary>
    /// BufferObjectAttributes determine how the BufferObject's vertices are passed to the shader pipeline.
    /// </summary>
    public class BufferObjectAttribute
    {
        private static readonly Dictionary<Type, uint> _supportedAttributeTypes = new Dictionary<Type, uint>
        {
            { typeof(float), GL.FLOAT }
        };

        /// <param name="type">The Type of this attributes components.</param>
        /// <param name="numComponents">The number of components in this attribute.</param>
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
