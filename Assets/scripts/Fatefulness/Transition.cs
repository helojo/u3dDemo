namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Transition
    {
        public Direction direction = Direction.both;
        public List<Transition> export_tnsis = new List<Transition>();
        public int id = -1;
        public string key = string.Empty;
        public Concentrator lanch_concent;
        public Fragment root_fragment;

        public bool Accessed()
        {
            return (this.export_tnsis.Count > 0);
        }

        public virtual void Deserialization(IOContext io_context)
        {
            io_context.DeserializationInt32(out this.id);
            int ovalue = 2;
            io_context.DeserializationInt32(out ovalue);
            this.direction = (Direction) ovalue;
        }

        public void Export(IOContext io_context)
        {
            int count = this.export_tnsis.Count;
            io_context.SerializationInt32(count);
            for (int i = 0; i != count; i++)
            {
                Transition transition = this.export_tnsis[i];
                io_context.SerializationInt32(transition.id);
            }
        }

        public void Import(IOContext io_context)
        {
            this.export_tnsis.Clear();
            int ovalue = 0;
            io_context.DeserializationInt32(out ovalue);
            for (int i = 0; i != ovalue; i++)
            {
                int num3 = -1;
                io_context.DeserializationInt32(out num3);
                Transition item = this.root_fragment.FindTransitionByID(num3);
                if (item == null)
                {
                    throw new UnityException("can not import the transition id = " + num3);
                }
                if (this.export_tnsis.Contains(item))
                {
                    throw new UnityException("the transition is already exsit id = " + num3);
                }
                this.export_tnsis.Add(item);
            }
        }

        public bool MakeLower(Transition target)
        {
            int index = this.export_tnsis.IndexOf(target);
            if (index >= (this.export_tnsis.Count - 1))
            {
                return false;
            }
            this.export_tnsis[index] = this.export_tnsis[index + 1];
            this.export_tnsis[index + 1] = target;
            return true;
        }

        public bool MakeUpper(Transition target)
        {
            int index = this.export_tnsis.IndexOf(target);
            if (index <= 0)
            {
                return false;
            }
            this.export_tnsis[index] = this.export_tnsis[index - 1];
            this.export_tnsis[index - 1] = target;
            return true;
        }

        public virtual void Receive(Intent intent, bool review)
        {
            if ((((!review ? Direction.output : Direction.input) != this.direction) && (this.lanch_concent != null)) && ((this.root_fragment != null) && (this.root_fragment.global_context != null)))
            {
                intent.PutString(IntentDecl.port, this.key);
                this.lanch_concent.Excude(intent);
            }
        }

        public void Replace(Transition src, Transition dst)
        {
            int index = this.export_tnsis.IndexOf(src);
            if (index != -1)
            {
                this.export_tnsis.Insert(index, dst);
                this.export_tnsis.Remove(src);
            }
        }

        public void Request(Intent intent)
        {
            int count = this.export_tnsis.Count;
            for (int i = 0; i != count; i++)
            {
                Transition to = this.export_tnsis[i];
                if (to != null)
                {
                    Debugger.Instance.OnInvoke(this, to, intent);
                    to.Receive(intent, false);
                }
            }
        }

        public void RequestReset(Intent intent)
        {
            int count = this.export_tnsis.Count;
            for (int i = 0; i != count; i++)
            {
                Transition transition = this.export_tnsis[i];
                if (transition != null)
                {
                    transition.lanch_concent.Reset();
                }
            }
        }

        public void Review(Intent intent)
        {
            Transition from = null;
            List<Transition> list = this.import_tnsis;
            if (list.Count > 0)
            {
                from = list[0];
            }
            if (from != null)
            {
                from.Receive(intent, true);
                Debugger.Instance.OnInvoke(from, this, intent);
            }
        }

        public virtual void Serialization(IOContext io_context)
        {
            string name = base.GetType().Name;
            io_context.SerializationString(name);
            io_context.SerializationInt32(this.id);
            io_context.SerializationInt32((int) this.direction);
        }

        public void SetProperty(string key, object value)
        {
            int count = this.export_tnsis.Count;
            for (int i = 0; i != count; i++)
            {
                Transition transition = this.export_tnsis[i];
                if (transition != null)
                {
                    Concentrator concentrator = transition.lanch_concent;
                    if (concentrator != null)
                    {
                        concentrator.SetSegmentValue(key, value);
                    }
                }
            }
            List<Transition> list = this.import_tnsis;
            count = list.Count;
            for (int j = 0; j != count; j++)
            {
                Transition transition2 = list[j];
                if (transition2 != null)
                {
                    Concentrator concentrator2 = transition2.lanch_concent;
                    if (concentrator2 != null)
                    {
                        concentrator2.SetSegmentValue(key, value);
                    }
                }
            }
        }

        public List<Transition> import_tnsis
        {
            get
            {
                List<Transition> list = new List<Transition>();
                if (this.root_fragment != null)
                {
                    foreach (Transition transition in this.root_fragment.transition_pool)
                    {
                        if ((transition != this) && transition.export_tnsis.Contains(this))
                        {
                            list.Add(transition);
                        }
                    }
                }
                return list;
            }
        }

        public enum Direction
        {
            input,
            output,
            both
        }
    }
}

