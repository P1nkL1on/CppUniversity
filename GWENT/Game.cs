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
            for (int i = 0; i < 25; i++)
            {
                rightDeck.Add(new Unit(rnd.Next(5, 15), "r_" + rnd.Next(0, 300) + "dasidpaosidpaosidpa", "", Rarity.bronze, new List<Tag>() { Tag.Monsters }));
                leftDeck.Add(new Unit(rnd.Next(5, 15), "l_" + rnd.Next(0, 300) + "aaaaaaa", "", Rarity.silver, new List<Tag>() { Tag.NothernRealms }));
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

            left.RandomUnitOnRow(rnd, -1).TriggerEvent(Event.deploy, this);
            //pingBoard(left.RandomUnitOnRow(rnd, -1), right.RandomUnitOnRow(rnd, -1), 1000, 20, ConsoleColor.Red);
        }
        void DrawBorders(Focus foc)
        {
            DRAW.border(1, 1, 18, 24, foc == Focus.cardViewer, ConsoleColor.Gray);
            DRAW.border(19, 1, 46, 35, foc == Focus.field, ConsoleColor.Gray);
            DRAW.border(1, 26, 18, 18, true, ConsoleColor.Gray);
            DRAW.border(19, 37, 46, 7, false, ConsoleColor.Gray);
        }
        public void TraceCardInfo(Card card)
        {
            card.TraceFull(20, 37, 44);
        }
        enum CountPlace
        {
            leftHand = 0,
            leftDeck = 1,
            leftGrave = 2,
            rightHand = 3,
            rightDeck = 4,
            rightGrave = 5,
            ALL = 6
        }
        void DrawCounts(CountPlace plc)
        {
            int hei = 31, widLeft = 9, widRight = 24;
            //Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (plc == CountPlace.leftHand || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widLeft, fieldTop + hei);
                DRAW.str("Hand :  " + leftHand.Count);
            }
            if (plc == CountPlace.leftDeck || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widLeft, fieldTop + hei + 1);
                DRAW.str("Deck :  " + leftDeck.Count);
            }
            if (plc == CountPlace.leftGrave || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widLeft, fieldTop + hei + 2);
                DRAW.str("Grave : " + leftGraveyard.Count);
            }

            //Console.ForegroundColor = ConsoleColor.DarkRed;
            if (plc == CountPlace.rightHand || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei);
                DRAW.str("Hand :  " + rightHand.Count);
            }
            if (plc == CountPlace.rightDeck || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei + 1);
                DRAW.str("Deck :  " + rightDeck.Count);
            }
            if (plc == CountPlace.rightHand || plc == CountPlace.ALL)
            {
                DRAW.setBuffTo(fieldLeft + widRight, fieldTop + hei + 2);
                DRAW.str("Grave : " + rightGraveyard.Count);
            }
            //if (plc != CountPlace.ALL)
            //    Console.Beep(600, 100);
            Console.ResetColor();
        }
        public void DrawACard(bool left)
        {
            if (left)
            {
                if (leftDeck.Count == 0) return;
                leftHand.Add(leftDeck[0]);
                leftDeck.RemoveAt(0);
                DrawCounts(CountPlace.leftDeck);
                DrawCounts(CountPlace.leftHand);
                view.Redraw();
            }
            else
            {
                if (rightDeck.Count == 0) return;
                rightHand.Add(rightDeck[0]);
                rightDeck.RemoveAt(0);
                DrawCounts(CountPlace.rightDeck);
                DrawCounts(CountPlace.rightHand);
                view.Redraw();
            }
            Thread.Sleep(420);
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
            if (sameSide) { if (toIsLeft) toP.Offset(100, 0); else toP.Offset(-100, 0); }

            Point curr = new Point(fromP.X, fromP.Y);
            List<Point> pts = new List<Point>();
            while (curr != toP)
            {
                pts.Add(new Point(curr.X, curr.Y - 1));

                if (curr.Y == toP.Y || curr.X < fieldLeft + 20 || curr.X > fieldLeft + 24)
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
            for (int i = 0; i < 5; i++)
                pts.Add(new Point(curr.X - 1, curr.Y));
            DRAW.pingField(clr, pts, time, tail);
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
        public void RoundStart(int roundIndex)
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

        public static List<Unit> selectFrom(int count, bool exactly, List<Unit> from)
        {
            List<Unit> res = new List<Unit>();
            for (int i = 0; i < Math.Max(count, from.Count); i++)
                res.Add(from[i]);
            return res;
        }
    }
}
