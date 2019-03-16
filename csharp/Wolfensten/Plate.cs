using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfensten
{
    enum PlateType
    {
        DotPlate = 0,
    }
    class Plate : AbstractObject, IDrawable
    {
        public Plate(PlateType pt, int xx, int yy)
        {
            if (pt == PlateType.DotPlate)
            {
                c = '.';
                color = new ColorBlock(ConsoleColor.Gray, ConsoleColor.DarkGray);
            }
            setXY(xx, yy);
        }

        protected ColorBlock color;
        protected char c;
        public ColorBlock Color { get { return color; } }
        public char Symbol { get { return c; } }
    }
}
