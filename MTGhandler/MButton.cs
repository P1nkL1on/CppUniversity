using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    delegate void Slot();

    class MButton : MLineWidget
    {
        public Slot onPressed;
        int width;
        public MButton(String S)
        {
            width = S.Length + 2;
            Text = S;
            MakeController();
        }
        public MButton(String S, int width)
        {
            Text = S;
            this.width = width;
            MakeController();
        }
        public MButton(String S, Slot slot)
        {
            width = S.Length + 2;
            Text = S;
            onPressed = slot;
            MakeController();
        }
        public MButton(String S, int width, Slot slot)
        {
            Text = S;
            this.width = width;
            onPressed = slot;
            MakeController();
        }
        public override string name
        {
            get { return "Button"; }
        }
        void MakeController()
        {
            Controller = new MWidgetController(this);
        }
        public override bool KeyPressAction(ConsoleKeyInfo key)
        {
            base.KeyPressAction(key);
            if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.Enter)
            {
                if (onPressed != null)
                    onPressed();
                return true;
            }
            return false;
        }

        public override string DrawText
        {
            get { return String.Format("[{0}]", MDrawHandler.Short(Text, width - 2)); }
        }
        public override int GetWidth
        { get { return width; } }
    }

    //class MDialog : MWidget
    //{
    //    int width;
    //    int height;
    //}
}
