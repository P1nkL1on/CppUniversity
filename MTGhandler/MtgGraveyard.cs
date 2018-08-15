using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class Graveyard : AbstractCardList
    {
        public Graveyard(Player host)
        {
            this.host = host;
        }

        protected override string name
        {
            get { return "Graveyard"; }
        }
    }
}
