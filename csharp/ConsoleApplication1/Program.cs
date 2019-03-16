using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        enum cttt
        {
            vit = 0,
            cmb = 1,
            inl = 2,
            src = 3,
            all = 4,
            none = 5
        }
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth / 2, Console.LargestWindowHeight);

            List<string> ww = new List<string>()
            {
                 "способность", "Атака в лоб", "без условий\n\nУменьшите силу выбранного персонажа противника на 4 ед.\n\n\n\n\n\n\nВсё гениальное просто."
                ,"способность", "Подсечка", "без условий\n\nУменьшает силу выбранного персонажа противника и использующего персонажа на 1 ед. Если бросок удался, понизьте его параметр интеллекта противника на 1 ед. "
                ,"способность", "Обезоружить", "без условий\n\nУничтожьте выбранный артефакт- оружие.\n\n\n\n\n\n\nИ чтобы я видел ваши руки!"
                ,"способность", "Защитная стойка", "без условий\n\nУвеличьте силу использующего персонажа на 5 ед."
                ,"способность", "Сокрушение", "бросок на боенавык\n\nУменьшите силу выбранного персонажа противника на 3 ед. Если бросок удался, то вместо этого на 5 ед.\n\n\nБросок успешен, когда выпавшее число не более указанной характеристики использующего."
                ,"способность", "Охрана", "бросок на боенавык\n\nПеренаправьте следующий урон по вашему выбранному персонажа на использующего.\nЕсли бросок удался, то усильте обоих на 2 ед.\n\n\n\nЛожитесь, господин президент!"
                ,"способность", "Сломать", "бросок на боенавык\n\nУменьшите силу выбранного персонажа противника на 3. Если бросок удался, понизьте его параметр телосложения на 1 ед.\n\n\n\nБей по коленям!"
                ,"способность", "Стальная защита", "бросок на боенавык\n\nУвеличьте силу использующего персонажа на его значение телосложения. Если бросок удался, то увеличьте ещё на 2 ед."

                ,"способность", "Холодный рассчёт", "без условий\n\nДо конца хода использующего персонажа его боенавык становится равным 6 ед."
                
                ,"артефакт", "Ржавый палаш", "оружие\n\nКогда вы наносите урон любому персонажу противника первый раз за ход, то увеличьте его на 1 ед."
                ,"артефакт", "Тайный стилет", "оружие\n\nЕсли вы понижаете тесложение противника, то понизьте его дополнительно на 1 ед."       
                ,"артефакт", "Трухлявый доспех", "экипировка\n\nКогда экипированный персонаж получает урон вы можете уничтожить трухлявый доспех. Если вы это делаете, уменьшите входящий урон на 4 ед."

                ,"артефакт", "Тяжёлый кастет", "оружие\n\nКогда вы делаете бросок на боенавык, делайте его с таким рассчётом, как будто бы у вас было на 1 ед. боенавыка больше."
                

                ,"способность", "Прямой блок", " боенавык и телосложение\n\nСледующий немагический урон, который получит использующий персонаж будет уменьшен на сумму значений его телосложения и боенавыка."
                

                ,"способность", "Финт", "без условий\n\nУвеличьте силу использующего персонажа на 2 ед. Уменьшите на то же значение силу выбранного вражеского персонажа."
                ,"способность", "Захват", "бросок на телесложение\n\nУменьшите силу выбранного персонажа противника на D3. Если бросок удался, то до начала следующего вашего хода боенавык этого персонажа противника становится равной 1 ед."
                ,"способность", "Лоб в лоб", "бросок на телесложение\n\nУменьшает силу выбранного персонажа противника и использующего персонажа на 1 ед. Если бросок удался, понизьте параметр интеллекта противника на 1 ед. "
                ,"способность", "Уворот", "бросок на телесложение\n\nЕсли бросок удался, то до начала следующего хода использующего персонажа он не может получать урон.\nЕсли бросок не удался, то увеличьте силу использующего персонажа на 2 ед."
                ,"способность", "Сальто", "бросок на телесложение\n\nПоложите 2 карты из вашей руки под низ колоды. Возьмите столько же карт. Если бросок удался, то вместо 2-х карт, поменяйте 3."
                ,"способность", "Ошеломляющий удар", "бросок на телосложение\n\nУменьшите силу выбранного персонажа противника на 3. Если бросок удался, то дополнительно уменьшите её на значение броска D3."
                
                ,"способность", "Сложение атланта", "без условий\n\nДо конца хода использующего персонажа его телосложение становится равным 6 ед."

                
                ,"способность", "Мнгновенный удар", "без условий\n\nВозьмите карту. Вы можете сыграть красную карту из вашей руки. Если вы этого не делаете, то сбросьте карту, которую взяли."
                ,"способность", "Подлый трюк", "без условий\n\nВозьмите две карты. Если среди них есть зелёная, то можете сыграть её. Оставшиеся карты положите в низ колоды."
              
			  };
            List<int> values = new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7, 4, 0, 0, 0, 0, 0, 6 };
            for (int i = 0; i < ww.Count / 3; ++i)
            {
                Console.Clear();
                traceCard(
                    (i < values.Count) ? ((cttt)(values[i] % 6)) : cttt.none,
                    ((i < values.Count) ? (values[i]) : 0) >= 6,
                    ww[i * 3], ww[i * 3 + 1], ww[i * 3 + 2]);
                if (Console.ReadLine() == "f") break;
            }

            List<string> hh = new List<string>()
            {
                 "персонаж", "Линда, Тихая Тень","эльфка убийца\n"
                ,"персонаж", "Луна, Принцесса Ночей","старейшая колдунья\n"
                ,"персонаж", "Ферфелд, Дурной Алхимик","старейший колдун алхимик\n"
                ,"персонаж", "Милл, Астральная Сказочница","эльхия ведьма\n"
                ,"персонаж", "Тут'хель, Дорожный Бард", "человек плут\n"
                ,"персонаж", "Сарий, Чёрный Граф", "нежить убийца\n"
                ,"персонаж", "Ег'хор, Искатель Истины", "эльх пилигрим\n"
            };
            List<List<int>> param = new List<List<int>>()
            {
                new List<int>(){9,  3, 2, 3, 1},
                new List<int>(){7, 1, 1, 3, 4},
                new List<int>(){10, 4, 1, 2, 2},
                new List<int>(){10, 2, 1, 3, 3},
                new List<int>(){8, 3, 2, 4, 0},
                new List<int>(){7, 2, 4, 3, 0},
                new List<int>(){11, 4, 3, 1, 1}
            };

            for (int h = 0; h < hh.Count / 3; ++h)
            {
                Console.Clear();
                int basePower = param[h][0];
                List<int> stats = param[h];
                traceCard(cttt.all, false, hh[h * 3], hh[h * 3 + 1], hh[h * 3 + 2]);

                List<string> chars = new List<string>() { 
                "Телосложение",
                "Боевые навыки", 
                "Интеллект", 
                "Колдовство"};
                for (int i = 0; i < 4; ++i)
                {
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.Write(" " + chars[i].PadRight(14) + ":: ");
                    Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = getColor(i, false);
                    Console.Write(" X ");
                    for (int st = 0; st < 6; ++st)
                    {
                        if (st == stats[i + 1])
                        {
                            Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.White;
                        }
                        Console.Write(" " + (st + 1) + " ");
                    }
                    Console.WriteLine();
                }
                Console.ResetColor();
                Console.WriteLine("\n Базовая сила  :: " + basePower);
                Console.ReadLine();

            }
            Console.ReadLine();
        }

        static void writeSomething(string S, int X, int Y, int wid, int hei, bool B, ConsoleColor bas)
        {
            S = " " + S;
            string inn = " ;:";
            string nums = "0123456789";
            int ox = 0, oy = 0;
            Console.ForegroundColor = (B) ? ConsoleColor.White : ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            int NN = S.IndexOf('\n');
            ox = wid / 2 - NN / 2;
            for (int i = 0; i < S.Length; ++i)
            {
                Console.SetCursorPosition(X + ox, Y + oy);
                if (S[i] == '\n' && i > 2 && S[i - 1] == '\n' && S[i - 2] == '\n')
                    Console.ForegroundColor = bas;
                if (S[i] != '\n' && !(ox < 1 && S[i] == ' ' && oy > 0))
                {
                    if (nums.IndexOf(S[i]) >= 0 && oy > 0)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(S[i]);

                    if (nums.IndexOf(S[i]) >= 0 && oy > 0)
                        Console.ResetColor();
                    ox++;
                }
                if (ox > wid || (inn.IndexOf(S[i]) >= 0 && ox > wid * 4 / 5) || S[i] == '\n')
                {
                    ox = 0; oy++; if (oy == 1)
                    {
                        Console.Write(' ');
                        Console.ResetColor();
                    }
                }
            }
        }
        static ConsoleColor getColor(int from, bool isLegend)
        {
            return getColor((cttt)from, isLegend);
        }
        static ConsoleColor getColor(cttt type, bool isLegend)
        {
            ConsoleColor clr = ConsoleColor.DarkGray;
            switch (type)
            {
                case cttt.all:
                    clr = (isLegend) ? ConsoleColor.Yellow : ConsoleColor.DarkYellow;
                    break;
                case cttt.vit:
                    clr = (isLegend) ? ConsoleColor.Green : ConsoleColor.DarkGreen;
                    break;
                case cttt.cmb:
                    clr = (isLegend) ? ConsoleColor.Red : ConsoleColor.DarkRed;
                    break;
                case cttt.inl:
                    clr = (isLegend) ? ConsoleColor.Cyan : ConsoleColor.DarkCyan;
                    break;
                case cttt.src:
                    clr = (isLegend) ? ConsoleColor.Blue : ConsoleColor.DarkBlue;
                    break;
                default:
                    break;
            }
            return clr;
        }
        static void traceCard(cttt type, bool isLegend, string TYP, string name, string descr)
        {
            int wid = 40;
            int toppart = 20;
            int hei = 30;

            ConsoleColor clr = getColor(type, isLegend);
            
            string SS = "";
            for (int i = 0; i < wid - 6; ++i)
                SS += (i % 2 == 0) ? '▄' : '▀';
            Console.ForegroundColor = (clr);

            Console.WriteLine("┼".PadRight(wid - 1, '▓') + "┼");
            Console.WriteLine("▓┌".PadRight(wid - 2, '─') + "┐▓");
            Console.WriteLine("▓│╔".PadRight(wid - 3, '═') + "╗│▓");
            Console.WriteLine("▓│║" + SS + "║│▓");
            for (int i = 0; i < toppart - 1; ++i)
                Console.WriteLine("▓│║".PadRight(wid - 3, ' ') + "║│▓");

            Console.WriteLine("▓│╟".PadRight(wid - 3, '─') + "╢│▓");

            Console.WriteLine("▓│║" + SS + "║│▓");
            for (int i = 0; i < hei - toppart; ++i)
                Console.WriteLine("▓│║".PadRight(wid - 3, ' ') + "║│▓");

            Console.WriteLine("▓│╚".PadRight(wid - 3, '═') + "╝│▓");
            Console.WriteLine("▓└".PadRight(wid - 2, '─') + "┘▓");
            Console.WriteLine("┼".PadRight(wid - 1, '▓') + "┼");



            Console.SetCursorPosition(wid / 2 - TYP.Length / 2 - 2, 3);
            Console.Write("█[" + TYP + "]█");

            ConsoleColor cl = Console.ForegroundColor;
            Console.ResetColor();
            Console.SetCursorPosition(wid / 2 - name.Length / 2 - 2, 1);
            Console.Write("┤ " + name + " ├");
            Console.SetCursorPosition(wid / 2 - name.Length / 2 - 2, 0);
            Console.Write("╒".PadRight(name.Length + 3, '═') + "╕");
            Console.SetCursorPosition(wid / 2 - name.Length / 2 - 2, 2);
            Console.Write("╧".PadRight(name.Length + 3, '═') + "╧");


            writeSomething(descr, 4, toppart + 4, wid - 9, hei - toppart, TYP.IndexOf("персонаж") < 0, cl);
            Console.SetCursorPosition(0, hei + 9);
        }
    }
}
