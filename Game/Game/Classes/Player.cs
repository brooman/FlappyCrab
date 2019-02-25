﻿using System;
namespace Game.Classes
{
    public class Player
    {
        public int YPos { get; private set; }
        public int XPos { get; }
        private readonly int Height = 2;
        private readonly int Width = 2;

        public Player()
        {
            this.YPos = Console.WindowHeight / 2;
            this.XPos = 40;
        }

        public void Show()
        {
            for (int i = 0; i < this.Height; i++)
            {
                Console.SetCursorPosition(this.XPos, this.YPos + i);

                for (int j = 0; j < this.Width; j++)
                {
                    Console.Write("\ud83e\udd80");
                }

                Console.SetCursorPosition(0, 0);
            }
        }

        public bool Update()
        {
            if(this.YPos >= Console.WindowHeight)
            {
                return false;
            }

            this.YPos += 1;

            return true;
        }

        public void Jump()
        {
            if(this.YPos > 5)
            {
                this.YPos -= 5;
            }
        }
    }
}