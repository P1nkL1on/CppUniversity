using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{

    delegate bool EventAction(List<int> param, MWidget w, MWidget sender);

    struct MEvent
    {
        public MWidget Sender;
        public MEventType Type;
        public List<int> Params;

        public MEvent(MEventType Type, List<int> Params,  MWidget sender)
        {
            this.Type = Type;
            this.Params = Params;
            this.Sender = sender;
        }

        public MEvent ButtonPressEvent(ConsoleKey key, MWidget sender)
        {
            MEventType t = MEventType.ButtonPress;
            List<int> p = new List<int>(1);
            // -1 wrong, 
            // 0 = ESC, 1 = SPACE, 2 = <-   3 = ^   4 = ->   5 = v
            p[0] = -1;
            if (key == ConsoleKey.Escape)
                p[0] = 0;
            if (key == ConsoleKey.Spacebar)
                p[0] = 1;
            if (key == ConsoleKey.LeftArrow)
                p[0] = 2;
            if (key == ConsoleKey.UpArrow)
                p[0] = 3;
            if (key == ConsoleKey.RightArrow)
                p[0] = 4;
            if (key == ConsoleKey.DownArrow)
                p[0] = 5;
            return new MEvent(t, p, sender);
        }
        public static MEvent LockEvent(MWidget sender)
        {
            return new MEvent(MEventType.Lock, new List<int>(), sender);
        }
        public static MEvent UnlockEvent(MWidget sender)
        {
            return new MEvent(MEventType.Unlock, new List<int>(), sender);
        }
        public static MEvent RedrawEvent(MPoint where, MWidget sender)
        {
            return new MEvent(MEventType.Redraw, new List<int>() { where.x, where.y }, sender);
        }
        public static MEvent PingEvent( MWidget sender)
        {
            return new MEvent(MEventType.Ping, new List<int>(), sender);
        }
    }

    enum MEventType
    {
        None = -1,
        Invalid = 0,
        BroadCast = 1,
        Ping = 2,
        ButtonPress = 3,
        Unlock = 4,
        Lock = 5,
        Redraw = 6
    }

    class MWidgetController
    {
        MWidget Widget = null;
        List<EventAction> actions;

        public MWidgetController(MWidget connectedTo)
        {
            Widget = connectedTo;
            SetStandartActions();
        }
        void SetStandartActions()
        {
            //return false;
            actions = new List<EventAction>();
            //Invalid = 0,
            actions.Add(new EventAction((param, w, sender) => { return false; }));
            //BroadCast = 1,
            actions.Add(new EventAction((param, w, sender) => { return true; }));
            //Ping = 2,
            actions.Add(new EventAction((param, w, sender) => { Console.Write(w.name + " | "); return true; }));
            //ButtonPress = 3,
            actions.Add(new EventAction((param, w, sender) => { return true; }));
            //Unlock = 4,
            actions.Add(new EventAction((param, w, sender) => { w.SetLock(true); return true; }));
            //Lock = 5,
            actions.Add(new EventAction((param, w, sender) => { w.SetLock(false); return false; }));
            //Redraw = 6
            actions.Add(new EventAction((param, w, sender) => { w.Redraw(new MPoint(param[0], param[1])); return false; }));
        }
        public void SendEvent(MEvent what)
        {
            if (ExecuteEvent(what))
                foreach (MWidget w in Widget.Children)
                    if (w.Controller != null)
                        w.Controller.SendEvent(what);
        }
        protected bool ExecuteEvent(MEvent E)
        {
            return actions[(int)E.Type](E.Params, Widget, E.Sender);
        }
    }
}
