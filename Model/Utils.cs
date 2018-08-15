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
        public static readonly object ConsoleWriterLock = new object();
        public static bool playerAgree(string question)
        {
            Console.Write(question + "? [y/n]  ");
            ConsoleKeyInfo t = new ConsoleKeyInfo();
            do{
                t = Console.ReadKey(true);
            }while(t.KeyChar != 'y' && t.KeyChar != 'n');
            Console.Write(((t.KeyChar == 'y')? "yes  " : "no   "));
            return (t.KeyChar == 'y');
        }
        public static int selectVariant(List<string> list)
        {
            return selectVariant(list, "Select one:");
        }
        public static int selectVariant(List<string> list, string question)
        {
            Console.WriteLine(question + ":");
            int maxLength = 0;
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].Length > maxLength) 
                    maxLength = list[i].Length;
                Console.WriteLine(String.Format("  {0}. {1};", i + 1, list[i]));
            }
            Console.WriteLine("Select : ");
            int x = Console.CursorLeft, y = Console.CursorTop;
            int nowOn = 0;
            ConsoleKeyInfo t = new ConsoleKeyInfo();
            string acceptKeys = "123456789";
            do
            {
                Console.SetCursorPosition(x, y);
                Console.Write(list[nowOn].PadRight(maxLength));
                t = Console.ReadKey(true);
                int isNumber = acceptKeys.IndexOf(t.KeyChar+"");
                if (isNumber >= 0 && isNumber < list.Count)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write("  You selected: " + list[isNumber].PadRight(maxLength));
                    Console.WriteLine();
                    return isNumber;
                }
                if (t.Key == ConsoleKey.UpArrow)
                    nowOn = (nowOn > 0)? (nowOn - 1) : (list.Count - 1); 
                if (t.Key == ConsoleKey.DownArrow)
                    nowOn = (nowOn + 1)% list.Count;
            } while (t.Key != ConsoleKey.Enter);
            
            Console.SetCursorPosition(x, y);
            Console.Write("  You selected: " + list[nowOn].PadRight(maxLength));
            Console.WriteLine();
            return nowOn;
        }
    }
}
