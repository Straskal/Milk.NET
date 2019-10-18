namespace Milk.Graphics
{
    public struct BufferObjectAttribute
    {
        public BufferObjectAttribute(uint offset, int numComponents)
        {
            Offset = offset;
            NumComponents = numComponents;
        }

        public uint Offset;
        public int NumComponents;
    }
}
