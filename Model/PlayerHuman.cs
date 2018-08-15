using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Model
{
    class PlayerHuman : Player
    {
        public PlayerHuman(string name)
        {
            this.name = name;
            this.health = 20;
        }
        public override void MakeTurn()
        {
            while (true)
                if (Console.ReadLine() == "f")
                    WaitTimer.finishCurrentTimer();
        }
    }
}
