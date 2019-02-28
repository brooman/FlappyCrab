using System;

namespace Game
{
    class MainClass
    {
        private static Game Game = new Game();

        public static void Main(string[] args)
        {
            while (true)
            {
                Handle(Console.ReadKey());
            }
        }

        //Handle key input
        private static void Handle(ConsoleKeyInfo cki)
        {
            String input = cki.Key.ToString().ToLower();

            switch (input)
            {
                case "r":
                    Game.Dispose();
                    Game = new Game();
                    break;
                case "d":
                    Game.player.Shield();
                    break;
                case "spacebar":
                    Game.player.Jump();
                    break;
            }
        }
    }
}
