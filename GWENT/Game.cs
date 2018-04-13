using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace GWENT
{
    enum Focus
    {
        none = -1,
        cardViewer = 0,
        field = 1
    }
    public delegate bool filter (Card card);
    public class Game
    {
        public static filter AnyCard = new filter((card)=>{return true;});
        public Random rnd;
        static Random rndStat = new Random();
        public Field left, right;
        int fieldLeft, fieldTop;
        //also lists
        List<Card> leftHand, leftDeck, leftGraveyard,
                   rightHand, rightDeck, rightGraveyard;
        CardListViewer view;
        //
        public Game(Random rnd)
        {
            fieldLeft = 20;
            fieldTop = 2;

            leftDeck = new List<Card>();
            leftHand = new List<Card>();
            leftGraveyard = new List<Card>();

            rightDeck = new List<Card>();
            rightHand = new List<Card>();
            rightGraveyard = new List<Card>();

            view = new CardListViewer(fieldTop, 2, 10);
            view.SetList("Deck", leftHand);

            List<Cards> types = new List<Cards>() { Cards.SiegeSupport , Cards.ReinforcedBallista};//, Cards.PoorInfantry,Cards.ReinforcedTrebuchet};//, Cards.ReaverScout, Cards.TridamInfantry, Cards.RedanianKnight, Cards.RedanianKnightElect, Cards.TemerianDrummer, Cards.AedirianMauler, Cards.DandelionPoet};
            List<Cards> specialTypes = new List<Cards>() { Cards.InpenetrableFog, Cards.TorrentioalRain, Cards.BitingFrost};
            for (int i = 0; i < 25; i++)
            {
                if (i < 8)leftDeck.Add(new Special(specialTypes[i % specialTypes.Count]));
                rightDeck.Add(new Unit(types[i % types.Count]));
                leftDeck.Add(new Unit(types[i % types.Count]));
            }

            this.rnd = rnd;
            left = new Field(fieldLeft, fieldTop, true); right = new Field(fieldLeft + 24, fieldTop, false);

            left.DrawStart();
            right.DrawStart();
            view.Redraw();
            DRAW.setBuffTo(fieldLeft + 21, fieldTop);
            DRAW.str("VS");
            DrawCounts(CountPlace.ALL);

            DrawBorders(Focus.none);
            TraceCardInfo(leftDeck[0]);

            //left.RandomUnitOnRow(rnd, -1).TriggerEvent(Event.deploy, this);
            //pingBoard(left.RandomUnitOnRow(rnd, -1), right.RandomUnitOnRow(rnd, -1), 1000, 20, ConsoleColor.Red);
            //foreach (Unit u in right.getUnits)
            //{
            //    u.TriggerEvent(Event.deploy, this);
            //}
            //foreach (Unit u in left.getUnits){
            //    u.TriggerEvent(Event.turnEnd, this);
            //}
        }
        void DrawBorders(Focus foc)
        {
            DRAW.border(1, 1, 18, 24, foc == Focus.cardViewer, ConsoleColor.Gray);
            DRAW.border(19, 1, 46, 35, foc == Focus.field, ConsoleColor.Gray);
            //DRAW.border(1, 26, 18, 18, true, ConsoleColor.DarkGray);
            DrawPicture(ConsoleColor.DarkGray, Cards.None);
            DrawCardBorder();
        }
        static void DrawCardBorder()
        {
            DRAW.border(19, 37, 46, 7, false, ConsoleColor.Gray);
        }
        public static void DrawPicture(ConsoleColor clr, Cards pictureType)
        {
            DRAW.border(1, 26, 18, 18, true, clr);
            DRAW.picture(pictureType);
        }
        public static void TraceCardInfo(Card card)
        {
            DrawCardBorder();
            for (int i = 37; i < 43; i++) { DRAW.setBuffTo(20, i + 1); DRAW.str("".PadLeft(44)); }
            card.TraceFull(20, 37, 43);
        }
        public static Unit HighestUnit(List<Unit> from)
        {
            int maxPower = 0;
            foreach (Unit u in from)
                if (u.Power > maxPower) maxPower = u.Power;
            List<Unit> highs = new List<Unit>();
            foreach (Unit u in from)
                if (u.Power == maxPower) highs.Add(u);
            if (highs.Count == 0)
                return null;
            return highs[rndStat.Next(highs.Count)];
        }
        public static Unit LowestUnit(List<Unit> from)
        {
            int maxPower = int.MaxValue;
            foreach (Unit u in from)
                if (u.Power < maxPower) maxPower = u.Power;
            List<Unit> highs = new List<Unit>();
            foreach (Unit u in from)
                if (u.Power == maxPower) highs.Add(u);
            if (highs.Count == 0)
                return null;
            return highs[rndStat.Next(highs.Count)];
        }
        public enum CountPlace
        {
            leftHand = 0,
            leftDeck = 1,
            leftGrave = 2,
            rightHand = 3,
            rightDeck = 4,
            rightGrave = 5,
            ALL = 6
        }
        public void DrawCounts(CountPlace plc)
        {
            int hei = 31, widLeft = 0, widRight = 33;
            //Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (plc == CountPlace.leftHand || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widLeft, fieldTop + hei);
                DRAW.str("Hand :  " + (leftHand.Count + "").PadLeft(2, ' '));
            }
            if (plc == CountPlace.leftDeck || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widLeft, fieldTop + hei + 1);
                DRAW.str("Deck :  " + (leftDeck.Count + "").PadLeft(2, ' '));
            }
            if (plc == CountPlace.leftGrave || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widLeft, fieldTop + hei + 2);
                DRAW.str("Grave : " + (leftGraveyard.Count + "").PadLeft(2, ' '));
            }

            //Console.ForegroundColor = ConsoleColor.DarkRed;
            if (plc == CountPlace.rightHand || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei);
                DRAW.str("Hand :  " + (rightHand.Count + "").PadLeft(2, ' '));
            }
            if (plc == CountPlace.rightDeck || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei + 1);
                DRAW.str("Deck :  " + (rightDeck.Count + "").PadLeft(2, ' '));
            }
            if (plc == CountPlace.rightHand || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei + 2);
                DRAW.str("Grave : " + (rightGraveyard.Count + "").PadLeft(2, ' '));
            }
            //if (plc != CountPlace.ALL)
            //    Console.Beep(600, 100);
            Console.ResetColor();
        }
        public Card DrawACard(bool left, filter f)
        {
            int time = 0;//400;
            
            if (left)
            {
                if (leftDeck.Count == 0) return null;
                int index = -1;
                
                    for (int i = 0; i < leftDeck.Count; i++)
                        if (index == -1 && f(leftDeck[i]))
                            index = i;
                if (index < 0)return null;
                Card a = leftDeck[index];
                leftDeck.RemoveAt(index);

                DrawCounts(CountPlace.leftDeck);
                pingBoard(new Point(31, 35), new Point(30, 34), ConsoleColor.Gray, 10, time, true, true, false);
                leftHand.Add(a);
                DrawCounts(CountPlace.leftHand);
                LOGS.Add("Left player draws '"+ a.name +"'");
                view.Redraw();
                return a;
            }
            else
            {
                if (rightDeck.Count == 0) return null;
                int index = -1;
                    for (int i = 0; i < rightDeck.Count; i++)
                        if (index == -1 && f(rightDeck[i]))
                            index = i;
                if (index < 0)return null;
                Card a = rightDeck[index];
                rightDeck.RemoveAt(index);

                DrawCounts(CountPlace.rightDeck);
                pingBoard(new Point(52, 35), new Point(52, 34), ConsoleColor.Gray, 10, time, true, false, false);
                rightHand.Add(a);
                DrawCounts(CountPlace.rightHand);
                LOGS.Add("Right player draws '" + a.name + "'");
                view.Redraw();
                return a;
            }
            //Thread.Sleep(time / 5);
        }
        

        public void pingBoard(Point fromP, Point toP, ConsoleColor clr, int tail, int time, bool sameSide, bool toIsLeft, bool finishGap)
        {
            if (sameSide) { if (toIsLeft) toP.Offset(100, 0); else toP.Offset(-100, 0); }
            Point curr = new Point(fromP.X, fromP.Y);
            List<Point> pts = new List<Point>();
            while (curr != toP)
            {
                pts.Add(new Point(curr.X, curr.Y - 1));

                if (curr.Y == toP.Y || curr.X < fieldLeft + 20 || curr.X > fieldLeft + 22)
                {
                    if (toP.X > curr.X) curr.Offset(1, 0);
                    else
                        if (toP.X < curr.X) curr.Offset(-1, 0);
                }
                else
                {
                    if (sameSide) { sameSide = false; if (toIsLeft) toP.Offset(-100, 0); else toP.Offset(100, 0); }
                    if (toP.Y > curr.Y) curr.Offset(0, 1);
                    else
                        if (toP.Y < curr.Y) curr.Offset(0, -1);
                }
            }
            if (finishGap)
                for (int i = 0; i < 5; i++)
                    pts.Add(new Point(curr.X - 1, curr.Y));
            DRAW.pingField(clr, pts, time, tail);
        }
        public void pingBoard(Card from, Card to, int time, int tail, ConsoleColor clr)
        {
            bool fromIsLeft = from.isBlue(this),
                 toIsLeft = to.isBlue(this);
            if (!left.isBlue) { fromIsLeft = !fromIsLeft; toIsLeft = !toIsLeft; }
            //
            Point fromP = from.leftTop, toP = to.leftTop;
            fromP.Offset(3, 0);
            toP.Offset(3, 0);
            //fromP.Offset(0, -1);

            bool sameSide = ((fromIsLeft && toIsLeft) || (!fromIsLeft && !toIsLeft)) && (fromP.Y != toP.Y);

            pingBoard(fromP, toP, clr, tail, time, sameSide, toIsLeft, true);
        }
        static void randomPing()
        {
            Random rnd = new Random();
            while (true)
            {
                List<Point> pts = new List<Point>();
                pts.Add(new Point(20, 20));
                for (int i = 0; i < 100; i++)
                {
                    int dir = rnd.Next(4);
                    int xoff = 0, yoff = 0;
                    if (dir == 0) { xoff = 1; }
                    if (dir == 1) { xoff = -1; }
                    if (dir == 2) { yoff = 1; }
                    if (dir == 3) { yoff = -1; }

                    pts.Add(new Point(pts[pts.Count - 1].X + xoff, pts[pts.Count - 1].Y + yoff));

                }
                DRAW.pingField(ConsoleColor.Blue, pts, 3000, 20);
                Console.Read();
            }//
        }

        public void RedrawTop()
        {
            left.RedrawTop();
            right.RedrawTop();
        }
        void RoundStart(int roundIndex)
        {
            if (roundIndex > 3)
                throw new Exception("game has only 3 rounds!");
            int cardCount = 0;
            if (roundIndex == 1) cardCount = 20;
            if (roundIndex == 2) cardCount = 4;
            if (roundIndex == 3) cardCount = 2;
            for (int i = 0; i < cardCount; i++)
                DrawACard(i % 2 == 0, AnyCard);
        }

        public enum From{
            friendlyDeck = 0,
            friendlyGraveyard = 1,
            
            enemyDeck = 3,
            enemyGraveyard = 4
        }

        public int sideOFCard(Card c)
        {
            if (leftHand.IndexOf(c) >= 0
                || leftDeck.IndexOf(c) >= 0
                || leftGraveyard.IndexOf(c) >= 0)
                return 0;
            if (rightHand.IndexOf(c) >= 0
                || rightDeck.IndexOf(c) >= 0
                || rightGraveyard.IndexOf(c) >= 0)
                return 1;
            return -1;
        }

        public List<Card> selectA (From f, Unit forThis){
            int rowIndex;
            if (left.IndexOf(forThis, out rowIndex) >= 0){
                    switch (f){
                        case From.friendlyDeck:
                            return leftDeck;
                        case From.friendlyGraveyard:
                            return leftGraveyard;
                        case From.enemyDeck:
                            return rightDeck;
                        case From.enemyGraveyard:
                            return rightGraveyard;
                        default:
                            break;
                    }
            }else{
                    switch (f){
                        case From.friendlyDeck:
                       return rightDeck;
                        case From.friendlyGraveyard:
                            return rightGraveyard;
                        case From.enemyDeck:
                            return leftDeck;
                        case From.enemyGraveyard:
                            return leftGraveyard;
                        default:
                            break;
                    }
            }
            return new List<Card>();
        }

        public Field FriendField(Unit forThis)
        {
            int rowIndex;
            if (left.IndexOf(forThis, out rowIndex) >= 0) return left;
            if (right.IndexOf(forThis, out rowIndex) >= 0) return right;
            return null;
        }
        public Field FriendField(bool isBlue)
        {
            return (isBlue)?left:right;
        }
        public Field EnemyField(Unit forThis)
        {
            int rowIndex;
            if (left.IndexOf(forThis, out rowIndex) >= 0) return right;
            if (right.IndexOf(forThis, out rowIndex) >= 0) return left;
            return null;
        }
        public Field EnemyField(bool isBlue)
        {
            return (!isBlue) ? left : right;
        }

        public static List<Card> selectFrom(int count, bool exactly, List<Card> from)
        {
            return selectFrom("Select " + count + " unit(s)", count, exactly, from);
        }

        public static List<Card> selectFrom(string what, int count, bool exactly, List<Card> from)
        {
            LOGS.Add(what);
            DRAW.PushColor(ConsoleColor.DarkGreen);
            DRAW.setBuffTo(39 - what.Length / 2, 32);
            DRAW.str(" " + what + " : ");
            DRAW.PopColor();

            List<Card> res = new List<Card>();
            for (int i = 0; i < Math.Min(count, from.Count); i++)
            {
                int currentSelected = 0, prevSelected = 0;
                ConsoleKey k = ConsoleKey.Enter;
                do
                {
                    from[prevSelected].RedrawSelected(false);
                    from[currentSelected].RedrawSelected(true);
                    k = Console.ReadKey().Key;
                    prevSelected = currentSelected;
                    if (k == ConsoleKey.DownArrow)
                        currentSelected++;
                    if (k == ConsoleKey.UpArrow)
                        currentSelected--;

                    currentSelected += from.Count;
                    currentSelected %= from.Count;

                    TraceCardInfo(from[currentSelected]);
                } while (k != ConsoleKey.Enter);

                from[currentSelected].RedrawSelected(false);
                res.Add(from[currentSelected]);
                from.Remove(from[currentSelected]);
            }
            DRAW.setBuffTo(20, 32);
            DRAW.str("".PadLeft(40));
            return res;
        }

        public void CheckDeadUnits()
        {
            List<Unit> units = left.getUnits;
            units.AddRange(right.getUnits);
            foreach (Unit u in units)
                if (u.Power <= 0 && !u.dead) u.Die(this);

        }

        public void PlayCard(bool isLeft)
        {
            if (leftHand.Count == 0) return;
            Card choosed = selectFrom("Choose a card to play", 1, true, leftHand)[0];
            leftHand.Remove(choosed);
            view.Redraw();
            PlayCard(isLeft, choosed);
        }
        public void PlayCard (bool isLeft, Card choosed){
            if ((choosed as Unit) != null)
            {
                if (isLeft)
                    left.SelectAndDeployUnit(choosed as Unit, this);
                else
                    right.SelectAndDeployUnit(choosed as Unit, this);
            }
            else
            {
                if (isLeft)
                    (choosed as Special).TriggerEvent(Event.deploy, this, true);
            }        
        }

        void NextTurn()
        {
            foreach (Unit u in left.getUnits)
            {
                u.TriggerEvent(Event.turnEnd, this, u);
            }
            right.onTurnStart(this);
            // right turn
            // ...
            left.onTurnStart(this);
            foreach (Unit u in left.getUnits)
            {
                u.TriggerEvent(Event.turnStart, this, u);
            }
        }
        public void AddToGraveyard(bool isLeft, Card what)
        {
            if (isLeft)
            {
                leftGraveyard.Add(what);
                DrawCounts(CountPlace.leftGrave);
            }
            else
            {
                rightGraveyard.Add(what);
                DrawCounts(CountPlace.rightGrave);
            }
        }

        public void Start()
        {
            this.RoundStart(1);
            while (true)
            {
                this.PlayCard(true);
                NextTurn();
            }
        }

        public List<Card> selectByFilter (List<Card> from, filter f){
            List<Card> res = new List<Card>();
            foreach (Card c in from)
                if (f(c))
                    res.Add(c);
            return res;
        }

        public List<Card> allUnitsOnField()
        {
            List<Card> l = left.getUnitsAsCards;
            l.AddRange(right.getUnitsAsCards);
            return l;
        }
        
    }
}
