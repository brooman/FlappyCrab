using System;
using System.Timers;
using Game.Classes;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Game : IDisposable
    {
        private Timer gameloop;
        private Boolean running = true;

        public Player player = new Player();
        private List<Pillar> pillars = new List<Pillar>();

        private int score = 0;

        public Game()
        {

            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                //Set the spacing between pillars
                int upperHeight = random.Next(10, 30);
                int spacing = random.Next(10, 15);

                int xpos = Console.WindowWidth + (50 * i);

                //Upper pillar
                pillars.Add(new Pillar(upperHeight, xpos, 0));

                //Lower pillar
                pillars.Add(new Pillar(
                    Console.WindowHeight - upperHeight - spacing,
                    xpos,
                    upperHeight + spacing
                ));
            }

            //Start gameloop
            gameloop = new Timer();
            gameloop.Elapsed += Loop;
            gameloop.Interval = 60;
            gameloop.Enabled = true;
        }

        //Write to console
        public void Loop(object sender, ElapsedEventArgs e)
        {
            Console.Clear();

            if (this.running)
            {

                this.running = this.player.Update();

                foreach (var pillar in this.pillars)
                {

                    if (pillar.Update())
                    {
                        score++;
                    }

                    if (pillar.CheckHit(this.player))
                    {
                        this.running = false;
                        break;
                    }
                }

                //Draw Score
                Console.SetCursorPosition(Console.WindowWidth - 10, 0);
                Console.Write(this.score);

                //Draw Ground
                Console.SetCursorPosition(0, Console.WindowHeight - 3);
                Console.Write(new String('_', Console.WindowWidth));

                //Draw Player
                this.player.Show();

                //Draw Pillars
                foreach (var pillar in this.pillars)
                {
                    pillar.Show();
                }


                //Reset cursor position
                Console.SetCursorPosition(Console.WindowWidth, Console.WindowHeight);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\ud83e\udd80 Gameover \ud83e\udd80");
                Console.WriteLine($"Your score was {this.score}");
                Console.WriteLine("Press R to play again");
            }
        }

        //Write to console

        public void Dispose()
        {
            gameloop.Dispose();
        }
    }
}