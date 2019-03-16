using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfensten
{
    enum MapPoint
    {
        free = -1,
        wall = 0,
    }
    class Map : ISteppable
    {
        List<AbstractObject> obs;

        static bool isWall(AbstractObject o)
        {
            return (o as Wall != null);
        }

        private Map()
        {
            obs = new List<AbstractObject>();
        }
        public static Map TestMap()
        {
            int leftTopX = -41, leftTopY = -21,
                rightBotX = 21, rightBotY = 21;

            Map res = new Map();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 400; ++i)
                res.addObject(new Wall(WallType.StoneWall, rnd.Next(leftTopX, rightBotX + 1), rnd.Next(leftTopY, rightBotY + 1)));
            for (int w = leftTopX; w <= rightBotX; ++w)
                for (int h = leftTopY; h <= rightBotY; ++h)
                    if (res.whatIsIn(w, h) == MapPoint.free)
                        res.addObject(new Plate(PlateType.DotPlate, w, h));
            return res;
        }
        public static void FullScreen()
        {
            Console.CursorVisible = false;
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(Console.LargestWindowWidth * 2 / 3, Console.LargestWindowHeight * 5 / 6);
        }

        public void step(Map m)
        {
            foreach (AbstractObject o in obs)
                if (o as ISteppable != null)
                    (o as ISteppable).step(this);
        }

        public MapPoint whatIsIn(int X, int Y)
        {
            foreach (AbstractObject a in obs)
            {
                if (isWall(a) && a.isIn(X, Y))
                    return MapPoint.wall;
            }
            return MapPoint.free;
        }

        public List<IDrawable> allDrawableObjects
        {
            get
            {
                List<IDrawable> res = new List<IDrawable>();
                for (int i = 0; i < obs.Count(); ++i)
                    if ((obs[i] as IDrawable) != null)
                        res.Add(obs[i] as IDrawable);
                return res;
            }
        }
        public List<IDrawable> allShouldBeUpdatedObjects
        {
            get
            {
                List<int> moves = new List<int>() { 1, 0, -1, 0, 0, 1, 0, -1 };
                List<IDrawable> res = new List<IDrawable>();
                List<IDrawable> plates = new List<IDrawable>();
                for (int i = 0; i < obs.Count(); ++i)
                    if ((obs[i] as AbstractMovableObject) != null)
                    {
                        res.Add(obs[i] as IDrawable);
                        foreach (AbstractObject a in obs)
                            if (a as Plate != null)
                                for (int dim = 0; dim < 4; ++dim)
                                    if (a.isIn(obs[i].X + moves[dim * 2], obs[i].Y + moves[dim * 2 + 1]))
                                        plates.Add(a as IDrawable);
                    }
                foreach (IDrawable d in res)
                    plates.Add(d);
                return plates;
            }
        }

        public void addObject(AbstractObject what)
        {
            obs.Add(what);
        }
    }
}
