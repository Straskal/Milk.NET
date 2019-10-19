using Milk.Graphics.OpenGL;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Milk.Graphics
{
    public enum BufferDrawMode
    {
        Points = 0,
        Triangles = 4,
    }

    /// <summary>
    /// BufferObjects are used to send vertices to the GPU.
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    public class BufferObject<TVertex> : IDisposable 
        where TVertex : unmanaged
    {
        private TVertex[] _vertices;

        private uint _id;
        private uint _bufferId;
        private bool _isDirty;

        internal unsafe BufferObject(BufferObjectAttribute[] attributes)
        {
            _vertices = new TVertex[0];
            _isDirty = true;
            
            Count = 0;

            GL.GenVertexArrays(1, ref _id);
            GL.BindVertexArray(_id);
            GL.GenBuffers(1, ref _bufferId);
            GL.BindBuffer(GL.ARRAY_BUFFER, _bufferId);

            int numAttributeComponents = attributes.Sum(attr => attr.NumComponents);
            int attributeOffset = 0;

            for (uint i = 0; i < attributes.Length; i++)
            {
                int stide = numAttributeComponents * Marshal.SizeOf(attributes[i].Type);
                IntPtr offset = new IntPtr((void*)(attributeOffset * Marshal.SizeOf(attributes[i].Type)));
                GL.VertexAttribPointer(i, attributes[i].NumComponents, attributes[i].TypeEnum, false, stide, offset);
                GL.EnableVertexAttribArray(i);
                attributeOffset += attributes[i].NumComponents;
            }

            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// The underlying size of the BufferObject.
        /// </summary>
        public int Size => _vertices.Length;

        /// <summary>
        /// The count of vertices in the BufferObject.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Add vertices to the BufferObject causing the BufferObject to update the GPU.
        /// </summary>
        /// <param name="vertices"></param>
        public void AddVertices(params TVertex[] vertices)
        {
            int currentSize = _vertices.Length;
            int numVertices = vertices.Length;
            int newSize = Count + numVertices;

            if (newSize > currentSize)
            {
                int doubled = currentSize * 2;
                Array.Resize(ref _vertices, newSize > doubled ? newSize : doubled);
            }

            for (int i = 0; i < vertices.Length; i++)
                _vertices[Count++] = vertices[i];

            _isDirty = true;
        }

        /// <summary>
        /// Clear the BufferObject of vertices.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            _isDirty = true;
        }

        /// <summary>
        /// Free the vertex memory in the GPU.
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffers(1, ref _bufferId);
            GL.DeleteVertexArrays(1, ref _id);
        }

        internal unsafe void Draw(BufferDrawMode mode)
        {
            if (_isDirty)
            {
                GL.BindBuffer(GL.ARRAY_BUFFER, _bufferId);

                fixed (TVertex* temp = &_vertices[0])
                    GL.BufferData( GL.ARRAY_BUFFER, new IntPtr(sizeof(TVertex) * Count), new IntPtr((void*)temp), GL.STATIC_DRAW);

                GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            }

            GL.BindVertexArray(_id);
            GL.DrawArrays((int)mode, 0, Count);
            GL.BindVertexArray(0);
        }
    }
}
