using Milk;
using System;

namespace MilkRunner
{
    public static class Program
    {
        static void OnLogged(LogLevel level, string message)
        {
            Console.WriteLine($"{level.ToString()}: {message}");
        }

        static void Main(string[] args)
        {
            var game = Engine.Instance;
            Logger.OnLog += OnLogged;
            game.Initialize();
            game.Run();
            Logger.OnLog -= OnLogged;
        }
    }
}
