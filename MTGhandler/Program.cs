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
            MLayoutVertical testV = new MLayoutVertical(1), testL = new MLayoutVertical(3), testL2 = new MLayoutVertical(1);
            test.AddWidget(new MLable("okay", 2));
            for (int i = 0; i < 7; ++i)
                if (i % 2 == 0)
                    testL2.AddWidget(new MCheckBox("Okay" + i));
                else
                    testL2.AddWidget(new MTestWidget());
                for (int i = 0; i < 3; ++i)
                    test.AddWidget(new MTestWidget());
            for (int i = 0; i < 3; ++i)
                testL.AddWidget(new MLable("Line number " + i * i * i));
            testL.AddWidget(new MListBox(10, -1, new List<MLineWidget>(){
                new MCheckBox("AAA", false),
                new MCheckBox("BBBBBB", false),
                new MCheckBox("CCC", false)}));
            for (int i = 0; i < 3; ++i)
                testL.AddWidget(new MLable("Line number " + i * i * i * (-2)));
            test.AddWidget(testL);
            for (int i = 0; i < 3; ++i)
                test.AddWidget(new MTestWidget());
            testL.AddWidget(new MListBox(10, -1, new List<MLineWidget>(){
                new MCheckBox("AaA", false),
                new MCheckBox("BbBbBb", false)}));
            test.AddWidget(new MListBox(20, -1, new List<MLineWidget>(){
                new MLable("---A---"),
                new MCheckBox("A1", false),
                new MCheckBox("A2", true),
                new MLable("---B---"),
                new MLable("---C---"),
                new MCheckBox("C1", false),
                new MCheckBox("C2", true),
                new MCheckBox("C3", true),
                }));
            test.AddWidget(new MTestWidget());
            test.AddWidget(testL2);
            test.AddWidget(new MTestWidget());
            for (int i = 0; i < 3; ++i)
                testV.AddWidget(new MTestWidget());

            testV.AddWidget(test);
            MListBox lsitBox = new MListBox(25, -1, new List<MLineWidget>(){
                new MCheckBox("Уничтожить вселенную", false),
                new MCheckBox("Сварить кофеёк", true),
                new MLable("Почесать котику пузико"),
                new MCheckBox("Злобный смех", false)});
            testV.AddWidget(lsitBox);
            // for (int i = 0; i < 3; ++i)
            //     testV.AddWidget(new MTestWidget());

            testV.Controller.SendEvent(MEvent.RedrawEvent(new MPoint(4, 10), testV));

            testV.Controller.SendEvent(MEvent.UnlockEvent(testV));
            testV.Redraw();

            Logs.Write = true;
            while (true)
            {
                testV.Controller.SendEvent(MEvent.ButtonPressEvent(Console.ReadKey(true), testV));
                //testV.Redraw();
            }

            Console.ReadLine();
        }
    }
}
