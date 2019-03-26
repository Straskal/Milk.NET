using System;

namespace Milk.Scene
{
    public interface IComponent
    {
        Actor Owner { get; set; }

        bool MarkedForRemoval { get; set; }
    }
}
