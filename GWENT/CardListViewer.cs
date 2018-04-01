using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWENT
{
    class CardListViewer
    {
        string name;
        List<Card> currentList;
        List<int> selectedIndexes;
        int bufTop, bufLeft, currentSelected, maxCards, currentTopIndex;
        public CardListViewer(int bufTop, int bufLeft, int maxCardAtTime)
        {
            selectedIndexes = new List<int>();
            currentList = new List<Card>();
            name = "List";
            this.bufTop = bufTop;
            this.bufLeft = bufLeft;
            currentSelected = 0;
            maxCards = maxCardAtTime;
            currentTopIndex = 0;
        }

        public void SetList(string name, List<Card> list)
        {
            this.name = name;
            this.currentList = list;
        }

        void ResetSelection()
        {
            selectedIndexes = new List<int>();
        }

        void TraceCard(int index, Card card)
        {
            card.TraceInList(bufLeft + 1, bufTop + 2 + index * 2);
        }
        void TraceEmptyCard(int index)
        {
            DRAW.setBuffTo(bufLeft + 1, bufTop + 2 + index * 2);
            DRAW.str("".PadLeft(15));
        }
        public void Redraw()
        {
            DRAW.setBuffTo(bufLeft, bufTop);
            DRAW.str(name + " :");
            for (int i = currentTopIndex; i < currentTopIndex + maxCards; i++)
                if (i - currentTopIndex <= maxCards && i < currentList.Count)
                    TraceCard(i, currentList[i]);
                else
                    TraceEmptyCard(i);
        }


    }
}
