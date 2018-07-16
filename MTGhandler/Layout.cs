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
                    w.SetLock(false);
                    w.Children[(w as MLayout).selectedWidgetIndex].Controller.SendEvent(new MEvent(MEventType.Unlock, param, sender));
                    return false;
                }));
        }
        protected void setDefaultColors()
        {
            setMainColor(new CColor(ConsoleColor.Gray, ConsoleColor.DarkGray));
            setLockColor(new CColor(ConsoleColor.DarkGray, ConsoleColor.Black));
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
    }
    class MLayoutHorizontal : MLayout
    {
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

    }

    class MLayoutVertical : MLayout
    {
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
    }
}
