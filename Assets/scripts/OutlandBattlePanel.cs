using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class OutlandBattlePanel : GUIEntity
{
    public GameObject _buffTips;
    public Transform _help;
    public UILabel _KeyLabel;
    public GameObject _keyPos;
    public UILabel _PLabel;
    public GameObject _siderPlayer;
    public GameObject _Tips;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache14;
    public GameObject boxModel;
    public UITexture boxTexture;
    private Dictionary<int, Transform> dictNewBuff = new Dictionary<int, Transform>();
    private Dictionary<int, Transform> dictOldBuff = new Dictionary<int, Transform>();
    private bool isKey;
    public UILabel layerLabel;
    private List<int> newAddBuffs = new List<int>();
    public Transform nodeBox;
    private List<int> oldListBuff = new List<int>();
    public UISlider progresSlider;
    public int rewardState;
    public UISprite sliderForeground;

    public void AfterCreateEventModel(GameObject gameObject)
    {
        if (gameObject != null)
        {
            gameObject.transform.parent = this._keyPos.transform;
            gameObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
    }

    public void AfterCreateEventModel(GameObject gameObject, GameObject parentGo)
    {
        if (gameObject != null)
        {
            gameObject.transform.parent = parentGo.transform;
            gameObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
    }

    public void CombatBackBuffDele()
    {
        <CombatBackBuffDele>c__AnonStorey1C6 storeyc = new <CombatBackBuffDele>c__AnonStorey1C6 {
            tempLists = new List<int>(),
            tempLists1 = new List<int>()
        };
        if ((this.oldListBuff != null) && (this.oldListBuff.Count > 0))
        {
            this.oldListBuff.ForEach(new Action<int>(storeyc.<>m__2C5));
            if (storeyc.tempLists1.Count > 0)
            {
                this.HideBuffGird(false);
                this.SetBuffIcon(storeyc.tempLists);
            }
        }
    }

    [DebuggerHidden]
    public IEnumerator DeleteBuff(List<int> buffLists)
    {
        return new <DeleteBuff>c__Iterator72 { buffLists = buffLists, <$>buffLists = buffLists, <>f__this = this };
    }

    private void DestoryObj(GameObject model)
    {
        if (model != null)
        {
            ObjectManager.DestoryObj(model);
            model = null;
        }
    }

    private void GetRewardCallback()
    {
        if (this.boxTexture != null)
        {
            this.boxTexture.mainTexture = BundleMgr.Instance.CreateBoxIcon("Ui_Tower_Icon_jinopen");
            TweenRotation component = this.boxTexture.GetComponent<TweenRotation>();
            if (component != null)
            {
                component.enabled = false;
            }
        }
    }

    public void HideBuffGird(bool isShow)
    {
        for (int i = 1; i <= 10; i++)
        {
            base.transform.FindChild("TopRight/BuffIcon/" + i + string.Empty).gameObject.SetActive(isShow);
        }
    }

    private void OnClickBoxBtn(GameObject go)
    {
        switch (this.rewardState)
        {
            case 0:
                this.OnPopTisp(this.nodeBox.gameObject);
                break;

            case 1:
                SocketMgr.Instance.RequestOutlandGetFloorBoxReward(BattleState.GetInstance().CurGame.battleGameData.gridGameData.entry, BattleState.GetInstance().CurGame.battleGameData.gridGameData.map_entry);
                break;

            case 2:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2aa6));
                break;
        }
    }

    private void OnClickBuffIconBtn(GameObject go, bool isPress)
    {
        object obj2 = GUIDataHolder.getData(go);
        int id = !(obj2 is int) ? 0 : ((int) obj2);
        outland_buffer_config _config = ConfigMgr.getInstance().getByEntry<outland_buffer_config>(id);
        if (_config != null)
        {
            if (isPress)
            {
                Transform transform;
                this.dictNewBuff.TryGetValue(id, out transform);
                if ((transform != null) && (this._buffTips != null))
                {
                    this._buffTips.transform.parent = transform.transform;
                    this._buffTips.transform.localPosition = new Vector3(1.77f, -7.96f, 0f);
                    UILabel component = this._buffTips.transform.FindChild("Desc").GetComponent<UILabel>();
                    UILabel label2 = this._buffTips.transform.FindChild("Name").GetComponent<UILabel>();
                    UITexture texture = this._buffTips.transform.FindChild("Icon/Sprite").GetComponent<UITexture>();
                    if (texture != null)
                    {
                        texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                    }
                    label2.text = "[078dff]" + _config.remark;
                    component.text = _config.remark2;
                    this._buffTips.transform.FindChild("Background").GetComponent<UISprite>().height = component.height + 150;
                    this._buffTips.gameObject.SetActive(true);
                }
            }
            else
            {
                this._buffTips.gameObject.SetActive(false);
            }
        }
    }

    private void OnClickHelpBtn(GameObject go)
    {
        if (<>f__am$cache14 == null)
        {
            <>f__am$cache14 = delegate (GUIEntity obj) {
                WorldCupRulePanel panel = (WorldCupRulePanel) obj;
                panel.Depth = 800;
                panel.SetOutlandRule();
            };
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cache14, null);
    }

    private void OnClickTipsBtn(GameObject go)
    {
        if (this._Tips != null)
        {
            this._Tips.SetActive(false);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.layerLabel = base.transform.FindChild("TopLeft/Layer/Label").GetComponent<UILabel>();
        if (this._siderPlayer != null)
        {
            this._PLabel = this._siderPlayer.transform.FindChild("Percen/PLabel").GetComponent<UILabel>();
            UITexture texture = this._siderPlayer.transform.FindChild("playIcon/Icon").GetComponent<UITexture>();
            UISprite sprite = this._siderPlayer.transform.FindChild("Faction").GetComponent<UISprite>();
            if (sprite != null)
            {
                switch (ActorData.getInstance().UserInfo.faction)
                {
                    case FactionType.Alliance:
                        sprite.spriteName = "Ui_Out_Icon_lm";
                        break;

                    case FactionType.Horde:
                        sprite.spriteName = "Ui_Out_Icon_bl";
                        break;
                }
            }
            if (texture != null)
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(ActorData.getInstance().UserInfo.headEntry);
                if (_config != null)
                {
                    texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                }
            }
        }
        this.nodeBox = base.transform.FindChild("TopLeft/ClearBox/node3");
        if (this.nodeBox != null)
        {
            UIEventListener.Get(this.nodeBox.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickBoxBtn);
        }
        if (this._Tips != null)
        {
            UIEventListener.Get(this._Tips.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickTipsBtn);
        }
        if (this._help != null)
        {
            UIEventListener.Get(this._help.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickHelpBtn);
        }
        UIButton component = base.transform.FindChild("TopLeft/CloseButton").GetComponent<UIButton>();
        if (component != null)
        {
            UIEventListener.Get(component.gameObject).onClick = new UIEventListener.VoidDelegate(this.returnBtnClick);
        }
        this.HideBuffGird(false);
    }

    private void OnPopTisp(GameObject go)
    {
        this._Tips.transform.FindChild("Gold").GetComponent<UILabel>().text = "2000";
        this._Tips.SetActive(true);
    }

    public void PlayBuffEffect(int entry, Vector3 worldPos)
    {
        base.StartCoroutine(this.StartMove(entry, worldPos));
    }

    public void PlayBuffRotationEffect(List<int> buffs)
    {
        base.StartCoroutine(this.DeleteBuff(buffs));
    }

    private void returnBtnClick(GameObject go)
    {
        ActorData.getInstance().isOutlandGrid = true;
        BattleStaticEntry.ExitBattle();
        GameStateMgr.Instance.ChangeState("EXIT_OUTLAND_GRID_EVENT");
    }

    private void SetActiveBox()
    {
        if (this.boxTexture != null)
        {
            if (this.nodeBox != null)
            {
                this.nodeBox.transform.FindChild("baoxiao_fuben").gameObject.SetActive(true);
            }
            this.boxTexture.mainTexture = BundleMgr.Instance.CreateBoxIcon("Ui_Tower_Icon_jin");
            TweenRotation component = this.boxTexture.GetComponent<TweenRotation>();
            if (component != null)
            {
                component.enabled = true;
            }
        }
    }

    public void SetBoxIcon(int state)
    {
        this.rewardState = state;
        switch (this.rewardState)
        {
            case 0:
                if (this.boxTexture != null)
                {
                    this.boxTexture.mainTexture = BundleMgr.Instance.CreateBoxIcon("Ui_Tower_Icon_grey");
                    TweenRotation component = this.boxTexture.GetComponent<TweenRotation>();
                    if (component != null)
                    {
                        component.enabled = false;
                    }
                }
                if (this.nodeBox != null)
                {
                    this.nodeBox.transform.FindChild("baoxiao_fuben").gameObject.SetActive(false);
                }
                break;

            case 1:
                this.SetActiveBox();
                break;

            case 2:
                this.GetRewardCallback();
                if (this.nodeBox != null)
                {
                    this.nodeBox.transform.FindChild("baoxiao_fuben").gameObject.SetActive(false);
                }
                break;
        }
    }

    public void SetBuffIcon(List<int> buffs)
    {
        this.dictNewBuff.Clear();
        int num = 1;
        if (this.oldListBuff.Count < buffs.Count)
        {
            foreach (int num2 in this.oldListBuff)
            {
                Transform ts = base.transform.FindChild("TopRight/BuffIcon/" + num + string.Empty);
                this.SetIcon(num2, ts);
                num++;
            }
            this.newAddBuffs = (from b in buffs
                where !this.oldListBuff.Contains(b)
                select b).ToList<int>();
            foreach (int num3 in this.newAddBuffs)
            {
                Transform transform2 = base.transform.FindChild("TopRight/BuffIcon/" + num + string.Empty);
                this.SetIcon(num3, transform2);
                num++;
            }
        }
        else
        {
            foreach (int num4 in buffs)
            {
                Transform transform3 = base.transform.FindChild("TopRight/BuffIcon/" + num + string.Empty);
                this.SetIcon(num4, transform3);
                num++;
            }
        }
        this.oldListBuff = buffs;
    }

    private void SetIcon(int buffEntry, Transform ts)
    {
        outland_buffer_config _config = ConfigMgr.getInstance().getByEntry<outland_buffer_config>(buffEntry);
        if ((_config != null) && (ts != null))
        {
            UITexture component = ts.transform.FindChild("Sprite").GetComponent<UITexture>();
            if (component != null)
            {
                component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            }
            ts.gameObject.SetActive(true);
            GUIDataHolder.setData(ts.gameObject, buffEntry);
            UIEventListener.Get(ts.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickBuffIconBtn);
            this.dictNewBuff.Add(buffEntry, ts);
        }
    }

    public void SetKey()
    {
        this.DelayCallBack(0.2f, delegate {
            if (!this.isKey)
            {
                this.isKey = true;
                this._KeyLabel.text = "1/1";
                TweenRotation component = base.transform.FindChild("TopRight/KeyPos/key").GetComponent<TweenRotation>();
                if (component != null)
                {
                    component.enabled = true;
                }
            }
        });
    }

    public void SetKeyEnable(bool isEnable)
    {
        if (!isEnable)
        {
            this.isKey = false;
            this._KeyLabel.text = "0/1";
            TweenRotation component = base.transform.FindChild("TopRight/KeyPos/key").GetComponent<TweenRotation>();
            if (component != null)
            {
                component.enabled = false;
            }
        }
    }

    public void SetPrSlider(int showGrid, int totalGrid)
    {
        if ((this.sliderForeground != null) && (totalGrid > 0))
        {
            this.sliderForeground.fillAmount = ((float) showGrid) / ((float) totalGrid);
            float num = this.sliderForeground.width * this.sliderForeground.fillAmount;
            this._siderPlayer.transform.localPosition = new Vector3(num - 327f, 0f, 0f);
            this._PLabel.text = ((int) (this.sliderForeground.fillAmount * 100f)).ToString();
        }
    }

    [DebuggerHidden]
    private IEnumerator StartMove(int entry, Vector3 worldPos)
    {
        return new <StartMove>c__Iterator73 { entry = entry, worldPos = worldPos, <$>entry = entry, <$>worldPos = worldPos, <>f__this = this };
    }

    public GameObject keyModel { get; set; }

    [CompilerGenerated]
    private sealed class <CombatBackBuffDele>c__AnonStorey1C6
    {
        internal List<int> tempLists;
        internal List<int> tempLists1;

        internal void <>m__2C5(int b)
        {
            outland_buffer_config _config = ConfigMgr.getInstance().getByEntry<outland_buffer_config>(b);
            if (_config != null)
            {
                if (_config.buffer_gen_type != 0)
                {
                    this.tempLists.Add(b);
                }
                else if (_config.buffer_gen_type == 0)
                {
                    this.tempLists1.Add(b);
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <DeleteBuff>c__Iterator72 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<int> <$>buffLists;
        internal List<int>.Enumerator <$s_572>__1;
        internal OutlandBattlePanel <>f__this;
        internal int <deleBuff>__2;
        internal List<int> <deleBuffs>__0;
        internal TweenRotation <tr>__4;
        internal Transform <ts>__3;
        internal List<int> buffLists;

        internal bool <>m__2C8(int b)
        {
            return !this.buffLists.Contains(b);
        }

        internal bool <>m__2C9(KeyValuePair<int, Transform> b)
        {
            return (b.Key == this.<deleBuff>__2);
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
                    if (this.<>f__this.oldListBuff == null)
                    {
                        break;
                    }
                    this.<deleBuffs>__0 = this.<>f__this.oldListBuff.Where<int>(new Func<int, bool>(this.<>m__2C8)).ToList<int>();
                    if (this.<deleBuffs>__0.Count <= 0)
                    {
                        break;
                    }
                    this.<$s_572>__1 = this.<deleBuffs>__0.GetEnumerator();
                    try
                    {
                        while (this.<$s_572>__1.MoveNext())
                        {
                            this.<deleBuff>__2 = this.<$s_572>__1.Current;
                            this.<ts>__3 = this.<>f__this.dictNewBuff.First<KeyValuePair<int, Transform>>(new Func<KeyValuePair<int, Transform>, bool>(this.<>m__2C9)).Value;
                            if (this.<ts>__3 != null)
                            {
                                this.<tr>__4 = this.<ts>__3.GetComponent<TweenRotation>();
                                this.<tr>__4.enabled = true;
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_572>__1.Dispose();
                    }
                    this.$current = new WaitForSeconds(20f);
                    this.$PC = 1;
                    return true;

                case 1:
                    break;

                default:
                    goto Label_014E;
            }
            this.<>f__this.HideBuffGird(false);
            this.<>f__this.SetBuffIcon(this.buffLists);
            this.$PC = -1;
        Label_014E:
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
    private sealed class <StartMove>c__Iterator73 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>entry;
        internal Vector3 <$>worldPos;
        internal OutlandBattlePanel <>f__this;
        internal GameObject <effect>__1;
        internal Vector3 <moveRef>__4;
        internal float <remainTime>__5;
        internal Vector3 <targetPos>__3;
        internal Transform <ts>__0;
        internal Vector3 <uiPos>__2;
        internal int entry;
        internal Vector3 worldPos;

        internal bool <>m__2CA(int b)
        {
            return (b == this.entry);
        }

        internal bool <>m__2CB(KeyValuePair<int, Transform> b)
        {
            return (b.Key == this.entry);
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
                    if (!this.<>f__this.newAddBuffs.Any<int>(new Func<int, bool>(this.<>m__2CA)))
                    {
                        goto Label_01A3;
                    }
                    this.<ts>__0 = this.<>f__this.dictNewBuff.First<KeyValuePair<int, Transform>>(new Func<KeyValuePair<int, Transform>, bool>(this.<>m__2CB)).Value;
                    this.<effect>__1 = ObjectManager.CreateTempObj("BattlePrefabs/Item3DObject", this.worldPos, 3.1f);
                    RecruitAnimation.ItemObjectMorph(this.<effect>__1, 0, 0);
                    this.<uiPos>__2 = this.<ts>__0.position;
                    this.<uiPos>__2.z = 10f;
                    this.<targetPos>__3 = BattleGlobalFunc.GUIToWorld(this.<uiPos>__2);
                    this.<moveRef>__4 = Vector3.zero;
                    this.<remainTime>__5 = 0.6f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_01AA;
            }
            if ((Vector3.Distance(this.<effect>__1.transform.position, this.<targetPos>__3) > 0.1f) && (this.<remainTime>__5 > 0f))
            {
                this.<uiPos>__2 = this.<ts>__0.position;
                this.<uiPos>__2.z = 10f;
                this.<targetPos>__3 = BattleGlobalFunc.GUIToWorld(this.<uiPos>__2);
                this.<effect>__1.transform.position = Vector3.SmoothDamp(this.<effect>__1.transform.position, this.<targetPos>__3, ref this.<moveRef>__4, 0.3f);
                this.<remainTime>__5 -= Time.deltaTime;
                this.$current = null;
                this.$PC = 1;
                return true;
            }
        Label_01A3:
            this.$PC = -1;
        Label_01AA:
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

