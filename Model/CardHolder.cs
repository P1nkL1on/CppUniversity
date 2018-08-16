using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    enum CardHolderTypes
    {
        hand = 0,
        deck = 1,
        graveyard = 3,
        exile = 4,
        battlefield = 2
    }
    class CardHolder
    {
        CardHolderTypes name;
        Player host;
        List<AbstractCard> cards;
        int maximumSize = -1;   // -1 == unlimited


        public override string ToString()
        {
            string res = String.Format("{0}'s {1} ", host.Name, name+"");
            return res.PadRight(30, '.') +   String.Format("cards {0}", cards.Count);
        }
        public CardHolder(Player host, CardHolderTypes type, int maxSize)
        {
            this.host = host;
            this.name = type;
            this.maximumSize = maxSize;
            cards = new List<AbstractCard>();
        }
        public CardHolder(Player host, CardHolderTypes type)
        {
            this.host = host;
            this.name = type;
            cards = new List<AbstractCard>();
        }
        public int count
        {
            get { return cards.Count; }
        }
        public bool needDiscard
        {
            get {
                if (maximumSize < 0)
                    return false;
                return count > maximumSize;
            }
        }
        public void addCard(AbstractCard card)
        {
            cards.Add(card);
        }
        public List<AbstractCard> Cards
        {
            get { return cards; }
        }
        public AbstractCard topCard()
        {
            if (cards.Count == 0)
                return null;
            AbstractCard top = cards.Last();
            cards.Remove(top);
            return top;
        }
        public AbstractCard topCard(int ind)
        {
            if (cards.Count == 0)
                return null;
            AbstractCard top = cards[ind];
            cards.Remove(top);
            return top;
        }
        public void drawFrom(AbstractCard card)
        {
            cards.Add(card);
        }
        public void drawFrom(CardHolder from)
        {
            drawFrom(from.topCard());
        }
    }
}
