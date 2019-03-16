using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GWENT
{



    public delegate void Action( Card sender, Game gameState );
    public abstract class Card
    {
        public Cards exapler;
        public string name;
        protected string description;
        public List<Tag> tags;
        public Rarity rarity;
        protected int innerTimer;
        protected int timerStep;

        public abstract void TraceField(int bufHorizontal, int bufVertical);
        public abstract void TraceInList(int bufHorizontal, int bufVertical);
        public abstract void TraceFull(int bufHorizontal, int bufVertical, int wid);
        public abstract void Redraw();
        public abstract void RedrawSelected(bool selected);

        public abstract int Power {get;}
        public abstract bool isBlue(Game game);
        public abstract Point leftTop { get; }

        protected void SetParams(string n, string d, List<Tag> t, Rarity r)
        {
            name = n;
            description = d;
            tags = t;
            rarity = r;
            innerTimer = -1;
            timerStep = -1;
        }
    }
}
