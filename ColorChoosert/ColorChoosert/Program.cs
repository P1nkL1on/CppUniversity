using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace ColorChoosert
{
    class Program
    {

        static List<ConsoleColor> availableBack = new List<ConsoleColor>() { ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkGray, ConsoleColor.DarkGreen, ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.Gray, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Yellow };
        static List<ConsoleColor> availableFore = availableBack;//availableBack.Except(new ConsoleColor[]{ ConsoleColor.Black });

        public struct Col
        {
            ConsoleColor back;
            ConsoleColor fore;
            char symbol;
            public Color associate;
            public void Print()
            {
                Console.BackgroundColor = back;
                Console.ForegroundColor = fore;
                Console.Write(symbol);
            }
            public Col(ConsoleColor back, ConsoleColor fore, char symbol, Color associate)
            {
                this.back = back;
                this.fore = fore;
                this.symbol = symbol;
                this.associate = associate;
            }
        }

        static List<char> list;
        static string s = " `.-',_=^:\u001c\"+\\/~\u001b\u001a|;<>()\u001d?{}%sc!][Itivxz\u0016r1\u007f*\u0018leo\u0019anuTfw73\u0013jyJ\u0011\u001059Y$C\u001262mLS\u001f4kpgqFP\u001ebhdVOXGEZAU\u0017W8KDH@R&\u0015BQ#0MN\u0014";

        static List<Col> dictionary = new List<Col>();
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.ReadLine();
            WriteAllVariants();
            //int X = 10;
            Bitmap b = new Bitmap("cards/nova.png");
            Print(b, 10);
            Console.ReadLine();
        }

        static void WriteAllVariants()
        {
            int step = 20, stepColor = 1, done = 0;
            for (int c = 0; c < s.Length; c+= step)
                for (int b = 0; b < availableBack.Count; b+= stepColor)
                    for (int f = 0; f < availableFore.Count; f+= stepColor)
                    {
                        done++;
                        Console.BackgroundColor = availableBack[b];
                        Console.ForegroundColor = availableFore[f];
                        dictionary.Add(new Col(availableBack[b], availableFore[f], s[c], CalculateColor(s[c])));
                        Console.ResetColor();
                        Console.WriteLine((done +"/" + s.Length / step * availableBack.Count * availableFore.Count / stepColor / stepColor+ "").PadLeft(50));
                        //Console.ReadKey();
                    }
        }
        static Color CalculateColor(char a)
        {
            int wid = 12;
            for (int i = 0; i < wid / 2; i++)
            {
                Console.SetCursorPosition(0, 10 + i);
                Console.Write("".PadLeft(wid, a));
            }
            PrintScreen ps = new PrintScreen();
            Image im = ps.CaptureScreen();
            double R = 0, G = 0, B = 0;
            using (Bitmap bit = (Bitmap)im)
            {
                for (int i = 40; i < 80; i++)
                    for (int j = 150; j < 190; j++)
                    {
                        //summLeft += (bit.GetPixel(i, j).R + bit.GetPixel(i, j).G + bit.GetPixel(i, j).B) * 2 / (3.0 * 40 * 40);
                        //bit.SetPixel(i, j, Color.Red);
                        R += bit.GetPixel(i, j).R / 1600.0;
                        G += bit.GetPixel(i, j).G / 1600.0;
                        B += bit.GetPixel(i, j).B / 1600.0;
                    }
            }
            return Color.FromArgb(Math.Min(255, (int)R), Math.Min(255, (int)G), Math.Min(255, (int)B));
        }
        static void SaveDictionary()
        {

        }
        static void WB()
        {
            List<List<char>> chs = new List<List<char>>();
            for (int i = 0; i < 255; i++)
                chs.Add(new List<char>());
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < s.Length; i++)
                chs[Math.Min(254, CalculateDarkness(s[i]))].Add(s[i]);

            Console.Clear();
            for (int i = 0; i < 255; i++)
                if (chs[i].Count == 0 && i > 0)
                    chs[i].Add(chs[i - 1][0]);

            Bitmap b = new Bitmap("cards/nova.png");
            Print(b, chs, 10);
            Console.ReadLine();

        }
        static int diff(Color a, Color b)
        {
            return Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B);
        }
        static void Print(Bitmap what, int resol)
        {
            Random rnd = new Random();
            int left = 0, top = 0;
            int resolHoriz = resol * 5 / 8;
            // what.RotateFlip(RotateFlipType.Rotate90FlipNone);
            for (int i = 0; i < what.Height; i += resol)
            {
                Console.SetCursorPosition(left, top + i / resol);
                for (int j = 0; j < what.Width; j += resolHoriz)
                {
                    Color clr = what.GetPixel(j, i);
                    int bestInd = 0, minDiff = 255 * 3;
                    for (int d = 0; d < dictionary.Count; d++)
                    {
                        int cd = diff(dictionary[d].associate, clr);
                        if (cd < minDiff) { minDiff = cd; bestInd = d; }
                    }
                    dictionary[bestInd].Print();
                }
            }
        }
        static void Print(Bitmap what, List<List<char>> with, int resol)
        {
            Random rnd = new Random();
            int left = 0, top = 0;
            int resolHoriz = resol * 5 / 8;
            // what.RotateFlip(RotateFlipType.Rotate90FlipNone);
            for (int i = 0; i < what.Height; i += resol)
            {
                Console.SetCursorPosition(left, top + i / resol);
                for (int j = 0; j < what.Width; j += resolHoriz)
                {
                    Color clr = what.GetPixel(j, i);
                    int pd = Math.Min((int)((clr.R + clr.G + clr.B) / 3.0), 254);
                    Console.Write(with[pd][rnd.Next(with[pd].Count)]);
                }
            }
        }
        
        static string Print()
        {
            Console.SetCursorPosition(0, 0);
            string l = "";
            for (int i = 0; i < list.Count; i++) l += list[i];
            Console.Write(l);
            return l;
        }
        static void PrintOp()
        {
            // `.-',_=^:\u001c\"+\\/~\u001b\u001a|;<>()\u001d?{}%sc!][Itivxz\u0016r1\u007f*\u0018leo\u0019anuTfw73\u0013jyJ\u0011\u001059Y$C\u001262mLS\u001f4kpgqFP\u001ebhdVOXGEZAU\u0017W8KDH@R&\u0015BQ#0MN\u0014
            string res = Print();
            int X = 10;
        }

        static int CalculateDarkness(char a)
        {
            int wid = 12;
            for (int i = 0; i < wid / 2; i++)
            {
                Console.SetCursorPosition(0, 10 + i);
                Console.Write("".PadLeft(wid, a));
            }

            PrintScreen ps = new PrintScreen();
            Image im = ps.CaptureScreen();
            double summLeft = 0, summRight = 0;
            using (Bitmap bit = (Bitmap)im)
            {
                for (int i = 40; i < 80; i++)
                    for (int j = 150; j < 190; j++)
                    {
                        summLeft += (bit.GetPixel(i, j).R + bit.GetPixel(i, j).G + bit.GetPixel(i, j).B) * 2 / (3.0 * 40 * 40);
                        bit.SetPixel(i, j, Color.Red);
                    }
            }
            return (int)summLeft;
        }
        static int Compare(char a, char b)
        {
            int wid = 80;
            for (int i = 0; i < wid / 2; i++)
            {
                Console.SetCursorPosition(0, 10 + i);
                Console.Write("".PadLeft(wid, a));
            }
            for (int i = 0; i < wid / 2; i++)
            {
                Console.SetCursorPosition(wid, 10 + i);
                Console.Write("".PadLeft(wid, b));
            }
            //ConsoleKey k = ConsoleKey.DownArrow;
            //while (k != ConsoleKey.LeftArrow && k != ConsoleKey.RightArrow && k != ConsoleKey.UpArrow)
            //{
            //    k = Console.ReadKey().Key;
            //}
            //Print();


            DirectoryInfo di = new DirectoryInfo("C:\\ss");
            if (!di.Exists) { di.Create(); }

            PrintScreen ps = new PrintScreen();
            Image im = ps.CaptureScreen();
            double summLeft = 0, summRight = 0;
            using (Bitmap bit = (Bitmap)im)
            {
                for (int i = 90; i < 500; i++)
                    for (int j = 300; j < 400; j++)
                        summLeft += (bit.GetPixel(i, j).R + bit.GetPixel(i, j).G + bit.GetPixel(i, j).B) / 755.0;
                for (int i = 790; i < 1200; i++)
                    for (int j = 300; j < 400; j++)
                        summRight += (bit.GetPixel(i, j).R + bit.GetPixel(i, j).G + bit.GetPixel(i, j).B) / 755.0;
                //bit.Save("pk.png");
            }
            Print();
            if (summLeft > summRight) return 1;
            if (summRight > summLeft) return -1;
            return 0;
            //if (k == ConsoleKey.LeftArrow)
            //    return 1;
            //if (k == ConsoleKey.RightArrow)
            //    return -1;
            ////
            ////
            //return 0;
        }
    }
}
