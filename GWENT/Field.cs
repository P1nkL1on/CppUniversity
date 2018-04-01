using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWENT
{
    public class Field
    {
        public bool isBlue;
        List<Row> rows;
        int bufHor;
        int bufVer;
        public Field(int bufHor, int bufVer, bool isBlue)
        {
            this.isBlue = isBlue;
            this.bufHor = bufHor;
            this.bufVer = bufVer;
            rows = new List<Row>();
            List<string> rowsName = new List<string>() { "Crusade", "Support", "Machines" };
            for (int i = 0; i < rowsName.Count; i++)
                rows.Add(new Row(bufHor + ((!isBlue) ? 7 * i : 14 - 7 * i), bufVer + 2, rowsName[i], isBlue));

            TestFieldSpawn();
        }
        public Row getRow(int i) { return rows[i]; }
        public void SelectAndDeployUnit(Unit unit, Game game)
        {
            int curRow = 0, curInd = 0, prevRow = 0;
            UnitDeployPlace udp = new UnitDeployPlace();
            ConsoleKey k;
            DRAW.PushColor(ConsoleColor.DarkGreen);
            String what = "Select a place for unit deploying";
            DRAW.setBuffTo(39 - what.Length / 2, 32);
            DRAW.str(" " + what+" : ");
            DRAW.PopColor();
            do
            {
                rows[curRow].getUnits.Insert(curInd, udp);
                rows[curRow].RedrawAll();
                rows[prevRow].RedrawAll();
                rows[curRow].getUnits.RemoveAt(curInd);

                k = Console.ReadKey().Key;
                if (k == ConsoleKey.DownArrow)
                    curInd++;
                if (k == ConsoleKey.UpArrow)
                    curInd--;


                prevRow = curRow;
                if (k == ConsoleKey.RightArrow)
                    curRow--;
                if (k == ConsoleKey.LeftArrow)
                    curRow++;
                curRow = (curRow + rows.Count) % rows.Count;
                curInd = (curInd + rows[curRow].unitCount + 1) % (rows[curRow].unitCount + 1);
            } while (k != ConsoleKey.Enter);


            DRAW.setBuffTo(20, 32);
            DRAW.str("".PadLeft(40));

            Row add = rows[curRow];
            add.DeployUnitOnRow(unit, curInd);
            add.RedrawAll();
            unit.TriggerEvent(Event.deploy, game);

        }

        void TestFieldSpawn()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < (3 - i * i); j++)
                    //{
                    if ((i + j) % 2 == 0)
                        rows[i].DeployUnitOnRow(new Unit(Cards.AedirianMauler), 0);//(new Unit(11, "Fiend", "", Rarity.bronze, new List<Tag>() { Tag.Monsters, Tag.Relict }), 0);
                    else
                        rows[i].DeployUnitOnRow(new Unit(Cards.TemerianDrummer), 0);
                //rows[i].DeployUnitOnRow(new Unit(16, "Geralt", "", Rarity.gold, new List<Tag>() { Tag.Neutral, Tag.Witcher }), 0);
                //}
            }
        }

        public int TotalPower
        {
            get
            {
                int res = 0;
                for (int i = 0; i < rows.Count; i++)
                    res += rows[i].TotalPower;
                return res;
            }
        }

        public int IndexOf(Unit unit, out int rowIndex)
        {
            rowIndex = -1;
            for (int i = 0; i < rows.Count; i++)
                if (rows[i].indexOf(unit) >= 0)
                {
                    rowIndex = i;
                    return rows[i].indexOf(unit);
                }
            return -1;
        }

        public Unit RandomUnitOnRow(Random rnd, int rowIndex)
        {
            if (rowIndex >= 0)
            {
                if (rows[rowIndex].unitCount == 0) return null;
                return rows[rowIndex].getUnit(rnd.Next(rows[rowIndex].unitCount));
            }
            List<Unit> uns = getUnits;
            return uns[rnd.Next(uns.Count)];
        }
        public List<Unit> getUnits
        {
            get { List<Unit> res = new List<Unit>(); for (int i = 0; i < rows.Count; i++) res.AddRange(rows[i].getUnits); return res; }
        }
        public List<Card> getUnitsAsCards
        {
            get { List<Card> res = new List<Card>(); for (int i = 0; i < rows.Count; i++) res.AddRange(rows[i].getUnits); return res; }
        }
        public void DrawStart()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (i < rows.Count - 1)
                {
                    DRAW.setBuffTo(bufHor + 7 * i + 6, bufVer + 2);
                    DRAW.str("+");
                }
                rows[i].RedrawAll();
            }
            rows[0].DrawTop(TotalPower, -2);
        }
        public void RedrawTop()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (i < rows.Count - 1)
                {
                    DRAW.setBuffTo(bufHor + 7 * i + 6, bufVer + 2);
                    DRAW.str("+");
                    rows[i].DrawTop(rows[i].TotalPower, 0);
                }
            }
            rows[0].DrawTop(TotalPower, -2);
        }

        public List<Unit> findByTag(List<Tag> tags)
        {
            List<Unit> res = new List<Unit>();
            for (int r = 0; r < rows.Count; r++)
            {
                res.AddRange(rows[r].findByTag(tags));
            }
            return res;
        }

        public Unit NearUnit(Unit un, bool left, int far)
        {
            for (int i = 0; i < rows.Count; i++)
                if (rows[i].indexOf(un) >= 0)
                    return rows[i].UnitNeightboor(un, far * ((left) ? -1 : 1));
            return null;
        }
        public Unit NearUnit(Unit un, bool left)
        {
            for (int i = 0; i < rows.Count; i++)
                if (rows[i].indexOf(un) >= 0)
                    return rows[i].UnitNeightboor(un, ((left) ? -1 : 1));
            return null;
        }
    }
    public class Row
    {
        List<Unit> units;
        string name;
        bool isBlue;
        int bufHoriz;
        int bufVert;

        public List<Unit> getUnits
        {
            get
            {
                //List<Unit> res = new List<Unit>();
                //for (int i = 0; i < units.Count; i++)
                //    if (!units[i].dead)
                //        res.Add(units[i]);
                return units;
            }
        }
        public Row(int bufHoriz, int bufVert, string name, bool isBlue)
        {
            this.isBlue = isBlue;
            this.name = name;
            this.bufHoriz = bufHoriz;
            this.bufVert = bufVert;
            units = new List<Unit>();
        }

        public int unitCount
        {
            get { return units.Count; }
        }
        public int indexOf(Unit unit)
        {
            return units.IndexOf(unit);
        }

        public void removeAt(Unit u)
        {
            units.Remove(u);
        }

        public bool CallRedraw(Unit unit, bool selected)
        {
            int unitIndex = indexOf(unit);
            if (unitIndex < 0)
                return false;
            DRAW.PushColor((selected) ? ConsoleColor.DarkGreen : ConsoleColor.Black);
            unit.Redraw();
            DRAW.PopColor();
            return true;
        }

        public bool RedrawAll()
        {
            DrawTop(TotalPower, 0);
            for (int i = 0; i < units.Count; i++)
                units[i].TraceField(bufHoriz, 2 + bufVert + 3 * i);
            if (units.Count < 9)
                UnitDeployPlace.ClearField(bufHoriz, 2 + bufVert + 3 * units.Count);
            return true;
        }

        public bool DeployUnitOnRow(Unit unit, int at)
        {
            if (units.Count >= 9)
                return false;

            if (at >= units.Count)
                units.Add(unit);
            else
                units.Insert(at, unit);
            return true;
        }

        public int TotalPower
        {
            get
            {
                int res = 0;
                for (int i = 0; i < units.Count; i++)
                    res += units[i].Power;
                return res;
            }
        }
        public void DrawTop(int number, int yOffset)
        {
            DRAW.setBuffTo(bufHoriz, bufVert + yOffset);
            DRAW.PushColor((isBlue) ? ConsoleColor.DarkBlue : ConsoleColor.DarkRed);
            DRAW.power(number, 4, ConsoleColor.Gray);
            DRAW.str("  ");
            DRAW.PopColor();
        }
        public Unit getUnit(int index)
        {
            if (index >= units.Count || index < 0)
                return null;
            return units[index];
        }

        public List<Unit> findByTag(List<Tag> tags)
        {
            List<Unit> res = new List<Unit>();
            for (int i = 0; i < units.Count; i++)
            {
                bool allTags = true;
                for (int j = 0; j < tags.Count; j++)
                    if (units[i].tags.IndexOf(tags[j]) < 0)
                        allTags = false;
                if (allTags)
                    res.Add(units[i]);
            }
            return res;
        }

        public Unit UnitNeightboor(Unit who, int offset)
        {
            int whoInd = units.IndexOf(who);
            if (whoInd + offset >= 0 && whoInd + offset < units.Count)
                return units[whoInd + offset];
            return null;
        }
    }
}
