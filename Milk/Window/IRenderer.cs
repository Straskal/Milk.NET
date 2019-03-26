using Milk.Graphics;
using Milk.Math;

namespace Milk.Window
{
    public interface IRenderer
    {
        void Render(Texture texture, Vector2 position, Rectangle sourceRectangle);
    }
}
