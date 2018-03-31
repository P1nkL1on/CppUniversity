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

        void DrawCard(int index, Card card)
        {
            card.TraceInList(bufLeft + 1, bufTop + 2 + index * 2);
        }
        public void Redraw()
        {
            DRAW.setBuffTo(bufLeft, bufTop);
            DRAW.str(name + " :");
            for (int i = currentTopIndex; i < currentList.Count; i++)
                if (i - currentTopIndex <= maxCards)
                    DrawCard(i, currentList[i]);
        }


    }
}
