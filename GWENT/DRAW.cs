using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace GWENT
{
    public static class DRAW
    {

        static Stack<ConsoleColor> colors = new Stack<ConsoleColor>();
        public static ConsoleColor PushColor(ConsoleColor clr)
        {
            colors.Push(clr);
            Console.BackgroundColor = clr;
            return clr;
        }
        public static ConsoleColor PopColor()
        {
            ConsoleColor clr = colors.Pop();
            if (colors.Count > 0)
                Console.BackgroundColor = colors.Peek();
            else
                Console.BackgroundColor = ConsoleColor.Black;
            return clr;

        }

        public static void rarity(int leng, Rarity rar)
        {
            ConsoleColor clr = ConsoleColor.DarkYellow;
            switch (rar)
            {
                case Rarity.silver:
                    clr = ConsoleColor.DarkGray;
                    break;
                case Rarity.gold:
                    clr = ConsoleColor.Yellow;
                    break;
                default:
                    break;
            }
            PushColor(clr);
            Console.Write("".PadLeft(leng, ' '));
            PopColor();
        }

        public static void str(string s, int maxLength, bool onePoint)
        {
            if (s.Length <= maxLength)
                Console.Write(s);
            else
                Console.Write(s.Remove(maxLength - ((onePoint) ? 1 : 2)) + ((onePoint) ? "." : ".."));
        }
        public static void str(string s)
        {
            Console.Write(s);
        }

        public static void setBuffTo(int horizont, int vert)
        {
            Console.SetCursorPosition(horizont, vert);
        }
        public static void moveBuffTo(int horizontAdd, int vertAdd)
        {
            Console.SetCursorPosition(Console.CursorLeft + horizontAdd, Console.CursorTop + vertAdd);
        }
        public static void power(int power, int pad, ConsoleColor clr)
        {
            Console.ForegroundColor = clr;
            Console.Write((power + "").PadLeft(pad, ' '));
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static int armor(int count)
        {
            if (count <= 0) return 0;
            PushColor(ConsoleColor.Yellow);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(count + "");
            Console.ForegroundColor = ConsoleColor.Gray;
            PopColor();
            return (count + "").Length;
        }
        public static int timer(int timer, int step)
        {
            if (timer < 0 || step <= 0) return 0;
            PushColor(ConsoleColor.Blue);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write((timer % step) + "");
            Console.ForegroundColor = ConsoleColor.Gray;
            PopColor();
            return ((timer % step) + "").Length;
        }
        public static int str(string S, int bufHoriz, int bufVert, int textboxWid, int heiAttempt)
        {
            int ret = 1;
            while (S.Length > textboxWid)
            {
                setBuffTo(bufHoriz, bufVert + ret - 1);
                ret++;
                Console.Write(S.Remove(textboxWid));
                S = S.Substring(textboxWid);
                while (S[0] == ' ') S = S.Substring(1);
            }
            setBuffTo(bufHoriz, bufVert + ret - 1);
            Console.Write(S);
            return ret - heiAttempt;
        }
        public static string tags(List<Tag> tags)
        {
            string s = "";
            for (int i = 0; i < tags.Count; i++, s += ((i < tags.Count) ? ", " : ""))
                s += tags[i].ToString();
            return s;
        }

        static void DrawPoint(char symbol, Point at, ConsoleColor clr)
        {
            PushColor(clr);
            setBuffTo(at.X, at.Y);
            Console.Write(symbol);
            PopColor();
        }
        public static void pingField(ConsoleColor clr, List<Point> points, int totalTimeMs, int tailLength)
        {
            ConsoleColor dop = ConsoleColor.Black;
            if (clr == ConsoleColor.Red) dop = ConsoleColor.DarkRed;
            if (clr == ConsoleColor.Blue) dop = ConsoleColor.DarkBlue;
            if (clr == ConsoleColor.Green) dop = ConsoleColor.DarkGreen;
            if (clr == ConsoleColor.Yellow) dop = ConsoleColor.DarkYellow;
            if (clr == ConsoleColor.Magenta) dop = ConsoleColor.DarkMagenta;
            if (clr == ConsoleColor.Cyan) dop = ConsoleColor.DarkCyan;
            if (clr == ConsoleColor.Gray) dop = ConsoleColor.DarkGray;
            Console.ForegroundColor = dop;
            int timeInterval = Math.Max(2, totalTimeMs / (points.Count + tailLength * 2));
            for (int i = 0; i < points.Count + tailLength * 2; i++)
            {
                if (i < points.Count) DrawPoint(' ',points[i], clr);
                if (i>= tailLength / 2 && i < points.Count + tailLength/2) DrawPoint(' ',points[i - tailLength/2], dop);
                if (i >= tailLength && i < points.Count + tailLength) DrawPoint('▓', points[i - tailLength], ConsoleColor.Black);
                if (i >= tailLength / 2 * 3 && i < points.Count + tailLength / 2 * 3) DrawPoint('░', points[i - tailLength / 2 * 3], ConsoleColor.Black);
                if (i >= tailLength * 2 && i < points.Count + tailLength * 2) DrawPoint(' ', points[i - tailLength * 2], ConsoleColor.Black);
                Thread.Sleep(timeInterval);
            }
        }

        static int lastBorderLeft = 0, lastBorderTop = 0, lastBorderWid = 2, lastBorderHei = 2;
        public static void border(int left, int top, int wid, int hei, bool isSelected, ConsoleColor clr)
        {
            Console.ForegroundColor = clr;
            borderStr(left, top, wid, hei,(isSelected)? "╔═╗║╚╝" : "┌─┐│└┘");
            Console.ResetColor();
        }
        public static void border(bool isSelected, ConsoleColor clr)
        {
            Console.ForegroundColor = clr;
            borderStr(lastBorderLeft, lastBorderTop, lastBorderWid, lastBorderHei, (isSelected) ? "╔═╗║╚╝" : "┌─┐│└┘");
            Console.ResetColor();
        }
        public static void eraseLastBorder()
        {
            borderStr(lastBorderLeft, lastBorderTop, lastBorderWid, lastBorderHei, "      ");
        }
        static void borderStr(int left, int top, int wid, int hei, string symbols)
        {
            lastBorderHei = hei;
            lastBorderLeft = left;
            lastBorderTop = top;
            lastBorderWid = wid;
            //
            setBuffTo(left, top);
            str(symbols[0] + "".PadLeft(wid - 2, symbols[1]) + symbols[2]);
            for (int i = 1; i < hei; i++)
            {
                setBuffTo(left + wid - 1, top + i); str(symbols[3] + "");
                setBuffTo(left, top + i); str(symbols[3]+"");
            }
            setBuffTo(left, top + hei);
            str(symbols[4] + "".PadLeft(wid - 2, symbols[1]) + symbols[5]);

        }

        public static void message(int left, int top, string what, ConsoleColor back, ConsoleColor fore, int msecWait)
        {
            setBuffTo(left, top);
            PushColor(back);
            Console.ForegroundColor = fore;
            Console.Write(what);
            Console.ForegroundColor = ConsoleColor.Gray;
            PopColor();
            Thread.Sleep(msecWait);
        }
    }
}
