using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    enum Place
    {
        exile = -1,
        hand = 0,
        graveyard = 1,
        deck = 2,
        battleground = 3
    }
    enum TurnPhase
    {
        beggining = 0,
        mainFirst = 1,
        combat = 2,
        mainSecond = 3,
        ending = 4
    }
    struct CardPlayWay
    {
        public String name;
        public Place from;
        public Place to;
        public ManaCost cost;
        public List<TurnPhase> ableTurnPhase;
        private CardPlayWay(String name, Place from, Place to, ManaCost cost, List<TurnPhase> when)
        {
            this.name = name;
            this.from = from;
            this.to = to;
            this.cost = cost;
            this.ableTurnPhase = when;
        }
        /// <summary>
        /// is free
        /// </summary>
        /// <returns></returns>
        public static CardPlayWay LandDrop()
        {
            return new CardPlayWay(
                "Land Drop", 
                Place.hand, 
                Place.battleground, 
                ManaCost.None(),
                new List<TurnPhase>() { TurnPhase.mainFirst, TurnPhase.mainSecond});
        }
    }

    enum PlayerActionType
    {
        None = -1,
        PlayCard = 0
    }
}
