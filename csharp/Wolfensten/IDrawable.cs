using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfensten
{
    interface ISteppable
    {
        void step(Map m);   
    }
    struct ColorBlock{
        public ColorBlock(ConsoleColor f, ConsoleColor b)
        {
            fore = f;
            back = b;
            ignoreBack = false;
        }
        public ColorBlock(ConsoleColor f)
        {
            fore = f;
            back = ConsoleColor.White;
            ignoreBack = true;
        }
        public ConsoleColor fore;
        public ConsoleColor back;
        public bool ignoreBack;
    }
    interface IDrawable
    {
        ColorBlock Color { get; }
        char Symbol { get; }
    }
    class AbstractObject
    {
        protected int x;
        protected int y;
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public void setXY(int setX, int setY){
            x = setX; y = setY;
        }
        public bool isIn(int xx, int yy)
        {
            return (X == xx && Y == yy);
        }
    }

    class AbstractMovableObject : AbstractObject
    {
        public void moveX (int moveX, Map map){
            if (map.whatIsIn(X + moveX, Y) == MapPoint.free)
                x += moveX;
        }
        public void moveY(int moveY, Map map)
        {
            if (map.whatIsIn(X, Y + moveY) == MapPoint.free)
                y += moveY;
        }
        public void move(int mX, int mY, Map map)
        {
            moveX(mX, map);
            moveY(mY, map);
        }
    }
}
