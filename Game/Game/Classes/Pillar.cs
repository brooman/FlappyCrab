using System;
using System.Linq;

namespace Game.Classes
{
    public class Pillar
    {
        public readonly string Type;
        public int XPos { get; private set; }
        public int YPos { get; private set; }
        public int Height { get; private set; }
        public int Width { get; set; }
        
        public int Speed { get; set; }
   
        public Pillar(int Height, int XPos, int YPos, string Type)
        {
            this.Type = Type;
            this.XPos = XPos;
            this.YPos = YPos;
            this.Height = Height;
            this.Width = 3;
            this.Speed = 1;
        }

        public bool Update()
        {
            if(this.XPos <= 0)
            {
                this.XPos = Console.WindowWidth + 20;
                return true;
            }

            this.XPos -= this.Speed;
            return false;
        }

        public void Rebuild(int Height, int YPos)
        {
            this.Height = Height;
            this.YPos = YPos;
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
                            Console.Write("█");
                        }

                    }
                }
            }

            Console.SetCursorPosition(0, 0);
        }

        public bool CheckHit(Player player)
         {
            //Check if player and pillar has same XPos
            if (Enumerable.Range(this.XPos, this.Width).Contains(player.XPos))
            {
                //Check if player is inside the pillar
                if(Enumerable.Range(this.YPos, this.Height).Contains(player.YPos))
                {
                        return true;
                }
            }

            return false;
        }
    }
}