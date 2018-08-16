using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class UserCommands
    {
        static List<string> things = new List<string>() { "stats", "manapoints", "healthpoints"};
        public static List<string> commands = new List<string>()
        {
             
        };
        public static string helpCommand { get { return "help"; } }
        public static string pauseCommand { get { return "pause"; } }
        public static string resumeCommand { get { return "resume"; } }
        public static string finishCommand { get { return "finish current phase"; } }
        public static string playCardCommand { get { return "play card"; } }
        public static string useAbilityCommand { get { return "use ability"; } }

        public static void initialiseWordList(Game context, Player forWho)
        {
            commands.Clear();
            commands.Add(helpCommand);
            commands.Add(resumeCommand);
            commands.Add(pauseCommand);
            commands.Add(finishCommand);

            commands.Add(playCardCommand);
            commands.Add(useAbilityCommand);

            commands.Add("show game");
            foreach (Player pl in context.players)
            {
                string plName = "";
                if (forWho != pl)
                    plName = pl.Name + "'s ";
                for (int i = 0; i < things.Count; ++i)
                    commands.Add(String.Format("show {0}{1}", plName, things[i]));
                for (int j = 0; j < 1; ++j)
                    for (int i = 0; i < 5; ++i)
                        commands.Add(String.Format("{2} {0}{1}", plName, (CardHolderTypes)i, (j==1)? "info" : "show"));
            }
            int x = 0;
        }
        public static string addCommand(string now)
        {
            now = now.TrimStart(' ');
            lock (Utils.ConsoleWriterLock)
            {
                int maxWid = Console.WindowWidth / 2;
                int consoleX = now.Length, consoleY = Console.CursorTop;
                Console.CursorTop++;
                Console.CursorLeft = 0;

                string[] nowT = now.Split(' '),
                         sT;
                string result = "";
                string lastSelected = "";
                int mu = 0;
                foreach (string s in commands)
                {
                    sT = s.Split(' ');
                    bool add = true;
                    int i = 0;
                    for (; i < Math.Min(nowT.Length, sT.Length); ++i)
                        if (add && sT[i].IndexOf(nowT[i]) != 0)
                            add = false;
                    if (/*mu < 3 &&*/ add)
                    {
                        if (result.IndexOf(sT[i - 1]) < 0)
                        {
                            ++mu;
                            result += sT[i - 1] + "  ";
                        }
                        lastSelected = s;
                    }
                }
                if (result.Length > maxWid)
                    result = result.Substring(0, maxWid - 2)+"..";
                Utils.ConsoleWriteLine(result.PadRight(maxWid), ConsoleColor.DarkCyan);
                Console.CursorLeft = consoleX;
                Console.CursorTop = consoleY;

                if (mu > 1)
                    return "";
                return lastSelected;//commands.IndexOf(lastSelected);
            }
        }
    }
}
