using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Model
{
    class PlayerBot : Player
    {
        protected ConsoleColor botTextColor = ConsoleColor.DarkGreen;
        public PlayerBot(string name)
        {
            this.name = name;
            this.health = 20;
        }
        public override void MakeTurn()
        {
            Thread.Sleep(1000);
            Utils.ConsoleWriteLine(name + " is thinking...", botTextColor);
            Thread.Sleep((new Random(DateTime.Now.Millisecond)).Next(10, 2000));
            Utils.ConsoleWriteLine(name + " decides to end turn.", botTextColor);
            Thread.Sleep(500);
            WaitTimer.finishCurrentTimer();
            return;
        }
        public override void GameStartProcess()
        {
            Thread.Sleep(new Random(DateTime.Now.Millisecond * (int)(name[name.Length - 1])).Next(1000, 5000));
            WaitTimer.playerReady();
            return;
        }
    }
}
