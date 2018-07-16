using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    abstract class MLineWidget : MWidget
    {
        public override int GetHeight
        {
            get { return 1; }
        }

        public String Text;
        protected int Width = 10;
        public void SetWidth(int width)
        {
            this.Width = width;
        }
        public override int GetWidth
        {
            get { return Width; }
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            MDrawHandler.DrawStringInPoint(leftUpCorner, Color, DrawText, Width);
        }
        public abstract String DrawText { get; }
        protected void Init()
        {
            Controller = new MWidgetController(this);
        }
    }
    class MLable : MLineWidget
    {
        public MLable(String S)
        {
            Text = S;
            Width = S.Length;
            Init();
        }
        public MLable(String S, int width)
        {
            Width = width; Text = S;
            Init();
        }
        public override string name
        {
            get { return "Label"; }
        }
        public override string DrawText
        {
            get { return Text.PadRight(Width); }
        }
    }
    class MCheckBox : MLineWidget
    {
        public bool isChecked;
        public MCheckBox(String S)
        {
            Text = S;
            Width = S.Length + 4;
            Init();
        }
        public MCheckBox(String S, bool isChecked)
        {
            Text = S;
            Width = S.Length + 4;
            this.isChecked = isChecked;
            Init();
        }
        public MCheckBox(String S, int width)
        {
            Width = width; Text = S;
            Init();
        }
        public MCheckBox(String S, int width, bool isChecked){
            Width = width; Text = S; this.isChecked = isChecked;
            Init();
        }
        public override string name
        {
            get { return "CheckBox"; }
        }
        public override string DrawText
        {
            get { return String.Format("[{0}] {1}",((isChecked)? "X" : " "),Text).PadRight(Width); }
        }
    }
}
