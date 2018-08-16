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
        public string cardName;
        protected manaCost cost;
        public Player host;

        public AbstractCard(string name, int cost, Player host)
        {
            setNameAndCost(name, new manaCost(cost), host);
        }
        void setNameAndCost(string name, manaCost cost, Player host)
        {
            cardName = name;
            this.cost = cost;
            this.host = host;
        }
        public override string ToString()
        {
            return String.Format("{0} ({1})", cardName, cost.value);
        }
        public manaCost Cost
        {
            get { return cost; }
        }
    }
}
