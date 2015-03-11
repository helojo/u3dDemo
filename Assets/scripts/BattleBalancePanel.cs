using FastBuf;
using Holoville.HOTween;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleBalancePanel : GUIEntity
{
    private float _DelayTime = 1f;
    [CompilerGenerated]
    private static Func<FighterBattleData, bool> <>f__am$cache1;
    [CompilerGenerated]
    private static Func<FighterBattleData, bool> <>f__am$cache2;
    [CompilerGenerated]
    private static Comparison<FighterBattleData> <>f__am$cache3;
    [CompilerGenerated]
    private static Comparison<FighterBattleData> <>f__am$cache4;
    [CompilerGenerated]
    private static Comparison<FighterBattleData> <>f__am$cache5;
    [CompilerGenerated]
    private static Comparison<FighterBattleData> <>f__am$cache6;
    [CompilerGenerated]
    private static Comparison<FighterBattleData> <>f__am$cache7;

    private void ExitPanel()
    {
    }

    private int GetListMaxValue(List<FighterBattleData> list)
    {
        List<FighterBattleData> list2 = new List<FighterBattleData>();
        foreach (FighterBattleData data in list)
        {
            list2.Add(data);
        }
        if (list2.Count <= 0)
        {
            return 0;
        }
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = (t1, t2) => Math.Abs(t2.TotalDemage) - Math.Abs(t1.TotalDemage);
        }
        list2.Sort(<>f__am$cache7);
        return Math.Abs(list2[0].TotalDemage);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    private void SetEnemy()
    {
    }

    public void SetValue()
    {
        Debug.Log("BBP 1 setvalue");
        List<FighterBattleData> fighterBattleData = BattleSecurityManager.Instance.GetFighterBattleData();
        Debug.Log("BBP 2 get battledatas ");
        if (fighterBattleData != null)
        {
            Debug.Log("BBP 3  battledatas count " + fighterBattleData.Count);
        }
        else
        {
            Debug.Log("BBP 3 battledatas is null ");
        }
        if ((fighterBattleData != null) && (fighterBattleData.Count > 0))
        {
            Debug.Log("BBP 4 select endmy hero");
            List<FighterBattleData> list2 = new List<FighterBattleData>();
            List<FighterBattleData> list3 = new List<FighterBattleData>();
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = f => !f.IsActor;
            }
            list2 = fighterBattleData.Where<FighterBattleData>(<>f__am$cache1).ToList<FighterBattleData>();
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = f => f.IsActor;
            }
            list3 = fighterBattleData.Where<FighterBattleData>(<>f__am$cache2).ToList<FighterBattleData>();
            if (list3.Count >= 0)
            {
                Debug.Log("BBP 4 select  huntMaxData start");
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = (h1, h2) => Math.Abs(h2.TotalDemage) - Math.Abs(h1.TotalDemage);
                }
                list3.Sort(<>f__am$cache3);
                int num = Math.Abs(list3[0].TotalDemage);
                if (list2.Count >= 0)
                {
                    if (<>f__am$cache4 == null)
                    {
                        <>f__am$cache4 = (e1, e2) => Math.Abs(e2.TotalDemage) - Math.Abs(e1.TotalDemage);
                    }
                    list2.Sort(<>f__am$cache4);
                    int num2 = Math.Abs(list2[0].TotalDemage);
                    int num3 = (num <= num2) ? num2 : num;
                    Debug.Log("BBP 4 select  huntMaxData end");
                    if (<>f__am$cache5 == null)
                    {
                        <>f__am$cache5 = (h1, h2) => h1.Id - h2.Id;
                    }
                    list3.Sort(<>f__am$cache5);
                    for (int i = 0; i < list3.Count; i++)
                    {
                        int num5 = i + 1;
                        FighterBattleData data = list3[i];
                        if ((data != null) && (data.Id != -1))
                        {
                            card_config _config;
                            Debug.Log(string.Concat(new object[] { "BBP hero id", data.Id, "hero TotalDemage ", data.TotalDemage }));
                            if (data.IsCard)
                            {
                                _config = ConfigMgr.getInstance().getByEntry<card_config>(data.CardEntry);
                            }
                            else
                            {
                                monster_config _config2 = ConfigMgr.getInstance().getByEntry<monster_config>(data.CardEntry);
                                if (_config2 == null)
                                {
                                    continue;
                                }
                                _config = ConfigMgr.getInstance().getByEntry<card_config>(_config2.card_entry);
                            }
                            if (_config != null)
                            {
                                Transform transform = base.transform.FindChild("HeroGroup/" + num5);
                                if (transform != null)
                                {
                                    transform.gameObject.SetActive(true);
                                    transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                                    CommonFunc.SetQualityBorder(transform.FindChild("QualityBorder").GetComponent<UISprite>(), data.Quality);
                                    for (int k = 0; k < 5; k++)
                                    {
                                        UISprite sprite2 = transform.transform.FindChild("Star/" + (k + 1)).GetComponent<UISprite>();
                                        sprite2.gameObject.SetActive(k < data.StarLevel);
                                        sprite2.transform.localPosition = new Vector3((float) (k * 0x17), 0f, 0f);
                                    }
                                    Transform transform2 = transform.transform.FindChild("Star");
                                    transform2.localPosition = new Vector3(-15.8f - ((data.StarLevel - 1) * 11.5f), transform2.localPosition.y, 0f);
                                    transform2.gameObject.SetActive(true);
                                    int num7 = Math.Abs(data.TotalDemage);
                                    UISprite component = transform.transform.FindChild("PercentS/Foreground").gameObject.GetComponent<UISprite>();
                                    if (component != null)
                                    {
                                        float num8 = ((float) num7) / ((float) num3);
                                        Holoville.HOTween.HOTween.To(component, this._DelayTime, "fillAmount", num8);
                                    }
                                    UILabel label = transform.transform.FindChild("LbPercent").gameObject.GetComponent<UILabel>();
                                    if (label != null)
                                    {
                                        label.text = (num7 > 0) ? num7.ToString("#,###") : "0";
                                    }
                                }
                            }
                        }
                    }
                    if (<>f__am$cache6 == null)
                    {
                        <>f__am$cache6 = (e1, e2) => e1.Id - e2.Id;
                    }
                    list2.Sort(<>f__am$cache6);
                    for (int j = 0; j < list2.Count; j++)
                    {
                        int num10 = j + 1;
                        FighterBattleData data2 = list2[j];
                        if ((data2 != null) && (data2.Id != -1))
                        {
                            card_config _config3;
                            Debug.Log(string.Concat(new object[] { "enemy id", data2.Id, "enemy TotalDemage ", data2.TotalDemage }));
                            monster_config _config4 = null;
                            if (data2.IsCard)
                            {
                                _config3 = ConfigMgr.getInstance().getByEntry<card_config>(data2.CardEntry);
                            }
                            else
                            {
                                _config4 = ConfigMgr.getInstance().getByEntry<monster_config>(data2.CardEntry);
                                if (_config4 == null)
                                {
                                    continue;
                                }
                                _config3 = ConfigMgr.getInstance().getByEntry<card_config>(_config4.card_entry);
                            }
                            if (_config3 != null)
                            {
                                Transform transform3 = base.transform.FindChild("EnemyGroup/" + num10);
                                if (transform3 != null)
                                {
                                    transform3.gameObject.SetActive(true);
                                    transform3.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config3.image);
                                    short num11 = (data2.StarLevel != 0) ? data2.StarLevel : ((short) 1);
                                    CommonFunc.SetQualityBorder(transform3.FindChild("QualityBorder").GetComponent<UISprite>(), data2.Quality);
                                    for (int m = 0; m < 5; m++)
                                    {
                                        UISprite sprite5 = transform3.transform.FindChild("Star/" + (m + 1)).GetComponent<UISprite>();
                                        sprite5.gameObject.SetActive(m < num11);
                                        sprite5.transform.localPosition = new Vector3((float) (m * 0x17), 0f, 0f);
                                    }
                                    Transform transform4 = transform3.transform.FindChild("Star");
                                    transform4.localPosition = new Vector3(-15.8f - ((num11 - 1) * 11.5f), transform4.localPosition.y, 0f);
                                    transform4.gameObject.SetActive(true);
                                    int num13 = Math.Abs(data2.TotalDemage);
                                    UISprite sprite6 = transform3.transform.FindChild("PercentS/Foreground").gameObject.GetComponent<UISprite>();
                                    if (sprite6 != null)
                                    {
                                        float num14 = ((float) num13) / ((float) num3);
                                        Holoville.HOTween.HOTween.To(sprite6, this._DelayTime, "fillAmount", num14);
                                    }
                                    UILabel label2 = transform3.transform.FindChild("LbPercent").gameObject.GetComponent<UILabel>();
                                    if (label2 != null)
                                    {
                                        label2.text = (num13 > 0) ? num13.ToString("#,###") : "0";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

