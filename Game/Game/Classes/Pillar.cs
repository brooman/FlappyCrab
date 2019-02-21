using System;
namespace Game.Classes
{
    public class Pillar
    {
        private int XPos;
        private int YPos;
        private readonly int Height;

        public Pillar(int Height, int XPos, int YPos)
        {
            this.XPos = XPos;
            this.YPos = YPos;
            this.Height = Height;
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

                        Console.Write("\ud83d\udd25\ud83d\udd25\ud83d\udd25");
                    }
                }
            }

            Console.SetCursorPosition(0, 0);
        }
    }
}
