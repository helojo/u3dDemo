using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ArenaFormationUtility
{
    public static List<Card> GetFighterFormation()
    {
        BattleFormation challengeArenaFormation = ActorData.getInstance().ChallengeArenaFormation;
        ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
        int length = challengeData.self_activity_formation.Length;
        List<Card> list = new List<Card>();
        List<Card> list2 = Identities2CardList(challengeArenaFormation.card_id, true);
        for (int i = 0; i != length; i++)
        {
            if (challengeData.self_activity_formation[i] && (i < list2.Count))
            {
                long num3 = list2[i].card_id;
                if (num3 > 0L)
                {
                    Card cardByID = ActorData.getInstance().GetCardByID(num3);
                    if (cardByID != null)
                    {
                        list.Add(cardByID);
                    }
                }
            }
        }
        return list;
    }

    public static List<Card> Identities2CardList(List<long> id_list, bool sort = true)
    {
        <Identities2CardList>c__AnonStorey16C storeyc = new <Identities2CardList>c__AnonStorey16C {
            card_list = new List<Card>()
        };
        id_list.ForEach(new Action<long>(storeyc.<>m__179));
        if (sort)
        {
            storeyc.card_list.Sort(new Comparison<Card>(CommonFunc.SortByPosition));
        }
        return storeyc.card_list;
    }

    public static void RefreshFighterUIObject(Card card, Transform body)
    {
        if (null != body)
        {
            Transform transform = body.FindChild("Card");
            if (card == null)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                RefreshFighterUIObject(card.cardInfo, body);
            }
        }
    }

    public static void RefreshFighterUIObject(CardInfo info, Transform body)
    {
        Transform transform = body.FindChild("Card");
        Transform transform2 = body.FindChild("IconBg");
        if (info == null)
        {
            transform.gameObject.SetActive(false);
        }
        else
        {
            int entry = (int) info.entry;
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(entry);
            if (_config == null)
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                transform.gameObject.SetActive(true);
                UILabel component = transform.FindChild("Level").GetComponent<UILabel>();
                UISprite sprite = transform.FindChild("QualityBorder").GetComponent<UISprite>();
                UITexture texture = transform.FindChild("Icon").GetComponent<UITexture>();
                UISprite icon = transform.FindChild("Job/jobIcon").GetComponent<UISprite>();
                component.text = info.level.ToString();
                texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                CommonFunc.SetQualityBorder(sprite, info.quality);
                CommonFunc.SetCardJobIcon(icon, _config.class_type);
                Transform transform3 = transform.FindChild("Star");
                transform3.gameObject.SetActive(true);
                for (int i = 0; i != 5; i++)
                {
                    int num3 = i + 1;
                    UISprite sprite3 = transform3.FindChild(num3.ToString()).GetComponent<UISprite>();
                    sprite3.gameObject.SetActive(i < info.starLv);
                    sprite3.transform.localPosition = new Vector3(i * 19f, 0f, 0f);
                }
                transform3.localPosition = new Vector3(-6.8f - ((info.starLv - 1) * 9.5f), transform3.localPosition.y, 0f);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Identities2CardList>c__AnonStorey16C
    {
        internal List<Card> card_list;

        internal void <>m__179(long id)
        {
            if (id >= 0L)
            {
                Card cardByID = ActorData.getInstance().GetCardByID(id);
                if (cardByID != null)
                {
                    this.card_list.Add(cardByID);
                }
            }
        }
    }
}

