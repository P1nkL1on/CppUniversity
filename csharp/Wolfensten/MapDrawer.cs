using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfensten
{
    class MapDrawer
    {
        public static ColorBlock defaultMapColor = new ColorBlock(ConsoleColor.DarkGray, ConsoleColor.DarkGray);
        public static void ClearConsole(){Console.ForegroundColor = defaultMapColor.fore; Console.BackgroundColor = defaultMapColor.back; Console.Clear();}
        protected Map map;
        protected int xOff = 50;
        protected int yOff = 50;

        public MapDrawer(Map map)
        {
            this.map = map;
        }

        public void traceSomething(int x, int y, char c, ColorBlock clr)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = clr.fore;

            Console.BackgroundColor = (!clr.ignoreBack)? clr.back : defaultMapColor.back;
            Console.Write(c);
        }

        public void drawStart()
        {
            drawList(map.allDrawableObjects);
        }

        public void drawUpdate()
        {
            drawList(map.allShouldBeUpdatedObjects);
        }

        private void drawList(List<IDrawable> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                AbstractObject ao = list[i] as AbstractObject;
                int fX = ao.X + xOff;
                int fY = ao.Y + yOff;
                if (fX >= 0 && fY >= 0)
                    traceSomething(fX, fY, list[i].Symbol, list[i].Color);
            }
        }
    }
}
