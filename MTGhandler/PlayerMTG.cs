using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    interface IHitable{
        int CurrentHealth {get;}
        void DealDamage(int X);
        void AddHealth(int X);
        void LooseHealth(int X);
    }
    class Player
    {
        public Player(String name)
        {
            PlayerName = name;
            hand = new Hand(this, 8);
            graveyard = new Graveyard(this);
            exile = new Exile(this);
            deck = new Deck(this);
            field = new Battlefield(this);
        }

        public List<Player> Opponents = new List<Player>();
        public String PlayerName;
        public Hand hand;// = new Hand(this, 8);
        public Graveyard graveyard;// = new Graveyard(this);
        public Exile exile;// = new Exile(this);
        public Deck deck;// = new Deck(this);
        public Battlefield field;
    }

    class PlayerHandler
    {
        public static void DrawPlayer (MPoint where, Player who, int Width){
            int maxWidth = Width / 2;
            int ostWidth = Width - maxWidth;
            MDrawHandler.DrawStringInPoint(where, MDrawHandlerMTG.DefaultColor, who.PlayerName, maxWidth);
            int offset = 1;
            who.hand.RedrawFull(where.AddY(offset), maxWidth); offset += who.hand.Count + 1;
            who.deck.RedrawFull(where.AddY(offset), maxWidth); offset += who.deck.Count + 1;
            who.graveyard.RedrawFull(where.AddY(offset), maxWidth); offset += who.graveyard.Count + 1;
            who.exile.RedrawFull(where.AddY(offset), maxWidth); offset += who.exile.Count + 1;
            who.field.RedrawFull(where.Add(maxWidth, 1), ostWidth);
        }
    }
}
