using Milk.Graphics.OpenGL;
using System;

namespace Milk.Graphics
{
    public enum BufferDrawMode
    {
        Points = 0,
        Triangles = 4,
    }

    public class BufferObject : IDisposable
    {
        public static BufferObjectAttribute[] DefaultAttributes => new BufferObjectAttribute[] 
        {
            new BufferObjectAttribute(2),
            new BufferObjectAttribute(4)
        };

        private readonly Vertex[] _vertices;

        private uint _id;
        private uint _bufferId;
        private int _length;
        private bool _isDirty;

        internal unsafe BufferObject(int numVertices, BufferObjectAttribute[] attributes) 
        {
            _vertices = new Vertex[numVertices];
            _length = 0;
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
                    GL.FLOAT, 
                    false,
                    stride * sizeof(float), 
                    new IntPtr((void*)(offset * sizeof(float)))
                );
                GL.EnableVertexAttribArray(i);
                offset += attributes[i].NumComponents;
            }

            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            GL.BindVertexArray(0);
        }

        public void AddVertices(params Vertex[] vertices)
        {
            if ((_length + vertices.Length) > _vertices.Length)
                throw new InvalidOperationException("Exceeds vertex limit!");

            for (int i = 0; i < vertices.Length; i++)
                _vertices[_length++] = vertices[i];

            _isDirty = true;
        }

        public void Clear()
        {
            _length = 0;
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

                fixed (Vertex* temp = &_vertices[0])
                    GL.BufferData(
                        GL.ARRAY_BUFFER, 
                        new IntPtr(sizeof(Vertex) * _length), 
                        new IntPtr((void*)temp), 
                        GL.STATIC_DRAW
                    );

                GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            }

            GL.BindVertexArray(_id);
            GL.DrawArrays((int)mode, 0, _length);
            GL.BindVertexArray(0);
        }
    }
}
