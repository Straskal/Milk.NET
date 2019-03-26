using Milk.Window;

namespace Milk.Scene.Callbacks
{
    public interface IRenderable
    {
        void OnRender(IRenderer renderer);
    }
}
