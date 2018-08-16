using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    struct manaCost
    {
        public int value;
        public manaCost(int X)
        {
            value = X;
        }
    }
    class AbstractCard
    {
        protected string cardName;
        protected manaCost cost;

        public AbstractCard(string name, int cost)
        {
            setNameAndCost(name, new manaCost(cost));
        }
        void setNameAndCost(string name, manaCost cost)
        {
            cardName = name;
            this.cost = cost;
        }
    }
}
