using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class MListBox : MWidget
    {
        List<MLineWidget> lines = new List<MLineWidget>();
        int selectedIndex = 0;
        int width = 0;
        int height = 0;
        CColor selectedColor = new CColor(ConsoleColor.Blue, ConsoleColor.White);
        public int LineCount { get { return lines.Count; } }
        public void AddLine(MLineWidget w)
        {
            AddLine(w, LineCount);
        }
        public void AddLine(MLineWidget w, int at)
        {
            w.SetParent(this);
            w.SetWidth(width - 2);
            if (at >= LineCount)
                lines.Add(w);
            else
                lines.Insert(at, w);
            selectedIndex = Math.Max(at, LineCount - 1);
        }
        public MListBox(int Width, int Height)
        {
            width = Width;
            height = Height;
            Controller = new MWidgetController(this);
        }
        public MListBox(int Width, int Height, List<MLineWidget> lines)
        {
            width = Width;
            height = Height;
            foreach (MLineWidget line in lines)
                AddLine(line);
            Controller = new MWidgetController(this);
        }
        // add a scroll bar
        public override int GetHeight
        {
            get { return ((height >= 0)? height : LineCount + 2); }
        }
        public override int GetWidth
        {
            get { return width; }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            
                for (int i = 0; i < ((height < 0)? LineCount : (height - 2)); ++i)
                {
                    if (i == selectedIndex) lines[i].setMainColor(selectedColor);
                    lines[i].Redraw(leftUpCorner.Add(1, 1 + i));
                    if (i == selectedIndex) lines[i].setDefaultColors();
                }
        }
        public override string name
        {
            get { return "ListBox"; }
        }
        
    }
}
