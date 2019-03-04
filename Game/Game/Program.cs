using System;

namespace Game
{
    class MainClass
    {
        private static Game Game;
        private static bool gameLive = false;

        public static void Main(string[] args)
        {
            Console.WriteLine("New game. Press ENTER to launch!");
            Handle(Console.ReadKey());
            while (gameLive)
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
                case "enter":
                    if (!gameLive)
                    {
                        gameLive = true;
                        Game = new Game();
                        break;
                    }
                    break;
            }
        }
    }
}
