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
        public override void MakeTurn(TurnPhase phase, Game context)
        {
            if ((phase + "").IndexOf("main") >= 0)
                Utils.ConsoleWriteLine("Type any command:", ConsoleColor.DarkCyan);
            while (true)
            {
                string answer = Utils.selectItemFromUserCommands();
                Console.CursorLeft = 0; Console.CursorTop--;
                Console.WriteLine(answer.PadRight(Console.WindowWidth / 2));

                WaitTimer pause = null;
                if (answer == UserCommands.pauseCommand)
                {
                    pause = new WaitTimer(this.name);
                }

                if (answer == UserCommands.resumeCommand && WaitTimer.isPaused)
                {
                    WaitTimer.unpause(this.name);
                }

                if (answer == UserCommands.finishCommand)
                {
                    Console.WriteLine("Ending phase...");
                    Thread.Sleep(200);
                    WaitTimer.finishCurrentTimer();
                    return;
                }
                if (answer == UserCommands.playCardCommand)
                {
                    WaitTimer.writeOnLastTimer(Name + " is selecting card to play");
                    List<int> workInds = new List<int>();
                    int selected = Utils.selectVariant(availableCardsToPlay(out workInds), "Select a card to play");
                    if (selected >= 0)
                    {
                        drawManaCrystals();
                        AbstractCard cardToPlay = hand.Cards[workInds[selected]];
                        if (Utils.playerAgree(String.Format("Are you sure to play {0} for {1} mana",
                            cardToPlay.cardName, cardToPlay.Cost.value)))
                            playCard(hand.topCard(workInds[selected]), context);
                    }
                }
                if (answer != "")
                    context.executePlayersCommand(this, answer);
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
