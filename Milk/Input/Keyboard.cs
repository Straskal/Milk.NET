using Milk.Platform;
using System;
using System.Runtime.InteropServices;

namespace Milk.Input
{
    public enum Keys
    {
        Unknown = -1,
        Escape = 256
    }

    public static class Keyboard
    {
        private static GLFW.keyfun _keyCallback;

        internal static void Init(IntPtr windowHandle)
        {
            _keyCallback = (IntPtr window, int key, int scancode, int action, int mods) => 
            {
                if (action == GLFW.PRESS)
                    OnKeyPressed?.Invoke((Keys)key);
            };

            GLFW.SetKeyCallback(windowHandle, Marshal.GetFunctionPointerForDelegate(_keyCallback));
        }

        public delegate void KeyPressedHandler(Keys key);
        public static event KeyPressedHandler OnKeyPressed;
    }
}
