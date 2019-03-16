using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfensten
{
    class Program
    {
        static void Main(string[] args)
        {
            Map.FullScreen();
            MapDrawer.ClearConsole();

            Map testMap = Map.TestMap();
            MapDrawer mapDrawer = new MapDrawer(testMap);

            for (int i = 0; i < 20; ++i)
            {
                Unit spider = Unit.Spider(); spider.setXY(12 - i, 12 - i);
                testMap.addObject(spider);
            }

            Unit player = Unit.Player();
            player.setXY(-32, -32);
            testMap.addObject(player);


            mapDrawer.drawStart();
            do
            {
                testMap.step(testMap);
                mapDrawer.drawUpdate();
            } while (true);

            Console.ReadKey(true);
        }
    }
}
