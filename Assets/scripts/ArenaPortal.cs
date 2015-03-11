using HutongGames.PlayMaker.Actions;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArenaPortal : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache0;
    [CompilerGenerated]
    private static Predicate<long> <>f__am$cache1;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3;

    private void OnClickChallenge(GameObject go)
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().lol_arena)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().lol_arena));
        }
        else
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = e => e >= 0L;
            }
            if (ActorData.getInstance().ChallengeArenaFormation.card_id.FindAll(<>f__am$cache1).Count < 8)
            {
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = delegate (GUIEntity e) {
                        SelectHeroPanel panel = e as SelectHeroPanel;
                        panel.mBattleType = BattleType.ChallengeDefense;
                        panel.SetButtonState(BattleType.ChallengeDefense);
                        panel.SetZhuZhanStat(false);
                    };
                }
                GUIMgr.Instance.DoModelGUI<SelectHeroPanel>(<>f__am$cache2, null);
            }
            else
            {
                ChallengeArenaPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ChallengeArenaPanel>();
                if ((null != gUIEntity) && gUIEntity.Hidden)
                {
                    if (<>f__am$cache3 == null)
                    {
                        <>f__am$cache3 = e => SocketMgr.Instance.RequestChallengeArenaInfo();
                    }
                    GUIMgr.Instance.PushGUIEntity<ChallengeArenaPanel>(<>f__am$cache3);
                }
                else
                {
                    GUIMgr.Instance.PushGUIEntity<ChallengeArenaPanel>(null);
                }
            }
        }
    }

    private void OnClickCloseButton(GameObject go)
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    private void OnClickRoutine(GameObject go)
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().arena_ladder)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().arena_ladder));
        }
        else
        {
            ArenaLadderPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ArenaLadderPanel>();
            if ((null != gUIEntity) && gUIEntity.Hidden)
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = e => SocketMgr.Instance.RequestArenaLadderInfo();
                }
                GUIMgr.Instance.PushGUIEntity<ArenaLadderPanel>(<>f__am$cache0);
            }
            else
            {
                GUIMgr.Instance.PushGUIEntity<ArenaLadderPanel>(null);
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
        SocketMgr.Instance.RequestAllArenaRank();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener listener1 = UIEventListener.Get(base.transform.FindChild("TopLeft/Close").gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickCloseButton));
        UIEventListener listener2 = UIEventListener.Get(base.transform.FindChild("Left/Collider").gameObject);
        listener2.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener2.onClick, new UIEventListener.VoidDelegate(this.OnClickChallenge));
        UIEventListener listener3 = UIEventListener.Get(base.transform.FindChild("Right/Collider").gameObject);
        listener3.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener3.onClick, new UIEventListener.VoidDelegate(this.OnClickRoutine));
    }

    public void RefreshRankInfo(int rank_routine, int rank_challenge)
    {
        Transform transform = base.transform.FindChild("Left");
        Transform transform2 = base.transform.FindChild("Left/Title");
        Transform transform3 = base.transform.FindChild("Right/Title");
        UILabel component = base.transform.FindChild("Left/Rank").GetComponent<UILabel>();
        UILabel label2 = base.transform.FindChild("Right/Rank").GetComponent<UILabel>();
        transform3.gameObject.SetActive(true);
        label2.gameObject.SetActive(true);
        bool flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().lol_arena;
        nguiTextureGrey.doChangeEnableGrey(transform.FindChild("Texture").GetComponent<UITexture>(), !flag);
        transform2.gameObject.SetActive(flag);
        component.gameObject.SetActive(flag);
        component.text = (rank_challenge >= 1) ? rank_challenge.ToString() : ConfigMgr.getInstance().GetWord(6);
        label2.text = rank_routine.ToString();
    }
}

