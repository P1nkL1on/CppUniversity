using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    abstract class MWindow : MWidget
    {
    }
    class MMessageBox : MWindow
    {
        static int MaxWidth = 40;
        public MMessageBox(String header, String message)
        {
            MLable Header = new MLable(header, MaxWidth);
            Header.setMainColor(new CColor(ConsoleColor.DarkGray, ConsoleColor.Gray));
            Header.setLockColor(new CColor(ConsoleColor.Gray, ConsoleColor.DarkGray));
            MLableMulty Text = new MLableMulty(message, MaxWidth - 2);
            MLayoutVertical layout = new MLayoutVertical(0);
            layout.AddWidget(Header);
            layout.AddWidget(Text);
            layout.AddWidget(new MButton(" OK "));
            Children.Add(layout);
            MakeController();
        }
        MWidget layout
        {
            get { return Children[0]; }
        }
        void MakeController()
        {
            Controller = new MWidgetController(this);
            Controller.SetAction(MEventType.Unlock,
                new EventAction((param, w, from)
                => { layout.Controller.SendEvent(MEvent.UnlockEvent(w)); }));
        }
        public override int GetHeight
        {
            get { return layout.GetHeight; }
        }
        public override int GetWidth
        {
            get { return layout.GetWidth; }
        }
        public override string name
        {
            get { return "MessageBox"; }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            layout.Redraw(leftUpCorner);
        }
    }
}
