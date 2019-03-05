using System;
using System.Timers;
using Game.Classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Reflection;
using Figgle;

namespace Game
{
    public class Game : IDisposable
    {
        //Gameloop
        private Timer gameloop;

        //Tick checks
        private Int64 gameTick;
        private Int64 latestHit;

        //Running
        // -1: Start Screen
        // 0: Game Running
        // 1: Game Over
        public int _running { get; set; }

        //Game object
        public Player player = new Player();
        private List<Pillar> pillars = new List<Pillar>();

        //Game status
        private int latestScore = 0;
        private int score = 0;

        //Utilities
        private Random random = new Random();
        private ProcessStartInfo info;
        private Process _shell;
        private bool MusicPlaying = false;
        
        public Game()
        {
            //Start screen
            this._running = -1;

            //Build pillars
            for (int i = 0; i < 4; i++)
            {
                //Set the spacing between pillars
                int upperHeight = random.Next(20, 30);
                int spacing = random.Next(6, 9);

                int xpos = Console.WindowWidth + (50 * i);

                //Upper pillar
                pillars.Add(new Pillar(upperHeight, xpos, 0, "Upper"));

                //Lower pillar
                pillars.Add(new Pillar(
                    Console.WindowHeight - upperHeight - spacing,
                    xpos,
                    upperHeight + spacing,
                    "Lower"
                ));
            }

            //Start gameloop
            gameloop = new Timer();
            gameloop.Elapsed += Loop;
            gameloop.Interval = 60;
            gameloop.Enabled = true;
            gameTick = 0;
            latestHit = 0;
        }

        //Write to console
        public void Loop(object sender, ElapsedEventArgs e)
        {
            //Clear Console
            Console.Clear();

            gameTick++;

            switch (this._running)
            {
                case -1:
                    gameloop.Enabled = false;
                    Console.Clear();

                    Crab.Print();

                    Console.WriteLine(
                        FiggleFonts.Ogre.Render("Floppy Crab")
                    );
                    Console.WriteLine(
                        FiggleFonts.Ogre.Render("Press any key to start")
                    );

                    Crab.Print();
                    break;
                case 0:
                    gameloop.Enabled = true;
                    this.Update();
                    this.Draw();
                    break;
                case 1:
                    gameloop.Enabled = false;
                    Console.Clear();
                    Crab.Print();

                    Console.WriteLine(
                        FiggleFonts.Ogre.Render("Gameover")
                    );
                    Console.WriteLine(
                        FiggleFonts.Ogre.Render($"Your score was {this.score}")
                    );
                    Console.WriteLine(
                        FiggleFonts.Ogre.Render("Press R to play again")
                    );

                    Crab.Print();
                    break;
            }
        }

        public void Update()
        {
            //Update player
            this._running = this.player.Update() ? 0 : 1;

            for (var i = 0; i < this.pillars.Count; i++)
            {
                Pillar pillar = this.pillars[i];

                //pillar.Update will return true if it needs rebuilding
                if (pillar.Update() && pillar.Type == "Upper")
                {
                    RebuildPillar(pillar, this.pillars[i + 1]);
                }

                if (pillar.CheckHit(this.player))
                {
                    this.HitHandler();
                    break;
                }
            }

            if (this.score % 5 == 0 && this.score > this.latestScore)
            {
                this.IncreaseDifficulty();
            }
        }
              
        public void Draw()
        {
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

            //Show shield status
            Console.Write("Shield (d): ");
            if (this.player.ShieldCharge >= 200)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ready");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Charging {(this.player.ShieldCharge / 2)}");
            }
            Console.ResetColor();

            //Star mode
            if (this.player.Shielded)
            {
                this.PlayMusic();
                Console.WriteLine("Shielded");
                Console.ForegroundColor = GetRandomConsoleColor();
            }

        }

        public void IncreaseDifficulty()
        {
            this.latestScore = this.score;
            foreach (var pillar in pillars)
            {
                if (pillar.Speed < 4)
                {
                    pillar.Speed++;
                }
                else if (pillar.Width < 8)
                {
                    pillar.Width++;
                }
            }
        }

        public bool HitHandler(){
            //1 sec CD
            // This bugs out on first level with no speed increase.
            if (this.player.Shielded && this.gameTick > this.latestHit + ((this.score > 4) ? 200 : 250))
            {
                this.latestHit = this.gameTick;

                this.player.Shielded = false;

                this.StopMusic();

                return true;
            }
            else
            {
                this._running = 1;
                return false;
            }
        }

        public void RebuildPillar(Pillar upperPillar, Pillar lowerPillar)
        {
            //Generate new values
            int upperHeight = random.Next(20, 30);
            int spacing = random.Next(6, 9);

            //Rebuild upper pillar
            upperPillar.Rebuild(upperHeight, 0);

            //Rebuild lower pillar
            lowerPillar.Rebuild(
                Console.WindowHeight - upperHeight - spacing,
                upperHeight + spacing
            );

            //Increase score
            score++;
        }


        public void Dispose()
        {
            gameloop.Dispose();
        }
        
        private ConsoleColor GetRandomConsoleColor()
        {
            var consoleColors = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)consoleColors.GetValue(random.Next(consoleColors.Length));
        }
        
        public void PlayMusic()
        {
            if (!this.MusicPlaying)
            {
                this.info = new ProcessStartInfo();

                info.FileName = "afplay";
                info.Arguments = "star.wav";

                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;

                _shell = Process.Start(info);
                
                this.MusicPlaying = true;
            }
        }
        

        public void StopMusic()
        {
            _shell.Kill();
            this.MusicPlaying = false;
        }
    }
}