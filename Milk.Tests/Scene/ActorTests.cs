using Milk.Asset;
using Milk.Scene;
using Milk.Scene.Callbacks;
using Milk.Window;
using NSubstitute;
using System;
using Xunit;

namespace Milk.Tests.Scene
{
    public sealed class ActorTests
    {
        [Fact]
        public void AddComponent_SetsComponentsOwner()
        {
            // Arrange
            var actor = new Actor();
            var component = Substitute.For<IComponent>();

            // Act
            actor.AddComponent(component);

            // Assert
            Assert.Equal(actor, component.Owner);
        }

        [Fact]
        public void AddComponent_AddOwnedComponent_ThrowsAppropriateException()
        {
            // Arrange
            var actor1 = new Actor();
            var actor2 = new Actor();
            var component = Substitute.For<IComponent>();
            actor1.AddComponent(component);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => actor2.AddComponent(component));
        }

        [Fact]
        public void AddComponent_AddsComponent()
        {
            // Arrange
            var actor = new Actor();
            var component = Substitute.For<IComponent>();

            // Act
            actor.AddComponent(component);

            // Assert
            var result = actor.GetComponent<IComponent>();
            Assert.Equal(component, result);
        }

        [Fact]
        public void RemoveComponent_RemovesComponent()
        {
            // Arrange
            var actor = new Actor();
            var component = Substitute.For<IComponent>();
            actor.AddComponent(component);

            // Act
            actor.RemoveComponent(component);

            // Assert
            var result = actor.GetComponent<IComponent>();
            Assert.Null(result);
        }

        [Fact]
        public void Update_ComponentCallbacksAreInvokedInCorrectOrder()
        {
            // Arrange
            var assetManager = Substitute.For<IAssetManager>();

            var scene = Substitute.For<IScene>();
            scene.Assets.Returns(assetManager);

            var actor = new Actor
            {
                Scene = scene
            };

            var component1 = Substitute.For<IComponent, ILoadable, IAwakable>();
            var component2 = Substitute.For<IComponent, IAwakable, IUpdatable>();
            var component3 = Substitute.For<IComponent, IUpdatable, IDestroyable>();
            actor.AddComponent(component1);
            actor.AddComponent(component2);
            actor.AddComponent(component3);

            // Act
            actor.Update(1f);

            // Assert
            Received.InOrder(() => 
            {
                ((ILoadable)component1).OnLoad(assetManager);
                ((IAwakable)component1).OnAwake();
                ((IAwakable)component2).OnAwake();
                ((IUpdatable)component2).OnUpdate(1f);
                ((IUpdatable)component3).OnUpdate(1f);
            });

            component1.ClearReceivedCalls();
            component2.ClearReceivedCalls();
            component3.ClearReceivedCalls();

            actor.RemoveComponent(component3);
            actor.Update(1f);

            Received.InOrder(() =>
            {
                ((IDestroyable)component3).OnDestroy();
                ((IUpdatable)component2).OnUpdate(1f);
            });
        }

        [Fact]
        public void Render_RendersAllRenderableComponents()
        {
            // Arrange
            var actor = new Actor();
            var component1 = Substitute.For<IComponent, IRenderable>();
            var component2 = Substitute.For<IComponent, IRenderable>();
            actor.AddComponent(component1);
            actor.AddComponent(component2);

            var renderer = Substitute.For<IRenderer>();

            // Act
            actor.Update(1f);
            actor.Render(renderer);

            // Assert
            Received.InOrder(() =>
            {
                ((IRenderable)component1).OnRender(renderer);
                ((IRenderable)component2).OnRender(renderer);
            });
        }
    }
}
