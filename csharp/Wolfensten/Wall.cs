using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfensten
{
    enum WallType
    {
        StoneWall = 0,
    }
    class Wall : AbstractObject, IDrawable
    {
        public Wall(WallType wt, int xx, int yy)
        {
            if (wt == WallType.StoneWall)
            {
                c = '▒';//'■';/////'▓';√│┤
                color = new ColorBlock(ConsoleColor.Black, ConsoleColor.DarkGray);
            }
            setXY(xx, yy);
        }

        protected ColorBlock color;
        protected char c;
        public ColorBlock Color { get { return color; } }
        public char Symbol { get { return c; } }
    }
}
