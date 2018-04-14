using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DungMap
{
    class DungeonMaker
    {
        int wid, hei;
        List<DungRoom> rooms;
        static int maxRoomsSize = 100;
        static int roomSpawnRegular = 1;
        static int minDistance = 3;
        static int maxRoadsFrom = 2;
        int maxRoomCount;

        DungRoom nowIn;

        public DungeonMaker(int wi, int he)
        {
            wid = wi; hei = he;
            rooms = new List<DungRoom>();
            Random rnd = new Random();

            maxRoomCount = (wi * he) / (2 * maxRoomsSize) * 2 / 3;
            int done = 0;

            while (!TryAdd(rnd))
            { Console.SetCursorPosition(5, 5); Console.Write((++done + "%").PadRight(30)); }

            done = 0;//Console.Write("FINISH");
            foreach (DungRoom r in rooms)
            {
                FindClosest(r, rnd);
                Console.SetCursorPosition(5, 6); Console.Write((++done + "/" + rooms.Count).PadRight(30));
            }
            Console.Clear();
            Draw(true);
            Console.ReadLine();
        }

        public void Adventure()
        {
            Console.Clear();

            List<ConsoleKey> availableKeys = new List<ConsoleKey>() { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7 };


            nowIn = rooms[0];

            while (true)
            {
                nowIn.DrawSelfCurrentRoom();
                ConsoleKey k = ConsoleKey.A;
                List<ConsoleKey> canGo = new List<ConsoleKey>();
                for (int i = 0; i < nowIn.near.Count; i++) canGo.Add(availableKeys[i]);
                canGo.Add(ConsoleKey.Q);
                do
                {
                    k = Console.ReadKey().Key;
                    if (k == ConsoleKey.Q)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed; Console.Write("Are you sure to exit : y/n ");
                        k = Console.ReadKey().Key;
                        if (k == ConsoleKey.Y)
                        {
                            Console.ResetColor();
                            Console.Clear();
                            break;
                        }
                    }
                } while (canGo.IndexOf(k) < 0);

                if (k == ConsoleKey.Y) break;

                int choosenRoad = canGo.IndexOf(k);
                nowIn.DrawSelf('.');

                if (nowIn == nowIn.near[choosenRoad].from)
                    nowIn = nowIn.near[choosenRoad].to;
                else
                    nowIn = nowIn.near[choosenRoad].from;
            }
        }

        bool TryAdd(Random rnd)
        {
            if ((rooms.Count < maxRoomCount || rnd.Next(20) != 0) && (rnd.Next(rooms.Count * roomSpawnRegular / 2) == 0 || rooms.Count == 0))
            {
                DungRoom r = new DungRoom();
                r.SetParmas(rnd.Next(wid - 4), rnd.Next(hei - 4), 4, 4);
                rooms.Add(r);
            }
            foreach (DungRoom r in rooms)
            {
                if (rnd.Next(6) == 0 && r.S < maxRoomsSize)
                    r.Scale(rnd.Next(2), rnd.Next(2));
                //if (rnd.Next(5) == 0)
                //    r.Move(rnd.Next(-1, 2), rnd.Next(-1, 2));
            }

            bool anyCollides = false;
            do
            {
                anyCollides = false;
                foreach (DungRoom r in rooms)
                    foreach (DungRoom ra in rooms)
                        if (r != ra)
                            if (r.HitTest(ra, minDistance))
                            {
                                anyCollides = true; r.MakeRed(ConsoleColor.Red); ra.MakeRed(ConsoleColor.Red); r.Move(rnd.Next(-1, 2), rnd.Next(-1, 2)); ra.Move(rnd.Next(-1, 2), rnd.Next(-1, 2));
                                //Draw(false);
                            }
            } while (anyCollides);
            return rooms.Count >= maxRoomCount;
        }
        void Draw(bool clear)
        {
            //Console.Clear();
            foreach (DungRoom r in rooms)
            {
                if (clear) r.MakeNormal();
                r.DrawSelf();
            }
        }
        List<Tuple<int, DungRoom>> FindClosest(DungRoom dr, Random rnd)
        {
            //Draw(true);
            //dr.MakeRed(ConsoleColor.Red);
            //Draw(false);
            //List<> res = new List<Tuple<int, DungRoom>>();
            List<Tuple<int, DungRoom>> closest = new List<Tuple<int, DungRoom>>();
            int maxCount = maxRoadsFrom;
            for (int i = 0; i < maxCount; i++) closest.Add(new Tuple<int, DungRoom>(100, null));
            //DungRoom dr = rooms[0];
            for (int w = 0; w < dr.wid * 2 + dr.hei * 2; w++)
            {
                int dist = 0;
                while (dist < Math.Min(wid, hei))
                {
                    ++dist; bool found = false;
                    foreach (DungRoom r in rooms) if (r != dr)
                            if ((w < dr.wid && r.HitTest(dr.left + w, dr.top - dist))
                                ||
                                (w >= dr.wid && w < dr.wid * 2 && r.HitTest(dr.left + w - dr.wid, dr.top + dr.hei + dist))
                                ||
                                (w >= dr.wid * 2 && w < dr.wid * 2 + dr.hei && r.HitTest(dr.left - dist, dr.top + w - dr.wid * 2))
                                ||
                                (w >= dr.wid * 2 + dr.hei && r.HitTest(dr.left + dr.wid + dist, dr.top + w - dr.wid * 2 - dr.hei))
                                )
                            {
                                found = true;
                                Tuple<int, DungRoom> foundT = new Tuple<int, DungRoom>(dist, r);
                                for (int i = 0; i < maxCount; i++)
                                    if (foundT.Item1 < closest[i].Item1)
                                    {
                                        if (closest.IndexOf(foundT) < 0)
                                        {
                                            if (closest[i].Item2 != foundT.Item2) for (int I = maxCount - 1; I > i; I--) closest[I] = closest[I - 1];
                                            closest[i] = foundT; break;
                                        }
                                    }
                                break;
                            }
                    if (found)
                        break;
                }
                //res.Add(new Tuple<int, DungRoom>());
            }
            for (int i = 0; i < maxCount; i++)
                if (closest[i].Item2 != null)
                {
                    closest[i].Item2.MakeRed(ConsoleColor.Green);
                    if (dr.NearRooms().IndexOf(closest[i].Item2) < 0)
                        dr.makeRoad(closest[i].Item2, rnd);
                }
            //Draw(false);

            return closest;
        }
    }
}
