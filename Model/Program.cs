using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            WaitTimer w = new WaitTimer(1, "Game starting in");
            w.setAction(new GameAction(() => {
                Game g = new Game();
                g.start();
            }));
        }
    }
}
