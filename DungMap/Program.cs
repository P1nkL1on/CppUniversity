using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungMap
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetBufferSize(1000, 1000);
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            while (true)
            {
                Console.Clear(); Console.WriteLine("Write:\nwidth\nheith\n\n");
                string input = Console.ReadLine();
                int wid = 50, hei = 50;
                if (input != "")
                { wid = int.Parse(input); hei = int.Parse(Console.ReadLine()); }
                DungeonMaker dm = new DungeonMaker(wid, hei);
                dm.Adventure();
                Console.ReadKey();
            }
        }
    }
}
