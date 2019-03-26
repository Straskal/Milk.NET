using Milk.Window;

namespace Milk.Scene
{
    public class Actor
    {
        private readonly ComponentList componentList;

        public Actor()
        {
            componentList = new ComponentList(this);
        }

        public IScene Scene { get; set; }

        public string Name { get; set; }

        public void AddComponent(IComponent component)
        {
            componentList.Add(component);
        }

        public void RemoveComponent(IComponent component)
        {
            componentList.Remove(component);
        }

        public T GetComponent<T>() where T : class
        {
            return componentList.Get<T>();
        }

        public void Update(float deltaTime)
        {
            componentList.Update(deltaTime);
        }

        public void Render(IRenderer renderer)
        {
            componentList.Render(renderer);
        }
    }
}
