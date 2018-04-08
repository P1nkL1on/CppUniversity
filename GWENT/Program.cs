using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GWENT
{
    class Program
    {
        static void Main(string[] args)
        {
            ASCIIdrawer.drawImage(0,0);
            //Console.WriteLine("Bronze", Color.FromArgb(150, 100,50));
            Console.SetWindowSize(Console.LargestWindowWidth * 4 / 5, Console.LargestWindowHeight * 7/ 8);
            Console.ReadLine();
            while (true)
            {
                Game g = new Game(new Random());
                g.Start();
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
