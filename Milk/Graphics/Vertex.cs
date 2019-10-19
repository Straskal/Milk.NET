namespace Milk.Graphics
{
    public struct PositionColor
    {
        public PositionColor(float x, float y, float r, float g, float b, float a)
        {
            X = x;
            Y = y;
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public float X;
        public float Y;
        public float R;
        public float G;
        public float B;
        public float A;
    }
}
