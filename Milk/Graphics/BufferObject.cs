using Milk.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace Milk.Graphics
{
    public enum BufferDrawMode
    {
        Points = 0,
        Triangles = 4,
    }

    public class BufferObject<TVertex> : IDisposable 
        where TVertex : unmanaged
    {
        private readonly TVertex[] _vertices;

        private uint _id;
        private uint _bufferId;
        private bool _isDirty;

        internal unsafe BufferObject(int size, BufferObjectAttribute[] attributes)
        {
            _vertices = new TVertex[size];
            Count = 0;
            _isDirty = true;

            GL.GenVertexArrays(1, ref _id);
            GL.BindVertexArray(_id);
            GL.GenBuffers(1, ref _bufferId);
            GL.BindBuffer(GL.ARRAY_BUFFER, _bufferId);

            int stride = 0;
            for (uint i = 0; i < attributes.Length; i++)
                stride += attributes[i].NumComponents;

            int offset = 0;
            for (uint i = 0; i < attributes.Length; i++)
            {
                GL.VertexAttribPointer(
                    i,
                    attributes[i].NumComponents,
                    attributes[i].TypeEnum,
                    false,
                    stride * Marshal.SizeOf(attributes[i].Type),
                    new IntPtr((void*)(offset * Marshal.SizeOf(attributes[i].Type)))
                );

                GL.EnableVertexAttribArray(i);
                offset += attributes[i].NumComponents;
            }

            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            GL.BindVertexArray(0);
        }

        public int Size => _vertices.Length;
        public int Count { get; private set; }

        public void AddVertices(params TVertex[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
                _vertices[Count++] = vertices[i];

            _isDirty = true;
        }

        public void Clear()
        {
            Count = 0;
            _isDirty = true;
        }

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
                    GL.BufferData(
                        GL.ARRAY_BUFFER,
                        new IntPtr(sizeof(TVertex) * Count),
                        new IntPtr((void*)temp),
                        GL.STATIC_DRAW
                    );

                GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            }

            GL.BindVertexArray(_id);
            GL.DrawArrays((int)mode, 0, Count);
            GL.BindVertexArray(0);
        }
    }
}
