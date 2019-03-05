using System;
using System.Runtime.InteropServices;

namespace Game
{
    class MainClass
    {
        private static Game Game;

        [DllImport("libc")]
        private static extern int system(string exec);

        public static void Main(string[] args)
        {
            //Hide cursor
            Console.CursorVisible = false;
            system(@"printf '\e[8;70;200t'");

            Game = new Game()
            {
                _running = -1
            };

            while (true)
            {
                Handle(Console.ReadKey());
            }
        }

        //Handle key input
        private static void Handle(ConsoleKeyInfo cki)
        {
            //Parse & handle key input
            String input = cki.Key.ToString().ToLower();

            //Redirect all keys to "R" on start screen
            if (Game._running == -1)
            {
                input = "r";
            }

            switch (input)
            {
                case "r":
                    Game.Dispose();
                    Game = new Game
                    {
                        _running = 0
                    };
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
