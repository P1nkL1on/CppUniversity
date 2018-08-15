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
        int turnCount = 0;
        public Game()
        {
            players = new List<Player>() { new PlayerHuman("Player"), new PlayerBot("Bot")/*, new PlayerBot("Bot2"), new PlayerBot("Bot3"), new PlayerBot("Bot4") */};
            currentTurnHostIndex = (new Random(DateTime.Now.Millisecond)).Next(players.Count) - 1;
        }

        Player currentPlayer
        {
            get { return players[currentTurnHostIndex]; }
        }
        void execute()
        {
            nextTurn();
            preGame();
        }
        void preGame()
        {
            WaitTimer preGameTimer = new WaitTimer(30, "Pre-game preparations", ConsoleColor.DarkGray, players.Count);
            for (int i = 0; i < players.Count; ++i)
            {
                Thread p = new Thread(players[i].GameStartProcess);
                p.Start();
            }
            preGameTimer.setAction(() => {
                Console.WriteLine("\n** Game starts! **\n\n");
                foreach (Player p in players)
                    p.startGame();
            });
        }
        void nextTurn()
        {
            currentTurnHostIndex = (currentTurnHostIndex + 1) % players.Count;
            if (turnCount == 0)
                Console.WriteLine(currentPlayer.Name + " will start a game;\n\n");
            else
                Console.WriteLine(String.Format("\n\n** {0}'s turn starts **", currentPlayer.Name));
            turnCount++;

            Thread currentPlayerTurn = new Thread(currentPlayerTurns);

            WaitTimer currentTurnTimer = new WaitTimer(90,
                String.Format("{0}'s turn", currentPlayer.Name),
                (currentTurnHostIndex == 0) ? ConsoleColor.Green : ConsoleColor.Red);
            currentTurnTimer.setAction(() => { currentPlayerTurn.Abort() ; nextTurn(); });

            WaitTimer turnStartWait = new WaitTimer(3, "Turn starts in...");
            turnStartWait.setAction(() =>
            {
                currentPlayerTurn.Start();
            });
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
