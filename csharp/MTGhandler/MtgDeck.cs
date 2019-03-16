using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class Deck : AbstractCardList
    {
        public Deck(Player host)
        {
            this.host = host;
        }
        protected override string name
        {
            get { return "Deck"; }
        }
    }
}
