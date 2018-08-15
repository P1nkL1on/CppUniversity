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
        MLable Header;
        MLableMulty Text;
        MButton OkButton;
        public MMessageBox(String header, String message)
        {
            Header = new MLable(header, MaxWidth);
            Header.setMainColor(new CColor(ConsoleColor.DarkGray, ConsoleColor.Gray));
            Header.setLockColor(new CColor(ConsoleColor.Gray, ConsoleColor.DarkGray));
            Text = new MLableMulty(message, MaxWidth - 2);
            OkButton = new MButton(" OK ", new Slot(() => { Logs.Trace("OK ON MESSAGE BOX PRESSED"); }));
            Children.Add(Header);
            Children.Add(Text);
            Children.Add(OkButton);
            MakeController();
        }
        void MakeController()
        {
            Controller = new MWidgetController(this);
            Controller.SetAction(
                MEventType.Unlock,
                new EventAction((param, w, sender) =>
                {
                    foreach (MWidget c in w.Children)
                        c.Controller.SendEvent(new MEvent(MEventType.Unlock, param, sender));
                    w.SetLock(false);
                }));
        }
        public override int GetHeight
        {
            get { return Text.GetHeight + 2; }
        }
        public override int GetWidth
        {
            get { return Text.GetWidth + 2; }
        }
        public override string name
        {
            get { return "MessageBox"; }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            Header.Redraw(leftUpCorner);
            Text.Redraw(leftUpCorner.Add(1, 1));
            OkButton.Redraw(leftUpCorner.AddX(GetWidth / 2 - 3).AddY(GetHeight - 1));
        }
    }
}
