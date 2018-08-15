using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Model
{
    class WaitTimer
    {
        private static List<WaitTimer> allTimers = new List<WaitTimer>();
        private GameAction finish;
        static string loadingBar = "/-\\|";
        static int readyAwwaits = 0;
        int totalAwaits = 1;
        int nowBar = 0;
        float secondLasts = 0;
        int secondsToWait = 0;
        int x = 0;
        int y = 0;
        
        static int wid = 30;
        ConsoleColor color;
        String name;

        public WaitTimer(int time, String name, ConsoleColor color)
        {
            secondLasts = this.secondsToWait = time;
            this.name = name;
            this.color = color;
            finish = new GameAction(() => { });
            calculateXY();
            start();
        }
        public WaitTimer(int time, String name, ConsoleColor color, int playerCount)
        {
            secondLasts = this.secondsToWait = time;
            this.name = name;
            this.color = color;
            finish = new GameAction(() => { });
            calculateXY();
            start(playerCount);
        }
        public WaitTimer(int time, String name)
        {
            secondLasts = this.secondsToWait = time;
            this.name = name;
            this.color = Console.ForegroundColor;
            finish = new GameAction(() => { });
            calculateXY();
            start();
        }
        public WaitTimer(int time)
        {
            secondLasts = this.secondsToWait = time;
            this.name = "Time";
            this.color = Console.ForegroundColor;
            finish = new GameAction(() => { });
            calculateXY();
            start();
        }
        bool needRedraw = false;
        void calculateXY()
        {
            needRedraw = (5 + 2 * allTimers.Count != y);
            x = Console.WindowWidth / 2;
            y = 5 + 2 * allTimers.Count;
        }
        public void setAction(GameAction ac)
        {
            this.finish = ac;
        }
        void setXY(int x, int y)
        {
            this.x = x; this.y = y;
        }
        void trace()
        {
            trace(this.x, this.y);
        }
        void trace(int x0, int y0)
        {
            lock (Utils.ConsoleWriterLock)
            {
                int wasX = Console.CursorLeft, wasY = Console.CursorTop;
                ConsoleColor wasFore = Console.ForegroundColor;
                Console.SetCursorPosition(x0, y0);
                Console.Write(String.Format("{0}:{1}", name, String.Format("{0}/{1} {2}", Math.Round(secondLasts + .2f), secondsToWait, loadingBar[(nowBar) % 4]).PadLeft(wid - name.Length - 1)));
                Console.SetCursorPosition(x0, y0 + 1);
                Console.ForegroundColor = color;
                int pos = (int)(wid * 1.0 * secondLasts / secondsToWait);
                Console.Write("".PadLeft(pos, '▓'));
                Console.Write("".PadLeft(wid - pos, '░'));
                if (totalAwaits > 1)
                {
                    string s = String.Format("Players ready {0} from {1}...", totalAwaits - readyAwwaits, totalAwaits);
                    Console.SetCursorPosition(x0 + wid - s.Length, y0 + 2);
                    Console.Write(s);
                }
                Console.ForegroundColor = wasFore;
                Console.SetCursorPosition(wasX, wasY);
            }
        }
        void erase()
        {
            erase(this.x, this.y);
        }
        void erase(int x0, int y0)
        {
            lock (Utils.ConsoleWriterLock)
            {
                int wasX = Console.CursorLeft, wasY = Console.CursorTop;
                int d = 2;
                if (totalAwaits > 1) d++;
                for (int i = 0; i < d; ++i)
                {
                    Console.SetCursorPosition(x0, y0 + i);
                    Console.Write("".PadLeft(wid));
                }
                Console.SetCursorPosition(wasX, wasY);
            }
        }
        public void setTime(int X)
        {
            secondLasts = X;
        }
        void execute()
        {
            Thread.Sleep(100);
            do
            {
                trace();
                Thread.Sleep(500);
                if (allTimers.Last() == this)
                {
                    secondLasts -= .5f;
                    nowBar++;
                }
            } while (secondLasts > 0);
            erase();
            allTimers.Remove(this);
            finish();
        }
        public void start()
        {
            start(1);
        }
        public void start(int playerNeedResolveToFinish)
        {
            allTimers.Add(this);
            Thread myThread = new Thread(execute);
            myThread.Start();
            readyAwwaits = playerNeedResolveToFinish;
            this.totalAwaits = readyAwwaits;
        }
        public static void finishCurrentTimer()
        {
            allTimers.Last().setTime(0);
        }
        public static void playerReady()
        {
            readyAwwaits--;
            //Console.WriteLine((readyAwwaits == 0) ? "Everyone is ready!" : ("Awaits for " + readyAwwaits + " players..."));
            if (readyAwwaits == 0)
                allTimers.Last().setTime(0);
        }
    }
}
