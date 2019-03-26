using Milk.Scene.Callbacks;
using Milk.Window;
using System;
using System.Collections.Generic;

namespace Milk.Scene
{
    public sealed class ComponentList
    {
        private readonly Actor actor;
        private readonly List<IComponent> components;
        private readonly List<IUpdatable> updatableComponents;
        private readonly List<IRenderable> renderableComponents;
        private readonly List<IComponent> toAdd;
        private readonly List<IComponent> toRemove;

        public ComponentList(Actor actor)
        {
            this.actor = actor;
            components = new List<IComponent>();
            updatableComponents = new List<IUpdatable>();
            renderableComponents = new List<IRenderable>();
            toAdd = new List<IComponent>();
            toRemove = new List<IComponent>();
        }

        public void Add(IComponent component)
        {
            if (component.Owner != null && component.Owner != actor)
                throw new InvalidOperationException("Cannot add an owned component to an actor!");

            if (component.Owner == actor)
            {
                Logger.Log(LogLevel.Warning, $"Component of type {component.GetType()} is being added multiple times to {actor.Name}");
                return;
            }

            component.Owner = actor;
            toAdd.Add(component);
        }

        public T Get<T>() where T : class
        {
            foreach (var component in toAdd)
                if (component is T requestedComponent && !component.MarkedForRemoval)
                    return requestedComponent;

            foreach (var component in components)
                if (component is T requestedComponent && !component.MarkedForRemoval)
                    return requestedComponent;

            return null;
        }

        public void Remove(IComponent component)
        {
            if (component.Owner != actor)
            {
                Logger.Log(LogLevel.Warning, "You may not remove a component from an actor which does not own it.");
                return;
            }

            if (!toRemove.Contains(component))
            {
                toRemove.Add(component);
                component.MarkedForRemoval = true;
            }
        }

        public void Update(float deltaTime)
        {
            UpdateInternal();

            foreach (var component in updatableComponents)
                component.OnUpdate(deltaTime);
        }

        public void Render(IRenderer renderer)
        {
            foreach (var component in renderableComponents)
                component.OnRender(renderer);
        }

        private void UpdateInternal()
        {
            if (toAdd.Count > 0)
            {
                foreach (var component in toAdd)
                {
                    components.Add(component);

                    if (component is ILoadable loadable)
                        loadable.OnLoad(actor.Scene.Assets);

                    if (component is IUpdatable updatable)
                        updatableComponents.Add(updatable);

                    if (component is IRenderable renderable)
                        renderableComponents.Add(renderable);
                }

                foreach (var component in toAdd)
                    if (component is IAwakable awakable)
                        awakable.OnAwake();

                toAdd.Clear();
            }

            if (toRemove.Count > 0)
            {
                foreach (var component in toRemove)
                    if (component is IDestroyable destroyable)
                        destroyable.OnDestroy();

                foreach (var component in toRemove)
                {
                    components.Remove(component);

                    if (component is IUpdatable updatable)
                        updatableComponents.Remove(updatable);

                    if (component is IRenderable renderable)
                        renderableComponents.Remove(renderable);
                }

                toRemove.Clear();
            }
        }
    }
}
