using System;

namespace Milk.Pltf
{
    internal abstract class Platform
    {
        internal abstract Func<string, IntPtr> GetProcAddress { get; }
    }
}
