using System;

namespace Game
{
    class MainClass
    {
        private static Game Game;

        public static void Main(string[] args)
        {
            //Hide cursor
            Console.CursorVisible = false;

            //Init game
            Game = new Game();

            while (true)
            {
                Handle(Console.ReadKey());
            }
        }

        //Handle key input
        private static void Handle(ConsoleKeyInfo cki)
        {
            //Start game on any key input
            if(Game._running == -1)
            {
                Game._running = 0;
                return;
            }

            //Parse & handle key input
            String input = cki.Key.ToString().ToLower();

            switch (input)
            {
                case "r":
                    Game.Dispose();
                    Game = new Game();
                    Game._running = 0;
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
