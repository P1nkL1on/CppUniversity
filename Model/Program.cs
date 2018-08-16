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
            Console.WindowWidth *= 2;


            WaitTimer w = new WaitTimer(1, "Game starting in");
            w.setAction(new GameAction(() =>
            {
                Game g = new Game();
                UserCommands.initialiseWordList(g, g.players[0]);
                g.start();
            }));
        }
    }
}
