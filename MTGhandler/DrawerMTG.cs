using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class MDrawHandlerMTG
    {
        public static CColor DefaultColor = new CColor(ConsoleColor.DarkGray, ConsoleColor.White);
        static CColor redManaColor = new CColor(ConsoleColor.Red, ConsoleColor.Black);
        static CColor blueManaColor = new CColor(ConsoleColor.Blue, ConsoleColor.White);
        static CColor greenManaColor = new CColor(ConsoleColor.Green, ConsoleColor.Black);
        static CColor whiteManaColor = new CColor(ConsoleColor.Yellow, ConsoleColor.Black);
        static CColor blackManaColor = new CColor(ConsoleColor.Black, ConsoleColor.White);
        static CColor anyManaColor = new CColor(ConsoleColor.DarkGray, ConsoleColor.White);

        public static CColor ColorOf(CardColor cardColor)
        {
            switch (cardColor)
            {
                case CardColor.Black:
                    return blackManaColor;
                case CardColor.Blue:
                    return blueManaColor;
                case CardColor.Green:
                    return greenManaColor;
                case CardColor.Red:
                    return redManaColor;
                case CardColor.White:
                    return whiteManaColor;
                default:
                    return anyManaColor;
            }
        }

        static char whiteColorManaSymbol = '*';
        static char blueColorManaSymbol = 'U';
        static char blackColorManaSymbol = '#';
        static char redColorManaSymbol = '@';
        static char greenColorManaSymbol = 'Y';

        public static void DrawManaCostAtCardHeader(MPoint where, ManaCost mc)
        {
            int offset = 0;
            if (mc.Any != 0 || mc.ConvertedManaCost == 0)
            {
                MDrawHandler.DrawStringInPoint(where.AddX(offset), anyManaColor, mc.Any.ToString());
                offset += mc.Any.ToString().Length;
            }
            MDrawHandler.DrawStringInPoint(where.AddX(offset), whiteManaColor, "".PadLeft(mc.White, whiteColorManaSymbol)); offset += mc.White;
            MDrawHandler.DrawStringInPoint(where.AddX(offset), blueManaColor, "".PadLeft(mc.Blue, blueColorManaSymbol)); offset += mc.Blue;
            MDrawHandler.DrawStringInPoint(where.AddX(offset), blackManaColor, "".PadLeft(mc.Black, blackColorManaSymbol)); offset += mc.Black;
            MDrawHandler.DrawStringInPoint(where.AddX(offset), redManaColor, "".PadLeft(mc.Red, redColorManaSymbol)); offset += mc.Red;
            MDrawHandler.DrawStringInPoint(where.AddX(offset), greenManaColor, "".PadLeft(mc.Green, greenColorManaSymbol)); offset += mc.Green;
            
        }
    }
}
