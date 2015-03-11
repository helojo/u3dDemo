using FastBuf;
using LevelLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SelectCampPanel : GUIEntity
{
    private CampType curCamp;
    private bool isPlaySkilling;
    private bool isStartSelectCamp;
    private Operation loadOp;
    private bool soundPlaySkill = true;

    private void CheckQQlogin()
    {
        if (GameDefine.getInstance().IsThirdPlatform())
        {
            base.transform.FindChild("Group/Input").GetComponent<UIInput>().value = GameDefine.getInstance().TencentLoginNickName;
        }
        if (base.transform.FindChild("Group/Input").GetComponent<UIInput>().value == string.Empty)
        {
            this.OnClickRandom();
        }
    }

    private void CreateHeroName()
    {
        UIInput component = base.transform.FindChild("Group/Input").GetComponent<UIInput>();
        string text = component.value.Trim();
        text.TrimStart(new char[0]);
        text.TrimEnd(new char[0]);
        if (text == string.Empty)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa344ee));
        }
        else if (text.Length > 6)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa344ef));
        }
        else if (ConfigMgr.getInstance().GetMaskWord(text).Contains("*"))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa344f0));
        }
        else
        {
            base.StartCoroutine(this.StartCreate(component.value.Trim()));
        }
    }

    private role_select_config GetCurRoleConfig()
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<role_select_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                role_select_config current = (role_select_config) enumerator.Current;
                if (this.curCamp == CampType.Buluo)
                {
                    if (current.camp == 2)
                    {
                        return current;
                    }
                }
                else if (current.camp == 1)
                {
                    return current;
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
        return null;
    }

    public override void GUIStart()
    {
        base.StartCoroutine(this.StartLoadScene());
    }

    private void HideCurCampUI()
    {
        base.transform.FindChild("Left/Lm").gameObject.SetActive(false);
        base.transform.FindChild("Right/Bl").gameObject.SetActive(false);
        base.transform.FindChild("Group").gameObject.SetActive(false);
    }

    private void On_SimpleTap(Gesture gesture)
    {
        RaycastHit hit;
        if ((((base.gameObject.GetComponent<UIPanel>().alpha == 1f) && ((gesture.fingerIndex == 0) && (Camera.main != null))) && (!this.isStartSelectCamp && !this.isPlaySkilling)) && (Physics.Raycast(Camera.main.ScreenPointToRay((Vector3) gesture.position), out hit, 100f) && (UICamera.hoveredObject == null)))
        {
            this.isPlaySkilling = true;
            GameObject obj2 = null;
            string skillName = null;
            if (this.curCamp == CampType.Buluo)
            {
                obj2 = GameObject.Find("zy_qiu/Bone002_qiu/buluo/01/Nan01_mt");
                skillName = "test_chongfeng_juesedenglu";
            }
            else
            {
                obj2 = GameObject.Find("zy_qiu/Bone002_qiu/lianmeng/01/Nv02_dxj");
                skillName = "Nv02_dxj_ssgh_tx_denglu";
            }
            if (this.soundPlaySkill)
            {
                base.StartCoroutine(this.ShowSkill(obj2, skillName, 0));
            }
            else
            {
                base.StartCoroutine(this.ShowRest(obj2));
            }
            this.soundPlaySkill = !this.soundPlaySkill;
        }
    }

    private void On_Swipe(Gesture gesture)
    {
        if ((gesture.swipe == EasyTouch.SwipeType.Left) || (gesture.swipe == EasyTouch.SwipeType.Right))
        {
            if (this.curCamp == CampType.Buluo)
            {
                this.SelectCamp(CampType.LianMeng, false);
            }
            else
            {
                this.SelectCamp(CampType.Buluo, false);
            }
        }
    }

    private void OnClickRandom()
    {
        base.transform.FindChild("Group/Input").GetComponent<UIInput>().value = CommonFunc.GetRamdomName();
    }

    public override void OnDestroy()
    {
        EasyTouch.On_SimpleTap -= new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
        GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
        if (obj2 != null)
        {
            LoginPanel component = obj2.GetComponent<LoginPanel>();
            if (component != null)
            {
                component.RestPanel();
            }
        }
    }

    private void OnEnter()
    {
        GameObject obj2 = GameObject.Find("zy_qiu/Bone002_qiu/buluo/01/Nan01_mt");
        GameObject obj3 = GameObject.Find("zy_qiu/Bone002_qiu/lianmeng/01/Nv02_dxj");
        GameObject obj4 = CardPlayer.CreateCardPlayer(0, null, CardPlayerStateType.Battle, 0);
        GameObject obj5 = CardPlayer.CreateCardPlayer(13, null, CardPlayerStateType.Battle, 0);
        if (obj4.transform.GetComponent<BoxCollider>() == null)
        {
            BoxCollider collider = obj4.AddComponent<BoxCollider>();
            collider.center = new Vector3(0f, 0.6f, 0f);
            collider.size = new Vector3(1.3f, 1.3f, 1.3f);
        }
        if (obj5.transform.GetComponent<BoxCollider>() == null)
        {
            BoxCollider collider2 = obj5.AddComponent<BoxCollider>();
            collider2.center = new Vector3(0f, 0.6f, 0f);
            collider2.size = new Vector3(1.3f, 1.3f, 1.3f);
        }
        obj4.transform.parent = obj2.transform.parent;
        obj4.transform.localPosition = obj2.transform.localPosition;
        obj4.transform.rotation = obj2.transform.rotation;
        obj4.transform.localScale = obj2.transform.localScale;
        obj5.transform.parent = obj3.transform.parent;
        obj5.transform.localPosition = obj3.transform.localPosition;
        obj5.transform.rotation = obj3.transform.rotation;
        obj5.transform.localScale = obj3.transform.localScale;
        obj3.transform.parent = null;
        obj2.transform.parent = null;
        obj3.SetActive(false);
        obj2.SetActive(false);
        this.SelectCamp(CampType.Buluo, true);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        if (GameDefine.getInstance().AccountIsActiveCode == 1)
        {
            base.transform.FindChild("InputCodeGroup").gameObject.SetActive(false);
        }
        else
        {
            base.transform.FindChild("InputCodeGroup").gameObject.SetActive(GameDefine.OpenActiveCode);
        }
        this.CheckQQlogin();
        EasyTouch.On_SimpleTap += new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
    }

    private void OnInputCode()
    {
        UIInput component = base.transform.FindChild("InputCodeGroup/InputCode").GetComponent<UIInput>();
        if (component.value.Trim() == string.Empty)
        {
            TipsDiag.SetText("请输入邀请码");
        }
        else
        {
            SocketMgr.Instance.RequestVerifyPermitCode(component.value, 0L, GameDefine.getInstance().lastAccountName, 0L, PlatformType.P_Test, GameDefine.getInstance().device_mac, GameDefine.getInstance().use_macaddr_login);
        }
    }

    public void OnSelectBL(GameObject go)
    {
        this.SelectCamp(CampType.Buluo, false);
    }

    public void OnSelectLM(GameObject go)
    {
        this.SelectCamp(CampType.LianMeng, false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if ((this.loadOp != null) && this.loadOp.IsDone())
        {
            this.loadOp = null;
            this.OnEnter();
        }
    }

    private void SelectCamp(CampType camp, bool force)
    {
        if (!this.isStartSelectCamp && !this.isPlaySkilling)
        {
            this.soundPlaySkill = true;
            if (this.curCamp != camp)
            {
                this.curCamp = camp;
                this.isStartSelectCamp = true;
                base.StartCoroutine(this.StartSelectCamp(force));
            }
        }
    }

    private void ShowCurCampUI()
    {
        base.transform.FindChild("Group").gameObject.SetActive(true);
        role_select_config curRoleConfig = this.GetCurRoleConfig();
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(curRoleConfig.card_entry);
        GameObject gameObject = base.transform.FindChild("Left/Lm").gameObject;
        GameObject obj3 = base.transform.FindChild("Right/Bl").gameObject;
        GameObject obj4 = null;
        GameObject obj5 = null;
        if (this.curCamp == CampType.Buluo)
        {
            obj4 = obj3;
            obj5 = gameObject;
        }
        else
        {
            obj5 = obj3;
            obj4 = gameObject;
        }
        obj4.SetActive(true);
        obj5.SetActive(false);
        obj4.transform.FindChild("Name").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(10), _config2.name);
        obj4.transform.FindChild("Desc").GetComponent<UILabel>().text = curRoleConfig.description;
    }

    [DebuggerHidden]
    private IEnumerator ShowRest(GameObject obj)
    {
        return new <ShowRest>c__Iterator8D { obj = obj, <$>obj = obj, <>f__this = this };
    }

    public static void ShowSelectCamp()
    {
        GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
        if (obj2 != null)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        GUIMgr.Instance.DoModelGUI("SelectCampPanel", null, null);
    }

    public void ShowSelectCampGroup()
    {
        base.transform.FindChild("CampGroup").gameObject.SetActive(true);
        base.transform.FindChild("InputCodeGroup").gameObject.SetActive(false);
    }

    [DebuggerHidden]
    private IEnumerator ShowSkill(GameObject obj, string skillName, int index)
    {
        return new <ShowSkill>c__Iterator8E { obj = obj, index = index, skillName = skillName, <$>obj = obj, <$>index = index, <$>skillName = skillName, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartCreate(string text)
    {
        return new <StartCreate>c__Iterator8A { text = text, <$>text = text, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartLoadScene()
    {
        return new <StartLoadScene>c__Iterator8B { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartSelectCamp(bool force)
    {
        return new <StartSelectCamp>c__Iterator8C { force = force, <$>force = force, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <ShowRest>c__Iterator8D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <$>obj;
        internal SelectCampPanel <>f__this;
        internal CardPlayer <player>__0;
        internal GameObject obj;

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
                    this.<player>__0 = this.obj.GetComponent<CardPlayer>();
                    if (this.<player>__0 == null)
                    {
                        this.<player>__0 = this.obj.AddComponent<CardPlayer>();
                    }
                    this.<player>__0.ChangeState(CardPlayerStateType.Battle);
                    if (this.<>f__this.curCamp == SelectCampPanel.CampType.Buluo)
                    {
                        this.obj.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.RestAnimName, 1f, 0f, false);
                    }
                    else
                    {
                        this.obj.GetComponent<AnimFSM>().PlayAnim("dengluxiuxian", 1f, 0f, false);
                    }
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.isPlaySkilling = false;
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
    private sealed class <ShowSkill>c__Iterator8E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>index;
        internal GameObject <$>obj;
        internal string <$>skillName;
        internal SelectCampPanel <>f__this;
        internal int <beijiIndex>__3;
        internal string <daijiAnim>__9;
        internal List<GameObject> <defenderList>__2;
        internal GameObject <denfendObj>__6;
        internal GameObject <group>__1;
        internal CardPlayer <player>__0;
        internal string <Select_Sound>__8;
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
                    this.<player>__0 = this.obj.GetComponent<CardPlayer>();
                    if (this.<player>__0 == null)
                    {
                        this.<player>__0 = this.obj.AddComponent<CardPlayer>();
                    }
                    this.<player>__0.ChangeState(CardPlayerStateType.Battle);
                    this.<group>__1 = GameObject.Find("PlayerGroup");
                    this.<defenderList>__2 = new List<GameObject>();
                    this.<beijiIndex>__3 = 0;
                    while (this.<beijiIndex>__3 < 5)
                    {
                        this.<targetName>__4 = "beiji" + this.index.ToString() + "_" + this.<beijiIndex>__3.ToString();
                        this.<trans>__5 = this.<group>__1.transform.FindChild(this.<targetName>__4);
                        if (this.<trans>__5 != null)
                        {
                            this.<denfendObj>__6 = this.<trans>__5.gameObject;
                            this.<denfendObj>__6.AddComponent<HangControler>();
                            this.<defenderList>__2.Add(this.<denfendObj>__6);
                        }
                        this.<beijiIndex>__3++;
                    }
                    this.obj.GetComponent<AnimFSM>().ResetAnim();
                    this.<showEffect>__7 = RealTimeShowEffect.CreateNewShowEffect(this.skillName, this.obj, this.<defenderList>__2[0]);
                    this.<showEffect>__7.ToDoStart();
                    this.$current = new WaitForSeconds(1.5f);
                    this.$PC = 1;
                    goto Label_0281;

                case 1:
                    this.<Select_Sound>__8 = null;
                    if (this.<>f__this.curCamp != SelectCampPanel.CampType.Buluo)
                    {
                        this.<Select_Sound>__8 = "dub_dxj_select";
                        break;
                    }
                    this.<Select_Sound>__8 = "dub_mt_select";
                    break;

                case 2:
                    this.obj.GetComponent<AnimFSM>().StopCurAnimForce();
                    this.<daijiAnim>__9 = string.Empty;
                    if (this.<>f__this.curCamp != SelectCampPanel.CampType.Buluo)
                    {
                        this.<daijiAnim>__9 = "dengludaiji";
                    }
                    else
                    {
                        this.<daijiAnim>__9 = "move01";
                    }
                    this.obj.GetComponent<AnimFSM>().PlayAnim(this.<daijiAnim>__9, 1f, 0f, false);
                    this.<>f__this.isPlaySkilling = false;
                    this.$PC = -1;
                    goto Label_027F;

                default:
                    goto Label_027F;
            }
            SoundManager.mInstance.SFXVolume = 2f;
            SoundManager.mInstance.PlaySFX(this.<Select_Sound>__8);
            SoundManager.mInstance.SFXVolume = 1f;
            this.$current = new WaitForSeconds(1.3f);
            this.$PC = 2;
            goto Label_0281;
        Label_027F:
            return false;
        Label_0281:
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
    private sealed class <StartCreate>c__Iterator8A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>text;
        internal SelectCampPanel <>f__this;
        internal string <okSound>__1;
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
                    this.<rsc>__0 = this.<>f__this.GetCurRoleConfig();
                    this.<okSound>__1 = null;
                    if (this.<>f__this.curCamp != SelectCampPanel.CampType.Buluo)
                    {
                        this.<okSound>__1 = "dub_dxj_yes";
                        break;
                    }
                    this.<okSound>__1 = "dub_mt_yes";
                    break;

                case 1:
                    GUIMgr.Instance.UnLock();
                    this.$PC = -1;
                    goto Label_0124;

                default:
                    goto Label_0124;
            }
            SoundManager.mInstance.SFXVolume = 2f;
            SoundManager.mInstance.PlaySFX(this.<okSound>__1);
            SoundManager.mInstance.SFXVolume = 1f;
            GUIMgr.Instance.Lock();
            SocketMgr.Instance.RequestRegister(GameDefine.getInstance().lastAccountName, GameDefine.getInstance().lastAcctId, GameDefine.getInstance().clientPlatformType, GameDefine.getInstance().device_mac, this.text, 1, this.<rsc>__0.camp, this.<rsc>__0.entry, GameDefine.getInstance().use_macaddr_login);
            this.$current = new WaitForSeconds(0.2f);
            this.$PC = 1;
            return true;
        Label_0124:
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
    private sealed class <StartLoadScene>c__Iterator8B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SelectCampPanel <>f__this;

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
                    this.$current = null;
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.loadOp = BundleMgr.Instance.LoadLevelAsync("ci_zhenying");
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
    private sealed class <StartSelectCamp>c__Iterator8C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool <$>force;
        internal Animation[] <$s_757>__15;
        internal int <$s_758>__16;
        internal Animation[] <$s_759>__20;
        internal int <$s_760>__21;
        internal Animation[] <$s_761>__32;
        internal int <$s_762>__33;
        internal SelectCampPanel <>f__this;
        internal Transform <addEffect>__25;
        internal string <addEffectName>__6;
        internal GameObject <addEffectNew>__35;
        internal Transform <anim>__10;
        internal Transform <anim>__12;
        internal Animation <anim>__17;
        internal Animation <anim>__22;
        internal Transform <anim>__30;
        internal Animation <anim>__34;
        internal Transform <anim>__38;
        internal Animation[] <anims>__14;
        internal Animation[] <anims>__19;
        internal Animation[] <anims>__31;
        internal string <daijiAnim>__5;
        internal Transform <effectObj>__23;
        internal Transform <effectObj2>__24;
        internal int <index>__36;
        internal MaterialFSM <materialFsm>__27;
        internal MaterialFSM <materialFsm>__29;
        internal Transform <node>__37;
        internal string <otherPlayerNodeName>__2;
        internal string <otherYun>__4;
        internal GameObject <otherYunObj>__26;
        internal Transform <playerNode>__8;
        internal string <playerNodeName>__1;
        internal string <qiuAnim>__0;
        internal GameObject <qiuObj>__7;
        internal Transform <taizhiObj>__13;
        internal Transform <taizhiObj2>__18;
        internal string <yun>__3;
        internal GameObject <yunObj>__28;
        internal Transform <zhiwuObj>__9;
        internal Transform <zhiwuObj2>__11;
        internal bool force;

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
                    this.<>f__this.HideCurCampUI();
                    this.<qiuAnim>__0 = string.Empty;
                    this.<playerNodeName>__1 = string.Empty;
                    this.<otherPlayerNodeName>__2 = string.Empty;
                    this.<yun>__3 = string.Empty;
                    this.<otherYun>__4 = string.Empty;
                    this.<daijiAnim>__5 = string.Empty;
                    this.<addEffectName>__6 = string.Empty;
                    if (this.<>f__this.curCamp != SelectCampPanel.CampType.Buluo)
                    {
                        this.<qiuAnim>__0 = "xz_lianmen";
                        this.<playerNodeName>__1 = "Bone002_qiu/lianmeng";
                        this.<otherPlayerNodeName>__2 = "Bone002_qiu/buluo";
                        this.<yun>__3 = "zy_lm_yun";
                        this.<otherYun>__4 = "zy_bl_yun";
                        this.<daijiAnim>__5 = "dengludaiji";
                        this.<addEffectName>__6 = "EffectPrefabs/rainbow";
                        break;
                    }
                    this.<qiuAnim>__0 = "xz_buluo";
                    this.<playerNodeName>__1 = "Bone002_qiu/buluo";
                    this.<otherPlayerNodeName>__2 = "Bone002_qiu/lianmeng";
                    this.<yun>__3 = "zy_bl_yun";
                    this.<otherYun>__4 = "zy_lm_yun";
                    this.<daijiAnim>__5 = "xingzou1";
                    this.<addEffectName>__6 = string.Empty;
                    break;

                case 1:
                    this.<qiuObj>__7.animation.Play(this.<qiuAnim>__0);
                    this.<otherYunObj>__26 = GameObject.Find(this.<otherYun>__4);
                    if (this.<otherYunObj>__26 != null)
                    {
                        this.<materialFsm>__27 = this.<otherYunObj>__26.GetComponent<MaterialFSM>();
                        if (this.<materialFsm>__27 == null)
                        {
                            this.<materialFsm>__27 = this.<otherYunObj>__26.AddComponent<MaterialFSM>();
                        }
                        this.<materialFsm>__27.StartAlphaChange(1.333333f, 0f);
                    }
                    this.$current = new WaitForSeconds(1.333333f);
                    this.$PC = 2;
                    goto Label_080D;

                case 2:
                    this.<yunObj>__28 = GameObject.Find(this.<yun>__3);
                    if (this.<yunObj>__28 != null)
                    {
                        this.<materialFsm>__29 = this.<yunObj>__28.GetComponent<MaterialFSM>();
                        if (this.<materialFsm>__29 == null)
                        {
                            this.<materialFsm>__29 = this.<yunObj>__28.AddComponent<MaterialFSM>();
                        }
                        this.<materialFsm>__29.StartAlphaChange(1.333333f, 1f);
                    }
                    this.$current = new WaitForSeconds(1.333333f);
                    this.$PC = 3;
                    goto Label_080D;

                case 3:
                    if (this.<zhiwuObj>__9 != null)
                    {
                        this.<anim>__30 = this.<zhiwuObj>__9.GetChild(0);
                        if (this.<anim>__30 != null)
                        {
                            this.<anim>__30.animation.Play("kai");
                        }
                    }
                    if (this.<taizhiObj>__13 != null)
                    {
                        this.<anims>__31 = this.<taizhiObj>__13.GetComponentsInChildren<Animation>();
                        this.<$s_761>__32 = this.<anims>__31;
                        this.<$s_762>__33 = 0;
                        while (this.<$s_762>__33 < this.<$s_761>__32.Length)
                        {
                            this.<anim>__34 = this.<$s_761>__32[this.<$s_762>__33];
                            this.<anim>__34.Play("shang");
                            this.<$s_762>__33++;
                        }
                    }
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 4;
                    goto Label_080D;

                case 4:
                    if (this.<effectObj>__23 != null)
                    {
                        this.<effectObj>__23.gameObject.SetActive(true);
                        this.<addEffectNew>__35 = ObjectManager.CreateObj(this.<addEffectName>__6);
                        if (this.<addEffectNew>__35 != null)
                        {
                            this.<addEffectNew>__35.name = "addEffect";
                            this.<addEffectNew>__35.transform.parent = this.<playerNode>__8.transform;
                        }
                    }
                    goto Label_06FD;

                case 5:
                    this.<>f__this.ShowCurCampUI();
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 6;
                    goto Label_080D;

                case 6:
                    this.<>f__this.isStartSelectCamp = false;
                    this.$PC = -1;
                    goto Label_080B;

                default:
                    goto Label_080B;
            }
            this.<qiuObj>__7 = GameObject.Find("zy_qiu");
            if (this.<qiuObj>__7 == null)
            {
                goto Label_07B5;
            }
            this.<playerNode>__8 = this.<qiuObj>__7.transform.Find(this.<playerNodeName>__1);
            if (!this.force)
            {
                this.<zhiwuObj>__9 = this.<playerNode>__8.transform.Find("zhiwu");
                if (this.<zhiwuObj>__9 != null)
                {
                    this.<anim>__10 = this.<zhiwuObj>__9.GetChild(0);
                    if (this.<anim>__10 != null)
                    {
                        this.<anim>__10.animation.Play("suo");
                    }
                }
                this.<zhiwuObj2>__11 = this.<qiuObj>__7.transform.Find(this.<otherPlayerNodeName>__2 + "/zhiwu");
                if (this.<zhiwuObj2>__11 != null)
                {
                    this.<anim>__12 = this.<zhiwuObj2>__11.GetChild(0);
                    if (this.<anim>__12 != null)
                    {
                        this.<anim>__12.animation.Play("suo");
                    }
                }
                this.<taizhiObj>__13 = this.<playerNode>__8.transform.Find("taizi");
                if (this.<taizhiObj>__13 != null)
                {
                    this.<anims>__14 = this.<taizhiObj>__13.GetComponentsInChildren<Animation>();
                    this.<$s_757>__15 = this.<anims>__14;
                    this.<$s_758>__16 = 0;
                    while (this.<$s_758>__16 < this.<$s_757>__15.Length)
                    {
                        this.<anim>__17 = this.<$s_757>__15[this.<$s_758>__16];
                        this.<anim>__17.Play("xia");
                        this.<$s_758>__16++;
                    }
                }
                this.<taizhiObj2>__18 = this.<qiuObj>__7.transform.Find(this.<otherPlayerNodeName>__2 + "/taizi");
                if (this.<taizhiObj2>__18 != null)
                {
                    this.<anims>__19 = this.<taizhiObj2>__18.GetComponentsInChildren<Animation>();
                    this.<$s_759>__20 = this.<anims>__19;
                    this.<$s_760>__21 = 0;
                    while (this.<$s_760>__21 < this.<$s_759>__20.Length)
                    {
                        this.<anim>__22 = this.<$s_759>__20[this.<$s_760>__21];
                        this.<anim>__22.Play("xia");
                        this.<$s_760>__21++;
                    }
                }
                this.<effectObj>__23 = this.<playerNode>__8.transform.Find("effect");
                if (this.<effectObj>__23 != null)
                {
                    this.<effectObj>__23.gameObject.SetActive(false);
                }
                this.<effectObj2>__24 = this.<qiuObj>__7.transform.Find(this.<otherPlayerNodeName>__2 + "/effect");
                if (this.<effectObj2>__24 != null)
                {
                    this.<effectObj2>__24.gameObject.SetActive(false);
                }
                this.<addEffect>__25 = this.<qiuObj>__7.transform.Find(this.<otherPlayerNodeName>__2 + "/addEffect");
                if (this.<addEffect>__25 != null)
                {
                    UnityEngine.Object.Destroy(this.<addEffect>__25.gameObject, 3f);
                }
                this.$current = new WaitForSeconds(0.8f);
                this.$PC = 1;
                goto Label_080D;
            }
        Label_06FD:
            this.<index>__36 = 1;
            while (this.<index>__36 < 4)
            {
                this.<node>__37 = this.<playerNode>__8.Find("0" + this.<index>__36.ToString());
                if (this.<node>__37 != null)
                {
                    this.<anim>__38 = this.<node>__37.GetChild(0);
                    if (this.<anim>__38 != null)
                    {
                        this.<anim>__38.animation[this.<daijiAnim>__5].wrapMode = WrapMode.Loop;
                        this.<anim>__38.animation.Play(this.<daijiAnim>__5);
                    }
                }
                this.<index>__36++;
            }
        Label_07B5:
            this.$current = new WaitForSeconds(0.5f);
            this.$PC = 5;
            goto Label_080D;
        Label_080B:
            return false;
        Label_080D:
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

    private enum CampType
    {
        None,
        Buluo,
        LianMeng
    }
}

