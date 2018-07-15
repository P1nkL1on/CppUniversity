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
        protected List<MWidget> _widgets = new List<MWidget>();
        protected List<MWidget> widgets { get { return _widgets; } set { _widgets = value; } }
        public void AddWidget(MWidget w)
        {
            widgets.Add(w);
            selectedWidgetIndex = widgets.Count - 1;
            w.Parent = this;
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
                return widgets.Count;
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
            setDefaultColors();
        }
        public MLayoutHorizontal(int _spaces)
        {
            spaceWidth = _spaces;
            setDefaultColors();
        }
        int spaceWidth;

        public override int GetHeight
        {
            get {
                int maxHeight = 0;
                foreach (MWidget w in widgets)
                    if (w.GetHeight > maxHeight)
                        maxHeight = w.GetHeight;
                return maxHeight;
            }
        }
        public override int GetWidth
        {
            get {
                int width = 0;
                foreach (MWidget w in widgets)
                    width += w.GetWidth;
                width += Math.Max(0, widgets.Count - 1) * spaceWidth;
                return width;
            }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            int xOffset = 0;
            int height = GetHeight;
            foreach (MWidget w in widgets)
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
            setDefaultColors();
        }
        public MLayoutVertical(int _spaces)
        {
            spaceHeight = _spaces;
            setDefaultColors();
        }
        int spaceHeight;

        public override int GetWidth
        {
            get
            {
                int maxWidth = 0;
                foreach (MWidget w in widgets)
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
                foreach (MWidget w in widgets)
                    height += w.GetHeight;
                height += Math.Max(0, widgets.Count - 1) * spaceHeight;
                return height;
            }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            int yOffset = 0;
            int width = GetWidth;
            foreach (MWidget w in widgets)
            {
                MPoint where = new MPoint(leftUpCorner.x + width / 2 - w.GetWidth / 2, leftUpCorner.y + yOffset);
                w.Redraw(where);
                yOffset += spaceHeight + w.GetHeight;
            }
        }
    }
}
