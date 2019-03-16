using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    abstract class MLayout : MWidget
    {
        protected int selectedWidgetIndex = -1;
        public void AddWidget(MWidget w)
        {
            w.SetParent(this);
            selectedWidgetIndex = Children.Count - 1;
        }
        protected void setDefaultParams()
        {
            setDefaultColors();
            Controller = new MWidgetController(this);
            Controller.SetAction(
                MEventType.Unlock,
                new EventAction((param, w, sender) =>
                {
                    w.Children[(w as MLayout).selectedWidgetIndex].Controller.SendEvent(new MEvent(MEventType.Unlock, param, sender));
                    w.SetLock(false);
                }));
        }
        protected void setDefaultColors()
        {
            setLockColor(new CColor(ConsoleColor.DarkGray, ConsoleColor.DarkGray));
            setMainColor(new CColor(ConsoleColor.Gray, ConsoleColor.Gray));
        }
        public override CColor Color
        {
            get
            {
                //foreach (MWidget c in Children)
                //    if (!c.IsLocked && ((c as MLayout) == null))
                //        return lockColor;
                //return mainColor;
                if (IsLocked)
                    return lockColor;
                foreach (MWidget c in Children)
                    if (((c as MLayout) != null) && c.Color.back == mainColor.back) // any colored layouts
                        return lockColor;
                return mainColor;
            }
        }
        public override int childrenCount
        {
            get
            {
                return Children.Count;
            }
        }
        public override string name
        {
            get { return "Layout"; }
        }
        protected abstract ConsoleKey keyDecrease { get; }
        protected abstract ConsoleKey keyIncrease { get; }
        protected int MoveModifyer = 4;
        public void AddIndex(int X)
        {
            int prevIndex = selectedWidgetIndex;
            selectedWidgetIndex += X;
            while (selectedWidgetIndex < 0)
                selectedWidgetIndex += childrenCount;
            while (selectedWidgetIndex >= childrenCount)
                selectedWidgetIndex -= childrenCount;
            MWidget was = Children[prevIndex];
            MWidget now = Children[selectedWidgetIndex];
            was.Controller.SendEvent(MEvent.LockEvent(this));
            now.Controller.SendEvent(MEvent.UnlockEvent(this));

            if (was as MLayout != null || now as MLayout != null)
                Redraw();
            else
            {
                RedrawChild(now); RedrawChild(was);
            }
            Logs.Trace(String.Format("{0}: selected {1}->{2}", name, prevIndex, selectedWidgetIndex));
        }
        public override bool KeyPressAction(ConsoleKeyInfo keyInfo)
        {
            base.KeyPressAction(keyInfo);
            // make it byhimself
            if ((int)keyInfo.Modifiers != MoveModifyer)
                return false;
            return Move(keyInfo);
        }
        public override bool KeyPressActionAndNoneChildrenAnswered(ConsoleKeyInfo keyInfo)
        {
            base.KeyPressActionAndNoneChildrenAnswered(keyInfo);
            // do not check a modifyer if none children dones
            return Move(keyInfo);
        }
        bool Move(ConsoleKeyInfo keyInfo)
        {
            MLayout childLay = this.FindChildrenLayoutWithSameControls(this);
            if (childLay != null)
                return false;
            if (keyInfo.Key == keyIncrease)
            {
                AddIndex(1);
                return true;
            }
            if (keyInfo.Key == keyDecrease)
            {
                AddIndex(-1);
                return true;
            }
            return false;
        }
        public bool CompareKeys(MLayout another)
        {
            return (keyDecrease == another.keyDecrease) && (keyIncrease == another.keyIncrease);
        }
    }
    class MLayoutHorizontal : MLayout
    {
        protected override ConsoleKey keyDecrease
        { get { return ConsoleKey.LeftArrow; } }
        protected override ConsoleKey keyIncrease
        { get { return ConsoleKey.RightArrow; } }

        public MLayoutHorizontal()
        {
            spaceWidth = 0;
            setDefaultParams();
        }
        public MLayoutHorizontal(int _spaces)
        {
            spaceWidth = _spaces;
            setDefaultParams();
        }
        int spaceWidth;

        public override int GetHeight
        {
            get
            {
                int maxHeight = 0;
                foreach (MWidget w in Children)
                    if (w.GetHeight > maxHeight)
                        maxHeight = w.GetHeight;
                return maxHeight;
            }
        }
        public override int GetWidth
        {
            get
            {
                int width = 0;
                foreach (MWidget w in Children)
                    width += w.GetWidth;
                width += Math.Max(0, Children.Count - 1) * spaceWidth;
                return width;
            }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            int xOffset = 0;
            int height = GetHeight;
            foreach (MWidget w in Children)
            {
                MPoint where = new MPoint(leftUpCorner.x + xOffset, leftUpCorner.y + height / 2 - w.GetHeight / 2);
                w.Redraw(where);
                xOffset += spaceWidth + w.GetWidth;
            }
        }
        public override string name
        {
            get { return "Horizontal Layout"; }
        }
    }

    class MLayoutVertical : MLayout
    {
        protected override ConsoleKey keyDecrease
        { get { return ConsoleKey.UpArrow; } }
        protected override ConsoleKey keyIncrease
        { get { return ConsoleKey.DownArrow; } }

        public MLayoutVertical()
        {
            spaceHeight = 0;
            setDefaultParams();
        }
        public MLayoutVertical(int _spaces)
        {
            spaceHeight = _spaces;
            setDefaultParams();
        }
        int spaceHeight;

        public override int GetWidth
        {
            get
            {
                int maxWidth = 0;
                foreach (MWidget w in Children)
                    if (w.GetWidth > maxWidth)
                        maxWidth = w.GetWidth;
                return maxWidth;
            }
        }
        public override int GetHeight
        {
            get
            {
                int height = 0;
                foreach (MWidget w in Children)
                    height += w.GetHeight;
                height += Math.Max(0, Children.Count - 1) * spaceHeight;
                return height;
            }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            int yOffset = 0;
            int width = GetWidth;
            foreach (MWidget w in Children)
            {
                MPoint where = new MPoint(leftUpCorner.x + width / 2 - w.GetWidth / 2, leftUpCorner.y + yOffset);
                w.Redraw(where);
                yOffset += spaceHeight + w.GetHeight;
            }
        }
        public override string name
        {
            get { return "Vertical Layout"; }
        }
    }
}
