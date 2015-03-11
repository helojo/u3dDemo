namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Fragment
    {
        [CompilerGenerated]
        private static Action<Concentrator> <>f__am$cacheC;
        [CompilerGenerated]
        private static Action<Concentrator> <>f__am$cacheD;
        private int alloc_count;
        public List<Concentrator> concent_pool = new List<Concentrator>();
        public int dispatch_count;
        public bool finished;
        public Intent global_context;
        public IOContext io_context = new HybridIOContext();
        public Concentrator pivot_concent;
        public string res_group = "fate";
        public string res_id = string.Empty;
        public Concentrator self_buff_remove;
        public Intent stack_context = new Intent();
        public List<Transition> transition_pool = new List<Transition>();

        public void ApplyProperties(Dictionary<string, string> prop_dic)
        {
            foreach (Property property in this.GetConcentrators<Property>())
            {
                string str = string.Empty;
                if (prop_dic.TryGetValue(property.Key, out str))
                {
                    property.Value = str;
                }
            }
        }

        private bool CheckAssetFile(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            FileStream stream = null;
            try
            {
                stream = File.Create(path);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return false;
        }

        private void CheckResGroup(string group)
        {
            string path = GetAppPath() + "/Resources/" + group;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void ConcreateConcentrator<T>(Vector2 pos) where T: Concentrator
        {
            Concentrator concentrator = Factory.CreateConcentrator(typeof(T).Name);
            if (concentrator != null)
            {
                concentrator.root_fragment = this;
                concentrator.tag_position = pos;
                concentrator.Concreate();
                concentrator.RegisterSerializableSegment();
            }
        }

        private void ConcreateFragment<T>() where T: Concentrator
        {
            this.alloc_count = 0;
            this.concent_pool.Clear();
            this.transition_pool.Clear();
            this.ConcreateConcentrator<T>(new Vector2(450f, 350f));
        }

        public string DebugKey()
        {
            return (this.res_id + "_" + this.res_group);
        }

        public void Deploy(Intent context)
        {
            this.global_context = context;
        }

        public void DispachRemoveBuff(Intent context)
        {
            if (!this.finished && (this.self_buff_remove != null))
            {
                this.self_buff_remove.Excude(context);
            }
        }

        public void Dispatch(Intent context)
        {
            if (!this.finished && (this.pivot_concent != null))
            {
                this.pivot_concent.Excude(context);
                this.dispatch_count++;
            }
        }

        public void Dispatch(string key_proxy, Intent context, Fragment host)
        {
            <Dispatch>c__AnonStorey166 storey = new <Dispatch>c__AnonStorey166 {
                key_proxy = key_proxy
            };
            if (!this.finished)
            {
                Concentrator concentrator = this.concent_pool.Find(new Predicate<Concentrator>(storey.<>m__146));
                if (concentrator != null)
                {
                    this.global_context = host.global_context;
                    concentrator.Excude(context);
                    this.dispatch_count = host.dispatch_count;
                }
            }
        }

        public bool Ensure()
        {
            return !this.finished;
        }

        public Concentrator FindConcentratorByID(int id)
        {
            <FindConcentratorByID>c__AnonStorey165 storey = new <FindConcentratorByID>c__AnonStorey165 {
                id = id
            };
            return this.concent_pool.Find(new Predicate<Concentrator>(storey.<>m__143));
        }

        public Transition FindTransitionByID(int id)
        {
            <FindTransitionByID>c__AnonStorey164 storey = new <FindTransitionByID>c__AnonStorey164 {
                id = id
            };
            return this.transition_pool.Find(new Predicate<Transition>(storey.<>m__142));
        }

        public static string GetAppPath()
        {
            return Application.dataPath;
        }

        public List<T> GetConcentrators<T>() where T: Concentrator
        {
            List<T> list = new List<T>();
            foreach (Concentrator concentrator in this.concent_pool)
            {
                if (typeof(T) == concentrator.GetType())
                {
                    list.Add(concentrator as T);
                }
            }
            return list;
        }

        public static Fragment Load(string res_group, string res_id)
        {
            Fragment fragment = new Fragment {
                res_group = res_group,
                res_id = res_id
            };
            fragment.LoadResource();
            return fragment;
        }

        public void Load<T>(string res_group, string res_id) where T: Concentrator
        {
            this.res_group = res_group;
            this.res_id = res_id;
            string[] textArray1 = new string[] { GetAppPath(), "/Resources/", res_group, "/", res_id, ".bytes" };
            string path = string.Concat(textArray1);
            this.CheckResGroup(res_group);
            if (!this.CheckAssetFile(path))
            {
                this.ConcreateFragment<T>();
                this.Save();
            }
            else
            {
                this.LoadResource();
            }
        }

        private void LoadFragment(IOContext io_context)
        {
            this.transition_pool.Clear();
            this.concent_pool.Clear();
            io_context.DeserializationInt32(out this.alloc_count);
            int ovalue = 0;
            io_context.DeserializationInt32(out ovalue);
            for (int i = 0; i != ovalue; i++)
            {
                string str = null;
                io_context.DeserializationString(out str);
                Concentrator item = Factory.CreateConcentrator(str);
                if (item == null)
                {
                    throw new UnityException("deserialization concentrator failed by type = " + str);
                }
                item.root_fragment = this;
                item.RegisterSerializableSegment();
                item.Deserialization(io_context);
                this.concent_pool.Add(item);
            }
            for (int j = 0; j != this.transition_pool.Count; j++)
            {
                this.transition_pool[j].Import(io_context);
            }
            if (!io_context.EOF())
            {
            }
        }

        public void LoadResource()
        {
            byte[] bytes = null;
            TextAsset asset = BundleMgr.Instance.LoadResource(this.res_group + "/" + this.res_id, ".bytes", typeof(TextAsset)) as TextAsset;
            if (null != asset)
            {
                bytes = asset.bytes;
            }
            if (bytes == null)
            {
                Debug.LogWarning("failed to load fate resource named " + this.res_id);
            }
            else
            {
                this.io_context.Reset();
                this.io_context.Write(bytes);
                this.io_context.SeekToBegin();
                this.LoadFragment(this.io_context);
            }
        }

        public void RegisterTransition(Transition tnsi)
        {
            tnsi.root_fragment = this;
            tnsi.id = this.alloc_count++;
            this.transition_pool.Add(tnsi);
        }

        public void RemoveConcentrator(Concentrator concent)
        {
            if (concent == this.pivot_concent)
            {
                throw new UnityException("can not remove the fucking pivot concentrator");
            }
            concent.RemoveAllPorts();
            this.concent_pool.Remove(concent);
        }

        public void Reset()
        {
            foreach (Concentrator concentrator in this.concent_pool)
            {
                if (concentrator != null)
                {
                    concentrator.Reset();
                }
            }
            this.finished = false;
            this.dispatch_count = 0;
            this.global_context = null;
            this.stack_context.Clear();
        }

        public void Save()
        {
            this.io_context.Reset();
            this.SaveFragment(this.io_context);
            FileStream fs = null;
            try
            {
                fs = File.Open(Application.dataPath + "/Resources/" + this.res_group + "/" + this.res_id + ".bytes", FileMode.OpenOrCreate);
                this.io_context.WriteToFS(fs);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        private void SaveFragment(IOContext io_context)
        {
            io_context.SerializationInt32(this.alloc_count);
            int count = this.concent_pool.Count;
            io_context.SerializationInt32(count);
            for (int i = 0; i != count; i++)
            {
                this.concent_pool[i].Serialization(io_context);
            }
            for (int j = 0; j != this.transition_pool.Count; j++)
            {
                this.transition_pool[j].Export(io_context);
            }
        }

        public void Terminate()
        {
            this.finished = true;
        }

        public void Tick()
        {
            if (<>f__am$cacheC == null)
            {
                <>f__am$cacheC = e => e.Tick();
            }
            this.concent_pool.ForEach(<>f__am$cacheC);
        }

        public void Update()
        {
            if (!this.finished)
            {
                if (<>f__am$cacheD == null)
                {
                    <>f__am$cacheD = e => e.Update();
                }
                this.concent_pool.ForEach(<>f__am$cacheD);
            }
        }

        [CompilerGenerated]
        private sealed class <Dispatch>c__AnonStorey166
        {
            internal string key_proxy;

            internal bool <>m__146(Concentrator con)
            {
                if (typeof(TransmitProxy) != con.GetType())
                {
                    return false;
                }
                TransmitProxy proxy = con as TransmitProxy;
                return (this.key_proxy == proxy.key);
            }
        }

        [CompilerGenerated]
        private sealed class <FindConcentratorByID>c__AnonStorey165
        {
            internal int id;

            internal bool <>m__143(Concentrator e)
            {
                return (e.id == this.id);
            }
        }

        [CompilerGenerated]
        private sealed class <FindTransitionByID>c__AnonStorey164
        {
            internal int id;

            internal bool <>m__142(Transition e)
            {
                return (e.id == this.id);
            }
        }
    }
}

