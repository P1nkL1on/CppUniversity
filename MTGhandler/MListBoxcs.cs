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
            MakeController();
        }
        public MListBox(int Width, int Height, List<MLineWidget> lines)
        {
            width = Width;
            height = Height;
            foreach (MLineWidget line in lines)
                AddLine(line);
            MakeController();
        }
        void MakeController()
        {
            Controller = new MWidgetController(this);
            Controller.SetAction(
                MEventType.Unlock,
                new EventAction((param, w, sender) =>
                { Controller.RepeatForChildren(MEventType.Unlock, param); w.SetLock(false); }));
        }
        public void AddIndex(int X)
        {
            int prevIndex = selectedIndex;
            selectedIndex += X;
            while (selectedIndex < 0)
                selectedIndex += childrenCount;
            while (selectedIndex >= childrenCount)
                selectedIndex -= childrenCount;
            Logs.Trace(String.Format("{0}: selected {1}->{2}", name, prevIndex, selectedIndex));
        }
        public override bool KeyPressAction(ConsoleKeyInfo key)
        {
            // make it byhimself
            if (key.Key == ConsoleKey.DownArrow)
            {
                AddIndex(1);
                return true;
            }
            if (key.Key == ConsoleKey.UpArrow)
            {
                AddIndex(-1);
                return true;
            }
            return false;
        }
        // add a scroll bar
        public override int GetHeight
        {
            get { return ((height >= 0) ? height : LineCount + 2); }
        }
        public override int GetWidth
        {
            get { return width; }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);

            for (int i = 0; i < ((height < 0) ? LineCount : (height - 2)); ++i)
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
        public override List<MWidget> SelectedChildren
        {
            get
            {
                return new List<MWidget>(){Children[selectedIndex]};
            }
        }

    }
}
