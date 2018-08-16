using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Model
{
    class PlayerBot : Player
    {
        protected ConsoleColor botTextColor = ConsoleColor.DarkRed;
        public PlayerBot(string name)
        {
            this.name = name;
            this.health = 20;
            constructDefaultHolders();
        }
        public override void MakeTurn(TurnPhase phase, Game context)
        {
            string phaseName = phase+"";
            if (phaseName.IndexOf("main") >= 0)
            {
                Thread.Sleep(1000);
                Utils.ConsoleWriteLine(name + " is thinking...", botTextColor);
                Thread.Sleep((new Random(DateTime.Now.Millisecond)).Next(10, 2000));
                Utils.ConsoleWriteLine(name + " does nothing;", botTextColor);
                Thread.Sleep(500);
                Utils.ConsoleWriteLine(name + " continues...", botTextColor);
                WaitTimer.finishCurrentTimer();
            }
            else
            {
                Thread.Sleep(100);
                Utils.ConsoleWriteLine(name + "  does nothing;", botTextColor);
            }
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
