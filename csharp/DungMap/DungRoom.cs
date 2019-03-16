using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungMap
{
    class DungRoom
    {
        static int number = 0;
        ConsoleColor clr;
        bool needRedraw, collides;
        public List<DungRoad> near;
        public int left, top, wid, hei;
        string personalName;
        public DungRoom()
        {
            personalName = "Room #" + number++;
            left = top = 0;
            wid = hei = 0;
            near = new List<DungRoad>();
            needRedraw = true;
            collides = false;
            clr = ConsoleColor.Gray;
        }
        public void SetParmas(int le, int to, int wi, int he)
        {
            left = le; top = to; wid = wi; hei = he;
        }
        public virtual void DrawSelf()
        {
            if (!needRedraw) return;
            DrawSelf('#');
            needRedraw = false;
        }
        public virtual void DrawSelf(char s)
        {
            for (int i = 0; i < near.Count; i++)
                near[i].Draw("");
                if (collides) Console.ForegroundColor = clr;
            for (int i = 0; i < hei; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write("".PadRight(wid, s));
            }
            Console.ResetColor();
        }
        public virtual void DrawSelfCurrentRoom()
        {
            for (int i = 0; i < near.Count; i++)
                near[i].Draw((i + 1) + "");
            if (collides) Console.ForegroundColor = clr;
            for (int i = 0; i < hei; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write("".PadRight(wid, '#'));
            }
            Console.SetCursorPosition(left + wid / 2, top + hei / 2);
            Console.ResetColor();
        }


        public int Move(int X, int Y)
        {
            if (left + X < 0 || top + Y < 0) return -1;
            //DrawSelf(' ');
            left += X;
            top += Y;
            needRedraw = true;
            return 0;
        }
        public int Scale(int W, int H)
        {
            if (wid + W < 0 || hei + H < 0) return -1;
            wid += W;
            hei += H;
            needRedraw = true;
            return 0;
        }
        public int S
        {
            get { return wid * hei; }
        }
        public bool HitTest(DungRoom a, int dop)
        {
            if ((left + wid + dop > a.left && left < a.left + a.wid + dop) && (top + hei + dop > a.top && top < a.hei + a.top + dop))
                return true;
            return false;
        }
        public bool HitTest(int X, int Y)
        {
            return (X >= left && X <= left + wid - 1) && (Y >= top && Y <= top + hei - 1);
        }

        public void MakeRed(ConsoleColor to)
        {
            clr = to;
            collides = true;
            needRedraw = true;
        }
        public void MakeNormal()
        {
            collides = false;
            needRedraw = true;
        }
        public override string ToString()
        {
            return personalName + " " + base.ToString();
        }
        public DungRoad makeRoad(DungRoom to, Random rnd)
        {
            DungRoad r = new DungRoad(this, to);

            {
                // finng road direction
                if (!(to.top + to.hei < top || to.top > top + hei))
                {
                    if (to.left > left + wid)
                        //A ----> B
                        r.SetParams(specRnd(rnd, Math.Max(1, to.top - top), Math.Min(hei, to.top + to.hei - top) - 1), wid, 0, to.left - left - wid);
                    if (to.left + to.wid < left)
                        r.SetParams(specRnd(rnd, Math.Max(1, to.top - top), Math.Min(hei, to.top + to.hei - top) - 1), 0, 1, left - to.left - to.wid);
                }
                else
                {
                    if (to.top > top + hei)
                        // A v B
                        r.SetParams(hei, specRnd(rnd, Math.Max(1, to.left - left), Math.Min(wid, to.left + to.wid - left)) - 1, 2, to.top - top - hei + 1);
                    if (to.top + to.hei < top)
                        r.SetParams(0, specRnd(rnd, Math.Max(1, to.left - left), Math.Min(wid, to.left + to.wid - left)) - 1, 3, top - to.top - to.hei + 1);
                }
            }

            //r.Draw();
            near.Add(r);
            to.near.Add(r);
            return r;
        }
        int specRnd(Random rnd, int a, int b)
        {
            return rnd.Next(Math.Min(a, b), Math.Max(a, b) + 1);
        }
        public List<DungRoom> NearRooms()
        {
            List<DungRoom> res = new List<DungRoom>();
            foreach (DungRoad r in near)
            {
                if (res.IndexOf(r.to) < 0)
                    res.Add(r.to);
                if (res.IndexOf(r.from) < 0)
                    res.Add(r.from);
            }
            return res;
        }
    }
}
