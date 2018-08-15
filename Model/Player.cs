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
        public abstract void MakeTurn();
        public abstract void GameStartProcess();

        public void startGame()
        {
            drawCard(startDrawCount);
            currentMaxMana = startMana;
            refillManaCrystalls();
        }








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
            Utils.ConsoleWriteLine(Utils.tab + String.Format("{0} now has {1}/{2} mana;", name, mana, currentMaxMana, maxMana));
        }
        public void refillManaCrystalls()
        {
            setManaCrystalls(currentMaxMana);
        }

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
