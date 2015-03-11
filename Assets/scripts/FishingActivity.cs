using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class FishingActivity : MonoBehaviour
{
    private Dictionary<int, PositionPlayer> _players = new Dictionary<int, PositionPlayer>();
    private bool CanRelease = true;
    [SerializeField]
    public GameObject[] Fish;
    [SerializeField]
    public GameObject FishBeginEffect;
    [SerializeField]
    public GameObject FishEndEffect;
    public Transform FishingCamear;
    [SerializeField]
    public GameObject FishingEffect;
    [SerializeField]
    public Vector3[] FishingEffectPosition = new Vector3[3];
    [SerializeField]
    public Vector3[] FishingMoveTargets = new Vector3[10];
    [SerializeField]
    public Vector3[] FishingPlayerPositon = new Vector3[3];
    [SerializeField, Range(0f, 20f)]
    public int FishMaxCount = 10;
    [SerializeField]
    public GameObject[] FishOutWeaterEffect;
    private List<FishMove> Fishs = new List<FishMove>();
    public Action<int, bool> OnRelease;
    private int ReleaseIndex = -1;
    [Range(1f, 10000f)]
    public int ShakePro1000 = 300;
    public Vector3 ShakeVerctor = new Vector3(0f, 0.08f, 0f);
    [SerializeField]
    public GameObject Yugan;

    public void Actived(bool flag)
    {
        base.StopAllCoroutines();
        base.StartCoroutine(this.RunFishCreater());
        this.FishingCamear.gameObject.SetActive(true);
        if (this.FishingEffect != null)
        {
            this.FishingEffect.SetActive(false);
        }
    }

    private void Awake()
    {
    }

    public void Clear()
    {
        foreach (int num in this._players.Keys.ToArray<int>())
        {
            this.RemovePosition(num);
        }
    }

    private void EasyTouch_On_TouchDown(Gesture gesture)
    {
        RaycastHit hit;
        if ((UICamera.hoveredObject == null) && Physics.Raycast(this.FishingCamear.GetComponent<Camera>().ScreenPointToRay((Vector3) gesture.position), out hit, 10000f))
        {
            TapIndex component = hit.collider.gameObject.GetComponent<TapIndex>();
            if (component != null)
            {
                this.Release(component.Index);
            }
        }
    }

    private GameObject EffectCreateEffect(GameObject effect, Vector3 target, float destoryDelay = -1)
    {
        if (effect == null)
        {
            return null;
        }
        GameObject obj2 = UnityEngine.Object.Instantiate(effect) as GameObject;
        obj2.SetActive(true);
        obj2.transform.position = target;
        obj2.transform.rotation = Quaternion.identity;
        if (destoryDelay > 0f)
        {
            obj2.AddComponent<AutoDelayDestory>().delay = destoryDelay;
        }
        return obj2;
    }

    internal void EndFish(int cardEntry)
    {
        foreach (KeyValuePair<int, PositionPlayer> pair in this._players.ToList<KeyValuePair<int, PositionPlayer>>())
        {
            if (pair.Value.Entry == cardEntry)
            {
                this.RemovePosition(pair.Key);
            }
        }
    }

    private void OnDestroy()
    {
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.EasyTouch_On_TouchDown);
        Instance = null;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        foreach (Vector3 vector in this.FishingEffectPosition)
        {
            Gizmos.DrawWireCube(vector, (Vector3) (Vector3.one * 0.5f));
        }
        foreach (Vector3 vector2 in this.FishingPlayerPositon)
        {
            Gizmos.DrawWireCube(vector2, Vector3.one);
        }
        Gizmos.color = Color.red;
        foreach (Vector3 vector3 in this.FishingMoveTargets)
        {
            Gizmos.DrawWireCube(vector3, (Vector3) (Vector3.one * 0.3f));
        }
    }

    public void Release(int index)
    {
        Debug.Log("Index:" + index);
        this.ReleaseIndex = index;
    }

    private void RemovePosition(int indexPosition)
    {
        if (this._players.ContainsKey(indexPosition))
        {
            PositionPlayer player = this._players[indexPosition];
            UnityEngine.Object.Destroy(player.Card);
            base.StopCoroutine(player.Ai);
            if (player.Effect != null)
            {
                UnityEngine.Object.Destroy(player.Effect);
            }
            this._players.Remove(indexPosition);
        }
    }

    [DebuggerHidden]
    private IEnumerator RunFishCreater()
    {
        return new <RunFishCreater>c__Iterator99 { <>f__this = this };
    }

    [DebuggerHidden]
    public IEnumerator RunWork(PositionPlayer player)
    {
        return new <RunWork>c__Iterator98 { player = player, <$>player = player, <>f__this = this };
    }

    public void SetCanRelease(bool can)
    {
        this.CanRelease = can;
    }

    public void ShowFishUp(int indexPosition, int cardId, int CardQuility)
    {
        GameObject obj2 = CardPlayer.CreateCardPlayer(cardId, null, CardPlayerStateType.Normal, CardQuility);
        obj2.GetComponent<CardPlayer>().UnequipAll();
        GameObject obj3 = UnityEngine.Object.Instantiate(this.Yugan) as GameObject;
        BoxCollider collider = obj2.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(1f, 2f, 1f);
        collider.center = new Vector3(0f, 1f, 0f);
        obj2.AddComponent<TapIndex>().Index = indexPosition;
        obj3.SetActive(true);
        obj2.GetComponent<HangControler>().AttachToHangPoint(obj3, HangPointType.CommonHand, Vector3.zero);
        Animator component = obj3.GetComponent<Animator>();
        obj2.transform.position = this.FishingPlayerPositon[indexPosition];
        obj2.transform.rotation = Quaternion.LookRotation(this.FishingEffectPosition[indexPosition].ZeroY() - this.FishingPlayerPositon[indexPosition].ZeroY());
        this.RemovePosition(indexPosition);
        PositionPlayer player = new PositionPlayer {
            Card = obj2,
            State = PositionPlayer.FishingState.NORMAL,
            Index = indexPosition,
            Controller = component,
            Entry = cardId
        };
        player.Ai = this.RunWork(player);
        this._players.Add(indexPosition, player);
        base.StartCoroutine(player.Ai);
    }

    private void Start()
    {
        EasyTouch.On_TouchUp += new EasyTouch.TouchUpHandler(this.EasyTouch_On_TouchDown);
        Instance = this;
    }

    private void Update()
    {
    }

    public static FishingActivity Instance
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
    private sealed class <RunFishCreater>c__Iterator99 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal FishingActivity <>f__this;
        internal Vector3 <dir>__5;
        internal FishingActivity.FishMove <f>__1;
        internal FishingActivity.FishMove <fData>__10;
        internal GameObject <fish>__9;
        internal FishingActivity.FishMove <fZero>__6;
        internal int <i>__0;
        internal int <index>__8;
        internal List<int> <indexs>__3;
        internal int <j>__4;
        internal GameObject <obj>__7;
        internal Vector3 <target>__2;

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
                    if (((this.<>f__this.Fish != null) && (this.<>f__this.Fish.Length != 0)) && ((this.<>f__this.FishingMoveTargets != null) && (this.<>f__this.FishingMoveTargets.Length >= 2)))
                    {
                        this.$current = null;
                        this.$PC = 2;
                    }
                    else
                    {
                        this.$current = null;
                        this.$PC = 1;
                    }
                    goto Label_0404;

                case 2:
                    break;

                case 3:
                    this.<i>__0 = 0;
                    while (this.<i>__0 < this.<>f__this.Fishs.Count)
                    {
                        this.<f>__1 = this.<>f__this.Fishs[this.<i>__0];
                        this.<target>__2 = this.<>f__this.FishingMoveTargets[this.<f>__1.target];
                        if (Vector3.Distance(this.<f>__1.Fish.transform.position, this.<target>__2) <= 0.2f)
                        {
                            this.<indexs>__3 = new List<int>();
                            this.<j>__4 = 0;
                            while (this.<j>__4 < this.<>f__this.FishingMoveTargets.Length)
                            {
                                if (this.<j>__4 != this.<f>__1.target)
                                {
                                    this.<indexs>__3.Add(this.<j>__4);
                                }
                                this.<j>__4++;
                            }
                            this.<f>__1.target = GRandomer.RandomList<int>(this.<indexs>__3);
                            this.<f>__1.Speed = UnityEngine.Random.Range((float) 0.1f, (float) 0.5f);
                        }
                        else
                        {
                            this.<dir>__5 = this.<target>__2 - this.<f>__1.Fish.transform.position;
                            this.<f>__1.Fish.transform.forward = Vector3.Lerp(this.<f>__1.Fish.transform.forward, this.<dir>__5.ZeroY(), Time.deltaTime * 5f);
                            Transform transform = this.<f>__1.Fish.transform;
                            transform.position += (Vector3) ((this.<dir>__5.normalized * this.<f>__1.Speed) * Time.deltaTime);
                        }
                        this.<i>__0++;
                    }
                    if ((this.<>f__this.Fishs.Count > 0) && (this.<>f__this.Fishs.Count > this.<>f__this.FishMaxCount))
                    {
                        this.<fZero>__6 = this.<>f__this.Fishs[0];
                        UnityEngine.Object.Destroy(this.<fZero>__6.Fish);
                        this.<>f__this.Fishs.RemoveAt(0);
                    }
                    else if (this.<>f__this.Fishs.Count < this.<>f__this.FishMaxCount)
                    {
                        this.<obj>__7 = GRandomer.RandomArray<GameObject>(this.<>f__this.Fish);
                        this.<index>__8 = GRandomer.RandomMinAndMax(0, this.<>f__this.FishingMoveTargets.Length);
                        this.<fish>__9 = UnityEngine.Object.Instantiate(this.<obj>__7, this.<>f__this.FishingMoveTargets[this.<index>__8], Quaternion.Euler(0f, (float) GRandomer.RandomMinAndMax(0, 360), 0f)) as GameObject;
                        this.<fish>__9.SetActive(true);
                        FishingActivity.FishMove move = new FishingActivity.FishMove {
                            Fish = this.<fish>__9,
                            Speed = 0.2f,
                            target = this.<index>__8
                        };
                        this.<fData>__10 = move;
                        this.<>f__this.Fishs.Add(this.<fData>__10);
                    }
                    break;

                default:
                    goto Label_0402;
            }
            this.$current = null;
            this.$PC = 3;
            goto Label_0404;
            this.$PC = -1;
        Label_0402:
            return false;
        Label_0404:
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

    [CompilerGenerated]
    private sealed class <RunWork>c__Iterator98 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal FishingActivity.PositionPlayer <$>player;
        internal FishingActivity <>f__this;
        internal AnimFSM <an>__1;
        internal GameObject <fish>__6;
        internal GameObject <fishObj>__5;
        internal float <lastShakeTime>__2;
        internal float <lastTime>__3;
        internal FishingActivity.PositionPlayer <obj>__0;
        internal float <shakeTime>__4;
        internal FishingActivity.PositionPlayer player;

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
                    break;

                case 1:
                    this.<>f__this.EffectCreateEffect(this.<>f__this.FishBeginEffect, this.<>f__this.FishingEffectPosition[this.player.Index], 3f);
                    this.<obj>__0.Effect = this.<>f__this.EffectCreateEffect(this.<>f__this.FishingEffect, this.<>f__this.FishingEffectPosition[this.player.Index], -1f);
                    this.<lastShakeTime>__2 = 0f;
                    this.<lastTime>__3 = Time.time + 2f;
                    goto Label_0146;

                case 2:
                    goto Label_0146;

                case 3:
                    if (this.player.IsShake)
                    {
                        this.player.IsShake = false;
                        this.<fishObj>__5 = GRandomer.RandomArray<GameObject>(this.<>f__this.Fish);
                        this.<fish>__6 = UnityEngine.Object.Instantiate(this.<fishObj>__5, this.<>f__this.FishingEffectPosition[this.player.Index], Quaternion.identity) as GameObject;
                        this.<fish>__6.GetComponent<FishPlayControllor>().WeaterOut = true;
                        this.<fish>__6.AddComponent<AutoDelayDestory>().delay = 1.5f;
                        this.<fish>__6.SetActive(true);
                    }
                    if (this.<obj>__0.Effect != null)
                    {
                        UnityEngine.Object.Destroy(this.<obj>__0.Effect);
                    }
                    this.<>f__this.EffectCreateEffect(this.<>f__this.FishEndEffect, this.<>f__this.FishingEffectPosition[this.player.Index], 3f);
                    this.<obj>__0.Effect = null;
                    this.$current = new WaitForSeconds(0.4f);
                    this.$PC = 4;
                    goto Label_0430;

                case 4:
                    if (this.<>f__this.OnRelease != null)
                    {
                        this.<>f__this.OnRelease(this.player.Index, this.player.IsShake);
                    }
                    this.<obj>__0.State = FishingActivity.PositionPlayer.FishingState.NORMAL;
                    this.<>f__this.ReleaseIndex = -1;
                    break;

                default:
                    goto Label_042E;
            }
            this.<obj>__0 = this.player;
            this.<obj>__0.State = FishingActivity.PositionPlayer.FishingState.FISHING;
            this.<an>__1 = this.<obj>__0.Card.GetComponent<AnimFSM>();
            this.<an>__1.PlayAnim("diaoyu", 1f, 0f, false);
            this.<obj>__0.Controller.SetTrigger("shuaigan");
            this.$current = new WaitForSeconds(1f);
            this.$PC = 1;
            goto Label_0430;
        Label_0146:
            if ((this.<lastTime>__3 + 1f) < Time.time)
            {
                this.<lastTime>__3 = Time.time;
                if (!this.player.IsShake)
                {
                    if (GRandomer.RandomPro10000(this.<>f__this.ShakePro1000))
                    {
                        this.player.IsShake = true;
                        this.<shakeTime>__4 = UnityEngine.Random.Range((float) 1.5f, (float) 5f);
                        iTween.ShakePosition(this.<obj>__0.Effect, this.<>f__this.ShakeVerctor, UnityEngine.Random.Range((float) 1f, (float) 2f));
                        this.<lastShakeTime>__2 = this.<shakeTime>__4 + Time.time;
                    }
                }
                else if (this.<lastShakeTime>__2 < Time.time)
                {
                    this.player.IsShake = false;
                }
            }
            if (this.<>f__this.CanRelease && (this.<>f__this.ReleaseIndex == this.player.Index))
            {
                this.<>f__this.ReleaseIndex = -1;
            }
            else
            {
                this.$current = null;
                this.$PC = 2;
                goto Label_0430;
            }
            this.<an>__1.PlayAnim("diaoyu2", 1f, 0f, false);
            this.<obj>__0.Controller.SetTrigger("lagan");
            this.$current = new WaitForSeconds(0.3f);
            this.$PC = 3;
            goto Label_0430;
            this.$PC = -1;
        Label_042E:
            return false;
        Label_0430:
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

    public class FishMove
    {
        public GameObject Fish;
        public float Speed = 5f;
        public int target;
    }

    public class PositionPlayer
    {
        public IEnumerator Ai;
        public GameObject Card;
        public Animator Controller;
        public GameObject Effect;
        public int Index;
        public bool IsShake;

        public int Entry { get; set; }

        public FishingState State { get; set; }

        public enum FishingState
        {
            NORMAL,
            FISHING
        }
    }

    public class TapIndex : MonoBehaviour
    {
        public int Index;
    }
}

