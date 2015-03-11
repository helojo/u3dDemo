using FastBuf;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChangeTeamPanel : GUIEntity
{
    private long AssistID = -1L;
    private List<GameObject> CardList = new List<GameObject>();
    public GameObject HeroIcon;
    private bool InAssistPage;
    private bool InitDefaultData;
    private const int LineCount = 6;
    public UIGrid ListRoot;
    private const int MaxCardCnt = 5;
    private BattleType mCurType;
    private const int RowCount = 2;
    private float ScaleParam = 1.3f;
    private List<Card> SelectHeroList = new List<Card>();
    public List<GameObject> SlotList = new List<GameObject>();

    private GameObject CreateSelItem(GameObject _obj, Card _card, bool _playAnim, out int Oindex)
    {
        GameObject go = UnityEngine.Object.Instantiate(this.HeroIcon) as GameObject;
        int nullSlot = this.GetNullSlot();
        go.name = "HeroIcon";
        go.transform.parent = this.SlotList[nullSlot].transform;
        go.transform.localScale = Vector3.one;
        if (_playAnim)
        {
            go.transform.position = _obj.transform.position;
        }
        else
        {
            go.transform.localPosition = Vector3.zero;
        }
        GUIDataHolder.setData(go, _card);
        UIEventListener listener1 = UIEventListener.Get(go);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickPopHero));
        this.UpdateHeroIconData(go, _card, null);
        this.SelectHeroList.Add(_card);
        GUIDataHolder.setData(this.SlotList[nullSlot], _card);
        Oindex = nullSlot;
        return go;
    }

    private int GetNullSlot()
    {
        int num = 0;
        foreach (GameObject obj2 in this.SlotList)
        {
            if (GUIDataHolder.getData(obj2) == null)
            {
                return num;
            }
            num++;
        }
        return 0;
    }

    private bool HasProcess(GameObject _obj, List<GameObject> _list)
    {
        foreach (GameObject obj2 in _list)
        {
            if (_obj == obj2)
            {
                return true;
            }
        }
        return false;
    }

    private void HideStar(GameObject obj)
    {
        for (int i = 1; i <= 5; i++)
        {
            obj.transform.FindChild("Star/" + i).gameObject.SetActive(false);
        }
    }

    private void OnClickHeroIcon(GameObject obj)
    {
        Card item = (Card) GUIDataHolder.getData(obj);
        if (ActorData.getInstance().GetCardByID(item.card_id) == null)
        {
            if ((this.AssistID > 0L) && !this.SelectHeroList.Contains(item))
            {
                TipsDiag.SetText("选择的人数已满!");
                return;
            }
        }
        else if (this.AssistID > 0L)
        {
            if ((this.SelectHeroList.Count >= 6) && !this.SelectHeroList.Contains(item))
            {
                TipsDiag.SetText("选择的人数已满!");
                return;
            }
        }
        else if ((this.SelectHeroList.Count >= 5) && !this.SelectHeroList.Contains(item))
        {
            TipsDiag.SetText("选择的人数已满!");
            return;
        }
        foreach (GameObject obj2 in this.SlotList)
        {
            object obj3 = GUIDataHolder.getData(obj2);
            if (obj3 != null)
            {
                Card card3 = (Card) obj3;
                if (item.card_id == card3.card_id)
                {
                    this.PopCard(obj, item);
                    return;
                }
            }
        }
        this.PushCard(obj, item);
    }

    private void OnClickPopHero(GameObject obj)
    {
        Card card = (Card) GUIDataHolder.getData(obj);
        this.PopCard(obj, card);
    }

    private void OnClickSaveBtn()
    {
        List<long> memberId = new List<long>();
        foreach (Card card in this.SelectHeroList)
        {
            memberId.Add(card.card_id);
            Debug.Log(card.cardInfo.entry);
        }
        if (memberId.Count > 0)
        {
            if (this.mCurType == BattleType.WarmmatchPk)
            {
                SocketMgr.Instance.RequestSetBattleMember(memberId, BattleFormationType.BattleFormationType_Arena_Def);
            }
            else if (this.mCurType == BattleType.WorldCupPk)
            {
                SocketMgr.Instance.RequestSetBattleMember(memberId, BattleFormationType.BattleFormationType_League_Def);
            }
            GUIMgr.Instance.PopGUIEntity();
        }
        else
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x1d));
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.ShowFuncList(false);
    }

    public override void OnInitialize()
    {
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        this.ShowFuncList(true);
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
    }

    [DebuggerHidden]
    private IEnumerator PlayAnimAndDestroyObj(GameObject obj)
    {
        return new <PlayAnimAndDestroyObj>c__Iterator54 { obj = obj, <$>obj = obj };
    }

    private void PopCard(GameObject _obj, Card _card)
    {
        GameObject obj2 = null;
        foreach (GameObject obj3 in this.CardList)
        {
            Card card = (Card) GUIDataHolder.getData(obj3);
            if (card.card_id == _card.card_id)
            {
                obj2 = obj3;
            }
        }
        foreach (GameObject obj4 in this.SlotList)
        {
            object obj5 = GUIDataHolder.getData(obj4);
            if (obj5 != null)
            {
                Card card2 = (Card) obj5;
                if (card2.card_id == _card.card_id)
                {
                    GameObject obj6 = UnityEngine.Object.Instantiate(this.HeroIcon) as GameObject;
                    obj6.transform.parent = obj4.transform.parent;
                    obj6.transform.position = obj4.transform.position;
                    obj6.transform.localScale = Vector3.one;
                    this.UpdateHeroIconData(obj6, _card, null);
                    UnityEngine.Object.Destroy(obj4.transform.FindChild("HeroIcon").gameObject);
                    GUIDataHolder.setData(obj4, null);
                    if (obj2 == null)
                    {
                        TweenTransform.Begin(obj6, 0.3f, this.ListRoot.transform).method = UITweener.Method.EaseOut;
                    }
                    else
                    {
                        TweenTransform.Begin(obj6, 0.3f, obj2.transform).method = UITweener.Method.EaseOut;
                    }
                    base.StartCoroutine(this.PlayAnimAndDestroyObj(obj6));
                    if (ActorData.getInstance().GetCardByID(_card.card_id) == null)
                    {
                        this.AssistID = -1L;
                    }
                }
            }
        }
        this.ResetSlotPos();
        this.SelectHeroList.Remove(_card);
        this.UpdateBufferState();
    }

    private void PushCard(GameObject _obj, Card _card)
    {
        Card cardByID = ActorData.getInstance().GetCardByID(_card.card_id);
        if ((this.AssistID > 0L) && (cardByID == null))
        {
            TipsDiag.SetText("只能选择一张助战卡牌!");
        }
        else
        {
            int oindex = 0;
            TweenTransform.Begin(this.CreateSelItem(_obj, _card, true, out oindex), 0.3f, this.SlotList[oindex].transform).method = UITweener.Method.EaseOut;
            if (cardByID == null)
            {
                this.AssistID = _card.card_id;
            }
            this.UpdateBufferState();
        }
    }

    private void ResetPanel(Transform tf)
    {
        SpringPanel component = tf.GetComponent<SpringPanel>();
        if (component != null)
        {
            component.target = Vector3.zero;
            component.enabled = false;
        }
        tf.GetComponent<ResetListPos>().PanelReset();
    }

    private void ResetSlot()
    {
        foreach (GameObject obj2 in this.SlotList)
        {
            Transform transform = obj2.transform.FindChild("HeroIcon");
            GUIDataHolder.setData(obj2, null);
            if (null != transform)
            {
                UnityEngine.Object.DestroyImmediate(transform.gameObject);
            }
        }
    }

    private void ResetSlotPos()
    {
        List<GameObject> list = new List<GameObject>();
        int num = 0;
        int num2 = 0;
        foreach (GameObject obj3 in this.SlotList)
        {
            num++;
            num2 = 0;
            if (GUIDataHolder.getData(obj3) == null)
            {
                foreach (GameObject obj5 in this.SlotList)
                {
                    num2++;
                    if ((num2 > num) && !this.HasProcess(obj5, list))
                    {
                        object data = GUIDataHolder.getData(obj5);
                        if (data != null)
                        {
                            GameObject gameObject = obj5.transform.FindChild("HeroIcon").gameObject;
                            gameObject.transform.parent = obj3.transform;
                            TweenPosition.Begin(gameObject, 0.3f, Vector3.zero).method = UITweener.Method.EaseOut;
                            GUIDataHolder.setData(obj3, data);
                            GUIDataHolder.setData(obj5, null);
                            list.Add(obj3);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void SetListState(GameObject _obj, bool _showMask)
    {
        nguiTextureGrey.doChangeEnableGrey(_obj.transform.FindChild("Icon").GetComponent<UITexture>(), _showMask);
    }

    public void SetLSXPkTargetInfo(S2C_WarmmatchTargetReq info)
    {
        this.mCurType = BattleType.WarmmatchPk;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
    }

    public void SetPkTargetInfo(S2C_GetFriendFormation info)
    {
        this.mCurType = BattleType.FriendPk;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
    }

    public void SetTeamPower(List<Card> cardList)
    {
        base.transform.FindChild("TeamPower").GetComponent<UILabel>().text = ActorData.getInstance().GetTeamPowerByCardList(cardList).ToString();
    }

    public void SetTowerInfo(VoidTowerData info)
    {
        this.mCurType = BattleType.TowerPk;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
    }

    public void SetWorldCupPkTargetInfo(S2C_GetLeagueOpponentFormation info)
    {
        this.mCurType = BattleType.WorldCupPk;
        GUIDataHolder.setData(base.transform.FindChild("StartBtn").gameObject, info);
    }

    private void ShowFuncList(bool _show)
    {
        TitleBar gUIEntity = GUIMgr.Instance.GetGUIEntity<TitleBar>();
        if (null != gUIEntity)
        {
            gUIEntity.ShowFuncBar(_show);
        }
    }

    private void StartFight()
    {
        List<long> cardGUIDs = new List<long>();
        ActorData.getInstance().mDefaultDupHeroList.Clear();
        foreach (Card card in this.SelectHeroList)
        {
            cardGUIDs.Add(card.card_id);
            ActorData.getInstance().mDefaultDupHeroList.Add(card);
        }
        ActorData.getInstance().SaveFormationData(BattleType.Normal);
        if (cardGUIDs.Count != 0)
        {
            Transform transform = base.transform.FindChild("StartBtn");
            switch (this.mCurType)
            {
                case BattleType.Normal:
                    SocketMgr.Instance.RequestEnterDup(ActorData.getInstance().CurDupEntry, ActorData.getInstance().CurTrenchEntry, ActorData.getInstance().CurDupType, cardGUIDs, this.AssistID);
                    break;

                case BattleType.WorldBoss:
                    SocketMgr.Instance.RequestWorldBossCombat(0, cardGUIDs);
                    break;

                case BattleType.FriendPk:
                {
                    object obj2 = GUIDataHolder.getData(transform.gameObject);
                    if (obj2 != null)
                    {
                        S2C_GetFriendFormation formation = obj2 as S2C_GetFriendFormation;
                        SocketMgr.Instance.RequestPrepareFriendCombat(cardGUIDs);
                    }
                    break;
                }
                case BattleType.WorldCupPk:
                {
                    object obj3 = GUIDataHolder.getData(transform.gameObject);
                    if (obj3 != null)
                    {
                        S2C_GetLeagueOpponentFormation formation2 = obj3 as S2C_GetLeagueOpponentFormation;
                        if ((ActorData.getInstance().LeagueOpponentInfo.userInfo.id == formation2.targetId) && (ActorData.getInstance().JoinLeagueInfo != null))
                        {
                            SocketMgr.Instance.RequestPrepareLeagueFight(formation2.leagueEntry, formation2.groupId, ActorData.getInstance().CurrWorldCupPkTargetId, cardGUIDs, ActorData.getInstance().LeagueOpponentInfo.rank, ActorData.getInstance().JoinLeagueInfo.rank, false);
                        }
                    }
                    break;
                }
                case BattleType.WarmmatchPk:
                {
                    object obj4 = GUIDataHolder.getData(transform.gameObject);
                    if (obj4 != null)
                    {
                        S2C_WarmmatchTargetReq req = obj4 as S2C_WarmmatchTargetReq;
                        SocketMgr.Instance.RequestWarmmatchCombat(req.target_id, cardGUIDs);
                    }
                    break;
                }
                case BattleType.TowerPk:
                {
                    object obj5 = GUIDataHolder.getData(transform.gameObject);
                    if (obj5 != null)
                    {
                        VoidTowerData data = obj5 as VoidTowerData;
                        SocketMgr.Instance.RequestVoidTowerCombat(cardGUIDs);
                    }
                    break;
                }
            }
        }
    }

    private void TestFight()
    {
        List<long> cardGUIDs = new List<long>();
        foreach (Card card in this.SelectHeroList)
        {
            cardGUIDs.Add(card.card_id);
        }
        if (cardGUIDs.Count != 0)
        {
            SocketMgr.Instance.RequestEnterDup(0, 1, DuplicateType.DupType_Normal, cardGUIDs, -1L);
        }
    }

    private void UpdateAssistCardList()
    {
        int num = 0;
        int num2 = 0;
        GameObject obj2 = null;
        this.ResetPanel(this.ListRoot.transform.parent);
        CommonFunc.DeleteChildItem(this.ListRoot.transform);
        this.CardList.Clear();
        this.InAssistPage = true;
        foreach (AssistUser user in ActorData.getInstance().AssistUserList)
        {
            Card data = new Card();
            data = user.userInfo.leaderInfo;
            data.card_id = user.userInfo.id;
            if ((num == 0) || ((num % 6) == 0))
            {
                obj2 = new GameObject();
                obj2.name = (num2 + 1).ToString();
                obj2.transform.parent = this.ListRoot.transform;
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.localScale = Vector3.one;
                num = 0;
                num2++;
            }
            GameObject item = UnityEngine.Object.Instantiate(this.HeroIcon) as GameObject;
            item.transform.parent = obj2.transform;
            item.transform.localPosition = new Vector3((float) (-343 + (num * 130)), 0f, 0f);
            item.transform.localScale = new Vector3(this.ScaleParam, this.ScaleParam, 1f);
            this.CardList.Add(item);
            GUIDataHolder.setData(item, data);
            UIEventListener listener1 = UIEventListener.Get(item);
            listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickHeroIcon));
            this.UpdateHeroIconData(item, data, user);
            num++;
        }
        UIScrollView component = this.ListRoot.transform.parent.GetComponent<UIScrollView>();
        if (num2 <= 2)
        {
            component.movement = UIScrollView.Movement.Custom;
            component.customMovement = Vector2.zero;
        }
        else
        {
            component.movement = UIScrollView.Movement.Vertical;
        }
        this.UpdateBufferState();
        this.ListRoot.repositionNow = true;
    }

    private void UpdateBufferState()
    {
        this.SetTeamPower(this.SelectHeroList);
        foreach (GameObject obj2 in this.CardList)
        {
            Card item = (Card) GUIDataHolder.getData(obj2);
            if (this.SelectHeroList.Contains(item))
            {
                this.SetListState(obj2, true);
            }
            else
            {
                this.SetListState(obj2, false);
            }
        }
    }

    public void UpdateCardList()
    {
        this.InAssistPage = false;
        PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
        if (component != null)
        {
            this.ResetPanel(this.ListRoot.transform.parent);
            FsmInt num = component.FsmVariables.FindFsmInt("StandType");
            int num2 = 0;
            int num3 = 0;
            GameObject obj2 = null;
            CommonFunc.DeleteChildItem(this.ListRoot.transform);
            this.CardList.Clear();
            foreach (Card card in ActorData.getInstance().CardList)
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.cardInfo.entry);
                if ((num.Value == 0) || (num.Value == _config.stand_type))
                {
                    if ((num2 == 0) || ((num2 % 6) == 0))
                    {
                        obj2 = new GameObject();
                        obj2.name = (num3 + 1).ToString();
                        obj2.transform.parent = this.ListRoot.transform;
                        obj2.transform.localPosition = Vector3.zero;
                        obj2.transform.localScale = Vector3.one;
                        num2 = 0;
                        num3++;
                    }
                    GameObject item = UnityEngine.Object.Instantiate(this.HeroIcon) as GameObject;
                    item.transform.parent = obj2.transform;
                    item.transform.localPosition = new Vector3((float) (-343 + (num2 * 130)), 0f, 0f);
                    item.transform.localScale = new Vector3(this.ScaleParam, this.ScaleParam, 1f);
                    this.CardList.Add(item);
                    GUIDataHolder.setData(item, card);
                    UIEventListener listener1 = UIEventListener.Get(item);
                    listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickHeroIcon));
                    this.UpdateHeroIconData(item, card, null);
                    if (!this.InitDefaultData)
                    {
                        switch (this.mCurType)
                        {
                            case BattleType.FriendPk:
                            case BattleType.WarmmatchPk:
                                if (ActorData.getInstance().ArenaFormation.card_id.Contains(card.card_id))
                                {
                                    int oindex = 0;
                                    this.CreateSelItem(item, card, false, out oindex);
                                }
                                break;

                            case BattleType.WorldCupPk:
                                if (ActorData.getInstance().WorldCupFormation.card_id.Contains(card.card_id))
                                {
                                    int num5 = 0;
                                    this.CreateSelItem(item, card, false, out num5);
                                }
                                break;
                        }
                    }
                    num2++;
                }
            }
            this.InitDefaultData = true;
            UIScrollView view = this.ListRoot.transform.parent.GetComponent<UIScrollView>();
            if (num3 <= 2)
            {
                view.movement = UIScrollView.Movement.Custom;
                view.customMovement = Vector2.zero;
            }
            else
            {
                view.movement = UIScrollView.Movement.Vertical;
            }
            this.UpdateBufferState();
            this.ListRoot.repositionNow = true;
        }
    }

    public void UpdateData(BattleType _type)
    {
        this.mCurType = _type;
        this.InitDefaultData = false;
        this.SelectHeroList.Clear();
        this.ResetSlot();
        PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
        if (component != null)
        {
            component.FsmVariables.FindFsmInt("StandType").Value = 0;
            base.transform.FindChild("CheckPart/Toggle1").GetComponent<UIToggle>().isChecked = true;
            this.UpdateCardList();
            Debug.Log("111111111");
        }
    }

    private void UpdateHeroIconData(GameObject _obj, Card _data, AssistUser _UserData)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _data.cardInfo.entry);
        if (_config == null)
        {
            Debug.LogWarning("CardCfg Is Null! Entry is " + _data.cardInfo.entry);
        }
        else
        {
            UITexture component = _obj.transform.FindChild("Icon").GetComponent<UITexture>();
            UILabel label = _obj.transform.FindChild("Label").GetComponent<UILabel>();
            UILabel label2 = _obj.transform.FindChild("name").GetComponent<UILabel>();
            UILabel label3 = _obj.transform.FindChild("FriendPoint/FriendPoint").GetComponent<UILabel>();
            UISprite sprite = _obj.transform.FindChild("frame").GetComponent<UISprite>();
            GameObject gameObject = _obj.transform.FindChild("Star").gameObject;
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            label.text = _data.cardInfo.level.ToString();
            CommonFunc.SetQualityColor(sprite, _data.cardInfo.quality);
            this.HideStar(_obj);
            for (int i = 1; i <= _data.cardInfo.starLv; i++)
            {
                _obj.transform.FindChild("Star/" + i).gameObject.SetActive(true);
            }
            if (_UserData != null)
            {
                label3.text = "+" + _UserData.reward_eq;
                label2.text = _UserData.userInfo.name;
                _obj.transform.FindChild("FriendPoint").gameObject.SetActive(true);
            }
            else
            {
                label3.text = string.Empty;
                label2.text = string.Empty;
                _obj.transform.FindChild("FriendPoint").gameObject.SetActive(false);
            }
        }
    }

    public BattleType BType
    {
        set
        {
            this.mCurType = value;
        }
    }

    [CompilerGenerated]
    private sealed class <PlayAnimAndDestroyObj>c__Iterator54 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <$>obj;
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
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    return true;

                case 1:
                    UnityEngine.Object.Destroy(this.obj);
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

