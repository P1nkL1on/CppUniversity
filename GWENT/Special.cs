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
            exapler = c;
            switch (c)
            {
                case Cards.AdrenalineRush:
                    SetParams("Adrenaline Rush", "Toggle a unit's Resilience status.", new List<Tag>() { Tag.Neutral, Tag.Organic, Tag.Special }, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Card> ens = game.AllUnitsOnField();
                        List<Card> enemies = Game.selectFrom("Select a unit", 1, true, ens);
                        Unit u = (enemies.Count > 0) ? enemies[0] as Unit : null;
                        if (u != null)
                        {
                            game.pingBoard(this, u, 1600, 15, ConsoleColor.Magenta);
                            u.ChangeStatus(Ability.resilence);
                        }
                    });
                    break;
                case Cards.AlzursThunder:
                    SetParams("Alzur's Thunder", "Deal 9 damage.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Spell }, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Card> enemies = Game.selectFrom("Select enemy to deal 9 damage", 1, true, game.AllUnitsOnField());
                        Unit enemy = (enemies.Count > 0) ? enemies[0] as Unit : null;
                        if (enemy != null)
                        {
                            game.pingBoard(this, enemy, 800, 10, ConsoleColor.Yellow);
                            enemy.Damage(9, this);
                        }
                    });
                    break;
                case Cards.DimetriumShakles:
                    SetParams("Dimetrium Shackles", "Toggle a unit's Lock status. If an enemy, deal 4 damage to it.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Alchemy, Tag.Item }, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Card> enemies = Game.selectFrom("Lock unit", 1, true, game.AllUnitsOnField());
                        Unit enemy = (enemies.Count > 0) ? enemies[0] as Unit : null;
                        if (enemy != null)
                        {
                            game.pingBoard(this, enemy, 900, 4, ConsoleColor.Gray);
                            if (game.FriendField(enemy).isBlue != game.FriendField(this.isLeft).isBlue)
                                enemy.Damage(4, this);

                            enemy.ChangeStatus(Ability.locked);
                        }
                    });
                    break;
                case Cards.BloodcurlingRoar:
                    SetParams("Bloodcurling Roar", "Destroy an ally. Spawn a Bear.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Organic }, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        List<Card> us = Game.selectFrom("Select ally to destroy", 1, true, game.FriendField(isLeft).getUnitsAsCards);
                        Unit u = (us.Count > 0) ? us[0] as Unit : null;
                        if (u != null)
                        {
                            game.pingBoard(this, u, 400, 20, ConsoleColor.Red);
                            u.Die(game);
                            Unit bear = new Unit(Cards.BearToken);
                            game.FriendField(isLeft).SelectAndDeployUnit(bear, game);
                        }
                    });
                    break;
                case Cards.CrowsEye:
                    SetParams("Crow's Eye", "Deal 4 damage to the Highest enemy on each row. Deal 1 extra damage for each copy of this card in your graveyard.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Organic , Tag.Alchemy}, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        Field f = game.EnemyField(this.isLeft);
                        int bonusDamage = 0;
                        foreach (Card ca in game.cardFrom(Game.From.friendlyGraveyard, this.isLeft))
                            if (ca.exapler == Cards.CrowsEye)
                                bonusDamage++;
                        
                        for (int i = 0; i < 3; i++)
                            Game.HighestUnit(f.getRow(i).getUnits).Damage(4 + bonusDamage, this);
                    });
                    break;


                case Cards.ArachasVenom:
                    SetParams("Arachas Venom", "Deal 4 damage to 3 adjacent units.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Organic }, Rarity.bronze);
                    onDeploy = new Action((card, game) =>
                    {
                        Field f = game.EnemyField(this.isLeft);
                        List<Card> enemies = Game.selectFrom("Select central enemy", 1, true, f.getUnitsAsCards);
                        Unit enemy = (enemies.Count > 0) ? enemies[0] as Unit : null;
                        if (enemy != null)
                        {
                            game.pingBoard(this, enemy, 800, 10, ConsoleColor.Red);
                            enemy.Damage(4, this);
                            int rowIndex, enInd = f.IndexOf(enemy, out rowIndex);
                            Unit len = f.getRow(rowIndex).getUnit(enInd - 1),
                                 ren = f.getRow(rowIndex).getUnit(enInd + 1);
                            if (len != null) len.Damage(4, this);
                            if (ren != null) ren.Damage(4, this);
                        }
                    });
                    break;
                case Cards.BitingFrost:
                    SetParams("Biting Frost", "Apply a Hazard to an enemy row that deals 2 damage to the lowest unit on turn start.", new List<Tag>() { Tag.Neutral, Tag.Special, Tag.Hazard }, Rarity.bronze);
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
                        Field f = game.EnemyField(this.isLeft);
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
