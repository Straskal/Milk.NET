namespace Milk.Gfx
{
    public static class GraphicsExtensions
    {
        public static BufferObject<TVertex> CreateBufferObject<TVertex>(this Graphics ga, BufferObjectAttribute[] attributes, TVertex[] vertices)
            where TVertex : unmanaged
        {
            var buffer = ga.CreateBufferObject<TVertex>(attributes);
            buffer.AddVertices(vertices);
            return buffer;
        }
    }
}
