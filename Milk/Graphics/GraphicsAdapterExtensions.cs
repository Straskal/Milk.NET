namespace Milk.Graphics
{
    public static class GraphicsAdapterExtensions
    {
        public static BufferObject<TVertex> CreateBufferObject<TVertex>(this GraphicsAdapter ga, BufferObjectAttribute[] attributes, TVertex[] vertices)
            where TVertex : unmanaged
        {
            var buffer = ga.CreateBufferObject<TVertex>(attributes);
            buffer.AddVertices(vertices);
            return buffer;
        }
    }
}
