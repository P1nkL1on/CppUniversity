using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    enum CardColor
    {
        None = 0,
        White = 1,
        Blue = 2,
        Black = 3,
        Red = 4,
        Green = 5
    }
    class Card
    {
        protected string name;
        protected Player host;
        protected Player hostCurrent;
        protected CardColor color;
        protected List<CardPlayWay> playVariants;
        protected bool tapped;
        public virtual void Tap()
        {
            tapped = true;
        }
        public virtual void Untap()
        {
            tapped = false;
        }
        public bool isTapped
        {
            get { return tapped; }
        }

        public override string ToString()
        {
            return name;
        }
        public virtual string Name { get { return name; } }
        public virtual void SetHost(Player playerWhoPlayedThisCard) { this.host = this.hostCurrent = playerWhoPlayedThisCard; }
        public virtual Player GetHost { get { return host; } }
        public virtual Player GetCurrentHost { get { return hostCurrent; } }

        public virtual void DrawHeader(MPoint where, int maxWidth)
        {
            MDrawHandler.DrawStringInPoint(where, MDrawHandlerMTG.ColorOf(color), name, maxWidth);
        }
    }

    
}
