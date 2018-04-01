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
            Console.SetWindowSize(Console.LargestWindowWidth * 4 / 5, Console.LargestWindowHeight * 7/ 8);
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
