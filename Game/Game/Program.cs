using System;
using System.Timers;

namespace Game
{
    class MainClass
    {
        private static Timer gameloop;

        public static void Main(string[] args)
        {
            Setup();

            while (true)
            {
                Handle(Console.ReadKey());
            }
        }


        //Bootstrap game
        public static void Setup()
        {
            Console.Clear();

            gameloop = new Timer();

            //On timed event
            gameloop.Elapsed += Loop;

            //Tickrate, Lower is faster
            gameloop.Interval = 16.6666666667;

            gameloop.Enabled = true;
        }

        //Write to console
        public static void Loop(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
        }

        //Handle key input
        private static void Handle(ConsoleKeyInfo cki)
        {
            String input = cki.Key.ToString().ToLower();

            switch (input)
            {
                case "q":
                    gameloop.Stop();
                    gameloop.Dispose();
                    
                    break;

                case "r":
                    Setup();
                    break;
            }
        }
    }
}
