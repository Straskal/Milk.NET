using Milk;

namespace MilkRunner
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.Initialize();
            game.Run();
        }
    }
}
