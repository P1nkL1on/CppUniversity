using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    delegate void GameAction();
    class Utils
    {
        private static ConsoleColor defaultFore = ConsoleColor.Gray;
        public static void ConsoleWrite(String s)
        {
            ConsoleWrite(s, defaultFore);
        }
        public static void ConsoleWriteLine(String s)
        {
            ConsoleWriteLine(s, defaultFore);
        }
        public static void ConsoleWrite(String s, ConsoleColor fore)
        {
            lock (ConsoleWriterLock)
            {
                Console.ForegroundColor = fore;
                Console.Write(s);
                Console.ForegroundColor = defaultFore;
            }
        }
        public static void ConsoleWriteLine(String s, ConsoleColor fore)
        {
            lock (ConsoleWriterLock)
            {
                Console.ForegroundColor = fore;
                Console.WriteLine(s);
                Console.ForegroundColor = defaultFore;
            }
        }
        public static readonly object ConsoleWriterLock = new object();
        public static bool playerAgree(string question)
        {
            ConsoleWrite(question + "? [y/n]  ");
            ConsoleKeyInfo t = new ConsoleKeyInfo();
            do{
                t = Console.ReadKey(true);
            }while(t.KeyChar != 'y' && t.KeyChar != 'n');
            ConsoleWrite(((t.KeyChar == 'y')? "yes  " : "no   "));
            return (t.KeyChar == 'y');
        }
        public static int selectVariant(List<string> list)
        {
            return selectVariant(list, "Select one:");
        }
        public static int selectVariant(List<string> list, string question)
        {
            ConsoleWriteLine(question + ":");
            int maxLength = 0;
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].Length > maxLength) 
                    maxLength = list[i].Length;
                ConsoleWriteLine(String.Format("  {0}. {1};", i + 1, list[i]));
            }
            ConsoleWriteLine("Select : ");
            int x = Console.CursorLeft, y = Console.CursorTop;
            int nowOn = 0;
            ConsoleKeyInfo t = new ConsoleKeyInfo();
            string acceptKeys = "123456789";
            do
            {
                Console.SetCursorPosition(x, y);
                ConsoleWrite(list[nowOn].PadRight(maxLength));
                t = Console.ReadKey(true);
                int isNumber = acceptKeys.IndexOf(t.KeyChar+"");
                if (isNumber >= 0 && isNumber < list.Count)
                {
                    Console.SetCursorPosition(x, y);
                    ConsoleWrite("  You selected: " + list[isNumber].PadRight(maxLength));
                    ConsoleWriteLine("");
                    return isNumber;
                }
                if (t.Key == ConsoleKey.UpArrow)
                    nowOn = (nowOn > 0)? (nowOn - 1) : (list.Count - 1); 
                if (t.Key == ConsoleKey.DownArrow)
                    nowOn = (nowOn + 1)% list.Count;
            } while (t.Key != ConsoleKey.Enter);
            
            Console.SetCursorPosition(x, y);
            ConsoleWrite("  You selected: " + list[nowOn].PadRight(maxLength));
            ConsoleWriteLine("");
            return nowOn;
        }
        public static int selectNumber(int min, int max)
        {
            return selectNumber(min, max, "");
        }
        public static int selectNumber(int min, int max, string question)
        {
            ConsoleWriteLine(String.Format("{0}Select number between {1} and {2}:", (question.Length > 0)?(question + "\n"):"", min, max));
            int nowIn = min + (max - min) / 2;

            int x = Console.CursorLeft, y = Console.CursorTop;
            ConsoleKeyInfo t = new ConsoleKeyInfo();
            do{
                int offset = max - nowIn;
                string line = "".PadLeft(max - min - offset,'+') + "".PadLeft(offset, '-');

                Console.SetCursorPosition(x, y);
                string left = (nowIn == min)? "MIN" : "<< ",
                    right = (nowIn == max)? "MAX" : " >>";
                ConsoleWrite(String.Format("{3} {0} {1} {2} {4}\nSelect: {5}", min,line,max, left, right, nowIn+"  "));
                t = Console.ReadKey(true);

                if (t.Key == ConsoleKey.LeftArrow)
                    nowIn = Math.Max(min, nowIn - 1);
                if (t.Key == ConsoleKey.RightArrow)
                    nowIn = Math.Min(max, nowIn + 1);

            } while (t.Key != ConsoleKey.Enter);
            return nowIn;
        }
        public static void selectNumberTight(ref int left, ref int right, int maxOffset)
        {
            selectNumberTight(ref left, ref right, maxOffset, "", "Left is {0} ptr", "Right is {0} ptr");
        }
        public static void selectNumberTight(ref int leftV, ref int rightV, int maxOffset, string question, string FormatLeft, string FormatRight)
        {
            ConsoleWriteLine(String.Format("{0}", (question.Length > 0) ? (question) : "Select balance between:"));
            int nowIn = 0;

            int x = Console.CursorLeft, y = Console.CursorTop;
            ConsoleKeyInfo t = new ConsoleKeyInfo();
            do
            {
                int offset = nowIn;
                string line = "".PadLeft(maxOffset + offset, '>') + "*" + "".PadLeft(maxOffset - offset, '<');

                Console.SetCursorPosition(x, y);
                ConsoleWrite(String.Format(" {0} {1} {2}\n{3}\n{4}", ((leftV - offset)+"").PadLeft(4), line, ((rightV + offset)+"").PadRight(4),
                    String.Format(FormatLeft, leftV - offset), String.Format(FormatRight, rightV + offset)));
                t = Console.ReadKey(true);

                if (t.Key == ConsoleKey.LeftArrow)
                    nowIn = Math.Max(-maxOffset, nowIn - 1);
                if (t.Key == ConsoleKey.RightArrow)
                    nowIn = Math.Min(maxOffset, nowIn + 1);

            } while (t.Key != ConsoleKey.Enter);
            leftV -= nowIn;
            rightV += nowIn;
        }
    }
}
