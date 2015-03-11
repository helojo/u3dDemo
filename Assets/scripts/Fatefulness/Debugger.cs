namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Debugger
    {
        private static Debugger _debugger;
        public Action action_break;
        public Action action_resume;
        public Action<StackContext> action_stack;
        private bool broken;
        private List<InvokeScene> call_stack = new List<InvokeScene>();
        private string key_debugger = string.Empty;
        private List<BreakPoint> lst_breakpoint = new List<BreakPoint>();
        private int stack_top = -1;
        private Status status;

        public void Attach(string key)
        {
            this.key_debugger = key;
            this.Reset();
        }

        public void Clear()
        {
            this.lst_breakpoint.Clear();
        }

        public void CollectStack(Transition from, Transition to, Intent intent_register, Intent intent_global, Intent intent_stack)
        {
            <CollectStack>c__AnonStorey161 storey = new <CollectStack>c__AnonStorey161();
            InvokeScene item = new InvokeScene();
            storey.root = null;
            Intent.Iterator iter = new Intent.Iterator(storey.<>m__13F);
            storey.root = item.global;
            intent_global.Foreach(iter);
            storey.root = item.register;
            intent_register.Foreach(iter);
            storey.root = item.stack;
            intent_stack.Foreach(iter);
            item.from = from;
            item.to = to;
            this.call_stack.Add(item);
        }

        public bool Connected()
        {
            return (Application.isEditor && Application.isPlaying);
        }

        public void OnInvoke(Transition from, Transition to, Intent intent)
        {
            <OnInvoke>c__AnonStorey162 storey = new <OnInvoke>c__AnonStorey162();
            if (this.Connected() && ((from != null) && (to != null)))
            {
                storey.key = from.root_fragment.DebugKey();
                if (storey.key == this.key_debugger)
                {
                    storey.id_from = from.id;
                    storey.id_to = to.id;
                    if (this.broken)
                    {
                        this.CollectStack(from, to, intent, from.root_fragment.global_context, from.root_fragment.stack_context);
                    }
                    else if ((this.lst_breakpoint.Find(new Predicate<BreakPoint>(storey.<>m__140)) != null) && (this.action_break != null))
                    {
                        this.broken = true;
                        this.stack_top = 0;
                        this.call_stack.Clear();
                        this.CollectStack(from, to, intent, from.root_fragment.global_context, from.root_fragment.stack_context);
                        this.action_break();
                        this.OnSimulateStack();
                    }
                }
            }
        }

        private void OnSimulateStack()
        {
            if (((this.action_stack != null) && this.broken) && ((this.stack_top < this.call_stack.Count) && (this.stack_top >= 0)))
            {
                StackContext context = new StackContext();
                InvokeScene scene = this.call_stack[this.stack_top];
                context.from = scene.from.id;
                context.to = scene.to.id;
                context.id = scene.from.lanch_concent.id;
                context.global = scene.global;
                context.register = scene.register;
                context.stack = scene.stack;
                context.global.expand = true;
                context.global.name = "global";
                context.stack.expand = true;
                context.stack.name = "stack";
                context.register.expand = true;
                context.register.name = "local";
                this.action_stack(context);
            }
        }

        public void Pause()
        {
        }

        private void RegisterBreakPoint(Concentrator concent)
        {
            Transition transition = concent.Launcher();
            if (transition != null)
            {
                BreakPoint item = new BreakPoint {
                    key = concent.root_fragment.DebugKey(),
                    id_tnsi = transition.id,
                    id_host = concent.id
                };
                this.lst_breakpoint.Add(item);
            }
        }

        public void Reset()
        {
            this.status = Status.RUNNING;
            this.lst_breakpoint.Clear();
        }

        public void Resume()
        {
            int num = this.stack_top + 1;
            int count = this.call_stack.Count;
            if (this.broken && (num < count))
            {
                int num3 = -1;
                for (int i = num; i != count; i++)
                {
                    <Resume>c__AnonStorey163 storey = new <Resume>c__AnonStorey163 {
                        scene = this.call_stack[i]
                    };
                    if (this.lst_breakpoint.Find(new Predicate<BreakPoint>(storey.<>m__141)) != null)
                    {
                        num3 = i;
                    }
                }
                if (num3 != -1)
                {
                    this.stack_top = num3;
                    this.OnSimulateStack();
                    return;
                }
            }
            this.broken = false;
            if (this.action_resume != null)
            {
                this.action_resume();
            }
        }

        public void StepInto()
        {
            if (this.broken && (this.stack_top >= 0))
            {
                if (++this.stack_top >= this.call_stack.Count)
                {
                    this.broken = false;
                    if (this.action_resume != null)
                    {
                        this.action_resume();
                    }
                }
                else
                {
                    this.OnSimulateStack();
                }
            }
        }

        public bool SyncBreakPoint(Concentrator concent)
        {
            <SyncBreakPoint>c__AnonStorey15F storeyf = new <SyncBreakPoint>c__AnonStorey15F();
            if (!this.Connected())
            {
                return false;
            }
            storeyf.id = concent.id;
            return (-1 != this.lst_breakpoint.FindIndex(new Predicate<BreakPoint>(storeyf.<>m__13D)));
        }

        public void ToggleBreakPoint(Concentrator concent)
        {
            <ToggleBreakPoint>c__AnonStorey160 storey = new <ToggleBreakPoint>c__AnonStorey160();
            if (this.Connected())
            {
                storey.id = concent.id;
                int index = this.lst_breakpoint.FindIndex(new Predicate<BreakPoint>(storey.<>m__13E));
                if (index != -1)
                {
                    this.lst_breakpoint.RemoveAt(index);
                }
                else
                {
                    this.RegisterBreakPoint(concent);
                }
            }
        }

        public static void TraceArrayObject(ObjectMetaData meta, Array array)
        {
            meta.descript = "...";
            int length = array.Length;
            for (int i = 0; i != length; i++)
            {
                object obj2 = array.GetValue(i);
                ObjectMetaData data = new ObjectMetaData {
                    name = "[" + i.ToString() + "]",
                    reference = obj2
                };
                TraceObject(data, obj2);
                meta.children.Add(data);
            }
        }

        public static void TraceClassObject(ObjectMetaData meta, object value)
        {
            if (value == null)
            {
                meta.descript = "null";
            }
            else if (value.GetType().GetInterface("ObjectTracer") != null)
            {
                (value as ObjectTracer).Trace(meta);
            }
            else
            {
                TraceValueObject(meta, value);
            }
        }

        public static void TraceDetails<T>(ObjectMetaData root, string key, T value)
        {
            ObjectMetaData meta = new ObjectMetaData {
                name = key,
                reference = value
            };
            TraceObject(meta, value);
            root.children.Add(meta);
        }

        public static void TraceListDetails<T>(ObjectMetaData root, string key, List<T> value)
        {
            ObjectMetaData meta = new ObjectMetaData {
                name = key,
                reference = value
            };
            TraceListObject<T>(meta, value);
            root.children.Add(meta);
        }

        public static void TraceListObject<T>(ObjectMetaData meta, List<T> value)
        {
            meta.descript = "...";
            int count = value.Count;
            for (int i = 0; i != count; i++)
            {
                T local = value[i];
                ObjectMetaData data = new ObjectMetaData {
                    name = "[" + i.ToString() + "]",
                    reference = local
                };
                TraceObject(data, local);
                meta.children.Add(data);
            }
        }

        public static void TraceObject(ObjectMetaData meta, object value)
        {
            if (value.GetType().IsClass)
            {
                TraceClassObject(meta, value);
            }
            else
            {
                TraceValueObject(meta, value);
            }
        }

        public static void TraceValueObject(ObjectMetaData meta, object value)
        {
            System.Type type = value.GetType();
            if (typeof(Array) == type.BaseType)
            {
                TraceArrayObject(meta, value as Array);
            }
            else if (typeof(List<object>) == type)
            {
                TraceListObject<object>(meta, value as List<object>);
            }
            else if (typeof(List<int>) == type)
            {
                TraceListObject<int>(meta, value as List<int>);
            }
            else
            {
                meta.descript = value.ToString();
            }
        }

        public static Debugger Instance
        {
            get
            {
                if (_debugger == null)
                {
                    _debugger = new Debugger();
                }
                return _debugger;
            }
        }

        [CompilerGenerated]
        private sealed class <CollectStack>c__AnonStorey161
        {
            internal ObjectMetaData root;

            internal void <>m__13F(string key, object value)
            {
                if (value != null)
                {
                    ObjectMetaData meta = new ObjectMetaData {
                        name = key,
                        reference = value
                    };
                    Debugger.TraceObject(meta, value);
                    this.root.children.Add(meta);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <OnInvoke>c__AnonStorey162
        {
            internal int id_from;
            internal int id_to;
            internal string key;

            internal bool <>m__140(BreakPoint e)
            {
                return ((e.key == this.key) && ((e.id_tnsi == this.id_from) || (e.id_tnsi == this.id_to)));
            }
        }

        [CompilerGenerated]
        private sealed class <Resume>c__AnonStorey163
        {
            internal InvokeScene scene;

            internal bool <>m__141(BreakPoint e)
            {
                return ((e.id_tnsi == this.scene.from.id) || (e.id_tnsi == this.scene.to.id));
            }
        }

        [CompilerGenerated]
        private sealed class <SyncBreakPoint>c__AnonStorey15F
        {
            internal int id;

            internal bool <>m__13D(BreakPoint e)
            {
                return (e.id_host == this.id);
            }
        }

        [CompilerGenerated]
        private sealed class <ToggleBreakPoint>c__AnonStorey160
        {
            internal int id;

            internal bool <>m__13E(BreakPoint e)
            {
                return (e.id_host == this.id);
            }
        }

        private enum Status
        {
            RUNNING,
            STEP_INTO
        }
    }
}

