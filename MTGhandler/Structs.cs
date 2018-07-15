using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MTGhandler
{
    struct CColor
    {
        public ConsoleColor back;
        public ConsoleColor fore;
        public CColor(ConsoleColor b, ConsoleColor f)
        {
            back = b; fore = f;
        }
        public CColor(ConsoleColor b)
        {
            back = b;
            fore = b;
        }
        public void Apply()
        {
            Console.BackgroundColor = back;
            Console.ForegroundColor = fore;
        }
    }

    struct MPoint
    {
        public int x;
        public int y;
        public MPoint(int _x, int _y)
        {
            x = _x; y = _y;
        }
        public MPoint Add(int _x, int _y)
        {
            return new MPoint(x + _x, y + _y);
        }
        public MPoint AddX(int _x)
        {
            return Add(_x, 0);
        }
        public MPoint AddY(int _y)
        {
            return Add(0, _y);
        }
    }
    struct MRectangle
    {
        public int leftTopX;
        public int leftTopY;
        public int rightBottomX;
        public int rightBottomY;
        public MRectangle(int _leftTopX, int _leftTopY, int width, int height)
        {
            leftTopX = _leftTopX;
            leftTopY = _leftTopY;
            rightBottomX = _leftTopX + width -1;
            rightBottomY = _leftTopY + height - 1;
        }
        public MRectangle(MPoint _leftTopCorner, int width, int height)
        {
            leftTopX = _leftTopCorner.x;
            leftTopY = _leftTopCorner.y;
            rightBottomX = _leftTopCorner.x + width - 1;
            rightBottomY = _leftTopCorner.y + height - 1;
        }
        public MRectangle(MPoint _leftTopCorner, MPoint _rightBottomCorner)
        {
            leftTopX = _leftTopCorner.x;
            leftTopY = _leftTopCorner.y;
            rightBottomX = _rightBottomCorner.x;
            rightBottomY = _rightBottomCorner.y;
        }
        public MPoint From
        {
            get { return new MPoint(leftTopX, leftTopY); }
        }
        public MPoint To
        {
            get { return new MPoint(rightBottomX, rightBottomY); }
        }
        public int Width
        {
            get { return rightBottomX - leftTopX; }
        }
        public int Height
        {
            get { return rightBottomY - leftTopY; }
        }
    }
    
}
