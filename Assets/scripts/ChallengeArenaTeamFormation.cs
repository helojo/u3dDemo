using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChallengeArenaTeamFormation : MonoBehaviour
{
    private UILabel _label_cooldown;
    private Transform _tns_cooldown_group;
    private Transform _tns_modify_button;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3;

    public void Invalidate()
    {
        UIEventListener.Get(base.transform.FindChild("Modify").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickModifyButton);
        this.RefreshFormation(ActorData.getInstance().ChallengeArenaFormation);
    }

    private void OnClickModifyButton(GameObject go)
    {
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = delegate (GUIEntity e) {
                SelectHeroPanel panel = e as SelectHeroPanel;
                panel.mBattleType = BattleType.ChallengeDefense;
                panel.SetButtonState(BattleType.ChallengeDefense);
                panel.SetZhuZhanStat(false);
            };
        }
        GUIMgr.Instance.DoModelGUI<SelectHeroPanel>(<>f__am$cache3, null);
    }

    public void RefreshCoolDown()
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        if (challengeData.modify_time > TimeMgr.Instance.ServerStampTime)
        {
            this.ModifyButtonTransform.gameObject.SetActive(false);
            this.CooldownGroupTransform.gameObject.SetActive(true);
            int num = challengeData.modify_time - TimeMgr.Instance.ServerStampTime;
            this.CooldownLabel.text = string.Format("{0}:{1}", (num / 60).ToString("D2"), (num % 60).ToString("D2"));
        }
        else
        {
            this.ModifyButtonTransform.gameObject.SetActive(true);
            this.CooldownGroupTransform.gameObject.SetActive(false);
        }
    }

    private void RefreshFormation(BattleFormation formation)
    {
        if (formation != null)
        {
            Transform transform = base.transform.FindChild("Group");
            if (null != transform)
            {
                List<Card> cardList = ArenaFormationUtility.Identities2CardList(formation.card_id, true);
                int childCount = transform.childCount;
                for (int i = 0; i != childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if (null != child)
                    {
                        ArenaFormationUtility.RefreshFighterUIObject(((i >= 0) && (i < cardList.Count)) ? cardList[i] : null, child);
                    }
                }
                base.transform.FindChild("FightPower").GetComponent<UILabel>().text = ActorData.getInstance().GetTeamPowerByCardList(cardList).ToString();
            }
        }
    }

    private Transform CooldownGroupTransform
    {
        get
        {
            if (null == this._tns_cooldown_group)
            {
                this._tns_cooldown_group = base.transform.FindChild("CoolDown");
            }
            return this._tns_cooldown_group;
        }
    }

    private UILabel CooldownLabel
    {
        get
        {
            if (null == this._label_cooldown)
            {
                this._label_cooldown = this.CooldownGroupTransform.FindChild("Cooldown").GetComponent<UILabel>();
            }
            return this._label_cooldown;
        }
    }

    private Transform ModifyButtonTransform
    {
        get
        {
            if (null == this._tns_modify_button)
            {
                this._tns_modify_button = base.transform.FindChild("Modify");
            }
            return this._tns_modify_button;
        }
    }
}

