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

        public Unit(int power, string nam, string descr, Rarity rar, List<Tag> unitags)
        {
            SetParams(nam, descr, unitags, rar);
            SetStandartActions(power);
        }

        public Unit(Cards name)
        {
            switch (name)
            {
                case Cards.AedirianMauler:
                    SetParams("Aedirian Mauler", "Deal 4 damage to enemy.", new List<Tag>() { Tag.NothernRealms, Tag.Solder }, Rarity.bronze);
                    SetStandartActions(7);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Unit> enemies = Game.selectFrom(1, true, game.EnemyField(this).getUnits);
                        Unit enemy = (enemies.Count > 0) ? enemies[0] : null;
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
                        List<Unit> allys = Game.selectFrom(1, true, game.FriendField(this).getUnits);
                        allys.Remove(this);
                        Unit targ = (allys.Count > 0) ? allys[game.rnd.Next(allys.Count)] : null;
                        if (targ != null)
                        {
                            game.pingBoard(this, targ, 800, 10, ConsoleColor.Green);
                            targ.Boost(6);
                        }
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
            DRAW.setBuffTo(bufHorizontal, bufVertical);
            DRAW.rarity(2, rarity);
            DRAW.power(Power, 3, PowerColor);
            DRAW.str(" " + name, 10, false);
        }
        public override void TraceField(int bufHorizontal, int bufVertical)
        {
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
        public void Redraw()
        {
            TraceField(lastHo, lastVe);
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
            i += DRAW.str((description.Length == 0) ? "no ability" : description, bufHorizontal + 1, bufVertical + 4 + i, wid, 5);
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
