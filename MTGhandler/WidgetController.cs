using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTGhandler
{

    delegate void EventAction(List<int> param, MWidget w, MWidget sender);
    class IO
    {
        public static List<ConsoleKey> KeyAvailable =
            new List<ConsoleKey>() { ConsoleKey.Escape, ConsoleKey.Spacebar, ConsoleKey.LeftArrow, ConsoleKey.UpArrow, ConsoleKey.RightArrow, ConsoleKey.DownArrow };
    }
    struct MEvent
    {
        public MWidget Sender;
        public MEventType Type;
        public List<int> Params;
        String listedParamsOnCreate;

        public MEvent(MEventType Type, List<int> Params, MWidget sender)
        {
            this.Type = Type;
            this.Params = Params;
            this.Sender = sender;
            listedParamsOnCreate = "";
            for (int i = 0; i < Params.Count; ++i)
                listedParamsOnCreate += Params[i] + ";";
        }
        public override string ToString()
        {
            return String.Format("{0}-event ({1})", Type, listedParamsOnCreate);
        }
        public static MEvent ButtonPressEvent(ConsoleKey key, MWidget sender)
        {
            MEventType t = MEventType.ButtonPress;
            List<int> p = new List<int>();
            // -1 wrong, 
            // 0 = ESC, 1 = SPACE, 2 = <-   3 = ^   4 = ->   5 = v
            p.Add(IO.KeyAvailable.IndexOf(key));
            return new MEvent(t, p, sender);
        }
        public static MEvent ButtonPressEvent(ConsoleKeyInfo keyInfo, MWidget sender)
        {
            MEventType t = MEventType.ButtonPress;
            List<int> p = new List<int>();
            p.Add(IO.KeyAvailable.IndexOf(keyInfo.Key));
            p.Add((int)keyInfo.Modifiers);
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
        public static MEvent RedrawEvent(MWidget sender)
        {
            return new MEvent(MEventType.Redraw, new List<int>(), sender);
        }
        public static MEvent PingEvent(MWidget sender)
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
            actions.Add(new EventAction((param, w, sender) => { }));
            //BroadCast = 1,
            actions.Add(new EventAction((param, w, sender) => { ExecuteForAllChildren(ME(MEventType.BroadCast, param)); }));
            //Ping = 2,
            actions.Add(new EventAction((param, w, sender) => { Console.Write(w.name + " | "); ExecuteForAllChildren(ME(MEventType.Ping, param)); }));
            //ButtonPress = 3,
            actions.Add(new EventAction((param, w, sender) => {
                w.HandleKeyPress(param);
            }));
            //Unlock = 4,
            actions.Add(new EventAction((param, w, sender) => { w.SetLock(false); }));
            //Lock = 5,
            actions.Add(new EventAction((param, w, sender) => { w.SetLock(true); ExecuteForAllUnlockedChildren(ME(MEventType.Lock, param)); }));
            //Redraw = 6
            actions.Add(new EventAction((param, w, sender) =>
            {
                if (param.Count >= 2)
                    w.Redraw(new MPoint(param[0], param[1]));
                else
                    w.Redraw();
            }));
        }
        public void SetAction(MEventType type, EventAction action)
        {
            actions[(int)type] = action;
        }
        public void SendEvent(MEvent E)
        {
            ExecuteEvent(E);
        }
        private void ExecuteForAllChildren(MEvent E)
        {
            foreach (MWidget w in Widget.Children)
                if (w.Controller != null)
                    w.Controller.SendEvent(E);
        }
        private void ExecuteForAllUnlockedChildren(MEvent E)
        {
            foreach (MWidget w in Widget.Children)
                if (w.Controller != null && !w.IsLocked)
                    w.Controller.SendEvent(E);
        }
        private MEvent ME(MEventType typ, List<int> param)
        {
            return new MEvent(typ, param, this.Widget);
        }
        public void RepeatForChildren(MEventType typ, List<int> param)
        {
            ExecuteForAllChildren(ME(typ, param));
        }
        protected void ExecuteEvent(MEvent E)
        {
            Logs.TraceMarked(String.Format("{0} executes {1}from {2}", Widget.name, E.ToString(), E.Sender.name), new List<String>() { Widget.name, E.ToString(), E.Sender.name });
            actions[(int)E.Type](E.Params, Widget, E.Sender);
        }
    }
}
