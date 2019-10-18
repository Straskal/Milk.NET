﻿using Milk.OpenGL;
using System;

namespace Milk.Graphics
{
    public class BufferObject
    {
        private readonly Vertex[] _vertices;
        private readonly uint _id;
        private readonly uint _bufferId;

        private int _length;
        private bool _isDirty;

        internal BufferObject(int numVertices) : this(new Vertex[numVertices])
        {            
        }
        
        internal BufferObject(params Vertex[] vertices)
        {
            _vertices = vertices;
            _length = vertices.Length;
            _isDirty = true;

            GL.GenVertexArrays(1, ref _id);
            GL.BindVertexArray(_id);
            GL.GenBuffers(1, ref _bufferId);
            GL.BindBuffer(GL.ARRAY_BUFFER, _bufferId);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, GL.FLOAT, false, 0, IntPtr.Zero);
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            GL.BindVertexArray(0);
        }

        public void AddVertex(params Vertex[] vertices)
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

        internal unsafe void Draw()
        {
            if (_isDirty)
            {
                GL.BindBuffer(GL.ARRAY_BUFFER, _bufferId);
                fixed (Vertex* temp = &_vertices[0])
                    GL.BufferData(GL.ARRAY_BUFFER, new IntPtr(sizeof(Vertex) * _length), new IntPtr((void*)temp), GL.STATIC_DRAW);

                GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            }

            GL.BindVertexArray(_id);
            GL.DrawArrays(GL.TRIANGLES, 0, _length);
            GL.BindVertexArray(0);
        }
    }
}
