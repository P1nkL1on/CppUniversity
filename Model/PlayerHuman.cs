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
            constructDefaultHolders();
        }
        public override void MakeTurn(TurnPhase phase)
        {
            Console.CursorVisible = true;
            while (true)
            {
                string answer = Console.ReadLine();
                if (answer == "m")
                {
                    drawManaCrystals();
                }
                if (answer == "f")
                {
                    Console.CursorVisible = false;
                    Console.WriteLine("Ending phase...");
                    Thread.Sleep(200);
                    WaitTimer.finishCurrentTimer();
                }
            }
        }
        public override void GameStartProcess()
        {
            Console.CursorVisible = true;
            Utils.selectNumberTight(ref startMana, ref startDrawCount, 2, "At game start:", 
                "You'll have {0} mana crystals;",
                "You'll draw {0} cards.");

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
            Console.CursorVisible = false;
            WaitTimer.playerReady();
        }
    }
}
