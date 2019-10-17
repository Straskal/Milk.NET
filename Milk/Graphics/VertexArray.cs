using System;

namespace Milk.Graphics
{
    public class VertexArray
    {
        private readonly float[] _vertices;
        private readonly uint _id;
        private readonly uint _bufferId;

        private int _vertexIndex = 0;

        public VertexArray()
        {
            _vertices = new float[512];

            GL.GenVertexArrays(1, ref _id);
            GL.BindVertexArray(_id);
            GL.GenBuffers(1, ref _bufferId);
            GL.BindBuffer(GL.ARRAY_BUFFER, _bufferId);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, GL.FLOAT, false, 0, IntPtr.Zero);
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            GL.BindVertexArray(0);
        }

        public void AddVertex(Vertex vertex)
        {
            _vertices[_vertexIndex++] = vertex.X;
            _vertices[_vertexIndex++] = vertex.Y;
        }

        public unsafe void Draw()
        {
            GL.BindVertexArray(_id);

            fixed (float* temp = &_vertices[0])
                GL.BufferData(
                    GL.ARRAY_BUFFER,
                    new IntPtr(sizeof(float) * _vertices.Length),
                    new IntPtr((void*)temp),
                    GL.STATIC_DRAW
                );

            GL.DrawArrays(GL.TRIANGLES, 0, 3);
            GL.BindVertexArray(0);
        }
    }
}
