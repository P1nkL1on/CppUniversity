using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Model
{
    class Game
    {
        List<Player> players;
        int currentTurnHostIndex = 0;
        public Game()
        {
            players = new List<Player>() { new PlayerHuman("Player"), new PlayerBot("Bot")};
            currentTurnHostIndex = (new Random(DateTime.Now.Millisecond)).Next(players.Count) - 1;
        }

        Player currentPlayer
        {
            get { return players[currentTurnHostIndex]; }
        }
        void execute()
        {
            nextTurn();
        }
        void nextTurn()
        {
            currentTurnHostIndex = (currentTurnHostIndex + 1) % players.Count;
            Console.WriteLine(String.Format("\n\n** {0}'s turn starts **", currentPlayer.Name));
            Thread myThread = new Thread(currentPlayerTurns);
            myThread.Start();

            WaitTimer currentTurnTimer = new WaitTimer(90,
                String.Format("{0}'s turn", currentPlayer.Name),
                (currentTurnHostIndex == 0) ? ConsoleColor.Green : ConsoleColor.Red);
            currentTurnTimer.setAction(() => { nextTurn(); });

            WaitTimer turnStartWait = new WaitTimer(3, "Turn starts in...");
        }
        void currentPlayerTurns()
        {
            currentPlayer.MakeTurn();
        }

        public void start()
        {
            Thread myThread = new Thread(execute);
            myThread.Start();
        }
    }
}
