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
        public override string ToString()
        {
            return String.Format("{0}HP: {1}/20", name.PadRight(15), health);
        }
        public string Name
        {
            get { return name; }
        }
        public abstract void MakeTurn();
    }
}
