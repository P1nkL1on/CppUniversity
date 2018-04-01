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
    public class Game
    {
        public Random rnd;
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

            List<Cards> types = new List<Cards>() { Cards.TridamInfantry, Cards.RedanianKnight, Cards.RedanianKnightElect, Cards.TemerianDrummer, Cards.AedirianMauler };
            for (int i = 0; i < 25; i++)
            {
                rightDeck.Add(new Unit(types[i % 5]));
                leftDeck.Add(new Unit(types[i % 5]));
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
                DRAW.str("Hand :  " + (leftHand.Count+"").PadLeft(2, ' '));
            }
            if (plc == CountPlace.leftDeck || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widLeft, fieldTop + hei + 1);
                DRAW.str("Deck :  " + (leftDeck.Count+"").PadLeft(2, ' '));
            }
            if (plc == CountPlace.leftGrave || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widLeft, fieldTop + hei + 2);
                DRAW.str("Grave : " + (leftGraveyard.Count+"").PadLeft(2, ' '));
            }

            //Console.ForegroundColor = ConsoleColor.DarkRed;
            if (plc == CountPlace.rightHand || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei);
                DRAW.str("Hand :  " + (rightHand.Count+"").PadLeft(2, ' '));
            }
            if (plc == CountPlace.rightDeck || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei + 1);
                DRAW.str("Deck :  " + (rightDeck.Count+"").PadLeft(2, ' '));
            }
            if (plc == CountPlace.rightHand || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei + 2);
                DRAW.str("Grave : " + (rightGraveyard.Count+"").PadLeft(2, ' '));
            }
            //if (plc != CountPlace.ALL)
            //    Console.Beep(600, 100);
            Console.ResetColor();
        }
        public void DrawACard(bool left)
        {
            int time = 00;
            if (left)
            {
                if (leftDeck.Count == 0) return;

                Card a = leftDeck[0];
                leftDeck.RemoveAt(0);
                DrawCounts(CountPlace.leftDeck);
                pingBoard(new Point(31, 35), new Point(30, 34), ConsoleColor.Gray, 10, time, true, true, false);
                leftHand.Add(a);
                DrawCounts(CountPlace.leftHand);
                view.Redraw();
            }
            else
            {
                if (rightDeck.Count == 0) return;
                Card a = rightDeck[0];
                rightDeck.RemoveAt(0);
                DrawCounts(CountPlace.rightDeck);
                pingBoard(new Point(52, 35), new Point(52, 34), ConsoleColor.Gray, 10, time, true, false, false);
                rightHand.Add(a);
                DrawCounts(CountPlace.rightHand);
                view.Redraw();
            }
            Thread.Sleep(time / 5);
        }
        void pingBoard(Point fromP, Point toP, ConsoleColor clr, int tail, int time, bool sameSide, bool toIsLeft, bool finishGap)
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
        public void pingBoard(Unit from, Unit to, int time, int tail, ConsoleColor clr)
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
                DrawACard(i % 2 == 0);
        }

        public Field FriendField(Unit forThis)
        {
            int rowIndex;
            if (left.IndexOf(forThis, out rowIndex) >= 0) return left;
            if (right.IndexOf(forThis, out rowIndex) >= 0) return right;
            return null;
        }
        public Field EnemyField(Unit forThis)
        {
            int rowIndex;
            if (left.IndexOf(forThis, out rowIndex) >= 0) return right;
            if (right.IndexOf(forThis, out rowIndex) >= 0) return left;
            return null;
        }

        public static List<Card> selectFrom(int count, bool exactly, List<Card> from)
        {
            return selectFrom("Select " + count + " unit(s)", count, exactly, from);
        }

        public static List<Card> selectFrom(string what, int count, bool exactly, List<Card> from)
        {
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

        public void PlayCard(bool isLeft)
        {
            if (leftHand.Count == 0) return;
            Card choosed = selectFrom("Choose a card to play", 1, true, leftHand)[0];
            leftHand.Remove(choosed);
            view.Redraw();
            if ((choosed as Unit) != null)
            {
                left.SelectAndDeployUnit(choosed as Unit, this);
            }
            else
            {
                // spell cast
            }
        }

        void NextTurn()
        {
            foreach (Unit u in left.getUnits)
            {
                u.TriggerEvent(Event.turnEnd, this);
            }

            foreach (Unit u in left.getUnits)
            {
                u.TriggerEvent(Event.turnStart, this);
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
    }
}
