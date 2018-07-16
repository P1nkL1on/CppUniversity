using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    abstract class MWidget
    {
        public MWidgetController Controller;
        public abstract int GetWidth { get; }
        public abstract int GetHeight { get; }
        protected bool isLocked = true;
        public bool IsLocked { get { return isLocked; } }
        public void SetLock(bool isLocked) { this.isLocked = isLocked; }
        public MWidget Parent = null;
        public List<MWidget> Children = new List<MWidget>();
        public virtual List<MWidget> SelectedChildren { get { return Children; } }
        protected CColor mainColor = new CColor(ConsoleColor.White, ConsoleColor.Black);
        protected CColor lockColor = new CColor(ConsoleColor.Gray, ConsoleColor.DarkGray);
        protected virtual CColor Color { get { return (isLocked) ? lockColor : mainColor; } }
        protected MPoint LastRedrawPoint = new MPoint(-1, -1);
        public void Redraw()
        {
            Redraw(LastRedrawPoint);
        }
        public void RedrawChild(MWidget who)
        {
            foreach (MWidget w in Children)
            {
                if (w == who)
                {
                    w.Redraw();
                    return;
                }
                w.RedrawChild(who);
            }
        }
        protected MLayout FindChildrenLayoutWithSameControls(MLayout who)
        {
            foreach (MWidget w in Children)
            {
                MLayout ml = w as MLayout;
                if (ml != null && ml.CompareKeys(who))
                    return ml;
                ml = w.FindChildrenLayoutWithSameControls(who);
                if (ml != null)
                    return ml;
            }
            return null;
        }
        public virtual void Redraw(MPoint leftUpCorner)
        {
            //Logs.Trace(String.Format("{0} was redrawn at ({1};{2})", name, leftUpCorner.x, leftUpCorner.y));
            //if (LastRedrawPoint.isEmpty)
            {
                MDrawHandler.DrawRectangle(leftUpCorner, GetWidth, GetHeight, Color);
                //MDrawHandler.DrawRectangleBorder(new MRectangle(leftUpCorner, GetWidth, GetHeight), new CColor(ConsoleColor.Gray));
            }
            LastRedrawPoint = leftUpCorner;


            //MDrawHandler.DrawStringInPoint(leftUpCorner, Color, String.Format("{0} -- ({1};{2})", name, GetWidth, GetHeight), GetWidth);
        }
        public virtual void setMainColor(CColor c)
        {
            mainColor = c;
        }
        public virtual void setLockColor(CColor c)
        {
            lockColor = c;
        }
        public virtual int childrenCount
        {
            get { return Children.Count; }
        }
        public abstract String name { get; }

        public void setDefaultColors()
        {
            mainColor = new CColor(ConsoleColor.White, ConsoleColor.Black);
            lockColor = new CColor(ConsoleColor.Gray, ConsoleColor.DarkGray);
        }
        public void SetParent(MWidget what)
        {
            this.Parent = what;
            what.Children.Add(this);
        }
        public bool HandleKeyPress(List<int> param)
        {
            if (param[0] < 0)
                return false;   // non usurped

            ConsoleKeyInfo key = new ConsoleKeyInfo(' ', IO.KeyAvailable[param[0]], false, false, param[1] >= 4);
            bool usurped = KeyPressAction(key);
            if (usurped)
            {
                Logs.Trace(String.Format("╔══►{0} handled key {1}", name, key + ""));
                return true;
            }    // usurped
            foreach (MWidget c in SelectedChildren)
                if (!c.IsLocked)
                {
                    usurped = c.HandleKeyPress(param);
                    if (usurped)
                    {
                        Logs.Trace(String.Format("╔══►{0} handled key {1}", name, key + ""));
                        return true;
                    }
                }
            return false;
        }
        public virtual bool KeyPressAction(ConsoleKeyInfo key)
        {
            // do nothing
            return false; // do not usurpait
        }
    }

    class MTestWidget : MWidget
    {
        public MTestWidget()
        {
            Controller = new MWidgetController(this);
        }
        public override int GetWidth { get { return 6; } }
        public override int GetHeight { get { return 6; } }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            for (int i = 0; i < 4; ++i)
                MDrawHandler.DrawStringInPoint(leftUpCorner.Add(1, 1 + i), Color, "TEST");
        }
        public override string name
        {
            get { return "TestWidget"; }
        }
    }
}
