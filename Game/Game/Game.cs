using System;
using System.Timers;
using Game.Classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Reflection;

namespace Game
{
    public class Game : IDisposable
    {
        private Timer gameloop;
        
        private Int64 gameTick;
        private Int64 latestHit;
        
        private Boolean running = true;
        private Random random = new Random();

        public Player player = new Player();
        private List<Pillar> pillars = new List<Pillar>();

        private int latestScore = 0;
        private int score = 0;
        
        private ProcessStartInfo info;
        private Process _shell;
        
        private bool MusicPlaying = false;
        
        public Game()
        {
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
            Console.CursorVisible = false;
        }

        //Write to console
        public void Loop(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
            gameTick++;
            Console.WriteLine(gameTick);
            
            if (this.running)
            {

                this.running = this.player.Update();

                if (!this.player.Shielded)
                {
                    this.player.ShieldCharge++;
                }
                
                for (var i = 0; i < this.pillars.Count; i++)
                {
                    Pillar pillar = this.pillars[i];

                    if (pillar.Update() && pillar.Type == "Upper")
                    {
                        //Generate new values
                        int upperHeight = random.Next(20, 30);
                        int spacing = random.Next(6, 9);

                        //Rebuild upper pillar
                        pillar.Rebuild(upperHeight, 0);

                        //Rebuild lower pillar
                        this.pillars[i + 1].Rebuild(
                            Console.WindowHeight - upperHeight - spacing,
                            upperHeight + spacing
                        );

                        //Increase score
                        score++;
                    }



                    if (pillar.CheckHit(this.player))
                    {
                        //1 sec CD
                        // This bugs out on first level with no speed increase.
                        if (this.player.Shielded)
                        {
                            if (this.gameTick > this.latestHit + ((this.score > 4) ? 200 : 250))
                            {
                                this.latestHit = this.gameTick;

                                this.player.Shielded = false;
                                
                                this.StopMusic();
                            }
                        }
                        else
                        {
                            this.running = false;
                            break;
                        }
                    }
                }



                if(this.score % 5 == 0 && this.score > this.latestScore)
                {
                    this.latestScore = this.score;
                    foreach (var pillar in pillars)
                    {
                        if (pillar.Speed < 4)
                        {
                            pillar.Speed++;
                        } else if (pillar.Width < 8)
                        {
                            pillar.Width++;
                        }
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

                if (this.player.Shielded)
                {
                    this.PlayMusic();
                    Console.WriteLine("Shielded");
                    Console.ForegroundColor = GetRandomConsoleColor();
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