using Milk.Graphics.OpenGL;
using System;

namespace Milk.Graphics
{
    public enum BufferDrawMode
    {
        Points = 0,
        Lines = 1,
        LineLoop = 2,
        Triangles = 4,
    }

    public class BufferObject : IDisposable
    {
        private readonly Vertex[] _vertices;

        private uint _id;
        private uint _bufferId;
        private int _length;
        private bool _isDirty;

        internal BufferObject(BufferObjectAttribute[] attributes, int numVertices) 
            : this(attributes, new Vertex[numVertices])
        {            
        }
        
        internal BufferObject(BufferObjectAttribute[] attributes, Vertex[] vertices)
        {
            _vertices = vertices;
            _length = vertices.Length;
            _isDirty = true;

            GL.GenVertexArrays(1, ref _id);
            GL.BindVertexArray(_id);
            GL.GenBuffers(1, ref _bufferId);
            GL.BindBuffer(GL.ARRAY_BUFFER, _bufferId);

            for (uint i = 0; i < attributes.Length; i++)
            {
                GL.EnableVertexAttribArray(i);
                GL.VertexAttribPointer(attributes[i].Offset, attributes[i].NumComponents, GL.FLOAT, false, 0, IntPtr.Zero);
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
                    GL.BufferData(GL.ARRAY_BUFFER, new IntPtr(sizeof(Vertex) * _length), new IntPtr((void*)temp), GL.STATIC_DRAW);

                GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            }

            GL.BindVertexArray(_id);
            GL.DrawArrays((int)mode, 0, _length);
            GL.BindVertexArray(0);
        }
    }
}
