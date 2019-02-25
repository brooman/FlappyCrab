﻿using System;
using System.Linq;

namespace Game.Classes
{
    public class Pillar
    {
        public int XPos { get; private set; }
        public int YPos { get; }
        public int Height { get; }
        public int Width { get; }

        public Pillar(int Height, int XPos, int YPos)
        {
            this.XPos = XPos;
            this.YPos = YPos;
            this.Height = Height;
            this.Width = 4;
        }

        public bool Update()
        {
            if(this.XPos <= 0)
            {
                this.XPos = Console.WindowWidth + 20;
                return true;
            }

            this.XPos -= 1;
            return false;
        }

        public void Show()
        {
            //Dont let game write outside console
            if (this.XPos > 10 && this.XPos < Console.WindowWidth - 10)
            {
                for (int i = 0; i < this.Height; i++)
                {
                    //Dont let game write outside console
                    if(this.YPos + 1 < Console.WindowHeight)
                    {
                        Console.SetCursorPosition(this.XPos, this.YPos + i);

                        for(int j = 0; j < this.Width; j++)
                        {
                            Console.Write("\ud83d\udd25");
                        }

                    }
                }
            }

            Console.SetCursorPosition(0, 0);
        }

        public bool CheckHit(Player player)
         {
            //Check if player and pillar has same XPos
            if (Enumerable.Range(this.XPos, this.XPos + this.Width - 1).Contains(player.XPos))
            {
                //Check if player is inside the pillar
                if(Enumerable.Range(this.YPos, this.YPos + this.Height - 1).Contains(player.YPos))
                {
                    Console.Clear();
                    Console.WriteLine($"Pillar Width: {this.XPos} - {this.XPos + this.Width - 1}");
                    Console.WriteLine($"Pillar Height: {this.YPos} - {this.YPos + this.Height - 1}");

                    Console.WriteLine();
                    Console.WriteLine($"Player X: {player.XPos}");
                    Console.WriteLine($"Player Y: {player.YPos}");

                       return true;
                }
            }

            return false;
        }
    }
}