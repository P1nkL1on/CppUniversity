using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class Logs
    {
        static List<String> logs = new List<string>();
        static CColor logColor = new CColor(ConsoleColor.Black, ConsoleColor.White);
        static CColor markedColor = new CColor(ConsoleColor.Black, ConsoleColor.Red);

        static MPoint leftTopCorner = new MPoint(100, 0);
        static int width = 100;
        static int height = 60;

        static bool Write = false;

        public static MPoint Trace(String S)
        {
            if (!Write) return new MPoint(-1,-1);
            logs.Add(S);
            MPoint where = leftTopCorner.AddY(logs.Count % height);
            MDrawHandler.DrawStringInPoint(where, logColor, S, width);
            return where;
        }
        public static MPoint TraceMarked(String S, List<String> Events)
        {
            if (!Write) return new MPoint(-1, -1);
            MPoint where = Trace(S);
            foreach (String e in Events)
            {
                int offset = S.IndexOf(e);
                if (offset >= 0)
                    MDrawHandler.DrawStringInPoint(where.AddX(offset), markedColor, e, e.Length);
            }
            return where;
        }
    }
}
