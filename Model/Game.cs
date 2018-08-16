using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Model
{
    class Game
    {
        public List<Player> players;
        int currentTurnHostIndex = 0;
        int turnCount = 0;
        TurnPhase currentTurnPhase;
        public Game()
        {
            players = new List<Player>() { new PlayerHuman("Player"), new PlayerBot("Bot")/*, new PlayerBot("Bot2"), new PlayerBot("Bot3"), new PlayerBot("Bot4") */};
            currentTurnHostIndex = (new Random(DateTime.Now.Millisecond)).Next(players.Count) - 1;
            UserCommands.initialiseWordList(this, players[0]);
        }

        /// <summary>
        /// Игрок, ход которого сейчас идёт.
        /// </summary>
        Player currentPlayer
        {
            get { return players[currentTurnHostIndex]; }
        }
        /// <summary>
        /// Запустить игру ^^
        /// </summary>
        void execute()
        {
            checkScreenThreadStart();
            nextTurn();
            preGame();
        }


        /// <summary>
        /// Выполняет комагду, тиап смотреть на то, смотреть на сё
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="command"></param>
        public void executePlayersCommand(Player sender, string command)
        {

        }



        /// <summary>
        /// Вызывает события игры, которые должны произойти во время фазы хода определённого игрока.
        /// Например во время анкипа вы берёте карту. И т.д.
        /// </summary>
        /// <param name="who"></param>
        /// <param name="phase"></param>
        public void gameOnPlayerTurn(Player who, TurnPhase phase)
        {
            if (phase == TurnPhase.beginning_draw)
            {
                // nobody gain mana at first turn
                if (turnCount > players.Count)
                    who.turnStartAddMana();
                else
                    Utils.ConsoleWriteLine(" xx No mana gain, at first turn;", ConsoleColor.DarkRed);

                if (turnCount > 1)
                    who.drawCard();
                else
                    Utils.ConsoleWriteLine(" xx No card draw, caused by intiative;", ConsoleColor.DarkRed);
                return;
            }
        }







        void checkScreenThreadStart()
        {

            Thread myThread = new Thread(checkScreenBottom);
            myThread.Start();
        }
        void checkScreenBottom()
        {
            while (true)
            {
                if (Console.CursorTop > Console.WindowHeight * 4 / 5)
                    Console.Clear();
            }
        }
        /// <summary>
        /// Вызывает таймеры. Заставляет каждого игрока принять несколько решений.
        /// Например определить соотношение между картами и стартовыми кристалами  начале партии.
        /// Создаётся таймер на 30 секунд, который ждёт каждого игрока.
        /// </summary>
        void preGame()
        {
            WaitTimer preGameTimer = new WaitTimer(30, "Pre-game preparations", ConsoleColor.DarkGray, players.Count);
            for (int i = 0; i < players.Count; ++i)
            {
                Thread p = new Thread(players[i].GameStartProcess);
                p.Start();
            }
            preGameTimer.setAction(() =>
            {
                Console.WriteLine("\n** Game starts! **\n\n");
                foreach (Player p in players)
                    p.startGame();
            });
        }
        /// <summary>
        /// Создаёт таймер на 3 секунды, который вызывает фазы хода игрока.
        /// Когда фазы кончатся, то ход перейдёт следующему в порядке игроку.
        /// </summary>
        void nextTurn()
        {
            currentTurnHostIndex = (currentTurnHostIndex + 1) % players.Count;
            if (turnCount == 0)
                Console.WriteLine(currentPlayer.Name + " will start a game;\n\n");
            else
                Console.WriteLine(String.Format("\n\n** {0}'s turn starts ** ({1})", currentPlayer.Name, turnCount + 1));
            turnCount++;

            Thread currentPlayerTurn = new Thread(currentPlayerTurns);

            WaitTimer currentTurnTimer = new WaitTimer(1,
                String.Format("{0}'s turn", currentPlayer.Name),
                (currentTurnHostIndex == 0) ? ConsoleColor.Green : ConsoleColor.Red);
            currentTurnTimer.setAction(() => { currentPlayerTurn.Abort(); nextTurn(); });

            WaitTimer turnStartWait = new WaitTimer(3, "Turn starts in...");
            turnStartWait.setAction(() =>
            {
                currentPlayerTurn.Start();
            });
        }
        /// <summary>
        /// Начинает ставить в поток фазы хода текущего игрока.
        /// </summary>
        void currentPlayerTurns()
        {
            currentPlayerNextPhase(0);
        }
        /// <summary>
        /// Задаёт таймер текущей фазы хода. Вызывает у игрока метод пропустить этап или отреагировать.
        /// </summary>
        /// <param name="turnphase"></param>
        void currentPlayerNextPhase(int turnphase)
        {
            while (((TurnPhase)turnphase + "").Length <= 2 && turnphase < 50)
                turnphase++;
            if (turnphase >= 50)
                return;
            // skipping phases
            //
            currentTurnPhase = (TurnPhase)turnphase;
            string phaseName = String.Format("'{0}'",  currentTurnPhase + "");
            int timer = 15;
            if (phaseName.IndexOf("main") >= 0) timer = 60;
            if (phaseName.IndexOf("beginning") >= 0 || phaseName.IndexOf("ending") >= 0) timer = 2;

            WaitTimer currentTurnTimer = new WaitTimer(timer, phaseName,
                (currentTurnHostIndex == 0) ? ConsoleColor.Green : ConsoleColor.Red);
            currentTurnTimer.setAction(() => { currentPlayerNextPhase(turnphase+1); });

            currentPlayer.MakeTurnOrSkip(currentTurnPhase, this);
        }
        /// <summary>
        /// Запустить игру. (Начинается с преигры)
        /// </summary>
        public void start()
        {
            Thread myThread = new Thread(execute);
            myThread.Start();
        }
    }
    /// <summary>
    /// Фазы хода одного игрока
    /// </summary>
    enum TurnPhase
    {
        pregame = -1,
        beginning_untap = 0,
        beginning_upkeep = 1,
        beginning_draw = 2,

        main = 10,

        combat_entering = 20,
        combat_declareAttackers = 21,
        combat_declareBlockers = 22,
        combat_damaging = 23,
        combat_finishing = 24,

        main_secondAfterCombat = 30,

        ending_turn = 40,
        ending_cleaning = 41
    }
}
