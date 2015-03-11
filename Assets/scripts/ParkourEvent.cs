using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParkourEvent
{
    [CompilerGenerated]
    private static Action<PropInfo> <>f__am$cacheA;
    public int baoxiangCount;
    public List<PropInfo> canFlyProp = new List<PropInfo>();
    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();
    public float gameTime;
    public int goldCoinCount;
    public bool isWin;
    private static ParkourEvent m_pEvents;
    public int missCount;
    private Dictionary<PropType, PropProperty> propsPool = new Dictionary<PropType, PropProperty>();
    private PropProperty runningProp;

    public void AddEventToProp(PropInfo info)
    {
        switch (info.type)
        {
            case PropType.JINGBI:
                info.enterTriggerEvent = new PropInfo.PropEvent(this.GoldCoin);
                info.enterTriggerDistance = 1f;
                break;

            case PropType.NAIPING:
                info.enterTriggerEvent = new PropInfo.PropEvent(this.NaiPing);
                info.enterTriggerDistance = 1f;
                break;

            case PropType.BAOXIANG:
                info.tDistanceEvent = new PropInfo.PropEvent(this.OpenBaoXiang);
                info.enterTriggerEvent = new PropInfo.PropEvent(this.BaoXiang);
                info.enterTriggerDistance = 1.5f;
                break;

            case PropType.JIASU:
                info.colideDistance = info.triggerDistance;
                info.coliderEvent = new PropInfo.PropEvent(this.CheckJiaSu);
                break;

            case PropType.HUOQIU:
                info.tDistanceEvent = new PropInfo.PropEvent(this.HuoQiu);
                break;

            case PropType.DIANQIU:
                info.colideDistance = 1f;
                info.coliderEvent = new PropInfo.PropEvent(this.DianQiu);
                break;

            case PropType.HUOQIANG:
                info.colideDistance = info.triggerDistance;
                info.coliderEvent = new PropInfo.PropEvent(this.HuoQiang);
                info.isCheckLengthEvent = true;
                break;

            case PropType.HUODUI:
                info.enterTriggerEvent = new PropInfo.PropEvent(this.HuoDui);
                info.enterTriggerDistance = 1f;
                break;

            case PropType.MONSTER:
                info.tDistanceEvent = new PropInfo.PropEvent(this.SetPlayerAttack);
                info.isCheckLengthEvent = true;
                info.colideDistance = 3f;
                info.coliderEvent = new PropInfo.PropEvent(this.MonsterAttack);
                info.propAnim["daiji"].wrapMode = WrapMode.Loop;
                info.propAnim.Play("daiji");
                break;

            case PropType.ANIMLUZHANG:
                info.tDistanceEvent = new PropInfo.PropEvent(this.AnimLuZhang);
                break;

            case PropType.END:
                info.tDistanceEvent = new PropInfo.PropEvent(this.WinAnim);
                break;
        }
    }

    private bool AddProp(PropType type, System.Action startAction, System.Action updateAction, System.Action endAction, int propEntry)
    {
        PropProperty property = null;
        float durationtime = ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(propEntry).durationtime;
        float num2 = ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(propEntry).value;
        if (this.propsPool.TryGetValue(type, out property))
        {
            property.duration = durationtime;
            Debug.LogWarning("Reset prop duration..." + type);
            return false;
        }
        PropProperty property2 = new PropProperty(startAction, updateAction, endAction, durationtime, num2);
        this.propsPool.Add(type, property2);
        Debug.LogWarning("Add new prop... : " + type);
        if (property2.startAction != null)
        {
            property2.startAction();
        }
        ParkourManager._instance.StartCoroutine(this.Timer(this.propsPool[type]));
        return true;
    }

    public void AnimLuZhang(PropInfo info)
    {
        info.propAnim.Play();
    }

    public void BaoXiang(PropInfo info)
    {
        this.PlayEffect("pk_tx_11", true);
        this.baoxiangCount++;
        info.gameObject.SetActive(false);
        PaokuInPanel._instance.UpdateBoxAndGold(this.baoxiangCount, this.goldCoinCount);
    }

    private bool CareatBaoXiang(PropInfo prop)
    {
        int num = UnityEngine.Random.Range(0, 10);
        if (this.baoxiangCount >= ConfigMgr.getInstance().getByEntry<guildrun_config>(ParkourInit._instance.chapteIndex).max_box_num)
        {
            return false;
        }
        if (num > 4)
        {
            return false;
        }
        GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(BattleGlobal.ParkourPropPrefab + "DJ_baoxiang01_18"), prop.transform.position, Quaternion.identity) as GameObject;
        this.OpenBaoXiang(obj2.GetComponent<PropInfo>());
        obj2.GetComponent<PropInfo>().StartCoroutine(this.PropFly(obj2.GetComponent<PropInfo>()));
        return true;
    }

    public void CheckCollisionObj(Collider col)
    {
        PropInfo component = col.GetComponent<PropInfo>();
        if ((component != null) && col.enabled)
        {
            col.enabled = false;
            PropType type = component.type;
            switch (type)
            {
                case PropType.HUDUN:
                    this.AddProp(PropType.HUDUN, new System.Action(this.StartHuDun), null, new System.Action(this.EndHuDun), component.propEntry);
                    component.gameObject.SetActive(false);
                    return;

                case PropType.ENDFLY:
                    this.m_characterCtrl.fly = false;
                    return;

                case PropType.DIANXIAN:
                    this.DianXian(component);
                    return;

                case PropType.CITIE:
                    this.AddProp(PropType.CITIE, new System.Action(this.StartCiTie), null, new System.Action(this.EndCiTie), component.propEntry);
                    component.gameObject.SetActive(false);
                    return;

                case PropType.FLY:
                    SoundManager.mInstance.PlaySFX(ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(component.propEntry).sound_name);
                    this.m_characterCtrl.CheckFly();
                    component.gameObject.SetActive(false);
                    return;

                case PropType.LUZHANG:
                    this.PropHit(component);
                    return;
            }
            if (type == PropType.ANIMLUZHANG)
            {
                this.PropHit(component);
            }
        }
    }

    public void CheckJiaSu(PropInfo info)
    {
        this.AddProp(PropType.JIASU, new System.Action(this.StartJiaSu), null, new System.Action(this.EndJiaSu), info.propEntry);
    }

    public void CheckPropFly(PropInfo info)
    {
        info.StartCoroutine(this.PropFly(info));
    }

    public void DDJSkill()
    {
        this.PlayEffect("pk_tx_16", true);
        if (!this.m_characterCtrl.fly)
        {
            this.m_characterCtrl.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.SKILL), 1f, 0.01f, false);
        }
        this.AddProp(PropType.HUDUN, new System.Action(this.StartHuDun), null, new System.Action(this.DDJSkillEnd), 0x16);
    }

    private void DDJSkillEnd()
    {
        this.PlayEffect("pk_tx_16", false);
        this.EndHuDun();
        this.propsPool.Remove(PropType.HUDUN);
    }

    public void DeadAnim()
    {
        this.m_characterCtrl.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.DEAD), 1f, 0.1f, false);
        ParkourManager._instance.parkourCamera.transform.parent.animation.Play("PK_DeadAnim");
    }

    public void DestoryParkourEvent()
    {
        this.effects.Clear();
        this.propsPool.Clear();
        m_pEvents = null;
        Debug.LogWarning("Event dic is clear...");
    }

    public void DianQiu(PropInfo info)
    {
        this.PlayEffect("pk_tx_25", true);
        Debug.Log(info.type);
        this.PropHit(info);
    }

    public void DianXian(PropInfo info)
    {
        this.PlayEffect("pk_tx_26", true);
        Debug.Log(info.type);
        this.PropHit(info);
    }

    public void DXJSkill()
    {
        this.PlayEffect("pk_tx_16", true);
        if (!this.m_characterCtrl.fly)
        {
            this.m_characterCtrl.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.SKILL), 1f, 0.01f, false);
        }
        this.PlayEffect("pk_tx_23", true);
        this.AddProp(PropType.NAIPING, null, new System.Action(this.UpdateNaiPing), new System.Action(this.DXJSkillEnd), 0x17);
    }

    private void DXJSkillEnd()
    {
        this.PlayEffect("pk_tx_16", false);
        this.EndNaiping();
        this.propsPool.Remove(PropType.NAIPING);
    }

    public void EndCiTie()
    {
        Debug.Log("End Ci Tie..");
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = delegate (PropInfo p) {
                if (((p.type == PropType.JINGBI) || (p.type == PropType.HUDUN)) || (p.type == PropType.BAOXIANG))
                {
                    p.tDistanceEvent = null;
                }
            };
        }
        this.canFlyProp.ForEach(<>f__am$cacheA);
        this.propsPool.Remove(PropType.CITIE);
        this.PlayEffect("pk_tx_06", false);
    }

    public void EndHuDun()
    {
        this.PlayEffect("pk_tx_09", false);
        this.PlayEffect("pk_tx_32", true);
        char[] separator = new char[] { '|' };
        SoundManager.mInstance.PlaySFX(ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(0x16).sound_name.Split(separator)[0]);
        this.propsPool.Remove(PropType.HUDUN);
    }

    public void EndHuoQiang()
    {
        Debug.Log("huo qiang Over... ");
        this.PlayEffect("pk_tx_12", false);
        this.propsPool.Remove(PropType.HUOQIANG);
    }

    private void EndJiaSu()
    {
        this.PlayEffect("pk_tx_28", false);
        this.m_characterCtrl.fastSpeed = 1f;
        this.propsPool.Remove(PropType.JIASU);
        Debug.Log("Jia su End...");
    }

    public void EndNaiping()
    {
        this.PlayEffect("pk_tx_23", false);
        this.propsPool.Remove(PropType.NAIPING);
    }

    public void GameReward()
    {
        ParkourManager._instance.GameStart = false;
        this.m_characterCtrl.isAction = false;
        ParkourManager._instance.StartCoroutine(this.SendOverInofToServer());
    }

    private bool GetProp(PropType type)
    {
        PropProperty property = null;
        return this.propsPool.TryGetValue(type, out property);
    }

    public void GoldCoin(PropInfo prop)
    {
        this.PlayEffect("pk_tx_11", true);
        prop.gameObject.SetActive(false);
        this.goldCoinCount++;
        if (SoundManager.mInstance != null)
        {
            SoundManager.mInstance.PlaySFX(ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(prop.propEntry).sound_name);
        }
        if (PaokuInPanel._instance != null)
        {
            PaokuInPanel._instance.UpdateBoxAndGold(this.baoxiangCount, this.goldCoinCount);
        }
    }

    private void Hit()
    {
        if (this.m_characterCtrl.parkouerInfo.GetHP >= 0)
        {
            this.m_characterCtrl.isAction = false;
            this.m_characterCtrl.isHit = true;
            this.missCount++;
            if (this.m_characterCtrl.GetAction(CharacterType.JUMP) && (this.m_characterCtrl.jumpTime > (this.m_characterCtrl.jumpAnimTime * 0.5f)))
            {
                this.m_characterCtrl.jumpTime = this.m_characterCtrl.jumpAnimTime * 0.5f;
            }
            this.m_characterCtrl.nextActionKey = CharacterType.None;
            this.m_characterCtrl.StopMove();
            this.m_characterCtrl.animFsm.PlayAnimAndAddFinishEvent(ParkourManager.AnimNmae(CharacterType.HIT), 1f, 0f, false, new System.Action(this.ResatCharacterType));
        }
    }

    public void HuoDui(PropInfo info)
    {
        this.AddProp(PropType.HUOQIANG, new System.Action(this.StartHuoQiang), new System.Action(this.HuoQiangDebuff), new System.Action(this.EndHuoQiang), info.propEntry);
    }

    public void HuoQiang(PropInfo info)
    {
        this.AddProp(PropType.HUOQIANG, new System.Action(this.StartHuoQiang), new System.Action(this.HuoQiangDebuff), new System.Action(this.EndHuoQiang), info.propEntry);
    }

    public void HuoQiangDebuff()
    {
        this.m_characterCtrl.parkouerInfo.UpdateHP(-((int) (this.propsPool[PropType.HUOQIANG].value * 0.5f)));
    }

    public void HuoQiu(PropInfo info)
    {
        info.propAnim.Play();
        info.triggerDistance = 1f;
        info.colideDistance = 2f;
        info.coliderEvent = new PropInfo.PropEvent(this.HuoQiuHit);
    }

    public void HuoQiuHit(PropInfo info)
    {
        this.PlayEffect("pk_tx_13", true);
        Debug.Log(info.type);
        info.gameObject.SetActive(false);
        this.PropHit(info);
    }

    public void Init()
    {
        for (int i = 0; i < ParkourManager._instance.effectObjs.Length; i++)
        {
            GameObject obj2;
            if (!this.effects.TryGetValue(ParkourManager._instance.effectObjs[i].name, out obj2))
            {
                this.effects.Add(ParkourManager._instance.effectObjs[i].name, ParkourManager._instance.effectObjs[i]);
            }
        }
        this.goldCoinCount = 0;
        this.baoxiangCount = 0;
        this.missCount = 0;
        this.gameTime = 0f;
        this.isWin = false;
    }

    public void MonsterAttack(PropInfo info)
    {
        if (this.m_characterCtrl.isAttack)
        {
            info.propAnim.Play("siwang");
            this.PlayEffect("pk_tx_31", true);
            info.StartCoroutine(this.MonsterDeadAnim(info));
        }
        else if (((Mathf.Abs((float) (ParkourManager._instance.cCtrl.characterTr.position.y - info.transform.position.y)) < 3f) && (Mathf.Abs((float) (ParkourManager._instance.cCtrl.characterTr.position.x - info.transform.position.x)) < 1f)) && (this.m_characterCtrl.thisT.position.z < info.transform.position.z))
        {
            info.propAnim.Play("gongji1");
            this.PropHit(info);
        }
        this.m_characterCtrl.isAttack = false;
        this.m_characterCtrl.isCanAttack = false;
    }

    [DebuggerHidden]
    public IEnumerator MonsterDeadAnim(PropInfo info)
    {
        return new <MonsterDeadAnim>c__IteratorC0 { info = info, <$>info = info, <>f__this = this };
    }

    public void MTSkill()
    {
        this.PlayEffect("pk_tx_16", true);
        if (!this.m_characterCtrl.fly)
        {
            this.m_characterCtrl.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.SKILL), 1f, 0.01f, false);
        }
        this.AddProp(PropType.CITIE, new System.Action(this.StartCiTie), null, new System.Action(this.MTSkillEnd), 0x12);
    }

    private void MTSkillEnd()
    {
        this.PlayEffect("pk_tx_16", false);
        this.EndCiTie();
        this.propsPool.Remove(PropType.CITIE);
    }

    public void NaiPing(PropInfo info)
    {
        info.gameObject.SetActive(false);
        this.PlayEffect("pk_tx_23", true);
        this.AddProp(PropType.NAIPING, null, new System.Action(this.UpdateNaiPing), new System.Action(this.EndNaiping), info.propEntry);
    }

    public void OpenBaoXiang(PropInfo info)
    {
        info.propAnim["kaiqi"].wrapMode = WrapMode.Once;
        info.propAnim["kaiqi"].speed = 2f;
        info.propAnim.Play("kaiqi");
    }

    public bool PlayEffect(string name, bool isActive)
    {
        GameObject obj2;
        if (!this.effects.TryGetValue(name, out obj2))
        {
            return false;
        }
        if (!isActive)
        {
            if (obj2.GetComponent<ParticleSystem>() != null)
            {
                obj2.GetComponent<ParticleSystem>().Clear();
            }
            obj2.SetActive(false);
            Debug.Log("Effect is Close...");
            return true;
        }
        if (!obj2.activeSelf)
        {
            obj2.SetActive(true);
        }
        if (obj2.GetComponent<ParticleSystem>() != null)
        {
            obj2.GetComponent<ParticleSystem>().Play();
        }
        else if (obj2.GetComponent<Animation>() != null)
        {
            if (!obj2.GetComponent<Animation>().isPlaying)
            {
                obj2.GetComponent<Animation>().Play();
            }
        }
        else if (obj2.GetComponent<Animator>() != null)
        {
            obj2.GetComponent<Animator>().Play(0);
        }
        else
        {
            return false;
        }
        return true;
    }

    [DebuggerHidden]
    private IEnumerator PropFly(PropInfo prop)
    {
        return new <PropFly>c__IteratorC1 { prop = prop, <$>prop = prop, <>f__this = this };
    }

    public void PropHit(PropInfo info)
    {
        info.gameObject.collider.enabled = false;
        if (!this.GetProp(PropType.WUDI))
        {
            if (this.GetProp(PropType.HUDUN))
            {
                this.propsPool[PropType.HUDUN].duration = 0f;
            }
            else
            {
                if (this.GetProp(PropType.JIASU))
                {
                    this.propsPool[PropType.JIASU].duration = 0f;
                    this.m_characterCtrl.fastSpeed = 1f;
                }
                this.PlayEffect("pk_tx_01", true);
                this.m_characterCtrl.parkouerInfo.UpdateHP(-((int) ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(info.propEntry).value));
                if (this.m_characterCtrl.parkouerInfo.GetHP > 0)
                {
                    this.Hit();
                }
            }
        }
    }

    private void ResatCharacterType()
    {
        this.m_characterCtrl.StartCoroutine(this.WaitCharacterEventOver(0.2f));
    }

    [DebuggerHidden]
    private IEnumerator SendOverInofToServer()
    {
        return new <SendOverInofToServer>c__IteratorBF { <>f__this = this };
    }

    public void SetPlayerAttack(PropInfo info)
    {
        if (Mathf.Abs((float) (ParkourManager._instance.cCtrl.characterTr.position.x - info.transform.position.x)) <= 1f)
        {
            info.isCheckLengthEvent = false;
            this.m_characterCtrl.isCanAttack = true;
            info.ps.Play();
        }
    }

    public void StartCiTie()
    {
        this.canFlyProp.ForEach(delegate (PropInfo p) {
            if (((p.type == PropType.JINGBI) || (p.type == PropType.HUDUN)) || (p.type == PropType.BAOXIANG))
            {
                p.tDistanceEvent = new PropInfo.PropEvent(this.CheckPropFly);
                p.triggerDistance = 10f;
            }
        });
        SoundManager.mInstance.PlaySFX(ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(0x12).sound_name);
        this.PlayEffect("pk_tx_06", true);
    }

    public void StartHuDun()
    {
        char[] separator = new char[] { '|' };
        SoundManager.mInstance.PlaySFX(ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(0x16).sound_name.Split(separator)[0]);
        this.PlayEffect("pk_tx_09", true);
    }

    public void StartHuoQiang()
    {
        this.PlayEffect("pk_tx_12", true);
        this.missCount++;
    }

    public void StartJiaSu()
    {
        this.PlayEffect("pk_tx_28", true);
        this.m_characterCtrl.fastSpeed = this.propsPool[PropType.JIASU].value;
    }

    [DebuggerHidden]
    private IEnumerator Timer(PropProperty prop)
    {
        return new <Timer>c__IteratorC3 { prop = prop, <$>prop = prop };
    }

    public void UpdateNaiPing()
    {
        this.m_characterCtrl.parkouerInfo.UpdateHP((int) (this.propsPool[PropType.NAIPING].value * 0.5f));
    }

    public void UseSkill()
    {
        this.m_characterCtrl.parkouerInfo.skill();
    }

    [DebuggerHidden]
    private IEnumerator WaitCharacterEventOver(float time)
    {
        return new <WaitCharacterEventOver>c__IteratorC2 { time = time, <$>time = time, <>f__this = this };
    }

    public void WinAnim(PropInfo info)
    {
        this.isWin = true;
        this.m_characterCtrl.animFsm.PlayAnim(ParkourManager.AnimNmae(CharacterType.STOPE), 1f, 0.1f, true);
        SoundManager.mInstance.PlaySFX(ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(info.propEntry).sound_name);
        this.GameReward();
    }

    public static ParkourEvent _instance
    {
        get
        {
            if (m_pEvents == null)
            {
                return (m_pEvents = new ParkourEvent());
            }
            return m_pEvents;
        }
    }

    private CharacterCtrl m_characterCtrl
    {
        get
        {
            return ParkourManager._instance.cCtrl;
        }
    }

    [CompilerGenerated]
    private sealed class <MonsterDeadAnim>c__IteratorC0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal PropInfo <$>info;
        internal ParkourEvent <>f__this;
        internal float <propAnimTime>__1;
        internal Transform <propT>__0;
        internal PropInfo info;

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
                    if (SoundManager.mInstance != null)
                    {
                        SoundManager.mInstance.PlaySFX("battle_monsterDead");
                    }
                    this.<propT>__0 = this.info.transform;
                    this.<propAnimTime>__1 = 0f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_0161;
            }
            while (this.<propAnimTime>__1 < 1f)
            {
                this.<propT>__0.position = Vector3.Lerp(this.<propT>__0.position, this.<propT>__0.position + new Vector3(0f, 0f, 2f), this.<propAnimTime>__1);
                this.<propT>__0.position = Vector3.Lerp(this.<propT>__0.position, new Vector3(this.<propT>__0.position.x, ParkourManager._instance.monsterDeadCurve.Evaluate(this.<propAnimTime>__1), this.<propT>__0.position.z), this.<propAnimTime>__1);
                this.<propAnimTime>__1 += Time.deltaTime * 2f;
                this.$current = 0;
                this.$PC = 1;
                return true;
            }
            this.<>f__this.CareatBaoXiang(this.info);
            this.$PC = -1;
        Label_0161:
            return false;
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
    private sealed class <PropFly>c__IteratorC1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal PropInfo <$>prop;
        internal ParkourEvent <>f__this;
        internal float <propAnimTime>__1;
        internal Transform <propT>__0;
        internal PropInfo prop;

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
                    this.<propT>__0 = this.prop.transform;
                    this.<propAnimTime>__1 = 0f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_021F;
            }
            if (this.<propAnimTime>__1 < 1f)
            {
                this.<propT>__0.position = Vector3.Lerp(this.<propT>__0.position, ParkourManager._instance.cCtrl.characterTr.position + new Vector3(0f, 0f, 0.5f), this.<propAnimTime>__1);
                this.<propT>__0.position = Vector3.Lerp(this.<propT>__0.position, new Vector3(this.<propT>__0.position.x, ParkourManager._instance.cCtrl.characterTr.position.y + ParkourManager._instance.propFlyCurve.Evaluate(this.<propAnimTime>__1), this.<propT>__0.position.z), this.<propAnimTime>__1);
                this.<propT>__0.localScale = Vector3.Lerp(this.<propT>__0.localScale, new Vector3(0.2f, 0.2f, 0.2f), this.<propAnimTime>__1);
                this.<propAnimTime>__1 += Time.deltaTime * 2f;
                this.$current = 0;
                this.$PC = 1;
                return true;
            }
            if (this.prop.type == PropType.HUDUN)
            {
                this.<>f__this.AddProp(PropType.HUDUN, new System.Action(this.<>f__this.StartHuDun), null, new System.Action(this.<>f__this.EndHuDun), this.prop.propEntry);
            }
            else if (this.prop.type == PropType.BAOXIANG)
            {
                Debug.Log("baoxiang");
            }
            else if (this.prop.type == PropType.JINGBI)
            {
                this.<>f__this.GoldCoin(this.prop);
            }
            this.$PC = -1;
        Label_021F:
            return false;
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
    private sealed class <SendOverInofToServer>c__IteratorBF : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ParkourEvent <>f__this;
        internal GuildParkour <g>__0;

        internal void <>m__5F4(GUIEntity obj)
        {
            ResultPaokuPanel panel = (ResultPaokuPanel) obj;
            if (panel != null)
            {
                panel.InitDataByResult(this.<g>__0);
            }
        }

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
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<g>__0 = new GuildParkour();
                    this.<g>__0.result = this.<>f__this.isWin;
                    this.<g>__0.box = this.<>f__this.baoxiangCount;
                    this.<g>__0.coin = this.<>f__this.goldCoinCount;
                    this.<g>__0.time = (uint) this.<>f__this.gameTime;
                    this.<g>__0.trap_count = this.<>f__this.missCount;
                    GUIMgr.Instance.CloseUniqueGUIEntity("PaokuInPanel");
                    GUIMgr.Instance.DoModelGUI("ResultPaokuPanel", new Action<GUIEntity>(this.<>m__5F4), null);
                    this.$PC = -1;
                    break;
            }
            return false;
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
    private sealed class <Timer>c__IteratorC3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ParkourEvent.PropProperty <$>prop;
        internal ParkourEvent.PropProperty prop;

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
                case 1:
                    if (this.prop.duration > 0f)
                    {
                        this.prop.duration -= 0.5f;
                        if (this.prop.updateAction != null)
                        {
                            this.prop.updateAction();
                        }
                        this.$current = new WaitForSeconds(0.5f);
                        this.$PC = 1;
                        return true;
                    }
                    this.prop.endAction();
                    this.$PC = -1;
                    break;
            }
            return false;
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
    private sealed class <WaitCharacterEventOver>c__IteratorC2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>time;
        internal ParkourEvent <>f__this;
        internal float time;

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
                    this.$current = new WaitForSeconds(this.time);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.m_characterCtrl.CheckMove();
                    this.<>f__this.m_characterCtrl.isAction = true;
                    this.<>f__this.m_characterCtrl.isHit = false;
                    Debug.LogWarning("Event Over...");
                    this.$PC = -1;
                    break;
            }
            return false;
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

    private class PropProperty
    {
        public float duration;
        public System.Action endAction;
        public System.Action startAction;
        public CharacterCtrl targetC;
        public System.Action updateAction;
        public float value;

        public PropProperty(System.Action _startAction, System.Action _updateAction, System.Action _endAction, float _duration, float _value)
        {
            this.startAction = _startAction;
            this.updateAction = _updateAction;
            this.endAction = _endAction;
            this.duration = _duration;
            this.value = _value;
            this.targetC = ParkourManager._instance.cCtrl;
        }
    }
}

