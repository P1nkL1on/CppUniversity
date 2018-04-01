using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GWENT
{
    public enum Event
    {
        deploy = 0,
        death = 1,
        turnEnd = 2,
        turnStart = 3
    }

    public class UnitDeployPlace : Unit
    {
        public UnitDeployPlace()
        {

        }
        public override void TraceField(int bufHorizontal, int bufVertical)
        {
            DRAW.PushColor(ConsoleColor.DarkGreen);
            DRAW.setBuffTo(bufHorizontal, bufVertical);
            DRAW.str("".PadRight(7)); 
            DRAW.setBuffTo(bufHorizontal, bufVertical + 1);
            DRAW.str("".PadRight(7));
            DRAW.PopColor();
        }
    }
    public class Unit : Card
    {
        protected int basePower;
        protected int currentHealth;
        int buffBonus;
        protected int armorCount;

        int lastHo;
        int lastVe;

        protected Action onDeploy;
        protected Action onEnterGraveyard;
        protected Action onTurnEnd;
        protected Action onTurnStart;


        public void TriggerEvent(Event even, Game game)
        {
            switch (even)
            {
                case Event.deploy:
                    onDeploy(this, game);
                    break;
                case Event.death:
                    onEnterGraveyard(this, game);
                    break;
                case Event.turnEnd:
                    onTurnEnd(this, game);
                    break;
                case Event.turnStart:
                    onTurnStart(this, game);
                    break;
                default:
                    break;
            }
            game.RedrawTop();
            game.DrawCounts(Game.CountPlace.ALL);
        }

        public Point leftTop
        {
            get { return new Point(lastHo, lastVe); }
        }

        void SetStandartActions(int pow)
        {
            onDeploy = new Action((card, game) => { LOGS.deployMessage(this.name); });
            onEnterGraveyard = new Action((card, game) => { });
            onTurnStart = new Action((card, game) => { });
            onTurnEnd = new Action((card, game) => { });
            basePower = currentHealth = pow;
            buffBonus = 0;
        }

        public Unit() { }
        public Unit(int power, string nam, string descr, Rarity rar, List<Tag> unitags)
        {
            exapler = Cards.None;
            SetParams(nam, descr, unitags, rar);
            SetStandartActions(power);
        }

        public Unit(Cards name)
        {
            exapler = name;
            switch (name)
            {
                case Cards.AedirianMauler:
                    SetParams("Aedirian Mauler", "Deal 4 damage to enemy.", new List<Tag>() { Tag.NothernRealms, Tag.Solder }, Rarity.bronze);
                    SetStandartActions(7);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Card> enemies = Game.selectFrom("Select enemy to deal 4 damage", 1, true, game.EnemyField(this).getUnitsAsCards);
                        Unit enemy = (enemies.Count > 0) ? enemies[0] as Unit : null;
                        if (enemy != null)
                        {
                            game.pingBoard(this, enemy, 800, 10, ConsoleColor.Red);
                            enemy.Damage(4);
                        }
                    });
                    break;
                case Cards.TemerianDrummer:
                    SetParams("Temerian Drummer", "Boost an ally by 6.", new List<Tag>() { Tag.NothernRealms, Tag.Temeria, Tag.Support }, Rarity.bronze);
                    SetStandartActions(5);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Card> selectFrom = game.FriendField(this).getUnitsAsCards;
                        selectFrom.Remove(this);
                        List<Card> allys = Game.selectFrom("Select ally to boost by 6",1, true, selectFrom);

                        Unit targ = (allys.Count > 0) ? allys[0] as Unit: null;
                        if (targ != null)
                        {
                            game.pingBoard(this, targ, 800, 10, ConsoleColor.Green);
                            targ.Boost(6);
                        }
                    });
                    break;
                case Cards.RedanianKnight:
                    SetParams("Redanian Knight", "If this unit has no Armor, boost it by 2 and give it 2 Armor on turn end.", new List<Tag>(){Tag.NothernRealms, Tag.Solder, Tag.Redanie}, Rarity.bronze);
                    SetStandartActions(7);
                    onTurnEnd = new Action((card, game) =>
                    {
                        if (armorCount <= 0) { armorCount = 2; Boost(2); }
                    });
                    break;
                case Cards.RedanianKnightElect:
                    SetParams("Redanian Knight-Elect", "If this unit has Armor on turn end, boost adjacent units by 1. 2 Armor.", new List<Tag>(){Tag.NothernRealms, Tag.Solder, Tag.Redanie}, Rarity.bronze);
                    SetStandartActions(7);
                    armorCount = 2;
                    onTurnEnd = new Action((card, game) =>
                    {
                        if (armorCount > 0) {
                            Field f = game.FriendField(this);
                            Unit left = f.NearUnit(this, true),
                                 right = f.NearUnit(this, false);
                            if (left != null) { game.pingBoard(this, left, 400, 5, ConsoleColor.Green); left.Boost(1); }
                            if (right != null) { game.pingBoard(this, right, 400, 5, ConsoleColor.Green); right.Boost(1); }
                        }
                    });
                    break;
                case Cards.TridamInfantry:
                    SetParams("Tridam Infantry", "4 Armor.", new List<Tag>() { Tag.NothernRealms, Tag.Solder }, Rarity.bronze);
                    SetStandartActions(10);
                    armorCount = 4;
                    break;
                default:
                    break;
            }
        }

        public override int Power
        {
            get
            { return currentHealth + buffBonus; }
        }

        public ConsoleColor PowerColor
        {
            get
            {
                if (Power < basePower)
                    return ConsoleColor.Red;
                if (Power > basePower)
                    return ConsoleColor.Green;
                return ConsoleColor.Gray;
            }
        }

        public override void TraceInList(int bufHorizontal, int bufVertical)
        {
            lastDrawType = 1;
            lastVe = bufVertical; lastHo = bufHorizontal;
            DRAW.setBuffTo(bufHorizontal, bufVertical);
            DRAW.rarity(2, rarity);
            DRAW.power(Power, 3, PowerColor);
            DRAW.str(" " + name, 10, false);
        }
        public override void TraceField(int bufHorizontal, int bufVertical)
        {
            lastDrawType = 0;
            lastVe = bufVertical; lastHo = bufHorizontal;
            DRAW.setBuffTo(bufHorizontal, bufVertical);
            DRAW.str(name, 6, true);
            DRAW.setBuffTo(bufHorizontal, bufVertical + 1);
            DRAW.rarity(1, rarity);
            DRAW.power(Power, 2, PowerColor);
            int dopCells =
                DRAW.armor(armorCount)
                + DRAW.timer(innerTimer, timerStep);
            DRAW.str("".PadLeft(3 - dopCells));
        }
        int lastDrawType;

        public override void Redraw()
        {
            if (lastDrawType == 0)
                TraceField(lastHo, lastVe);
            else
                TraceInList(lastHo, lastVe);
        }
        public override void RedrawSelected(bool selected)
        {
            DRAW.PushColor((selected) ? ConsoleColor.DarkGreen : ConsoleColor.Black);
            Redraw();
            DRAW.PopColor();
        }
        public override void TraceFull(int bufHorizontal, int bufVertical, int wid)
        {
            DRAW.setBuffTo(bufHorizontal, bufVertical);
            DRAW.str(name);
            int i = DRAW.str(DRAW.tags(tags), bufHorizontal + 1, bufVertical + 1, wid, 1);

            DRAW.setBuffTo(bufHorizontal, bufVertical + 2 + i);
            DRAW.str(" Power:  "); DRAW.power(Power, 2, PowerColor);
            DRAW.setBuffTo(bufHorizontal, bufVertical + 3 + i);
            DRAW.str(" Rarity: "); DRAW.rarity(2, rarity); DRAW.str(" " + rarity.ToString());
            i += DRAW.str((description.Length == 0) ? "no ability" : description, bufHorizontal, bufVertical + 4 + i, wid + 1, 5);
            Game.DrawPicture(DRAW.rarityColor(rarity), exapler);
        }

        public bool Damage(int dmg)
        {
            if (armorCount > 0 && dmg >= armorCount)
                Console.Beep(400, 200);
            Console.Beep(1200, 200);
            DRAW.message(lastHo, lastVe, ("-" + dmg).PadLeft(3, ' '), ConsoleColor.Black, ConsoleColor.Red, 800);

            if (armorCount >= dmg) { armorCount -= dmg; }
            if (armorCount > 0) { dmg -= armorCount; armorCount = 0; }
            if (buffBonus > 0) { dmg -= buffBonus; buffBonus = 0; }
            currentHealth -= dmg;

            Redraw();
            return (currentHealth <= 0);
        }
        public void Boost(int buff)
        {
            Console.Beep(1400, 100);
            DRAW.message(lastHo, lastVe, ("+" + buff).PadLeft(3, ' '), ConsoleColor.Black, ConsoleColor.Green, 800);
            buffBonus += buff;
            Redraw();
        }
        public void Strengthlen(int str)
        {
            Console.Beep(400, 100);
            DRAW.message(lastHo, lastVe, ("+" + str).PadLeft(3, ' '), ConsoleColor.Black, ConsoleColor.White, 800);
            basePower += str;
            currentHealth += str;
        }

        public bool isBlue(Game game)
        {
            int rowIndex;
            if (game.left.IndexOf(this, out rowIndex) >= 0)
                return game.left.isBlue;
            if (game.right.IndexOf(this, out rowIndex) >= 0)
                return game.right.isBlue;
            throw new Exception("No such unit in a field!");
        }
    }
}
