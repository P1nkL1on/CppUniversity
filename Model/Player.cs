using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    abstract class Player
    {
        protected string name;
        protected int health;
        protected int mana = 2, currentMaxMana = 2, startMana = 2, maxMana = 10, startDrawCount = 5;

        public override string ToString()
        {
            return String.Format("{0}HP: {1}/20", name.PadRight(15), health);
        }
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Игрок делает ход. Если игра видит, что он ничего в этот ход не может сделать, то игрок его автоматически пропускат.
        /// Бегины и енды длятся 2 секунды.
        /// Мейны 60.
        /// </summary>
        /// <param name="phase">Фаза хода</param>
        /// <param name="context">Вызывающий это экземпляр игры</param>
        public void MakeTurnOrSkip(TurnPhase phase, Game context)
        {
            bool skip = false;
            if ((battlefield.count == 0 && (phase+"").ToLower().IndexOf("combat") >= 0) || phase == TurnPhase.combat_declareBlockers)
                skip = true;

            if (skip)
            {
                Utils.ConsoleWriteLine(" ** " + Name + " skips phase \'" + phase + "\'", ConsoleColor.DarkGray);
                context.gameOnPlayerTurn(this, phase);
                WaitTimer.finishCurrentTimer();
                return;
            }
            Utils.ConsoleWriteLine(" ** " + Name + " enters phase \'" + phase + "\'");
            context.gameOnPlayerTurn(this, phase);
            MakeTurn(phase, context);
        }
        public abstract void MakeTurn(TurnPhase phase, Game context);
        public abstract void GameStartProcess();
        
        /// <summary>
        /// При старте игры взять нужное число карт и наполнить минимальное число кристаллов.
        /// </summary>
        public void startGame()
        {
            drawCard(startDrawCount);
            currentMaxMana = startMana;
            refillManaCrystalls();
        }
        protected List<CardHolder> holds;
        /// <summary>
        /// Задаёт руку, кладбище, колоду, изгнание и поле боя для игрока.
        /// </summary>
        protected void constructDefaultHolders()
        {
            holds = new List<CardHolder>();
            holds.Add(new CardHolder(this, CardHolderTypes.hand, 7));
            holds.Add(new CardHolder(this, CardHolderTypes.deck));
            holds.Add(new CardHolder(this, CardHolderTypes.battlefield));
            holds.Add(new CardHolder(this, CardHolderTypes.graveyard));
            holds.Add(new CardHolder(this, CardHolderTypes.exile));

            setUpDeck();
        }

        protected void setUpDeck()
        {
            for (int i = 0; i < 60; ++i)
                deck.addCard(new AbstractCard("Card"+i, (new Random(i)).Next(11)));
        }

        public CardHolder hand { get { return holds[0]; } }
        public CardHolder deck { get { return holds[1]; } }
        public CardHolder battlefield { get { return holds[2]; } }
        public CardHolder graveyard { get { return holds[3]; } }
        public CardHolder exile { get { return holds[4]; } }
        public CardHolder where(CardHolderTypes type)
        { return holds[(int)type]; }







        public void drawCard()
        {
            drawCard(1);
        }
        public void drawCard(int cardCount)
        {
            Utils.ConsoleWriteLine(Utils.tab + String.Format("{0} draws {1} card(s);", name, cardCount));
        }
        public void gainManaCrystall(int manaCount)
        {
            mana += manaCount;
            Utils.ConsoleWriteLine(Utils.tab + String.Format("{0} gain {1} mana;", name, manaCount), ConsoleColor.DarkCyan);
        }
        public void setManaCrystalls(int to)
        {
            mana = to;
            Utils.ConsoleWriteLine(Utils.tab + String.Format("{0} now has {1}/{2} mana;", name, mana, currentMaxMana, maxMana), ConsoleColor.DarkCyan);
        }
        public void refillManaCrystalls()
        {
            setManaCrystalls(currentMaxMana);
        }
        public bool turnStartAddMana()
        {
            if (currentMaxMana >= maxMana)
                return false;
            Utils.ConsoleWriteLine(Utils.tab + String.Format("{0}'s manapool increased by 1. (Now {1}/{2} MAX)", name, currentMaxMana, maxMana), ConsoleColor.DarkCyan);
            currentMaxMana++;
            setManaCrystalls(currentMaxMana);
            return true;
        }
        /// <summary>
        /// Выводит игроку информацию о его манакристаллах.
        /// </summary>
        protected void drawManaCrystals()
        {
            setManaCrystalls(mana);
            Console.Write(Utils.tab + "[");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("".PadRight(mana, '*'));
            if (mana < currentMaxMana)
                Console.Write("".PadRight(currentMaxMana - mana, '°'));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("".PadRight(maxMana - Math.Max(mana, currentMaxMana), '-') + ']');
        }
    }
}
