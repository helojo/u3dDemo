using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class MiningAcitivity : MonoBehaviour
{
    private List<MiningData> _cards = new List<MiningData>();
    private Dictionary<int, GameObject> _chest = new Dictionary<int, GameObject>();
    private Dictionary<int, TargetPosition> _position = new Dictionary<int, TargetPosition>();
    [CompilerGenerated]
    private static Func<MiningData, int> <>f__am$cache12;
    [CompilerGenerated]
    private static Func<KeyValuePair<int, GameObject>, int> <>f__am$cache13;
    [CompilerGenerated]
    private static Func<KeyValuePair<int, TargetPosition>, int> <>f__am$cache14;
    [CompilerGenerated]
    private static Func<MiningData, int> <>f__am$cache15;
    [CompilerGenerated]
    private static Func<KeyValuePair<int, GameObject>, int> <>f__am$cache16;
    [Range(-180f, 180f), SerializeField]
    public float Angle = -15f;
    [SerializeField]
    public GameObject[] BaoShi;
    [SerializeField]
    public GameObject ChestFx;
    [Range(0.1f, 10f)]
    public float ChestMoveSpeed = 2f;
    [SerializeField]
    public GameObject ChuiZi;
    private GameObject currentFlag;
    private int CurrentTapIndex = -1;
    [SerializeField]
    public GameObject Flag;
    private float lastTime;
    [SerializeField]
    public Camera MiningCamera;
    [SerializeField]
    public MiningPosition[] MiningPositions;
    public System.Action OnTapChest;
    [Range(1f, 10000f)]
    public int RandPro = 300;
    [SerializeField]
    public GameObject TapChestEffect;

    private void Awake()
    {
        Instance = this;
    }

    public void BeginMinig(int cardId, int quility)
    {
        GameObject obj2 = CardPlayer.CreateCardPlayer(cardId, null, CardPlayerStateType.Normal, quility);
        obj2.GetComponent<CardPlayer>().UnequipAll();
        GameObject obj3 = UnityEngine.Object.Instantiate(this.ChuiZi) as GameObject;
        obj3.SetActive(true);
        obj2.GetComponent<HangControler>().AttachToHangPoint(obj3, HangPointType.MiningHand, Vector3.zero);
        int index = this.GetIndex();
        TargetPosition position = this._position[index];
        obj2.transform.position = position.IndexPos;
        MiningData item = new MiningData {
            Card = obj2,
            Index = index,
            State = MiningData.MiningState.Mining,
            Entry = cardId
        };
        this._cards.Add(item);
        item.Runer = this.RunAI(item);
        base.StartCoroutine(item.Runer);
    }

    public void Clear()
    {
        this.CurrentTapIndex = -1;
        base.StopAllCoroutines();
        foreach (MiningData data in this._cards)
        {
            UnityEngine.Object.Destroy(data.Card);
        }
        this._cards.Clear();
    }

    private void CreateChest()
    {
        <CreateChest>c__AnonStorey24F storeyf = new <CreateChest>c__AnonStorey24F();
        if ((this._chest.Count <= (this._cards.Count + 2)) || (this._chest.Count <= 5))
        {
            if (<>f__am$cache12 == null)
            {
                <>f__am$cache12 = t => t.Index;
            }
            storeyf.useIndex = this._cards.Select<MiningData, int>(<>f__am$cache12).ToArray<int>();
            if (<>f__am$cache13 == null)
            {
                <>f__am$cache13 = t => t.Key;
            }
            storeyf.haveIndex = this._chest.Select<KeyValuePair<int, GameObject>, int>(<>f__am$cache13).ToArray<int>();
            if (<>f__am$cache14 == null)
            {
                <>f__am$cache14 = t => t.Key;
            }
            int[] arrary = this._position.Where<KeyValuePair<int, TargetPosition>>(new Func<KeyValuePair<int, TargetPosition>, bool>(storeyf.<>m__527)).Select<KeyValuePair<int, TargetPosition>, int>(<>f__am$cache14).ToArray<int>();
            if (arrary.Length > 0)
            {
                int key = GRandomer.RandomArray<int>(arrary);
                GameObject original = GRandomer.RandomArray<GameObject>(this.BaoShi);
                if (original != null)
                {
                    GameObject obj3 = UnityEngine.Object.Instantiate(original, this._position[key].Target + new Vector3(0f, -1f, 0f), Quaternion.Euler(0f, (float) GRandomer.RandomMinAndMax(0, 360), 0f)) as GameObject;
                    obj3.SetActive(true);
                    MoveToPositon positon = obj3.AddComponent<MoveToPositon>();
                    positon.speed = this.ChestMoveSpeed;
                    positon.Totarget = this._position[key].Target;
                    this._chest.Add(key, obj3);
                    BoxCollider collider = obj3.AddComponent<BoxCollider>();
                    obj3.AddComponent<MineIndex>().Index = key;
                    collider.center = new Vector3(0f, 0.5f, 0f);
                    collider.size = Vector3.one;
                }
            }
        }
    }

    private void DestoryChest(int index)
    {
        GameObject obj2;
        if (this._chest.TryGetValue(index, out obj2))
        {
            obj2.AddComponent<AutoDelayDestory>().delay = 2f;
            obj2.AddComponent<MaterialFSM>().StartAlphaChange(0.5f, 0f);
            UnityEngine.Object.Destroy(obj2.GetComponent<Collider>());
            this._chest.Remove(index);
        }
        this.lastTime = Time.time;
    }

    private void EasyTouch_On_TouchUp(Gesture gesture)
    {
        RaycastHit hit;
        if ((((UICamera.hoveredObject == null) && Physics.Raycast(this.MiningCamera.ScreenPointToRay((Vector3) gesture.position), out hit, 10000f)) && (this._cards.Count != 0)) && (this.CurrentTapIndex <= 0))
        {
            MineIndex component = hit.collider.transform.GetComponent<MineIndex>();
            if (component != null)
            {
                this.TapIndex(component.Index);
                if (this.TapChestEffect != null)
                {
                    GameObject obj2 = UnityEngine.Object.Instantiate(this.TapChestEffect, hit.collider.transform.position, Quaternion.identity) as GameObject;
                    obj2.AddComponent<AutoDelayDestory>().delay = 2f;
                    obj2.SetActive(true);
                }
            }
        }
    }

    internal void EndMinig(int cardEntry)
    {
        foreach (MiningData data in this._cards.ToList<MiningData>())
        {
            if (data.Entry == cardEntry)
            {
                base.StopCoroutine(data.Runer);
                UnityEngine.Object.Destroy(data.Card);
                this._cards.Remove(data);
                if (this.currentFlag != null)
                {
                    this.CurrentTapIndex = -1;
                    UnityEngine.Object.Destroy(this.currentFlag);
                    this.currentFlag = null;
                }
            }
        }
    }

    private int GetIndex()
    {
        <GetIndex>c__AnonStorey250 storey = new <GetIndex>c__AnonStorey250();
        if (<>f__am$cache15 == null)
        {
            <>f__am$cache15 = t => t.Index;
        }
        storey.useIndexs = this._cards.Select<MiningData, int>(<>f__am$cache15).ToArray<int>();
        if (<>f__am$cache16 == null)
        {
            <>f__am$cache16 = t => t.Key;
        }
        return GRandomer.RandomArray<int>(this._chest.Where<KeyValuePair<int, GameObject>>(new Func<KeyValuePair<int, GameObject>, bool>(storey.<>m__52A)).Select<KeyValuePair<int, GameObject>, int>(<>f__am$cache16).ToArray<int>());
    }

    private void OnDestroy()
    {
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.EasyTouch_On_TouchUp);
        Instance = null;
    }

    public void OnDrawGizmosSelected()
    {
        if (this.MiningPositions != null)
        {
            foreach (MiningPosition position in this.MiningPositions)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(position.Pos, (Vector3) (Vector3.one * 0.5f));
                if (position.MiningPositions != null)
                {
                    Gizmos.color = Color.white;
                    foreach (Vector3 vector in position.MiningPositions)
                    {
                        Gizmos.DrawWireCube(vector, Vector3.one);
                    }
                }
            }
        }
    }

    private void PlayAn(GameObject obj, string an)
    {
        AnimFSM component = obj.GetComponent<AnimFSM>();
        if (component != null)
        {
            component.PlayAnim(an, 1f, 0f, false);
        }
    }

    [DebuggerHidden]
    private IEnumerator RunAI(MiningData data)
    {
        return new <RunAI>c__Iterator9A { data = data, <$>data = data, <>f__this = this };
    }

    public void SetActived(bool flag)
    {
        this.MiningCamera.gameObject.SetActive(flag);
    }

    public void Start()
    {
        EasyTouch.On_TouchUp += new EasyTouch.TouchUpHandler(this.EasyTouch_On_TouchUp);
        if (this.MiningPositions == null)
        {
            this.MiningPositions = new MiningPosition[3];
            for (int i = 0; i < this.MiningPositions.Length; i++)
            {
                MiningPosition position2 = new MiningPosition {
                    Pos = base.transform.position
                };
                position2.MiningPositions = new Vector3[] { base.transform.position, base.transform.position, base.transform.position };
                this.MiningPositions[i] = position2;
            }
        }
        int key = 0;
        foreach (MiningPosition position in this.MiningPositions)
        {
            foreach (Vector3 vector in position.MiningPositions)
            {
                key++;
                TargetPosition position3 = new TargetPosition {
                    Target = position.Pos,
                    IndexPos = vector
                };
                this._position.Add(key, position3);
            }
        }
        int num5 = 0;
        while (num5++ < 4)
        {
            this.CreateChest();
        }
    }

    public void TapIndex(int index)
    {
        if (this.CurrentTapIndex < 0)
        {
            this.CurrentTapIndex = index;
            MiningData data = null;
            foreach (MiningData data2 in this._cards)
            {
                if (data2.Index == this.CurrentTapIndex)
                {
                    data = data2;
                }
            }
            if (data == null)
            {
                data = GRandomer.RandomList<MiningData>(this._cards);
            }
            if (data != null)
            {
                if (this.Flag != null)
                {
                    this.currentFlag = UnityEngine.Object.Instantiate(this.Flag, this._position[index].Target + Vector3.up, Quaternion.identity) as GameObject;
                    this.currentFlag.SetActive(true);
                }
                data.State = MiningData.MiningState.ProcessMine;
                base.StopCoroutine(data.Runer);
                data.Index = index;
                data.Runer = this.RunAI(data);
                base.StartCoroutine(data.Runer);
            }
        }
    }

    public void Update()
    {
        if ((this.lastTime + 1f) < Time.time)
        {
            this.lastTime = Time.time;
            this.CreateChest();
        }
    }

    public static MiningAcitivity Instance
    {
        [CompilerGenerated]
        get
        {
            return <Instance>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            <Instance>k__BackingField = value;
        }
    }

    [CompilerGenerated]
    private sealed class <CreateChest>c__AnonStorey24F
    {
        internal int[] haveIndex;
        internal int[] useIndex;

        internal bool <>m__527(KeyValuePair<int, MiningAcitivity.TargetPosition> t)
        {
            return !(this.useIndex.Contains<int>(t.Key) || this.haveIndex.Contains<int>(t.Key));
        }
    }

    [CompilerGenerated]
    private sealed class <GetIndex>c__AnonStorey250
    {
        internal int[] useIndexs;

        internal bool <>m__52A(KeyValuePair<int, GameObject> t)
        {
            return !this.useIndexs.Contains<int>(t.Key);
        }
    }

    [CompilerGenerated]
    private sealed class <RunAI>c__Iterator9A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MiningAcitivity.MiningData <$>data;
        internal MiningAcitivity <>f__this;
        internal Vector3 <dir>__2;
        internal Vector3 <dir>__7;
        internal GameObject <ef>__5;
        internal GameObject <ef>__9;
        internal GameObject <effect>__10;
        internal GameObject <effect>__4;
        internal AnimFSM <fsm>__0;
        internal int <i>__8;
        internal int <index>__3;
        internal MiningAcitivity.TargetPosition <moveTarget>__6;
        internal int <NextIndex>__11;
        internal MiningAcitivity.TargetPosition <target>__1;
        internal MiningAcitivity.MiningData data;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<fsm>__0 = this.data.Card.GetComponent<AnimFSM>();
                    break;

                case 1:
                    goto Label_00B3;

                case 2:
                case 3:
                    if (GRandomer.RandomPro10000(this.<>f__this.RandPro))
                    {
                        this.<index>__3 = this.<>f__this.GetIndex();
                        if (this.<>f__this._chest.ContainsKey(this.<index>__3))
                        {
                            if (this.<>f__this.ChestFx != null)
                            {
                                this.<effect>__4 = UnityEngine.Object.Instantiate(this.<>f__this.ChestFx, this.<>f__this._position[this.data.Index].Target, Quaternion.identity) as GameObject;
                                this.<effect>__4.SetActive(true);
                                this.<effect>__4.AddComponent<AutoDelayDestory>().delay = 3f;
                            }
                            this.<>f__this.DestoryChest(this.data.Index);
                            this.data.Index = this.<index>__3;
                            this.data.State = MiningAcitivity.MiningData.MiningState.Walk;
                            goto Label_0937;
                        }
                    }
                    this.<ef>__5 = UnityEngine.Object.Instantiate(this.<>f__this.TapChestEffect, this.<>f__this._position[this.data.Index].Target + new Vector3(UnityEngine.Random.Range((float) -0.4f, (float) 0.5f), UnityEngine.Random.Range((float) 0f, (float) 0.5f), UnityEngine.Random.Range((float) -0.4f, (float) 0.5f)), Quaternion.identity) as GameObject;
                    this.<ef>__5.hideFlags = HideFlags.HideInHierarchy;
                    this.<ef>__5.AddComponent<AutoDelayDestory>().delay = 0.8f;
                    this.<ef>__5.SetActive(true);
                    this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 0.8f, (float) 1.5f));
                    this.$PC = 3;
                    goto Label_0958;

                case 4:
                    goto Label_04CC;

                case 5:
                    goto Label_06A8;

                case 6:
                    goto Label_08AD;

                case 7:
                    this.<NextIndex>__11 = this.<>f__this.GetIndex();
                    if (!this.<>f__this._chest.ContainsKey(this.<NextIndex>__11))
                    {
                        goto Label_08AD;
                    }
                    this.<>f__this.DestoryChest(this.data.Index);
                    this.data.Index = this.<NextIndex>__11;
                    this.data.State = MiningAcitivity.MiningData.MiningState.Walk;
                    goto Label_0937;

                case 8:
                    break;
                    this.$PC = -1;
                    goto Label_0956;

                default:
                    goto Label_0956;
            }
            switch (this.data.State)
            {
                case MiningAcitivity.MiningData.MiningState.Walk:
                    this.<target>__1 = this.<>f__this._position[this.data.Index];
                    this.<fsm>__0.PlayAnim("move", 1f, 0f, false);
                    break;

                case MiningAcitivity.MiningData.MiningState.Mining:
                    this.<fsm>__0.PlayAnim("caikuang", 1f, 0f, false);
                    this.data.Card.transform.rotation = Quaternion.LookRotation((this.<>f__this._position[this.data.Index].Target - this.data.Card.transform.position).ZeroY()) * Quaternion.Euler(0f, this.<>f__this.Angle, 0f);
                    this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 0.8f, (float) 1.5f));
                    this.$PC = 2;
                    goto Label_0958;

                case MiningAcitivity.MiningData.MiningState.ProcessMine:
                    this.<moveTarget>__6 = this.<>f__this._position[this.data.Index];
                    if (Vector3.Distance(this.data.Card.transform.position.ZeroY(), this.<moveTarget>__6.IndexPos.ZeroY()) <= 0.2f)
                    {
                        goto Label_05EC;
                    }
                    this.<fsm>__0.PlayAnim("move", 1f, 0f, false);
                    goto Label_04CC;

                default:
                    goto Label_0937;
            }
        Label_00B3:
            this.<dir>__2 = this.<target>__1.IndexPos - this.data.Card.transform.position;
            if (Vector3.Distance(this.data.Card.transform.position.ZeroY(), this.<target>__1.IndexPos.ZeroY()) <= 0.2f)
            {
                this.data.State = MiningAcitivity.MiningData.MiningState.Mining;
                this.<fsm>__0.StopCurAnim("move");
                goto Label_0937;
            }
            Transform transform = this.data.Card.transform;
            transform.position += (Vector3) ((this.<dir>__2.normalized * 5f) * Time.deltaTime);
            this.data.Card.transform.rotation = Quaternion.LookRotation((this.<target>__1.IndexPos - this.data.Card.transform.position).ZeroY());
            this.$current = null;
            this.$PC = 1;
            goto Label_0958;
        Label_04CC:
            this.<dir>__7 = this.<moveTarget>__6.IndexPos - this.data.Card.transform.position;
            if (Vector3.Distance(this.data.Card.transform.position.ZeroY(), this.<moveTarget>__6.IndexPos.ZeroY()) <= 0.2f)
            {
                this.data.State = MiningAcitivity.MiningData.MiningState.Mining;
                this.<fsm>__0.StopCurAnim("move");
            }
            else
            {
                Transform transform2 = this.data.Card.transform;
                transform2.position += (Vector3) ((this.<dir>__7.normalized * 5f) * Time.deltaTime);
                this.data.Card.transform.rotation = Quaternion.LookRotation((this.<moveTarget>__6.IndexPos - this.data.Card.transform.position).ZeroY());
                this.$current = null;
                this.$PC = 4;
                goto Label_0958;
            }
        Label_05EC:
            this.<fsm>__0.PlayAnim("caikuang", 1f, 0f, false);
            this.data.Card.transform.rotation = Quaternion.LookRotation((this.<>f__this._position[this.data.Index].Target - this.data.Card.transform.position).ZeroY()) * Quaternion.Euler(0f, this.<>f__this.Angle, 0f);
            this.<i>__8 = 0;
            while (this.<i>__8++ <= 5)
            {
                this.$current = new WaitForSeconds(0.7f);
                this.$PC = 5;
                goto Label_0958;
            Label_06A8:
                this.<ef>__9 = UnityEngine.Object.Instantiate(this.<>f__this.TapChestEffect, this.<>f__this._position[this.data.Index].Target + new Vector3(UnityEngine.Random.Range((float) -0.4f, (float) 0.5f), UnityEngine.Random.Range((float) 0f, (float) 0.5f), UnityEngine.Random.Range((float) -0.4f, (float) 0.5f)), Quaternion.identity) as GameObject;
                this.<ef>__9.hideFlags = HideFlags.HideInHierarchy;
                this.<ef>__9.AddComponent<AutoDelayDestory>().delay = 0.8f;
                this.<ef>__9.SetActive(true);
            }
            if (this.<>f__this.ChestFx != null)
            {
                this.<effect>__10 = UnityEngine.Object.Instantiate(this.<>f__this.ChestFx, this.<>f__this._position[this.data.Index].Target, Quaternion.identity) as GameObject;
                this.<effect>__10.SetActive(true);
                this.<effect>__10.AddComponent<AutoDelayDestory>().delay = 3f;
            }
            this.<fsm>__0.PlayStandAnim();
            this.data.Card.transform.rotation = Quaternion.LookRotation((this.<>f__this.MiningCamera.transform.position - this.data.Card.transform.position).ZeroY());
            if (this.<>f__this.currentFlag != null)
            {
                UnityEngine.Object.Destroy(this.<>f__this.currentFlag);
                this.<>f__this.currentFlag = null;
            }
            if (this.<>f__this.OnTapChest != null)
            {
                this.<>f__this.CurrentTapIndex = -1;
                this.<>f__this.OnTapChest();
            }
            this.$current = new WaitForSeconds(2f);
            this.$PC = 6;
            goto Label_0958;
        Label_08AD:
            this.$current = new WaitForSeconds(0.3f);
            this.$PC = 7;
            goto Label_0958;
        Label_0937:
            this.$current = null;
            this.$PC = 8;
            goto Label_0958;
        Label_0956:
            return false;
        Label_0958:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    public class MineIndex : MonoBehaviour
    {
        public int Index;
    }

    public class MiningData
    {
        public GameObject Card;
        public int Entry;
        public int Index;
        public IEnumerator Runer;
        public MiningState State;

        public enum MiningState
        {
            Walk,
            Mining,
            ProcessMine
        }
    }

    [Serializable]
    public class MiningPosition
    {
        [SerializeField]
        public Vector3[] MiningPositions;
        [SerializeField]
        public Vector3 Pos;
    }

    public class TargetPosition
    {
        public Vector3 IndexPos { get; set; }

        public Vector3 Target { get; set; }
    }
}

