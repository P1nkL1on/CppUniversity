using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GWENT
{
    class Special : Card
    {
        int lastHo;
        int lastVe;

        Action onDeploy;
        bool isLeft;

        public void TriggerEvent(Event even, Game game, bool isLeft)
        {
            this.isLeft = isLeft;
            onDeploy(this, game);
            game.CheckDeadUnits();
            game.RedrawTop();
            game.DrawCounts(Game.CountPlace.ALL);
        }

        public override int Power
        {
            get { return 0; }
        }
        public override void TraceInList(int bufHorizontal, int bufVertical)
        {
            lastDrawType = 1;
            lastVe = bufVertical; lastHo = bufHorizontal;
            DRAW.setBuffTo(bufHorizontal, bufVertical);
            DRAW.rarity(2, rarity);
            DRAW.str("  *");
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
            DRAW.setBuffTo(bufHorizontal, bufVertical + 3 + i);
            DRAW.str(" Rarity: "); DRAW.rarity(2, rarity); DRAW.str(" " + rarity.ToString());
            i += DRAW.str((description.Length == 0) ? "no ability" : description, bufHorizontal, bufVertical + 4 + i, wid + 1, 5);
            Game.DrawPicture(DRAW.rarityColor(rarity), exapler);
        }

        public override Point leftTop
        {
            get { return new Point(lastHo, lastVe); }
        }

        public Special(Cards c)
        {
            switch (c){
                case Cards.BitingFrost:
                    SetParams("Biting Frost", "Apply a Hazard to an enemy row that deals 2 damage to the lowest unit on turn start.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Hazard}, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        Field f = game.EnemyField(this.isLeft);
                        f.ApplyHazzardToRow(f.SelectRow(), Hazard.Frost);
                    });
                    break;
                case Cards.InpenetrableFog:
                    SetParams("Inpenetrable Fog", "Apply a Hazard to an enemy row that deals 2 damage to the highest unit on turn start.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Hazard }, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        Field f = game.EnemyField(this.isLeft);
                        f.ApplyHazzardToRow(f.SelectRow(), Hazard.Tuman);
                    });
                    break;
                case Cards.TorrentioalRain:
                    SetParams("Torrential Rain", "Apply a Hazard to an enemy row that deals 1 damage to 2 random units on turn start.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Hazard }, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        Field f = game.EnemyField(!this.isLeft);
                        f.ApplyHazzardToRow(f.SelectRow(), Hazard.Rain);
                    });
                    break;
                case Cards.BloodFlail:
                    SetParams("Bloody Flail", "Deal 5 damage and Spawn a Specter on a random row.", new List<Tag>() { Tag.NothernRealms, Tag.Special, Tag.Item }, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Card> enemies = Game.selectFrom("Select enemy to deal 5 damage", 1, true, game.EnemyField(this.isLeft).getUnitsAsCards);
                        Unit enemy = (enemies.Count > 0) ? enemies[0] as Unit : null;
                        if (enemy != null)
                        {
                            game.pingBoard(this, enemy, 800, 10, ConsoleColor.Red);
                            enemy.Damage(5, this);
                            if (isLeft)
                                game.left.RandomlyDeployUnit(new Unit(Cards.Specter), game);
                            else
                                game.right.RandomlyDeployUnit(new Unit(Cards.Specter), game);
                        }
                    });
                    break;

                default:
                    break;
            }
        }

        public override bool isBlue(Game game)
        {
            return isLeft;
            //int side = game.sideOFCard(this);
            //return (side == 0);
        }
    }
}
