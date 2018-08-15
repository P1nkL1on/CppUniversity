using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    abstract class  AbstractCardList
    {
        protected List<Card> list = new List<Card>();
        protected Player host;
        protected abstract String name { get; }
        public virtual void Add(Card what)
        {
            list.Add(what);
        }
        public virtual void Remove(Card what)
        {
            list.Remove(what);
        }
        public virtual bool IsIn(Card what)
        {
            return list.IndexOf(what) >= 0;
        }
        public virtual void RedrawFull(MPoint where, int maxWidth)
        {
            MDrawHandler.DrawStringInPoint(where, MDrawHandlerMTG.DefaultColor, String.Format("{0}'s {1}", host.PlayerName, name), maxWidth);
            for (int i = 0; i < list.Count; ++i)
                list[i].DrawHeader(where.Add(i + 1, 2), maxWidth - 2);
        }
        public virtual int Count
        {
            get { return list.Count; }
        }
    }
}
