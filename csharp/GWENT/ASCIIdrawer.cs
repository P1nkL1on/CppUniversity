using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Console = Colorful.Console;

namespace GWENT
{
    public static class ASCIIdrawer
    {
        public static void drawImage(int top, int left)
        {
            Bitmap b = new Bitmap("cards/a.png");
            //b = new Bitmap(b, b.Width / 30, b.Height / 30);
            int step = 30;
            Color bc;
            for (int i = 0; i < b.Height; i+= step)
            {
                DRAW.setBuffTo(left, top + i / step);
                for (int j = 0; j < b.Width; j += step)
                {
                    bc = b.GetPixel(j, i);
                    //Console.ForegroundColor = Color.FromArgb(255, bc.R, bc.G, bc.B);//Color.Red;
                    //Console.Write("*", bc);
                    DrawPixel(bc);
                }
                //Console.ReadKey();
            }
        }
        static string symbolsOfDraw = " .,-~+*o8#@";
        static void DrawPixel(Color pixel)
        {
            int darkness = pixel.B + pixel.G + pixel.R, ind = darkness *(symbolsOfDraw.Length-1) / (255*3);
            Console.Write(symbolsOfDraw[ind]);
        }
    }
}
