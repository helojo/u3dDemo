using FastBuf;
using HutongGames.PlayMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CreateRoleName : GUIEntity
{
    public GameObject _blGroup;
    public GameObject _InputGroup;
    public GameObject _lmGroup;
    private int curSelectIndex = -1;
    private List<GameObject> FakeList = new List<GameObject>();
    private bool isShowing;
    private int mCamp;
    public List<role_select_config> mCurrRoleList;
    private bool mIsSendReq;
    private static readonly float moveTime = 1f;
    private string[] OK_Sound = new string[] { "dub_mt_yes", "dub_lr_yes", "dub_sm_yes", "dub_fz_yes", "dub_dxj_yes", "dub_dzh_yes" };
    private string[] Select_Sound = new string[] { "dub_mt_select", "dub_lr_select", "dub_sm_select", "dub_fz_select", "dub_dxj_select", "dub_dzh_select" };

    private void CheckQQlogin()
    {
        if (GameDefine.getInstance().IsThirdPlatform())
        {
            base.transform.FindChild("Group/Input").GetComponent<UIInput>().text = GameDefine.getInstance().TencentLoginNickName;
        }
    }

    public void ClearAll3DRole()
    {
        for (int i = 0; i < this.FakeList.Count; i++)
        {
            if (this.FakeList[i] != null)
            {
                UnityEngine.Object.DestroyObject(this.FakeList[i]);
            }
        }
    }

    private GameObject CreateHeroInfo(int cardEntry, int posIdx)
    {
        GameObject obj2 = CardPlayer.CreateCardPlayer(cardEntry, null, CardPlayerStateType.Normal, 0);
        if (obj2 != null)
        {
            Transform transform = GameObject.Find("PlayerGroup/Pos" + posIdx).transform;
            obj2.transform.parent = transform;
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            obj2.transform.position = transform.position;
            obj2.transform.rotation = transform.rotation;
            obj2.name = posIdx.ToString();
            if (obj2.transform.GetComponent<BoxCollider>() == null)
            {
                BoxCollider collider = obj2.AddComponent<BoxCollider>();
                collider.center = new Vector3(0f, 0.6f, 0f);
                collider.size = new Vector3(1.3f, 1.3f, 1.3f);
            }
        }
        this.PlayStandAnim(obj2);
        return obj2;
    }

    private void CreateHeroName()
    {
        UIInput component = base.transform.FindChild("Group/Input").GetComponent<UIInput>();
        if (component.text.Trim() == string.Empty)
        {
            TipsDiag.SetText("人物昵称不能为空");
        }
        else if (this.curSelectIndex == -1)
        {
            TipsDiag.SetText("您还未选择角色！");
        }
        else if (ConfigMgr.getInstance().GetMaskWord(component.text.Trim()).Contains("*"))
        {
            TipsDiag.SetText("输入文字中有敏感字！");
        }
        else
        {
            base.StartCoroutine(this.StartCreate(component.text.Trim()));
        }
    }

    private void CreateRole()
    {
        for (int i = 0; i < this.mCurrRoleList.Count; i++)
        {
            role_select_config _config = this.mCurrRoleList[i];
            if (ConfigMgr.getInstance().getByEntry<card_config>(_config.card_entry) != null)
            {
                this.FakeList[i] = this.CreateHeroInfo(_config.card_entry, i);
                GameObject obj2 = GameObject.Find("PlayerGroup/Target0");
                this.FakeList[i].transform.position = obj2.transform.position;
                this.FakeList[i].transform.rotation = obj2.transform.rotation;
            }
        }
        this.curSelectIndex = 0;
    }

    private int GetCurSelectRole()
    {
        return this.curSelectIndex;
    }

    private List<role_select_config> GetHeroList(int camp)
    {
        List<role_select_config> list = new List<role_select_config>();
        IEnumerator enumerator = ConfigMgr.getInstance().getList<role_select_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                role_select_config current = (role_select_config) enumerator.Current;
                if (current.camp == camp)
                {
                    list.Add(current);
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        return list;
    }

    private void On_SimpleTap(Gesture gesture)
    {
        RaycastHit hit;
        if (((base.gameObject.GetComponent<UIPanel>().alpha == 1f) && ((gesture.fingerIndex == 0) && (Camera.main != null))) && ((Physics.Raycast(Camera.main.ScreenPointToRay((Vector3) gesture.position), out hit, 100f) && (UICamera.hoveredObject == null)) && (hit.collider.gameObject.name.Length == 1)))
        {
            int index = StrParser.ParseDecInt(hit.collider.gameObject.name);
            SoundManager.mInstance.PlaySFX("sound_select");
            this.SelectHero(index);
        }
    }

    private void OnClickRandom()
    {
        base.transform.FindChild("Group/Input").GetComponent<UIInput>().text = CommonFunc.GetRamdomName();
    }

    public override void OnDestroy()
    {
        this.ClearAll3DRole();
        EasyTouch.On_SimpleTap -= new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
    }

    public override void OnInitialize()
    {
        for (int i = 0; i < 3; i++)
        {
            this.FakeList.Add(null);
        }
        base.OnInitialize();
        if (GUIMgr.Instance.Root.GetComponent<PlayMakerFSM>() != null)
        {
            FsmInt num2 = FsmVariables.GlobalVariables.FindFsmInt("CurrSelectCamp");
            Debug.Log(num2.Value + "--------------------");
            this.SetCamp(num2.Value);
        }
        this.CheckQQlogin();
        EasyTouch.On_SimpleTap += new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        ObjectManager.Instance.Tick();
    }

    private void PlayStandAnim(GameObject obj)
    {
        AnimFSM component = obj.GetComponent<AnimFSM>();
        if (!component.PlayAnim("kaohuo", 1f, 0f, false))
        {
            component.PlayAnim(BattleGlobal.StandAnimName, 1f, 0f, false);
        }
    }

    private void SelectHero(int index)
    {
        if (!this.isShowing)
        {
            this.isShowing = true;
            Debug.Log(index);
            if (index != this.curSelectIndex)
            {
                if (this.curSelectIndex >= 0)
                {
                    base.StartCoroutine(this.StartHeroMoveBack(this.curSelectIndex));
                }
                this.SetCurSelectRole(index);
            }
            string skillName = this.mCurrRoleList[index].skill_name;
            base.StartCoroutine(this.ShowSkill(this.FakeList[index], skillName, index));
        }
    }

    private void SetCamp(int type)
    {
        this.mCamp = type;
        this._lmGroup.SetActive(type != 1);
        this._blGroup.SetActive(type == 1);
        this._InputGroup.transform.localPosition = (type != 1) ? Vector3.zero : new Vector3(160f, 0f, 0f);
        this.mCurrRoleList = this.GetHeroList(type);
        this.CreateRole();
        TweenAlpha component = base.GetComponent<TweenAlpha>();
        component.delay = (type == 1) ? 2.5f : 4f;
        component.enabled = true;
        if (type == 1)
        {
            SoundManager.mInstance.PlayMusic("create_lianmeng");
        }
        else if (type == 2)
        {
            SoundManager.mInstance.PlayMusic("create_buluo");
        }
    }

    public void SetCurSelectRole(int entry)
    {
        if (base.transform.FindChild("Group/Input").GetComponent<UIInput>().text == string.Empty)
        {
            this.OnClickRandom();
        }
        this.curSelectIndex = entry;
        role_select_config _config = this.mCurrRoleList[entry];
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(_config.card_entry);
        if (_config2 != null)
        {
            if (this.mCamp == 2)
            {
                this._lmGroup.transform.FindChild("Name").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(10), _config2.name);
                this._lmGroup.transform.FindChild("Desc").GetComponent<UILabel>().text = _config.description;
            }
            else
            {
                this._blGroup.transform.FindChild("Name").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(10), _config2.name);
                this._blGroup.transform.FindChild("Desc").GetComponent<UILabel>().text = _config.description;
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator ShowSkill(GameObject obj, string skillName, int index)
    {
        return new <ShowSkill>c__Iterator88 { obj = obj, index = index, skillName = skillName, <$>obj = obj, <$>index = index, <$>skillName = skillName, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartCreate(string text)
    {
        return new <StartCreate>c__Iterator89 { text = text, <$>text = text, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartHeroMoveBack(int index)
    {
        return new <StartHeroMoveBack>c__Iterator87 { index = index, <$>index = index, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <ShowSkill>c__Iterator88 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>index;
        internal GameObject <$>obj;
        internal string <$>skillName;
        internal CreateRoleName <>f__this;
        internal int <beijiIndex>__3;
        internal List<GameObject> <defenderList>__2;
        internal GameObject <denfendObj>__6;
        internal GameObject <group>__0;
        internal Transform <moveTargetTrans>__1;
        internal role_select_config <rsc>__8;
        internal RealTimeShowEffect <showEffect>__7;
        internal string <targetName>__4;
        internal Transform <trans>__5;
        internal int index;
        internal GameObject obj;
        internal string skillName;

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
                {
                    this.obj.GetComponent<CardPlayer>().ChangeState(CardPlayerStateType.Battle);
                    if (Camera.main.GetComponent<Animator>() != null)
                    {
                        Camera.main.GetComponent<Animator>().enabled = false;
                    }
                    this.<group>__0 = GameObject.Find("PlayerGroup");
                    this.<moveTargetTrans>__1 = this.<group>__0.transform.FindChild("Target" + this.index.ToString());
                    if (this.<moveTargetTrans>__1 == null)
                    {
                        this.<moveTargetTrans>__1 = this.<group>__0.transform.FindChild("Target");
                    }
                    if (Vector3.Distance(this.<moveTargetTrans>__1.position, this.obj.transform.position) <= 0.5f)
                    {
                        break;
                    }
                    this.obj.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.MoveAnimName, 1f, 0f, false);
                    object[] args = new object[] { "position", this.<moveTargetTrans>__1.position, "time", CreateRoleName.moveTime, "easetype", iTween.EaseType.linear, "orienttopath", true };
                    iTween.MoveTo(this.obj, iTween.Hash(args));
                    this.$current = new WaitForSeconds(CreateRoleName.moveTime + 0.1f);
                    this.$PC = 1;
                    goto Label_03AE;
                }
                case 1:
                    this.obj.GetComponent<AnimFSM>().StopCurAnim(BattleGlobal.MoveAnimName);
                    this.obj.transform.rotation = this.<moveTargetTrans>__1.rotation;
                    break;

                case 2:
                    this.<rsc>__8 = this.<>f__this.mCurrRoleList[this.<>f__this.curSelectIndex];
                    if ((this.<rsc>__8.entry >= 0) && (this.<rsc>__8.entry < this.<>f__this.Select_Sound.Length))
                    {
                        SoundManager.mInstance.SFXVolume = 2f;
                        SoundManager.mInstance.PlaySFX(this.<>f__this.Select_Sound[this.<rsc>__8.entry]);
                        SoundManager.mInstance.SFXVolume = 1f;
                    }
                    this.$current = new WaitForSeconds(1.5f);
                    this.$PC = 3;
                    goto Label_03AE;

                case 3:
                    this.obj.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.FightStandAnimName, 1f, 0f, false);
                    this.<>f__this.isShowing = false;
                    this.$PC = -1;
                    goto Label_03AC;

                default:
                    goto Label_03AC;
            }
            this.<defenderList>__2 = new List<GameObject>();
            this.<beijiIndex>__3 = 0;
            while (this.<beijiIndex>__3 < 5)
            {
                this.<targetName>__4 = "beiji" + this.index.ToString() + "_" + this.<beijiIndex>__3.ToString();
                this.<trans>__5 = this.<group>__0.transform.FindChild(this.<targetName>__4);
                if (this.<trans>__5 != null)
                {
                    this.<denfendObj>__6 = this.<trans>__5.gameObject;
                    this.<denfendObj>__6.AddComponent<HangControler>();
                    this.<defenderList>__2.Add(this.<denfendObj>__6);
                }
                this.<beijiIndex>__3++;
            }
            this.<showEffect>__7 = RealTimeShowEffect.CreateNewShowEffect(this.skillName, this.obj, this.<defenderList>__2[0]);
            this.<showEffect>__7.ToDoStart();
            this.$current = new WaitForSeconds(1.5f);
            this.$PC = 2;
            goto Label_03AE;
        Label_03AC:
            return false;
        Label_03AE:
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
    private sealed class <StartCreate>c__Iterator89 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>text;
        internal CreateRoleName <>f__this;
        internal role_select_config <rsc>__0;
        internal string text;

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
                    this.<rsc>__0 = this.<>f__this.mCurrRoleList[this.<>f__this.curSelectIndex];
                    if ((this.<rsc>__0.entry >= 0) && (this.<rsc>__0.entry < this.<>f__this.Select_Sound.Length))
                    {
                        SoundManager.mInstance.SFXVolume = 2f;
                        SoundManager.mInstance.PlaySFX(this.<>f__this.OK_Sound[this.<rsc>__0.entry]);
                        SoundManager.mInstance.SFXVolume = 1f;
                    }
                    GUIMgr.Instance.Lock();
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 1;
                    return true;

                case 1:
                    SocketMgr.Instance.RequestRegister(GameDefine.getInstance().lastAccountName, GameDefine.getInstance().lastAcctId, GameDefine.getInstance().clientPlatformType, GameDefine.getInstance().device_mac, this.text, 1, this.<>f__this.mCamp, this.<>f__this.mCurrRoleList[this.<>f__this.GetCurSelectRole()].entry, GameDefine.getInstance().use_macaddr_login);
                    Debug.Log(GameDefine.getInstance().lastAccountName);
                    Debug.Log(GameDefine.getInstance().lastAcctId);
                    Debug.Log(GameDefine.getInstance().clientPlatformType);
                    Debug.Log(GameDefine.getInstance().device_mac);
                    GUIMgr.Instance.UnLock();
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
    private sealed class <StartHeroMoveBack>c__Iterator87 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>index;
        internal CreateRoleName <>f__this;
        internal GameObject <obj>__1;
        internal Transform <posTr>__0;
        internal int index;

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
                {
                    this.<posTr>__0 = GameObject.Find("PlayerGroup/Pos" + this.index).transform;
                    this.<obj>__1 = this.<>f__this.FakeList[this.index];
                    this.<obj>__1.GetComponent<CardPlayer>().ChangeState(CardPlayerStateType.Normal);
                    this.<obj>__1.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.MoveAnimName, 1f, 0f, false);
                    object[] args = new object[] { "position", this.<posTr>__0.position, "time", CreateRoleName.moveTime, "easetype", iTween.EaseType.linear, "orienttopath", true };
                    iTween.MoveTo(this.<obj>__1, iTween.Hash(args));
                    this.$current = new WaitForSeconds(CreateRoleName.moveTime + 0.1f);
                    this.$PC = 1;
                    return true;
                }
                case 1:
                    this.<obj>__1.GetComponent<AnimFSM>().StopCurAnim(BattleGlobal.MoveAnimName);
                    this.<>f__this.PlayStandAnim(this.<obj>__1);
                    this.<obj>__1.transform.rotation = this.<posTr>__0.rotation;
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
}

