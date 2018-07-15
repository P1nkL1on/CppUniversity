using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    abstract class MWidget
    {
        public abstract int GetWidth { get; }
        public abstract int GetHeight { get; }
        protected bool isLocked = false;
        public MWidget Parent = null;
        protected CColor mainColor = new CColor(ConsoleColor.White, ConsoleColor.Black);
        protected CColor lockColor = new CColor(ConsoleColor.Gray, ConsoleColor.DarkGray);
        protected CColor Color { get { return (isLocked)? lockColor : mainColor; } }

        public virtual void Redraw(MPoint leftUpCorner)
        {
            MDrawHandler.DrawRectangle(leftUpCorner, GetWidth, GetHeight, Color);
            MDrawHandler.DrawStringInPoint(leftUpCorner, Color, String.Format("{0} -- ({1};{2})", name, GetWidth, GetHeight), GetWidth);
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
            get { return 0; }
        }
        public abstract String name { get; }
        
    }

    class MTestWidget : MWidget
    {
        public MTestWidget()
        {

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
