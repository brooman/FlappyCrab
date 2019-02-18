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

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Handle(Console.ReadLine());

            Console.WriteLine("Terminating the application...");
        }

        //Bootstrap game
        public static void Setup()
        {
            Console.Clear();

            gameloop = new Timer();

            //On timed event
            gameloop.Elapsed += Draw;

            //Tickrate, Lower is faster
            gameloop.Interval = 16.6666666667;

            gameloop.Enabled = true;
        }

        //Will be called 60 times a second
        public static void Draw(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                          e.SignalTime);
        }

        public static void Handle(string Input)
        {
            switch (Input)
            {
                case "Q":
                    gameloop.Stop();
                    gameloop.Dispose();
                    Console.WriteLine("Exiting...");
                    break;
                case "R":
                    Setup();
                    Console.WriteLine("Reseting");
                    break;
            }
        }
    }
}
