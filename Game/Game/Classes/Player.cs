using System;
namespace Game.Classes
{
    public class Player
    {
        public int YPos { get; private set; }
        public int XPos { get; }
        private readonly int Height = 1;
        private readonly int Width = 1;
        public bool Shielded { get; set; }
        public int ShieldCharge { get; set; }

        public Player()
        {
            this.YPos = Console.WindowHeight / 2;
            this.XPos = 40;
            this.ShieldCharge = 0;
            this.Shielded = false;
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
            //Regenerate shield charge
            if (!this.Shielded)
            {
                this.ShieldCharge++;
            }

            //Check if we hit ground
            if (this.YPos >= Console.WindowHeight)
            {
                return false;
            }

            //Gravity
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

        public void Shield()
        {
            if (this.ShieldCharge >= 200)
            {
                this.Shielded = true;
                this.ShieldCharge = 0;       
            }
        }
    }
}
