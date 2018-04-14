using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungMap
{
    class DungRoad
    {
        public DungRoom from, to;
        public int fromTopOffset, fromLeftOffset, direction, length;
        public DungRoad(DungRoom from, DungRoom to)
        {
            this.from = from; this.to = to;
        }
        public void SetParams(int fromTopOffset, int fromLeftOffset, int direction, int length)
        {
            this.fromTopOffset = fromTopOffset; this.fromLeftOffset = fromLeftOffset; this.direction = direction; this.length = length;
        }
        public void Draw(string what)
        {
            switch (direction)
            {
                case 0: // ->
                    Console.SetCursorPosition(from.left + fromLeftOffset, from.top + fromTopOffset);
                    Console.Write(what.PadRight(length / 2, '-').PadLeft(length, '-'));
                    break;
                case 1: // <-
                    int newLeng = Math.Min(from.left + fromLeftOffset, length);
                    Console.SetCursorPosition(from.left + fromLeftOffset - newLeng, from.top + fromTopOffset);
                    Console.Write(what.PadRight(newLeng / 2, '-').PadLeft(newLeng, '-'));
                    break;
                case 2: // v
                    for (int i = 0; i < length; i++)
                    {
                        Console.SetCursorPosition(from.left + fromLeftOffset, from.top + fromTopOffset + i);
                        if (i == length / 2 && what != "")
                            Console.Write(what);
                        else
                            Console.Write('|');
                    }
                    break;
                case 3: // ^
                    for (int i = 0; i < length; i++)
                    {
                        Console.SetCursorPosition(from.left + fromLeftOffset, from.top + fromTopOffset - i);
                        if (i == length / 2 && what != "")
                            Console.Write(what);
                        else
                            Console.Write('|');
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
