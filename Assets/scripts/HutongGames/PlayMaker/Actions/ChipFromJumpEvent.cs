namespace HutongGames.PlayMaker.Actions
{
    using FastBuf;
    using HutongGames.PlayMaker;
    using System;
    using System.Runtime.CompilerServices;

    [Tooltip("Gets the event that caused the transition to the current state, and stores it in a String Variable."), ActionCategory(ActionCategory.StateMachine)]
    public class ChipFromJumpEvent : FsmStateAction
    {
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache1;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache2;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache3;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache4;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache5;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache6;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache7;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache8;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache9;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cacheA;

        public override void OnEnter()
        {
            if (ActorData.getInstance().mCurrDupReturnPrePara != null)
            {
                switch (ActorData.getInstance().mCurrDupReturnPrePara.enterDuptype)
                {
                    case EnterDupType.From_EquipBreak:
                        if (<>f__am$cache4 == null)
                        {
                            <>f__am$cache4 = delegate (GUIEntity entity) {
                                HeroPanel panel = (HeroPanel) entity;
                                if (<>f__am$cache9 == null)
                                {
                                    <>f__am$cache9 = delegate (GUIEntity obj) {
                                        SocketMgr.Instance.RequestGetSkillPoint();
                                        HeroInfoPanel panel = (HeroInfoPanel) obj;
                                        panel.InitCardInfo(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo);
                                        panel.SetCurrShowCardList(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoShowCardList);
                                        if (<>f__am$cacheA == null)
                                        {
                                            <>f__am$cacheA = beObj => ((BreakEquipPanel) beObj).ResetDupJumpStat(ActorData.getInstance().mCurrDupReturnPrePara);
                                        }
                                        GUIMgr.Instance.PushGUIEntity("BreakEquipPanel", <>f__am$cacheA);
                                    };
                                }
                                GUIMgr.Instance.PushGUIEntity("HeroInfoPanel", <>f__am$cache9);
                            };
                        }
                        GUIMgr.Instance.PushGUIEntity("HeroPanel", <>f__am$cache4);
                        break;

                    case EnterDupType.From_HeroInfoPanel:
                        if (<>f__am$cache3 == null)
                        {
                            <>f__am$cache3 = delegate (GUIEntity entity) {
                                if (<>f__am$cache8 == null)
                                {
                                    <>f__am$cache8 = delegate (GUIEntity obj) {
                                        HeroInfoPanel panel = (HeroInfoPanel) obj;
                                        if (ActorData.getInstance().mCurrDupReturnPrePara.mIsCardPartInfo)
                                        {
                                            int entry = (int) ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo.cardInfo.entry;
                                            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(entry);
                                            if (_config != null)
                                            {
                                                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(entry, _config.evolve_lv);
                                                if (cardExCfg != null)
                                                {
                                                    panel.ShowCardPartInfo(cardExCfg);
                                                    panel.ShowFromTab();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            SocketMgr.Instance.RequestGetSkillPoint();
                                            panel.InitCardInfo(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoPanelCardInfo);
                                            panel.SetCurrShowCardList(ActorData.getInstance().mCurrDupReturnPrePara.heroInfoShowCardList);
                                            panel.ShowFromTab();
                                        }
                                    };
                                }
                                GUIMgr.Instance.PushGUIEntity("HeroInfoPanel", <>f__am$cache8);
                            };
                        }
                        GUIMgr.Instance.PushGUIEntity("HeroPanel", <>f__am$cache3);
                        break;

                    case EnterDupType.From_HeroPanel:
                        if (<>f__am$cache2 == null)
                        {
                            <>f__am$cache2 = delegate (GUIEntity entity) {
                                if (<>f__am$cache7 == null)
                                {
                                    <>f__am$cache7 = obj => ((DetailsPanel) obj).InitList(ActorData.getInstance().mCurrDupReturnPrePara.heroPanelPartEntry);
                                }
                                GUIMgr.Instance.PushGUIEntity("DetailsPanel", <>f__am$cache7);
                            };
                        }
                        GUIMgr.Instance.PushGUIEntity("HeroPanel", <>f__am$cache2);
                        break;

                    case EnterDupType.From_BagDetails:
                        if (<>f__am$cache0 == null)
                        {
                            <>f__am$cache0 = delegate (GUIEntity entity) {
                                BagPanel panel = (BagPanel) entity;
                                panel.CreateItemList(true);
                                panel.InitItemList(ShowItemType.All);
                                if (<>f__am$cache5 == null)
                                {
                                    <>f__am$cache5 = delegate (GUIEntity obj) {
                                        DetailsPanel panel = (DetailsPanel) obj;
                                        Item item = new Item {
                                            entry = ActorData.getInstance().mCurrDupReturnPrePara.heroPanelPartEntry
                                        };
                                        panel.InitItemDetails(item, EnterDupType.From_BagDetails);
                                    };
                                }
                                GUIMgr.Instance.PushGUIEntity("DetailsPanel", <>f__am$cache5);
                            };
                        }
                        GUIMgr.Instance.PushGUIEntity("BagPanel", <>f__am$cache0);
                        break;

                    case EnterDupType.From_FragmentBagDetails:
                        if (<>f__am$cache1 == null)
                        {
                            <>f__am$cache1 = delegate (GUIEntity entity) {
                                ((FragmentBagPanel) entity).CreateItemList();
                                if (<>f__am$cache6 == null)
                                {
                                    <>f__am$cache6 = delegate (GUIEntity obj) {
                                        DetailsPanel panel = (DetailsPanel) obj;
                                        Item item = new Item {
                                            entry = ActorData.getInstance().mCurrDupReturnPrePara.heroPanelPartEntry
                                        };
                                        panel.InitItemDetails(item, EnterDupType.From_FragmentBagDetails);
                                    };
                                }
                                GUIMgr.Instance.PushGUIEntity("DetailsPanel", <>f__am$cache6);
                            };
                        }
                        GUIMgr.Instance.PushGUIEntity("FragmentBagPanel", <>f__am$cache1);
                        break;
                }
            }
        }

        [UIHint(UIHint.Variable)]
        public override void Reset()
        {
        }
    }
}

