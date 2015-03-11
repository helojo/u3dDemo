using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArenaPanel : GUIEntity
{
    public GameObject _BuyPkCountBtn;
    public GameObject _EnemyRefreshBtn;
    public UILabel _EnemyRefreshTimeLabel;
    public UILabel _RemainTimeLabel;
    public GameObject _RewardBtn;
    public GameObject _ShuoMingBtn;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheE;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheF;
    private bool isClickSend;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private int mEndTime;
    private bool mIsRefreshPlayer;
    private bool mIsStart;
    public bool mLockPkBtnEvent;
    private WarmmatchData mWarmmatchData;

    private void BuyPkCount()
    {
        if (this.mWarmmatchData.buy_times < 1)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2817));
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
                <BuyPkCount>c__AnonStorey16F storeyf = new <BuyPkCount>c__AnonStorey16F {
                    <>f__this = this,
                    gui = (MessageBox) obj
                };
                storeyf.gui.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x2816), 50, this.mWarmmatchData.buy_times), new UIEventListener.VoidDelegate(storeyf.<>m__189), null, false);
            }, base.gameObject);
        }
    }

    private bool CheckIsPass()
    {
        if (this.mWarmmatchData == null)
        {
            return false;
        }
        int num = 0;
        foreach (byte num2 in this.mWarmmatchData.enemy_state)
        {
            if (num2 == 1)
            {
                num++;
            }
        }
        if (num == 3)
        {
            this._EnemyRefreshBtn.SetActive(false);
            this.mIsRefreshPlayer = false;
        }
        return (num == 3);
    }

    private void EnemyRefresh()
    {
        if (!this.isClickSend)
        {
            this.isClickSend = true;
            if (!this.CheckIsPass())
            {
                if (this.mIsRefreshPlayer)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x25b));
                }
                else
                {
                    this._EnemyRefreshBtn.SetActive(false);
                    SocketMgr.Instance.RequestWarmmatchRefresh();
                }
            }
        }
    }

    private bool GetEnemyStat(BriefEnemy info)
    {
        if (this.mWarmmatchData != null)
        {
            for (int i = 0; i < this.mWarmmatchData.enemy.Count; i++)
            {
                if (info.userInfo.id == this.mWarmmatchData.enemy[i].userInfo.id)
                {
                    return (this.mWarmmatchData.enemy_state[i] == 1);
                }
            }
        }
        return false;
    }

    [DebuggerHidden]
    private IEnumerator InitTargetPlayer(WarmmatchData data)
    {
        return new <InitTargetPlayer>c__Iterator52 { data = data, <$>data = data, <>f__this = this };
    }

    private void OnClickChangeTeam()
    {
        if (<>f__am$cacheF == null)
        {
            <>f__am$cacheF = delegate (GUIEntity entity) {
                SelectHeroPanel panel = (SelectHeroPanel) entity;
                panel.mBattleType = BattleType.WarmmatchDefense;
                panel.SetButtonState(BattleType.WarmmatchDefense);
            };
        }
        GUIMgr.Instance.PushGUIEntity("SelectHeroPanel", <>f__am$cacheF);
    }

    private void OnClickIcon(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            BriefEnemy info = obj2 as BriefEnemy;
            if (info != null)
            {
                if (this.GetEnemyStat(info))
                {
                    TipsDiag.SetText("目标已被击败！");
                }
                else
                {
                    ActorData.getInstance().IsOnlyShowTargetTeam = true;
                    SocketMgr.Instance.RequestWarmmatchTarget(info.userInfo.id);
                }
            }
        }
    }

    private void OnClickPkBtn(GameObject go)
    {
        if (!this.mLockPkBtnEvent && (this.mWarmmatchData != null))
        {
            if (this.mWarmmatchData.pk_times <= 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2802));
            }
            else
            {
                object obj2 = GUIDataHolder.getData(go);
                if (obj2 != null)
                {
                    BriefEnemy enemy = obj2 as BriefEnemy;
                    if (enemy != null)
                    {
                        ActorData.getInstance().IsOnlyShowTargetTeam = false;
                        ActorData.getInstance().mCurrWarmmatchEndTime = this.mEndTime;
                        SocketMgr.Instance.RequestWarmmatchTarget(enemy.userInfo.id);
                        this.mLockPkBtnEvent = true;
                    }
                }
            }
        }
    }

    private void OnClickShuoMing(GameObject go)
    {
        if (<>f__am$cacheE == null)
        {
            <>f__am$cacheE = delegate (GUIEntity obj) {
                WorldCupRulePanel panel = (WorldCupRulePanel) obj;
                panel.Depth = 800;
                panel.ShowWorldCup(false);
            };
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cacheE, null);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.mLockPkBtnEvent = false;
        SocketMgr.Instance.RequestWarmmatch();
        this.UpdateTeamInfo();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(base.transform.FindChild("ShuoMingBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShuoMing);
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        this._RemainTimeLabel.text = string.Empty;
        this._EnemyRefreshTimeLabel.text = string.Empty;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                if (this.CheckIsPass())
                {
                    this.mIsStart = false;
                    this.SetRewardBtnState(true);
                }
                else if (TimeMgr.Instance.ServerStampTime >= this.mEndTime)
                {
                    this.mIsStart = false;
                    SocketMgr.Instance.RequestWarmmatch();
                    if (GUIMgr.Instance.GetGUIEntity<TargetTeamPanel>() != null)
                    {
                        GUIMgr.Instance.ExitModelGUI("TargetTeamPanel");
                    }
                }
                else
                {
                    this._RemainTimeLabel.text = TimeMgr.Instance.GetRemainTime(this.mEndTime);
                }
                if (this.mIsRefreshPlayer)
                {
                    if (TimeMgr.Instance.ServerStampTime < ActorData.getInstance().ArenaRefleshTime)
                    {
                        this._EnemyRefreshTimeLabel.text = TimeMgr.Instance.GetRemainTime(ActorData.getInstance().ArenaRefleshTime);
                    }
                    else
                    {
                        this._EnemyRefreshTimeLabel.text = string.Empty;
                        this.mIsRefreshPlayer = false;
                        this._EnemyRefreshBtn.SetActive(true);
                    }
                }
            }
        }
    }

    public void PickReward()
    {
        if (this.CheckIsPass())
        {
            SocketMgr.Instance.RequestWarmmatchGains();
        }
    }

    private void SetPassIcon(List<byte> _stateList)
    {
        int num = 0;
        for (int i = 0; i < 3; i++)
        {
            if (_stateList[i] > 0)
            {
                num++;
            }
        }
        for (int j = 0; j < 3; j++)
        {
            Transform transform = base.transform.FindChild("RankInfo/LianShen/" + (j + 1) + "/Icon");
            if (j < num)
            {
                transform.gameObject.SetActive(true);
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
    }

    private void SetPkCountLabel(uint pk_times)
    {
        UILabel component = base.transform.FindChild("RankInfo/Count").GetComponent<UILabel>();
        if (pk_times < 0)
        {
            component.text = "0/" + ActorData.getInstance().MaxArenaCount;
        }
        else
        {
            component.text = pk_times + "/" + ActorData.getInstance().MaxArenaCount;
        }
        this._BuyPkCountBtn.gameObject.SetActive(false);
        if (((this.mWarmmatchData != null) && (this.mWarmmatchData.buy_times > 0)) && (pk_times < 1))
        {
            this._BuyPkCountBtn.gameObject.SetActive(true);
        }
    }

    private void SetRewardBtnState(bool _isShow)
    {
        this.isClickSend = false;
        this._RewardBtn.gameObject.SetActive(_isShow);
        this._ShuoMingBtn.gameObject.SetActive(!_isShow);
    }

    private unsafe void SetTargetInfo(Transform obj, BriefEnemy _info, byte _state)
    {
        if (_info != null)
        {
            obj.FindChild("Name").GetComponent<UILabel>().text = _info.userInfo.name;
            obj.FindChild("FightPower").GetComponent<UILabel>().text = _info.power.ToString();
            obj.FindChild("Level").GetComponent<UILabel>().text = _info.userInfo.level.ToString();
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_info.userInfo.head_entry);
            if (_config != null)
            {
                UITexture component = obj.FindChild("Icon").GetComponent<UITexture>();
                component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                GUIDataHolder.setData(component.gameObject, _info);
                UIEventListener.Get(component.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickIcon);
                nguiTextureGrey.doChangeEnableGrey(component, _state > 0);
                obj.FindChild("KO").gameObject.SetActive(_state > 0);
                obj.FindChild("QualityBorder").GetComponent<UISprite>().color = *((Color*) &(GameConstant.ConstQuantityColor[_config.quality]));
                Transform transform = obj.FindChild("PkBtn");
                GUIDataHolder.setData(transform.gameObject, _info);
                UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPkBtn);
                transform.gameObject.SetActive(_state <= 0);
            }
        }
    }

    private void SetTeamInfo(Transform obj, Card _card)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                obj.FindChild("Level").GetComponent<UILabel>().text = _card.cardInfo.level.ToString();
                CommonFunc.SetQualityColor(obj.FindChild("QualityBorder").GetComponent<UISprite>(), _card.cardInfo.quality);
                obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                for (int i = 0; i < 5; i++)
                {
                    UISprite component = obj.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                    component.gameObject.SetActive(i < _card.cardInfo.starLv);
                    component.transform.localPosition = new Vector3((float) (i * 0x17), 0f, 0f);
                }
                Transform transform = obj.transform.FindChild("Star");
                transform.localPosition = new Vector3(-6.8f - ((_card.cardInfo.starLv - 1) * 12.5f), transform.localPosition.y, 0f);
            }
        }
    }

    public void SetTeamPower(List<Card> cardList)
    {
        base.transform.FindChild("Team/FightPower").GetComponent<UILabel>().text = ActorData.getInstance().GetTeamPowerByCardList(cardList).ToString();
    }

    public void UpdatePkCount(WarmmatchData data)
    {
        this.mWarmmatchData = data;
        this.SetPkCountLabel(data.pk_times);
    }

    public void UpdatePlayerList(WarmmatchData data)
    {
        this.m_time = 1f;
        this.isClickSend = false;
        this.mWarmmatchData = data;
        this.SetPkCountLabel(data.pk_times);
        base.StartCoroutine(this.InitTargetPlayer(data));
        this.mEndTime = TimeMgr.Instance.ServerStampTime + data.fresh_time;
        this.mIsStart = true;
        this.mIsRefreshPlayer = true;
        this.SetPassIcon(data.enemy_state);
        this.SetRewardBtnState(false);
    }

    public void UpdateTeamInfo()
    {
        if (ActorData.getInstance().ArenaFormation != null)
        {
            List<Card> cardList = new List<Card>();
            foreach (long num in ActorData.getInstance().ArenaFormation.card_id)
            {
                if (num != -1L)
                {
                    Card cardByID = ActorData.getInstance().GetCardByID(num);
                    if (cardByID != null)
                    {
                        cardList.Add(cardByID);
                    }
                }
            }
            cardList.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
            int num2 = 0;
            for (int i = 0; i < 5; i++)
            {
                Transform transform = base.transform.FindChild("Team/Pos" + (i + 1));
                if (num2 < cardList.Count)
                {
                    this.SetTeamInfo(transform, cardList[num2]);
                    num2++;
                    transform.gameObject.SetActive(true);
                }
                else
                {
                    transform.gameObject.SetActive(false);
                }
            }
            this.SetTeamPower(cardList);
        }
    }

    [CompilerGenerated]
    private sealed class <BuyPkCount>c__AnonStorey16F
    {
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        internal ArenaPanel <>f__this;
        internal MessageBox gui;

        internal void <>m__189(GameObject box)
        {
            if (ActorData.getInstance().Stone < 50)
            {
                string str = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0]);
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                }
                this.gui.SetDialog(str, <>f__am$cache2, null, false);
            }
            else
            {
                uint num = 1;
                vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
                if (_config != null)
                {
                    Debug.Log(_config.warmmatch_pk_times + "----------" + this.<>f__this.mWarmmatchData.buy_times);
                    num = (uint) _config.warmmatch_pk_times;
                }
                SocketMgr.Instance.RequestBuyWarmmatchCount(1);
            }
        }

        private static void <>m__18A(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <InitTargetPlayer>c__Iterator52 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal WarmmatchData <$>data;
        internal ArenaPanel <>f__this;
        internal int <i>__1;
        internal int <idx>__0;
        internal Transform <target>__2;
        internal WarmmatchData data;

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
                    this.<idx>__0 = 0;
                    this.<i>__1 = 0;
                    break;

                case 1:
                    this.<i>__1++;
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_0169;

                default:
                    goto Label_0169;
            }
            if (this.<i>__1 < 3)
            {
                this.<target>__2 = this.<>f__this.transform.FindChild("Target/" + (this.<i>__1 + 1));
                if ((this.<idx>__0 < this.data.enemy.Count) && (this.data.enemy[this.<idx>__0].userInfo.id > 0L))
                {
                    this.<target>__2.gameObject.SetActive(true);
                    this.<>f__this.SetTargetInfo(this.<target>__2, this.data.enemy[this.<idx>__0], this.data.enemy_state[this.<idx>__0]);
                    this.<idx>__0++;
                }
                else
                {
                    this.<target>__2.gameObject.SetActive(false);
                }
                this.$current = new WaitForSeconds(0.01f);
                this.$PC = 1;
            }
            else
            {
                this.$current = null;
                this.$PC = 2;
            }
            return true;
        Label_0169:
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

