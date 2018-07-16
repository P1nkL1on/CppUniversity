using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            MDrawHandler.MaximiseWindow();
            CColor red = new CColor(ConsoleColor.Red);


            MLayoutHorizontal test = new MLayoutHorizontal(4);
            MLayoutVertical testV = new MLayoutVertical(1);
            test.AddWidget(new MLable("okay", 2));
            for (int i = 0; i < 4; ++i)
                test.AddWidget(new MTestWidget());
            for (int i = 0; i < 3; ++i)
                testV.AddWidget(new MTestWidget());
            testV.AddWidget(test);

            testV.AddWidget(
                new MListBox(25, -1, new List<MLineWidget>(){
                new MCheckBox("Уничтожить вселенную", false),
                new MCheckBox("Сварить кофеёк", true),
                new MLable("Почесать котику пузико"),
                new MCheckBox("Злобный смех", false)}));

            //testV.Redraw(new MPoint(10, 20));
            testV.Controller.SendEvent(MEvent.RedrawEvent(new MPoint(20,10), null));
            testV.Controller.SendEvent(MEvent.PingEvent(null));

            Console.ReadLine();
        }
    }
}
