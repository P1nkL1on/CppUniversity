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
        turnStart = 3,
        unitSpawned = 4,
        unitDied = 5,
        move = 6,

    }
    public enum Ability
    {
        none = 0,
        crew = 1,
        spy = 2,
        resilence = 3,
        locked = 4,
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
            DRAW.str("".PadRight(6));
            DRAW.setBuffTo(bufHorizontal, bufVertical + 1);
            DRAW.str("".PadRight(6));
            DRAW.PopColor();
        }
        public static void ClearField(int bufHorizontal, int bufVertical)
        {
            DRAW.setBuffTo(bufHorizontal, bufVertical);
            DRAW.str("".PadRight(6));
            DRAW.setBuffTo(bufHorizontal, bufVertical + 1);
            DRAW.str("".PadRight(6));
        }
    }
    public class Unit : Card
    {
        public bool dead;
        protected int basePower;
        protected int currentHealth;
        int buffBonus;
        protected int armorCount;

        int lastHo;
        int lastVe;
        public List<Ability> abilities;
        protected
        Action onDeploy, onEnterGraveyard, onTurnEnd, onTurnStart,
               onUnitSpawned, onUnitDie, onThisMove, onUnitMove;

        public void ChangeStatus(Ability a)
        {
            if (abilities.IndexOf(a) < 0)
                abilities.Add(a);
            else
                abilities.Remove(a);
            LOGS.Add(name + "'s " + a.ToString() + " status is now " + (abilities.IndexOf(a) >= 0));
        }
        public void TriggerEvent(Event even, Game game, Unit sender)
        {
            switch (even)
            {
                case Event.deploy:
                    this.SpawnEffect(game);
                    Redraw();
                    onDeploy(sender, game);
                    foreach (Card c in game.AllUnitsOnField())
                    {
                        Unit u = c as Unit;
                        if (u != this)
                            u.TriggerEvent(Event.unitSpawned, game, this);
                    }
                    break;
                case Event.unitSpawned:
                    onUnitSpawned(sender, game);
                    break;
                case Event.unitDied:
                    onUnitDie(sender, game);
                    break;
                case Event.death:
                    onEnterGraveyard(sender, game);
                    foreach (Card c in game.AllUnitsOnField())
                    {
                        Unit u = c as Unit;
                        if (u != this)
                            u.TriggerEvent(Event.unitDied, game, this);
                    }
                    break;
                case Event.turnEnd:
                    onTurnEnd(sender, game);
                    break;
                case Event.turnStart:
                    onTurnStart(sender, game);
                    break;
                default:
                    break;
            }
            game.CheckDeadUnits();
            game.RedrawTop();
            game.DrawCounts(Game.CountPlace.ALL);

        }

        public override Point leftTop
        {
            get { return new Point(lastHo, lastVe); }
        }

        void SetStandartActions(int pow)
        {
            abilities = new List<Ability>();
            dead = false;
            onDeploy = new Action((card, game) => { });
            onEnterGraveyard = new Action((card, game) => { });
            onTurnStart = new Action((card, game) => { });
            onTurnEnd = new Action((card, game) => { });
            onUnitDie = new Action((card, game) => { });
            onUnitSpawned = new Action((card, game) => { });
            onThisMove = new Action((card, game) => { });
            onUnitMove = new Action((card, game) => { });
            basePower = currentHealth = pow;
            buffBonus = 0;
        }

        public Unit() { dead = false; }
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
                case Cards.BearToken:
                    SetParams("Bear", "", new List<Tag>() { Tag.Neutral, Tag.Beast, Tag.Token }, Rarity.bronze);
                    SetStandartActions(11);
                    break;
                case Cards.ReinforcedBallista:
                    SetParams("Reinforced Ballista", "Deal 2 damage to an enemy. Crewed: Repeat its ability.", new List<Tag>() { Tag.NothernRealms, Tag.Machine }, Rarity.bronze);
                    SetStandartActions(7);
                    onDeploy = new Action((card, game) =>
                    {
                        Field f = game.FriendField(this);
                        for (int i = 0; i < 1 + f.isCrewed(this); i++){
                            List<Card> enemies = Game.selectFrom("Select enemy to deal 2 damage", 1, true, game.EnemyField(this).getUnitsAsCards);
                            Unit enemy = (enemies.Count > 0) ? enemies[0] as Unit : null;
                            if (enemy != null)
                            {
                                game.pingBoard(this, enemy, 500, 10, ConsoleColor.Red);
                                enemy.Damage(2, this);
                            }
                        }
                    });
                    break;
                case Cards.SiegeSupport:
                    SetParams("Siege Support", "Whenever an ally appears, boost it by 1. If it's a Machine, also give it 1 Armor. Crew.", new List<Tag>() { Tag.NothernRealms, Tag.Kaedwen, Tag.Support }, Rarity.bronze);
                    SetStandartActions(7);
                    abilities.Add(Ability.crew);
                    onUnitSpawned = new Action((card, game) =>
                    {
                        if (card != null)
                        {
                            game.pingBoard(this, card, 300, 10, ConsoleColor.Green);
                            Unit u = card as Unit;
                            u.Boost(1, this);
                            if (u.tags.IndexOf(Tag.Machine) >= 0)
                                u.BoostArmor(1, this);
                        }
                    });
                    break;
                case Cards.LeftFlankInfantry:
                    SetParams("Left Flank Infantry", "", new List<Tag>() { Tag.NothernRealms, Tag.Solder, Tag.Temeria }, Rarity.bronze);
                    SetStandartActions(2);
                    break;
                case Cards.RightFlankInfantry:
                    SetParams("Right Flank Infantry", "", new List<Tag>() { Tag.NothernRealms, Tag.Solder, Tag.Temeria }, Rarity.bronze);
                    SetStandartActions(2);
                    break;
                case Cards.PoorInfantry:
                    SetParams("Poor F'ing Infantry", "Spawn Left Flank Infantry and Right Flank Infantry to the left and right of this unit, respectively.", new List<Tag>() { Tag.NothernRealms, Tag.Solder, Tag.Temeria }, Rarity.bronze);
                    SetStandartActions(6);
                    onDeploy = new Action((card, game) =>
                    {
                        int rowIndex;
                        Field f = game.FriendField(this);
                        int atInd = f.IndexOf(this, out rowIndex);
                        f.DeployUnitNear(new Unit(Cards.RightFlankInfantry), game, this, 1);
                        f.DeployUnitNear(new Unit(Cards.LeftFlankInfantry), game, this, 0);
                    });
                    break;
                case Cards.Specter:
                    SetParams("Specter", "", new List<Tag>() { Tag.NothernRealms, Tag.Token }, Rarity.bronze);
                    SetStandartActions(5);
                    break;
                case Cards.ReaverScout:
                    SetParams("Reaver Scout", "Choose a different Bronze ally and play a copy of it from your deck.", new List<Tag>() { Tag.NothernRealms, Tag.Support, Tag.Redanie }, Rarity.bronze);
                    SetStandartActions(1);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Card> selectFrom = game.selectByFilter(
                            game.FriendField(this).getUnitsAsCards,
                            new filter((c) => { return (c as Unit != null) && ((c as Unit).rarity == Rarity.bronze); }));
                        selectFrom.Remove(this);
                        List<Card> allys = Game.selectFrom("Select bronze ally to play copy", 1, true, selectFrom);
                        Unit targ = (allys.Count > 0) ? allys[0] as Unit : null;
                        if (targ != null)
                        {
                            bool isLeft = game.FriendField(this).isBlue;
                            Card drawen = game.DrawACard(isLeft, new filter((c) =>
                            { return (c.exapler == targ.exapler) && (c.exapler != Cards.ReaverScout); }));
                            if (drawen != null)
                                game.PlayCard(isLeft, drawen);
                        }
                    });
                    break;
                case Cards.TemerianInfantry:
                    SetParams("Temerian Infantry", "Summon all copies of this unit.", new List<Tag>() { Tag.NothernRealms, Tag.Temeria, Tag.Solder }, Rarity.bronze);
                    SetStandartActions(3);
                    onDeploy = new Action((card, game) =>
                    {

                        bool isLeft = game.FriendField(this).isBlue;
                        Card drawen = game.DrawACard(isLeft, new filter((c) =>
                        { return (c.exapler == this.exapler); }));
                        if (drawen != null)
                            game.PlayCard(isLeft, drawen);
                    });
                    break;
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
                            enemy.Damage(4, this);
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
                        List<Card> allys = Game.selectFrom("Select ally to boost by 6", 1, true, selectFrom);

                        Unit targ = (allys.Count > 0) ? allys[0] as Unit : null;
                        if (targ != null)
                        {
                            game.pingBoard(this, targ, 800, 10, ConsoleColor.Green);
                            targ.Boost(6, this);
                        }
                    });
                    break;
                case Cards.RedanianKnight:
                    SetParams("Redanian Knight", "If this unit has no Armor, boost it by 2 and give it 2 Armor on turn end.", new List<Tag>() { Tag.NothernRealms, Tag.Solder, Tag.Redanie }, Rarity.bronze);
                    SetStandartActions(7);
                    onTurnEnd = new Action((card, game) =>
                    {
                        if (armorCount <= 0) { armorCount = 2; Boost(2, this); }
                    });
                    break;
                case Cards.RedanianKnightElect:
                    SetParams("Redanian Knight-Elect", "If this unit has Armor on turn end, boost adjacent units by 1. 2 Armor.", new List<Tag>() { Tag.NothernRealms, Tag.Solder, Tag.Redanie }, Rarity.bronze);
                    SetStandartActions(7);
                    armorCount = 2;
                    onTurnEnd = new Action((card, game) =>
                    {
                        if (armorCount > 0)
                        {
                            Field f = game.FriendField(this);
                            Unit left = f.NearUnit(this, true),
                                 right = f.NearUnit(this, false);
                            if (left != null) { game.pingBoard(this, left, 400, 5, ConsoleColor.Green); left.Boost(1, this); }
                            if (right != null) { game.pingBoard(this, right, 400, 5, ConsoleColor.Green); right.Boost(1, this); }
                        }
                    });
                    break;
                case Cards.TridamInfantry:
                    SetParams("Tridam Infantry", "4 Armor.", new List<Tag>() { Tag.NothernRealms, Tag.Solder }, Rarity.bronze);
                    SetStandartActions(10);
                    armorCount = 4;
                    break;

                case Cards.ReinforcedTrebuchet:
                    SetParams("Reinforced Trebuchet", "Deal 1 damage to a random enemy on turn end.", new List<Tag>() { Tag.NothernRealms, Tag.Machine }, Rarity.bronze);
                    SetStandartActions(8);
                    onTurnEnd = new Action((card, game) =>
                    {
                        List<Card> enemies = game.EnemyField(this).getUnitsAsCards;
                        Unit enemy = (enemies.Count > 0) ? enemies[game.rnd.Next(enemies.Count)] as Unit : null;
                        if (enemy != null)
                        {
                            game.pingBoard(this, enemy, 400, 15, ConsoleColor.Red);
                            enemy.Damage(1, this);
                        }
                    });
                    break;

                case Cards.DandelionPoet:
                    SetParams("Dandelion : Poet", "Draw a card, then play a card.", new List<Tag>() { Tag.Neutral, Tag.Support }, Rarity.gold);
                    SetStandartActions(5);
                    onDeploy = new Action((card, game) =>
                    {
                        bool isLeft = game.FriendField(this).isBlue;
                        game.DrawACard(isLeft, Game.AnyCard);
                        game.PlayCard(isLeft);
                    });
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

        public bool Damage(int dmg, Card from)
        {
            return Damage(dmg, from.name);
        }
        public bool Damage(int dmg, String from)
        {
            LOGS.Add(from + " deal " + dmg + " damage to " + name);
            if (armorCount > 0 && dmg >= armorCount)
                Console.Beep(400, 200);
            Console.Beep(1200, 200);
            DRAW.message(lastHo, lastVe, ("-" + dmg).PadLeft(3, ' '), ConsoleColor.Black, ConsoleColor.Red, 800);

            if (armorCount >= dmg) { armorCount -= dmg; dmg = 0; }
            else
            if (armorCount > 0 && dmg > 0) { dmg -= armorCount; armorCount = 0; }
            if (buffBonus > 0) { dmg -= buffBonus; buffBonus = 0; }
            currentHealth -= dmg;
            if (currentHealth < 0) currentHealth = 0;
            Redraw();
            return (currentHealth <= 0);
        }

        public void Boost(int buff, Card from)
        {
            Boost(buff, from.name);
        }
        public void Boost(int buff, String from)
        {
            LOGS.Add(from + " boost " + name + " for +" + buff);
            Console.Beep(1400, 100);
            DRAW.message(lastHo, lastVe, ("+" + buff).PadLeft(3, ' '), ConsoleColor.Black, ConsoleColor.Green, 800);
            buffBonus += buff;
            Redraw();
        }
        public void BoostArmor(int buff, Card from)
        {
            LOGS.Add(from.name + " added to " + name + " " + buff+" armor");
            Console.Beep(300, 50);
            DRAW.message(lastHo, lastVe, ("+" + buff).PadLeft(3, ' '), ConsoleColor.Black, ConsoleColor.Yellow, 800);
            armorCount += buff;
            Redraw();
        }
        public void Strengthlen(int str, Card from)
        {
            LOGS.Add(from.name + " strengt " + name + " for +" + str);
            Console.Beep(400, 100);
            DRAW.message(lastHo, lastVe, ("+" + str).PadLeft(3, ' '), ConsoleColor.Black, ConsoleColor.White, 800);
            basePower += str;
            currentHealth += str;
        }

        public override bool isBlue(Game game)
        {
            int rowIndex;
            if (game.left.IndexOf(this, out rowIndex) >= 0)
                return game.left.isBlue;
            if (game.right.IndexOf(this, out rowIndex) >= 0)
                return game.right.isBlue;
            throw new Exception("No such unit in a field!");
        }

        public void Die(Game game)
        {
            LOGS.Add(name + " died");
            dead = true;
            DRAW.die(game.rnd, lastHo, lastVe, 6, 2, 500, true, "*");
            this.TriggerEvent(Event.death, game, this);
            DRAW.die(game.rnd, lastHo, lastVe, 6, 2, 500, false, " ");
            Field at = game.FriendField(this);
            int rowIndex, charIndex = at.IndexOf(this, out rowIndex);
            Row row = at.getRow(rowIndex);

            if (!at.isBlue)
                game.pingBoard(new Point(lastHo, lastVe), new Point(52, 36), ConsoleColor.Gray, 10, 500, true, false, false);
            else
                game.pingBoard(new Point(lastHo, lastVe), new Point(30, 36), ConsoleColor.Gray, 10, 500, true, true, true);

            row.removeAt(this);
            row.RedrawAll();
            game.AddToGraveyard(at.isBlue, this);

            this.currentHealth = this.basePower;
            this.armorCount = 0;
            this.buffBonus = 0;
        }

        public void SpawnEffect(Game game)
        {
            LOGS.Add(name + " spawned");
            DRAW.die(game.rnd, lastHo, lastVe, 6, 2, 0, false, " ");
            DRAW.die(game.rnd, lastHo, lastVe, 6, 2, 400, false, "%");
        }
    }
}
