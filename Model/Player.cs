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
        protected int mana = 2, currentMaxMana = 2, startMana = 2, maxMana = 10, startDrawCount = 6;

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
            Utils.ConsoleWriteLine(String.Format("{0} draws {1} card(s);", name, cardCount));
        }
        public void gainManaCrystall(int manaCount)
        {
            mana += manaCount; 
            Utils.ConsoleWriteLine(String.Format("{0} gain {1} mana;", name, manaCount), ConsoleColor.DarkCyan);
        }
        public void setManaCrystalls(int to)
        {
            mana = to;
            Utils.ConsoleWriteLine(String.Format("{0} now has {1}/{2} mana;  (MAX:{3})", name, mana, currentMaxMana, maxMana), ConsoleColor.DarkCyan);
        }
        public void refillManaCrystalls()
        {
            setManaCrystalls( currentMaxMana);
        }
    }
}
