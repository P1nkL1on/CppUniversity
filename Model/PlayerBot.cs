using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Model
{
    class PlayerBot : Player
    {
        public PlayerBot(string name)
        {
            this.name = name;
            this.health = 20;
        }
        public override void MakeTurn()
        {
            Thread.Sleep(1000);
            Console.WriteLine(name + " is thinking...");
            Thread.Sleep(2000);
            Console.WriteLine(name + " decides to end turn.");
            Thread.Sleep(500);
            WaitTimer.finishCurrentTimer();
            return;
        }
    }
}
