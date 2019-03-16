using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class ManaCostHandler : MLineWidget
    {
        ManaCost mana;
        public ManaCostHandler(ManaCost mana)
        {
            this.mana = mana;
        }
        public override void Redraw(MPoint leftUpCorner)
        {
            base.Redraw(leftUpCorner);
            MDrawHandlerMTG.DrawManaCostAtCardHeader(leftUpCorner, mana);
        }
        public override int GetWidth
        {
            get
            {
                return mana.ConvertedManaCost - mana.Any + ((mana.Any == 0) ? 0 : (mana.Any.ToString()).Length); // only colors
            }
        }
        public override string DrawText
        {
            get { return ""; }
        }
        public override string name
        {
            get { return "ManaCostHandler"; }
        }
    }
    class ManaCost
    {
        int[] colorMana = new int[5];
        int anyColorMana = 0;
        public int Red { get { return colorMana[0]; } set { colorMana[0] = value; } }
        public int Blue { get { return colorMana[1]; } set { colorMana[1] = value; } }
        public int Green { get { return colorMana[2]; } set { colorMana[2] = value; } }
        public int White { get { return colorMana[3]; } set { colorMana[3] = value; } }
        public int Black { get { return colorMana[4]; } set { colorMana[4] = value; } }
        public int Any { get { return anyColorMana; } set { anyColorMana = value; } }

        private ManaCost(int r, int b, int g, int w, int bl, int a)
        {
            Red = r; Blue = b; Green = g; Black = bl; White = w; Any = a;
        }
        public static ManaCost RedManaCost(int red, int any)
        {
            return new ManaCost(red, 0, 0, 0, 0, any);
        }
        public static ManaCost BlueManaCost(int blue, int any)
        {
            return new ManaCost(0, blue, 0, 0, 0, any);
        }
        public static ManaCost GreenManaCost(int green, int any)
        {
            return new ManaCost(0, 0, green, 0, 0, any);
        }
        public static ManaCost WhiteManaCost(int white, int any)
        {
            return new ManaCost(0, 0, 0, white, 0, any);
        }
        public static ManaCost BlackManaCost(int black, int any)
        {
            return new ManaCost(0, 0, 0, 0, black, any);
        }
        public static ManaCost MultiColorManaCost(int r, int b, int g, int w, int bl, int a)
        {
            return new ManaCost(r, b, g, w, bl, a);
        }
        public static ManaCost None()
        {
            return new ManaCost(0,0,0,0,0,0);
        }
        public int ConvertedManaCost
        {
            get { return Red + Blue + Green + White + Black + Any; }
        }
    }
}
