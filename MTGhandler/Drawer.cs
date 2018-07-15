using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class MDrawHandler
    {
        public static void MaximiseWindow()
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
        }
        private static Stack<MPoint> pointStack = new Stack<MPoint>();
        private static List<CColor> colorStack = new List<CColor>();

        public static void DrawRectangleBorder(MRectangle rec, CColor color)
        {
            SetCursor(rec.From);
            MPoint leftBottom = MoveCursorFromLastY(rec.Height);
            MPoint rightTop = MoveCursorFromLastX(rec.Width);
            DrawStringInPoint(rec.From, color, "".PadLeft(rec.Width));
            DrawStringInPoint(leftBottom, color, "".PadLeft(rec.Width));
            DrawLine(rec.From, leftBottom, color);
            DrawLine(rec.To, rightTop, color);
        }

        public static void DrawRectangle(MRectangle rec, CColor color)
        {
            SetCursor(rec.From);
            for (int i = 0; i <= rec.Height; ++i)
            {
                DrawStringInPoint(LastPoint(), color, "".PadLeft(rec.Width + 1));
                SetCursor(MoveCursorFromLastY(1));
            }
        }
        public static void DrawRectangle(MPoint where, int width, int height, CColor color)
        {
            DrawRectangle(new MRectangle(where, width, height), color);
        }
        public static void DrawRectangle(MPoint from, MPoint to, CColor color)
        {
            DrawRectangle(new MRectangle(from, to), color);
        }
        public static void DrawRectangle(int x, int y, int width, int height, CColor color)
        {
            DrawRectangle(x, y, width, height, color);
        }

        public static void DrawLine(MPoint from, MPoint to, CColor color)
        {
            DrawLine(from.x, from.y, to.x, to.y, color);
        }
        private static void Swap(ref int A, ref int B)
        {
            int T = A;
            A = B;
            B = T;
        }
        private static void DrawLine(int x0, int y0, int x1, int y1, CColor color)
        {
            bool steep = false;
            if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
                steep = true;
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            int dx = x1 - x0;
            int dy = y1 - y0;
            int derror2 = Math.Abs(dy) * 2;
            int error2 = 0;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    DrawPoint(y, x, color);
                }
                else
                {
                    DrawPoint(x, y, color);
                }
                error2 += derror2;

                if (error2 > dx)
                {
                    y += (y1 > y0 ? 1 : -1);
                    error2 -= dx * 2;
                }
            }
        }
        public static void DrawPoint(int x, int y, CColor c)
        {
            DrawStringInPoint(x, y, c, " ");
        }
        public static void DrawStringInPoint(int x, int y, CColor c, String S)
        {
            SetCursor(x, y);
            c.Apply();
            Console.Write(S);
        }
        public static void DrawStringInPoint(MPoint where, CColor c, String S)
        {
            SetCursor(where.x, where.y);
            c.Apply();
            Console.Write(S);
        }
        public static void DrawStringInPoint(MPoint where, CColor c, String S, int maxSymbols)
        {
            SetCursor(where.x, where.y);
            c.Apply();
            Console.Write(Short(S,maxSymbols));
        }
        public static String Short(String S, int maxSymbols)
        {
            if (S.Length <= maxSymbols)
                return S;
            return S.Substring(0, maxSymbols - 2) + "..";
        }
        private static void SetCursor(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            pointStack.Push(new MPoint(x, y));
        }
        private static void SetCursor(MPoint where)
        {
            Console.SetCursorPosition(where.x, where.y);
            pointStack.Push(where);
        }
        private static void MoveCursor(MPoint where)
        {
            Console.SetCursorPosition(where.x, where.y);
        }
        private static MPoint PopPoint()
        {
            if (pointStack.Count == 0)
                return new MPoint(-1, -1);
            return pointStack.Pop();
        }
        private static MPoint LastPoint()
        {
            if (pointStack.Count == 0)
                return new MPoint(-1, -1);
            return pointStack.Peek();
        }
        private static MPoint MoveCursorFromLast(int _x, int _y)
        {
            return LastPoint().Add(_x, _y);
        }
        private static MPoint MoveCursorFromLastX(int _x)
        {
            return LastPoint().AddX(_x);
        }
        private static MPoint MoveCursorFromLastY(int _y)
        {
            return LastPoint().AddY(_y);
        }
    }
}
