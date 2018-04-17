using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ColorChoosert
{
    class Program
    {

        static List<ConsoleColor> availableBack = new List<ConsoleColor>() { ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.DarkGreen, ConsoleColor.DarkMagenta, ConsoleColor.DarkRed, ConsoleColor.DarkYellow, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.Red, ConsoleColor.Yellow };
        static List<Color> sameColors = new List<Color>() { Color.Black, Color.DarkGray, Color.Gray, Color.White, Color.Blue, Color.DarkBlue, Color.Cyan, Color.DarkCyan, Color.DarkGreen, Color.DarkMagenta, Color.DarkRed, Color.FromArgb(122, 122, 0), Color.Green, Color.Magenta, Color.Red, Color.Yellow };
        static List<ConsoleColor> availableFore = availableBack;

        public class ColNode
        {
            int L;
            public Color separat;
            public List<Col> child;
            public ColNode left;
            public ColNode right;
            int RGB;    // 0 - R, 1 - G, 2 - B
            public ColNode(int RGB, Color separat)
            {
                this.RGB = RGB;
                this.separat = separat;
                left = right = null;
                child = new List<Col>();
            }
            public void AddChild(Col n, int rgb, int level)
            {
                L = level;
                this.RGB = rgb;
                if (level >= 6) { child.Add(n); return; }
                AddLeft(level + 1);
                AddRight(level + 1);
                switch (rgb)
                {
                    case 0: // R
                        if (n.associate.R <= separat.R)
                            AddLeft(level + 1).AddChild(n, (RGB + 1) % 3, level + 1);
                        else
                            AddRight(level + 1).AddChild(n, (RGB + 1) % 3, level + 1);
                        break;
                    case 1: // G
                        if (n.associate.G <= separat.G)
                            AddLeft(level + 1).AddChild(n, (RGB + 1) % 3, level + 1);
                        else
                            AddRight(level + 1).AddChild(n, (RGB + 1) % 3, level + 1);
                        break;
                    case 2: // B
                        if (n.associate.B <= separat.B)
                            AddLeft(level + 1).AddChild(n, (RGB + 1) % 3, level + 1);
                        else
                            AddRight(level + 1).AddChild(n, (RGB + 1) % 3, level + 1);
                        break;
                    default:
                        throw new Exception("Incorrecrt rgb " + rgb);
                }
            }
            public ColNode AddLeft(int level)
            {
                int newrgb = (RGB);
                if (left == null)
                {
                    Color nC = Color.FromArgb(separat.R - ((newrgb == 0) ? 255 / (int)Math.Pow(2, level) : 0),
                                                 separat.G - ((newrgb == 1) ? 255 / (int)Math.Pow(2, level) : 0),
                                                separat.B - ((newrgb == 2) ? 255 / (int)Math.Pow(2, level) : 0));
                    left = new ColNode(newrgb, nC);
                }
                return left;
            }
            public ColNode AddRight(int level)
            {
                int newrgb = (RGB);
                if (right == null)
                {
                    Color nC = Color.FromArgb(separat.R + ((newrgb == 0) ? 255 / (int)Math.Pow(2, level) : 0),
                                               separat.G + ((newrgb == 1) ? 255 / (int)Math.Pow(2, level) : 0),
                                               separat.B + ((newrgb == 2) ? 255 / (int)Math.Pow(2, level) : 0));
                    right = new ColNode(newrgb, nC);
                }
                return right;
            }
            public Col FindFast(int rgb, Color need)
            {
                if (left == null && right == null && child.Count == 0)
                    return new Col();
                if (child.Count > 0)
                {
                    int bestInd = -1, minDiff = 255 * 3, minClose = 255 * 3;
                    for (int d = 0; d < child.Count; d++)
                    {
                        int cd = diff(child[d].associate, need)
                            //
                        ;// +(int)(child[d].colorDiff / 300.0);
                        //
                        if (cd < minDiff || ((cd <= minDiff) && (minClose < child[d].colorDiff))) { minDiff = cd; bestInd = d; minClose = child[d].colorDiff; }
                        if (cd < minDiff) { minDiff = cd; bestInd = d; }
                        if (minDiff <= 0) break;
                    }
                    if (bestInd >= 0)
                        return child[bestInd];
                }
                this.L += 0;
                if (rgb == 0)
                { if (need.R <= separat.R) return left.FindFast((rgb + 1) % 3, need); else  return right.FindFast((rgb + 1) % 3, need); }
                if (rgb == 1)
                { if (need.G <= separat.G) return left.FindFast((rgb + 1) % 3, need); else  return right.FindFast((rgb + 1) % 3, need); }

                if (need.B <= separat.B) return left.FindFast((rgb + 1) % 3, need); else return right.FindFast((rgb + 1) % 3, need);
            }
        }

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
            public override string ToString()
            {
                return String.Format("{0},{1},{2},{3},{4},{5}",
                    associate.R, associate.G, associate.B, availableBack.IndexOf(back), availableFore.IndexOf(fore), symbol);
            }

            public int colorDiff
            {
                get { return diff(sameColors[availableFore.IndexOf(fore)], sameColors[availableBack.IndexOf(back)]); }
            }
        }

        static List<char> list;
        static string s = " `.-',_=^:\u001c\"+\\/~\u001b\u001a|;<>()\u001d?{}%sc!][Itivxz\u0016r1\u007f*\u0018leo\u0019anuTfw73\u0013jyJ\u0011\u001059Y$C\u001262mLS\u001f4kpgqFP\u001ebhdVOXGEZAU\u0017W8KDH@R&\u0015BQ#0MN\u0014";

        static List<Col> dictionary = new List<Col>();
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(2000, 2000);
            Console.WriteLine("Press ENTER to start calibration;");
            string S = Console.ReadLine();
            if (S.ToLower() == "calibrate")
                WriteAllVariants();
            else
                LoadDictionary(S == "load");
            while (true)
            {
                string fileName = "";
                int res = 20;
                Console.WriteLine("\nName a picture path and set a resolution step;\nExample:\n\n\t\tnova.png\n\t\t7\n");
                try
                {
                    fileName = Console.ReadLine();
                    res = int.Parse(Console.ReadLine().Trim());
                    //Bitmap b = new Bitmap("cards/" + fileName);
                    Image[] frames = getFrames(Image.FromFile("cards/" + fileName));
                    foreach (Image i in frames.ToList())
                        Print((Bitmap)i, res);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Press ENTER to retry;");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static Image[] getFrames(Image originalImg)
        {
            int numberOfFrames = originalImg.GetFrameCount(FrameDimension.Time);
            Image[] frames = new Image[numberOfFrames];

            for (int i = 0; i < numberOfFrames; i++)
            {
                originalImg.SelectActiveFrame(FrameDimension.Time, i);
                frames[i] = ((Image)originalImg.Clone());

                Console.SetCursorPosition(5,5);
                Console.Write((i+"/" + numberOfFrames).PadLeft(20));
            }

            return frames;
        }

        static void WriteAllVariants()
        {
            Console.WriteLine("Calibration started. Enter misssteps and color barrier in format \"S S X\"");
            string[] parts = Console.ReadLine().Split(' ');
            int step = int.Parse(parts[0]), stepColor = int.Parse(parts[1]), done = 0, colorToper = int.Parse(parts[2]);
            if (colorToper < 0) colorToper = 600;

            for (int c = 0; c < s.Length; c += step)
                for (int b = 0; b < Math.Min(colorToper, availableBack.Count); b += stepColor)
                    for (int f = 0; f < Math.Min(colorToper, availableFore.Count); f += stepColor)
                    {
                        done++;
                        Console.BackgroundColor = availableBack[b];
                        Console.ForegroundColor = availableFore[f];
                        dictionary.Add(new Col(availableBack[b], availableFore[f], s[c], CalculateColor(s[c])));
                        Console.ResetColor();
                        Console.WriteLine((done + "/" + s.Length / step * availableBack.Count * availableFore.Count / stepColor / stepColor + "").PadLeft(50));
                        //Console.ReadKey();
                    }
            Console.Clear();
            Console.WriteLine("Calibration is over;");
            SaveDictionary();
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
            // Create a file to write to.
            string createText = "";//"Hello and Welcome" + Environment.NewLine;
            for (int i = 0; i < dictionary.Count; i++)
                createText += dictionary[i].ToString() + "\n";
            File.WriteAllText("save.txt", createText);
        }
        static
            ColNode root = new ColNode(0, Color.FromArgb(127, 127, 127));
        static void LoadDictionary(bool print)
        {
            //try
            //{
            string[] lines = System.IO.File.ReadAllLines(@"save.txt");
            Console.WriteLine();
            int lineInd = 0;
            foreach (string line in lines)
            {
                lineInd++;
                string[] parts = line.Split(new char[] { ',' }, 6);
                Col a = new Col(availableBack[int.Parse(parts[3])], availableFore[int.Parse(parts[4])], parts[5][0],
                    Color.FromArgb(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));
                if (print) { a.Print(); Console.ResetColor(); }
                dictionary.Add(a);
            }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("\nError in founding save! " + e.Message);
            //}
            foreach (Col c in dictionary)
                root.AddChild(c, 0, 1);
            int X = 10;
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
        public static int diff(Color a, Color b)
        {
            return Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B);
        }
        static void Print(Bitmap what, int resol)
        {
            Random rnd = new Random();
            int left = 0, top = 0;
            int resolHoriz = resol * 3 / 4;
            // what.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Dictionary<Color, int> inds = new Dictionary<Color, int>();

            for (int i = 0; i < what.Height; i += resol)
            {
                Console.SetCursorPosition(left, top + i / resol);
                for (int j = 0; j < what.Width; j += resolHoriz)
                {
                    int[] rgb = new int[3] { 0, 0, 0 }; int countPixels = 0;
                    for (int i2 = 0; i2 < resol; i2++)
                        for (int j2 = 0; j2 < resolHoriz; j2++)
                        {
                            if (i + i2 >= what.Height) continue;
                            if (j + j2 >= what.Width) continue;
                            rgb[0] += what.GetPixel(j + j2, i + i2).R;
                            rgb[1] += what.GetPixel(j + j2, i + i2).G;
                            rgb[2] += what.GetPixel(j + j2, i + i2).B;
                            countPixels++;
                        }
                    Color clr = Color.FromArgb((int)(rgb[0] / (countPixels * 1.0)), (int)(rgb[1] / (countPixels * 1.0)), (int)(rgb[2] / (countPixels * 1.0)));//what.GetPixel(j, i);

                    //root.FindFast(0, clr).Print();
                    //if (inds.ContainsKey(clr))
                    //{
                    //    dictionary[inds.FirstOrDefault(x => x.Key == clr).Value].Print();
                    //}
                    //else
                    //{
                    //Col c = ;
                    root.FindFast(0, clr).Print();
                    //    inds.Add(clr, dictionary.IndexOf(c));
                    //}
                    //    int bestInd = 0, minDiff = 255 * 3, minClose = 255 * 3;
                    //    for (int d = 0; d < dictionary.Count; d++)
                    //    {
                    //        int cd = diff(dictionary[d].associate, clr)
                    //            //
                    //        ;// +(int)(dictionary[d].colorDiff / 300.0);
                    //        //
                    //        if (cd < minDiff || ((cd <= minDiff) && (minClose < dictionary[d].colorDiff))) { minDiff = cd; bestInd = d; minClose = dictionary[d].colorDiff; }
                    //        if (cd < minDiff) { minDiff = cd; bestInd = d; }
                    //        if (minDiff <= 0) break;
                    //    }
                    //    dictionary[bestInd].Print();
                    //    inds.Add(clr, bestInd);
                    //}
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
