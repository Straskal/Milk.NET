using Milk;
using System;

namespace Milk.Main
{
    public static class Program
    {
        static void OnLogged(LogLevel level, string message)
        {
            Console.WriteLine($"{level.ToString()}: {message}");
        }

        static void Main(string[] args)
        {
            Logger.OnLog += OnLogged;

            using (Engine.Instance)
            {
                if (!Engine.Instance.Initialize())
                    Environment.Exit(-1);

                bool successfulRun = Engine.Instance.Run();

                Logger.OnLog -= OnLogged;

                Environment.Exit(successfulRun ? 0 : -1);
            }
        }
    }
}
