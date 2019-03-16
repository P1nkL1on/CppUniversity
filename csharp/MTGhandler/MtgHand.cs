using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class Hand : AbstractCardList
    {
        int maximumSize;
        public Hand(Player host, int maxSize)
        {
            this.host = host;
            maximumSize = maxSize;
        }
        protected override string name
        {
            get { return "Hand"; }
        }
    }
}
