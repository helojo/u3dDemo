using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBanHero : GUIEntity
{
    private UILabel _label_enemy_power;
    private UILabel _label_self_power;
    private int self_ban_count;

    public void BanHero(long id, bool ban)
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        if (challengeData.activity_enemy != null)
        {
            int index = challengeData.activity_enemy.cardIdList.IndexOf(id);
            if ((index >= 0) && (index < challengeData.enemy_activity_formation.Length))
            {
                challengeData.enemy_activity_formation[index] = !ban;
                this.self_ban_count++;
                this.Invalidate();
            }
        }
    }

    private void ClearFormation(Transform group)
    {
        if (null != group)
        {
            Transform transform = group.FindChild("Group");
            if (null != transform)
            {
                int childCount = transform.childCount;
                for (int i = 0; i != childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    Transform transform3 = child.FindChild("Card");
                    UISprite component = child.FindChild("IconBg").GetComponent<UISprite>();
                    UISprite sprite2 = child.FindChild("Disable").GetComponent<UISprite>();
                    BoxCollider collider = child.FindChild("Collider").GetComponent<BoxCollider>();
                    transform3.gameObject.SetActive(false);
                    component.gameObject.SetActive(true);
                    sprite2.gameObject.SetActive(false);
                    collider.enabled = false;
                }
                group.FindChild("FightPower").GetComponent<UILabel>().text = string.Empty;
            }
        }
    }

    private bool FighterEnabled(int index, ArenaHeroStatus.Type type)
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        bool[] flagArray = (type != ArenaHeroStatus.Type.Enemy) ? challengeData.self_activity_formation : challengeData.enemy_activity_formation;
        if (index >= flagArray.Length)
        {
            return false;
        }
        return flagArray[index];
    }

    public void Invalidate()
    {
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        if (challengeData.activity_enemy != null)
        {
            Transform group = base.transform.FindChild("Enemy");
            Transform transform2 = base.transform.FindChild("Self");
            List<CardInfo> list = new List<CardInfo>();
            foreach (Card card in ArenaFormationUtility.Identities2CardList(ActorData.getInstance().ChallengeArenaFormation.card_id, true))
            {
                if (card.cardInfo != null)
                {
                    list.Add(card.cardInfo);
                }
            }
            this.RefreshFormation(group, challengeData.activity_enemy.cardList, ArenaHeroStatus.Type.Enemy);
            this.RefreshFormation(transform2, list, ArenaHeroStatus.Type.Self);
        }
    }

    private void OnClickClose(GameObject go)
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    private void OnClickFighter(GameObject go)
    {
        ArenaHeroStatus component = go.GetComponent<ArenaHeroStatus>();
        if ((null != component) && (component.type != ArenaHeroStatus.Type.Self))
        {
            int index = component.status_id;
            ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
            if ((((challengeData.activity_enemy != null) && ((index >= 0) && (index < challengeData.activity_enemy.cardIdList.Count))) && (index < challengeData.enemy_activity_formation.Length)) && challengeData.enemy_activity_formation[index])
            {
                long num2 = challengeData.activity_enemy.cardIdList[index];
                SocketMgr.Instance.RequestBanArenaHero(num2, challengeData.activity_enemy.order);
            }
        }
    }

    private void OnClickStartFight(GameObject go)
    {
        BattleFormation challengeArenaFormation = ActorData.getInstance().ChallengeArenaFormation;
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        if ((challengeData.activity_enemy != null) && (challengeArenaFormation != null))
        {
            if (challengeData.EnemyBanCount < 3)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x89b));
            }
            else
            {
                long targetId = challengeData.activity_enemy.targetId;
                int order = challengeData.activity_enemy.order;
                int num3 = challengeData.activity_enemy.target_type;
                List<Card> list = ArenaFormationUtility.Identities2CardList(challengeArenaFormation.card_id, true);
                List<long> list2 = new List<long>();
                int length = challengeData.self_activity_formation.Length;
                for (int i = 0; i != length; i++)
                {
                    if (challengeData.self_activity_formation[i] && (i < list.Count))
                    {
                        list2.Add(list[i].card_id);
                    }
                }
                SocketMgr.Instance.RequstBeginChallengeArenaCombat(targetId, list2, order, num3);
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        Transform group = base.transform.FindChild("Enemy");
        Transform transform2 = base.transform.FindChild("Self");
        this.ClearFormation(group);
        this.ClearFormation(transform2);
        this.self_ban_count = 0;
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        if (challengeData.activity_enemy != null)
        {
            SocketMgr.Instance.RequestChallengeSelfBanInfo(challengeData.activity_enemy.order);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(base.transform.FindChild("Fight").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickStartFight);
        UIEventListener.Get(base.transform.FindChild("TopLeft/Close").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickClose);
    }

    public virtual void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        Transform group = base.transform.FindChild("Enemy");
        Transform transform2 = base.transform.FindChild("Self");
        this.ClearFormation(group);
        this.ClearFormation(transform2);
    }

    public void RefreshEnemyPower(int power)
    {
        this.EmemyPowerLabel.text = power.ToString();
    }

    private void RefreshFormation(Transform group, List<CardInfo> card_list, ArenaHeroStatus.Type type)
    {
        if (null != group)
        {
            UILabel component = group.FindChild("FightPower").GetComponent<UILabel>();
            Transform transform = group.FindChild("Group");
            if (null != transform)
            {
                ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
                int num = 0;
                int childCount = transform.childCount;
                for (int i = 0; i != childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    Transform transform3 = child.FindChild("Card");
                    UISprite sprite = child.FindChild("IconBg").GetComponent<UISprite>();
                    UISprite sprite2 = child.FindChild("Disable").GetComponent<UISprite>();
                    BoxCollider collider = child.FindChild("Collider").GetComponent<BoxCollider>();
                    if (i >= card_list.Count)
                    {
                        transform3.gameObject.SetActive(false);
                        sprite2.gameObject.SetActive(false);
                        sprite.gameObject.SetActive(true);
                        collider.enabled = false;
                    }
                    else
                    {
                        bool flag = this.FighterEnabled(i, type);
                        if ((type == ArenaHeroStatus.Type.Self) && !flag)
                        {
                            if (num >= this.self_ban_count)
                            {
                                flag = true;
                            }
                            num++;
                        }
                        transform3.gameObject.SetActive(true);
                        sprite.gameObject.SetActive(false);
                        ArenaFormationUtility.RefreshFighterUIObject(card_list[i], child);
                        UITexture texture = transform3.FindChild("Icon").GetComponent<UITexture>();
                        sprite2.gameObject.SetActive(!flag);
                        nguiTextureGrey.doChangeEnableGrey(texture, !flag);
                        collider.enabled = flag;
                        if (type == ArenaHeroStatus.Type.Enemy)
                        {
                            UIEventListener.Get(collider.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickFighter);
                        }
                        else
                        {
                            collider.enabled = false;
                        }
                        ArenaHeroStatus status = collider.GetComponent<ArenaHeroStatus>();
                        if (null == status)
                        {
                            status = collider.gameObject.AddComponent<ArenaHeroStatus>();
                        }
                        status.type = type;
                        status.status_id = i;
                    }
                }
            }
        }
    }

    public void RefreshSelfPower(int power)
    {
        this.SelfPowerLabel.text = power.ToString();
    }

    private UILabel EmemyPowerLabel
    {
        get
        {
            if (null == this._label_enemy_power)
            {
                this._label_enemy_power = base.transform.FindChild("Enemy/FightPower").GetComponent<UILabel>();
            }
            return this._label_enemy_power;
        }
    }

    private UILabel SelfPowerLabel
    {
        get
        {
            if (null == this._label_self_power)
            {
                this._label_self_power = base.transform.FindChild("Self/FightPower").GetComponent<UILabel>();
            }
            return this._label_self_power;
        }
    }
}

