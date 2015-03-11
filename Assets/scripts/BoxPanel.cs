using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BoxPanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache9;
    public bool CanClick = true;
    private int CopperKey;
    private GameObject EffectObj;
    private int GoldKey;
    private OpenBoxData mBoxData;
    private OpenBoxType mCurOpenBoxType = OpenBoxType.E_OBT_COPPER;
    public List<UITexture> OpenTextureList = new List<UITexture>();
    private int SliverKey;
    public List<UITexture> TextureList = new List<UITexture>();

    private void ClickCopperBox()
    {
        if (this.CanClick)
        {
            if (this.CopperKey > 0)
            {
                this.mCurOpenBoxType = OpenBoxType.E_OBT_COPPER;
                this.CanClick = false;
                SocketMgr.Instance.RequestOpenBoxOpen(OpenBoxType.E_OBT_COPPER, OpenBoxPayType.E_OBTP_KEY);
            }
            else
            {
                <ClickCopperBox>c__AnonStorey26E storeye = new <ClickCopperBox>c__AnonStorey26E {
                    <>f__this = this
                };
                if (this.mBoxData.copper_open_times <= 0)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x1b9));
                }
                else
                {
                    storeye.BoxCfg = ConfigMgr.getInstance().getByEntry<open_box_config>(2);
                    if (ActorData.getInstance().Stone < storeye.BoxCfg.stone_cost)
                    {
                        <ClickCopperBox>c__AnonStorey26D storeyd = new <ClickCopperBox>c__AnonStorey26D {
                            <>f__this = this,
                            title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                        };
                        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyd.<>m__58B), null);
                    }
                    else
                    {
                        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeye.<>m__58C), null);
                    }
                }
            }
        }
    }

    private void ClickGoldBox()
    {
        if (this.CanClick)
        {
            if (this.GoldKey > 0)
            {
                this.mCurOpenBoxType = OpenBoxType.E_OBT_GOLD;
                this.CanClick = false;
                SocketMgr.Instance.RequestOpenBoxOpen(OpenBoxType.E_OBT_GOLD, OpenBoxPayType.E_OBTP_KEY);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x286c));
            }
        }
    }

    private void ClickSilverBox()
    {
        if (this.CanClick)
        {
            if (this.SliverKey > 0)
            {
                this.mCurOpenBoxType = OpenBoxType.E_OBT_SILVER;
                this.CanClick = false;
                SocketMgr.Instance.RequestOpenBoxOpen(OpenBoxType.E_OBT_SILVER, OpenBoxPayType.E_OBTP_KEY);
                GUIMgr.Instance.Lock();
            }
            else
            {
                <ClickSilverBox>c__AnonStorey26C storeyc = new <ClickSilverBox>c__AnonStorey26C {
                    <>f__this = this
                };
                if (this.mBoxData.silver_open_times <= 0)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x1b9));
                }
                else
                {
                    storeyc.BoxCfg = ConfigMgr.getInstance().getByEntry<open_box_config>(1);
                    if (ActorData.getInstance().Stone < storeyc.BoxCfg.stone_cost)
                    {
                        <ClickSilverBox>c__AnonStorey26B storeyb = new <ClickSilverBox>c__AnonStorey26B {
                            <>f__this = this,
                            title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                        };
                        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyb.<>m__589), null);
                    }
                    else
                    {
                        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyc.<>m__58A), null);
                    }
                }
            }
        }
    }

    private GameObject CreateEffect(Vector3 _pos, string _str)
    {
        GameObject obj2 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/" + _str);
        obj2.transform.localScale = Vector3.one;
        obj2.transform.position = _pos;
        return obj2;
    }

    public void ExitPanel()
    {
        if (this.EffectObj != null)
        {
            UnityEngine.Object.Destroy(this.EffectObj);
        }
        GUIMgr.Instance.PopGUIEntity();
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
        this.CanClick = true;
    }

    public override void OnInitialize()
    {
        SocketMgr.Instance.RequestOpenBoxReq();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
        this.SetBoxState(false);
    }

    private void OpendBoxDesc()
    {
        if (<>f__am$cache9 == null)
        {
            <>f__am$cache9 = delegate (GUIEntity obj) {
                BoxExplanPanel panel = (BoxExplanPanel) obj;
                panel.Depth = 500;
            };
        }
        GUIMgr.Instance.DoModelGUI("BoxExplanPanel", <>f__am$cache9, null);
    }

    public void OpenUpdata(S2C_OpenBoxOpen _data)
    {
        base.StartCoroutine(this.PlayAnim(_data));
        this.mBoxData = _data.data;
    }

    [DebuggerHidden]
    private IEnumerator PlayAnim(S2C_OpenBoxOpen _data)
    {
        return new <PlayAnim>c__IteratorA3 { _data = _data, <$>_data = _data, <>f__this = this };
    }

    private void PlayEffect()
    {
        if (this.mCurOpenBoxType == OpenBoxType.E_OBT_COPPER)
        {
            SoundManager.mInstance.PlaySFX("sound_ui_t_10");
            if (this.EffectObj != null)
            {
                UnityEngine.Object.Destroy(this.EffectObj);
                this.EffectObj = null;
            }
            this.EffectObj = this.CreateEffect(new Vector3(this.TextureList[2].gameObject.transform.position.x, this.TextureList[2].gameObject.transform.position.y, 0f), "baoxiangtexiao_dakai_1");
        }
        else if (this.mCurOpenBoxType == OpenBoxType.E_OBT_SILVER)
        {
            SoundManager.mInstance.PlaySFX("sound_ui_t_10");
            if (this.EffectObj != null)
            {
                UnityEngine.Object.Destroy(this.EffectObj);
                this.EffectObj = null;
            }
            this.EffectObj = this.CreateEffect(new Vector3(this.TextureList[1].gameObject.transform.position.x, this.TextureList[1].gameObject.transform.position.y, 0f), "baoxiangtexiao_dakai_2");
        }
        else if (this.mCurOpenBoxType == OpenBoxType.E_OBT_GOLD)
        {
            SoundManager.mInstance.PlaySFX("sound_ui_t_11");
            if (this.EffectObj != null)
            {
                UnityEngine.Object.Destroy(this.EffectObj);
                this.EffectObj = null;
            }
            this.EffectObj = this.CreateEffect(new Vector3(this.TextureList[0].gameObject.transform.position.x, this.TextureList[0].gameObject.transform.position.y, 0f), "baoxiangtexiao_dakai_3");
        }
    }

    private void SetBoxState(bool _Open)
    {
        if (this.mCurOpenBoxType == OpenBoxType.E_OBT_COPPER)
        {
            if (_Open)
            {
                this.TextureList[2].gameObject.SetActive(false);
                this.OpenTextureList[2].gameObject.SetActive(true);
            }
            else
            {
                this.TextureList[2].gameObject.SetActive(true);
                this.OpenTextureList[2].gameObject.SetActive(false);
            }
        }
        else if (this.mCurOpenBoxType == OpenBoxType.E_OBT_SILVER)
        {
            if (_Open)
            {
                this.TextureList[1].gameObject.SetActive(false);
                this.OpenTextureList[1].gameObject.SetActive(true);
            }
            else
            {
                this.TextureList[1].gameObject.SetActive(true);
                this.OpenTextureList[1].gameObject.SetActive(false);
            }
        }
        else if (this.mCurOpenBoxType == OpenBoxType.E_OBT_GOLD)
        {
            if (_Open)
            {
                this.TextureList[0].gameObject.SetActive(false);
                this.OpenTextureList[0].gameObject.SetActive(true);
            }
            else
            {
                this.TextureList[0].gameObject.SetActive(true);
                this.OpenTextureList[0].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateData(OpenBoxData _BoxData)
    {
        this.mBoxData = _BoxData;
        this.UpdateKey();
    }

    private void UpdateKey()
    {
        UILabel component = base.gameObject.transform.FindChild("Top/Grop1/Label").GetComponent<UILabel>();
        UILabel label2 = base.gameObject.transform.FindChild("Top/Grop2/Label").GetComponent<UILabel>();
        UILabel label3 = base.gameObject.transform.FindChild("Top/Grop3/Label").GetComponent<UILabel>();
        component.text = "0";
        label2.text = "0";
        label3.text = "0";
        Item goldKey = ActorData.getInstance().GetGoldKey();
        Item sliverKey = ActorData.getInstance().GetSliverKey();
        Item copperKey = ActorData.getInstance().GetCopperKey();
        if (goldKey != null)
        {
            component.text = goldKey.num.ToString();
            this.GoldKey = goldKey.num;
        }
        else
        {
            this.GoldKey = 0;
        }
        if (sliverKey != null)
        {
            label2.text = sliverKey.num.ToString();
            this.SliverKey = sliverKey.num;
        }
        else
        {
            this.SliverKey = 0;
        }
        if (copperKey != null)
        {
            label3.text = copperKey.num.ToString();
            this.CopperKey = copperKey.num;
        }
        else
        {
            this.CopperKey = 0;
        }
    }

    [CompilerGenerated]
    private sealed class <ClickCopperBox>c__AnonStorey26D
    {
        internal BoxPanel <>f__this;
        internal string title;

        internal void <>m__58B(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            e.Achieve<MessageBox>().SetDialog(this.title, delegate (GameObject _go) {
                GUIMgr.Instance.ExitModelGUI(this.<>f__this);
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }, null, false);
        }

        internal void <>m__591(GameObject _go)
        {
            GUIMgr.Instance.ExitModelGUI(this.<>f__this);
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <ClickCopperBox>c__AnonStorey26E
    {
        internal BoxPanel <>f__this;
        internal open_box_config BoxCfg;

        internal void <>m__58C(GUIEntity obj)
        {
            MessageBox box = obj.Achieve<MessageBox>();
            string str = string.Format(ConfigMgr.getInstance().GetWord(0xa652b6), this.BoxCfg.stone_cost);
            box.SetDialog(str, delegate (GameObject go) {
                this.<>f__this.mCurOpenBoxType = OpenBoxType.E_OBT_COPPER;
                this.<>f__this.CanClick = false;
                SocketMgr.Instance.RequestOpenBoxOpen(OpenBoxType.E_OBT_COPPER, OpenBoxPayType.E_OBTP_STONE);
                GUIMgr.Instance.Lock();
            }, null, false);
        }

        internal void <>m__590(GameObject go)
        {
            this.<>f__this.mCurOpenBoxType = OpenBoxType.E_OBT_COPPER;
            this.<>f__this.CanClick = false;
            SocketMgr.Instance.RequestOpenBoxOpen(OpenBoxType.E_OBT_COPPER, OpenBoxPayType.E_OBTP_STONE);
            GUIMgr.Instance.Lock();
        }
    }

    [CompilerGenerated]
    private sealed class <ClickSilverBox>c__AnonStorey26B
    {
        internal BoxPanel <>f__this;
        internal string title;

        internal void <>m__589(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            e.Achieve<MessageBox>().SetDialog(this.title, delegate (GameObject _go) {
                GUIMgr.Instance.ExitModelGUI(this.<>f__this);
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }, null, false);
        }

        internal void <>m__58F(GameObject _go)
        {
            GUIMgr.Instance.ExitModelGUI(this.<>f__this);
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <ClickSilverBox>c__AnonStorey26C
    {
        internal BoxPanel <>f__this;
        internal open_box_config BoxCfg;

        internal void <>m__58A(GUIEntity obj)
        {
            MessageBox box = obj.Achieve<MessageBox>();
            string str = string.Format(ConfigMgr.getInstance().GetWord(0xa652b6), this.BoxCfg.stone_cost);
            box.SetDialog(str, delegate (GameObject go) {
                this.<>f__this.mCurOpenBoxType = OpenBoxType.E_OBT_SILVER;
                this.<>f__this.CanClick = false;
                SocketMgr.Instance.RequestOpenBoxOpen(OpenBoxType.E_OBT_SILVER, OpenBoxPayType.E_OBTP_STONE);
                GUIMgr.Instance.Lock();
            }, null, false);
        }

        internal void <>m__58E(GameObject go)
        {
            this.<>f__this.mCurOpenBoxType = OpenBoxType.E_OBT_SILVER;
            this.<>f__this.CanClick = false;
            SocketMgr.Instance.RequestOpenBoxOpen(OpenBoxType.E_OBT_SILVER, OpenBoxPayType.E_OBTP_STONE);
            GUIMgr.Instance.Lock();
        }
    }

    [CompilerGenerated]
    private sealed class <PlayAnim>c__IteratorA3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal S2C_OpenBoxOpen _data;
        internal S2C_OpenBoxOpen <$>_data;
        internal BoxPanel <>f__this;

        internal void <>m__58D(GUIEntity entity)
        {
            (entity as RewardPanel).ShowBattleReward(this._data.reward);
            this.<>f__this.UpdateKey();
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
                    GUIMgr.Instance.Lock();
                    this.<>f__this.PlayEffect();
                    this.<>f__this.SetBoxState(true);
                    this.$current = new WaitForSeconds(0.8f);
                    this.$PC = 1;
                    goto Label_00F7;

                case 1:
                    GUIMgr.Instance.UnLock();
                    GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(this.<>m__58D), null);
                    this.$current = new WaitForSeconds(0.3f);
                    this.$PC = 2;
                    goto Label_00F7;

                case 2:
                    this.<>f__this.SetBoxState(false);
                    if (this.<>f__this.EffectObj != null)
                    {
                        UnityEngine.Object.Destroy(this.<>f__this.EffectObj);
                        this.<>f__this.EffectObj = null;
                    }
                    this.<>f__this.CanClick = true;
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_00F7:
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
}

