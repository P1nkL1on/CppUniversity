using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolfensten
{
    delegate void unitStep(Map m, AbstractObject self);
    class UnitHelper
    {
        public static unitStep doNothing = new unitStep((m, u) => { });
        public  static unitStep movementSystem = new unitStep((m, u) => {
                ConsoleKeyInfo k = Console.ReadKey(true);
                AbstractMovableObject ma = u as AbstractMovableObject;
                if (ma == null)
                    return;
                switch (k.Key)
                {
                    case ConsoleKey.LeftArrow:
                        ma.moveX(-1, m);
                        break;
                    case ConsoleKey.RightArrow:
                        ma.moveX(1, m);
                        break;
                    case ConsoleKey.UpArrow:
                        ma.moveY(-1, m);
                        break;
                    case ConsoleKey.DownArrow:
                        ma.moveY(1, m);
                        break;
                    default:
                        break;
                }
            });
    }
    class Unit : AbstractMovableObject, IDrawable, ISteppable
    {

        public static Unit Human(){
            Unit human = new Unit(); 
            human.setColors('i',  new ColorBlock(ConsoleColor.Yellow));
            return human;
        }

        public static Unit Player()
        {
            Unit player = Human();
            player.setAction(UnitHelper.movementSystem);
            return player;
        }

        public static Unit Spider()
        {
            Unit spider = new Unit();
            spider.setColors('m', new ColorBlock(ConsoleColor.White));
            spider.setAction(new unitStep((m, u) => {
                Random rnd = new Random(DateTime.Now.Millisecond);
                List<int> moves = new List<int>() { 1,0 , -1, 0,  0,1,  0,-1};
                int selectedWay = rnd.Next(4);
                AbstractMovableObject ua = u as AbstractMovableObject;
                ua.move(moves[selectedWay * 2], moves[selectedWay * 2 + 1], m);
            }));

            return spider;
        }
        
        private Unit()
        {
            setXY(0, 0);
            setAction(UnitHelper.doNothing);
        }
        protected void setColors(char c, ColorBlock clr){
            this.c = c;
            this.color = clr;
        }

        public void step(Map map)
        {
            action(map, this);
        }

        public void setAction(unitStep value) {  action = value;  }

        protected ColorBlock color;
        protected char c;
        public ColorBlock Color { get { return color; } }
        public char Symbol { get { return c; } }

        protected unitStep action;



    }
}
