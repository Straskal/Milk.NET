using System;

namespace Milk.Pltf.Desktop
{
    internal class GLFWPlatform : Platform
    {
        internal override Func<string, IntPtr> GetProcAddress => GLFW.GetProcAddress;
    }
}
