using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class SignInPanel : GUIEntity
{
    private Transform _TipsDiag;
    private List<GameObject> CreateBuffer = new List<GameObject>();
    public UIGrid Grid;
    private const int LineCount = 7;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    public GameObject SignInObj;
    private string strGoldIcon = "Item_Icon_Gold";
    private string strStoneIcon = "Item_Icon_Stone";

    private void ClickSignBtn()
    {
        SocketMgr.Instance.RequestRegistration();
    }

    private void ExitPanel()
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    private ArrayList GetCfgByMonth()
    {
        int month = TimeMgr.Instance.ServerDateTime.Month;
        switch (month)
        {
            case 1:
                return ConfigMgr.getInstance().getList<sign_reward_0_config>();

            case 2:
                return ConfigMgr.getInstance().getList<sign_reward_1_config>();

            case 3:
                return ConfigMgr.getInstance().getList<sign_reward_2_config>();

            case 4:
                return ConfigMgr.getInstance().getList<sign_reward_3_config>();

            case 5:
                return ConfigMgr.getInstance().getList<sign_reward_4_config>();

            case 6:
                return ConfigMgr.getInstance().getList<sign_reward_5_config>();

            case 7:
                return ConfigMgr.getInstance().getList<sign_reward_6_config>();

            case 8:
                return ConfigMgr.getInstance().getList<sign_reward_7_config>();

            case 9:
                return ConfigMgr.getInstance().getList<sign_reward_8_config>();

            case 10:
                return ConfigMgr.getInstance().getList<sign_reward_9_config>();

            case 11:
                return ConfigMgr.getInstance().getList<sign_reward_10_config>();

            case 12:
                return ConfigMgr.getInstance().getList<sign_reward_11_config>();
        }
        return null;
    }

    private void GetCfgDataByMonth(object _CfgData, out SignRewardData _data)
    {
        int month = TimeMgr.Instance.ServerDateTime.Month;
        switch (month)
        {
            case 1:
            {
                sign_reward_0_config _config = (sign_reward_0_config) _CfgData;
                _data.entry = _config.entry;
                _data.reward_gold_base = _config.reward_gold_base;
                _data.reward_stone = _config.reward_stone;
                _data.reward_type = _config.reward_type;
                _data.reward_param_1 = _config.reward_param_1;
                _data.reward_param_2 = _config.reward_param_2;
                _data.vip = _config.vip_double;
                break;
            }
            case 2:
            {
                sign_reward_1_config _config2 = (sign_reward_1_config) _CfgData;
                _data.entry = _config2.entry;
                _data.reward_gold_base = _config2.reward_gold_base;
                _data.reward_stone = _config2.reward_stone;
                _data.reward_type = _config2.reward_type;
                _data.reward_param_1 = _config2.reward_param_1;
                _data.reward_param_2 = _config2.reward_param_2;
                _data.vip = _config2.vip_double;
                break;
            }
            case 3:
            {
                sign_reward_2_config _config3 = (sign_reward_2_config) _CfgData;
                _data.entry = _config3.entry;
                _data.reward_gold_base = _config3.reward_gold_base;
                _data.reward_stone = _config3.reward_stone;
                _data.reward_type = _config3.reward_type;
                _data.reward_param_1 = _config3.reward_param_1;
                _data.reward_param_2 = _config3.reward_param_2;
                _data.vip = _config3.vip_double;
                break;
            }
            case 4:
            {
                sign_reward_3_config _config4 = (sign_reward_3_config) _CfgData;
                _data.entry = _config4.entry;
                _data.reward_gold_base = _config4.reward_gold_base;
                _data.reward_stone = _config4.reward_stone;
                _data.reward_type = _config4.reward_type;
                _data.reward_param_1 = _config4.reward_param_1;
                _data.reward_param_2 = _config4.reward_param_2;
                _data.vip = _config4.vip_double;
                break;
            }
            case 5:
            {
                sign_reward_4_config _config5 = (sign_reward_4_config) _CfgData;
                _data.entry = _config5.entry;
                _data.reward_gold_base = _config5.reward_gold_base;
                _data.reward_stone = _config5.reward_stone;
                _data.reward_type = _config5.reward_type;
                _data.reward_param_1 = _config5.reward_param_1;
                _data.reward_param_2 = _config5.reward_param_2;
                _data.vip = _config5.vip_double;
                break;
            }
            case 6:
            {
                sign_reward_5_config _config6 = (sign_reward_5_config) _CfgData;
                _data.entry = _config6.entry;
                _data.reward_gold_base = _config6.reward_gold_base;
                _data.reward_stone = _config6.reward_stone;
                _data.reward_type = _config6.reward_type;
                _data.reward_param_1 = _config6.reward_param_1;
                _data.reward_param_2 = _config6.reward_param_2;
                _data.vip = _config6.vip_double;
                break;
            }
            case 7:
            {
                sign_reward_6_config _config7 = (sign_reward_6_config) _CfgData;
                _data.entry = _config7.entry;
                _data.reward_gold_base = _config7.reward_gold_base;
                _data.reward_stone = _config7.reward_stone;
                _data.reward_type = _config7.reward_type;
                _data.reward_param_1 = _config7.reward_param_1;
                _data.reward_param_2 = _config7.reward_param_2;
                _data.vip = _config7.vip_double;
                break;
            }
            case 8:
            {
                sign_reward_7_config _config8 = (sign_reward_7_config) _CfgData;
                _data.entry = _config8.entry;
                _data.reward_gold_base = _config8.reward_gold_base;
                _data.reward_stone = _config8.reward_stone;
                _data.reward_type = _config8.reward_type;
                _data.reward_param_1 = _config8.reward_param_1;
                _data.reward_param_2 = _config8.reward_param_2;
                _data.reward_param_1 = _config8.reward_param_2;
                _data.vip = _config8.vip_double;
                break;
            }
            case 9:
            {
                sign_reward_8_config _config9 = (sign_reward_8_config) _CfgData;
                _data.entry = _config9.entry;
                _data.reward_gold_base = _config9.reward_gold_base;
                _data.reward_stone = _config9.reward_stone;
                _data.reward_type = _config9.reward_type;
                _data.reward_param_1 = _config9.reward_param_1;
                _data.reward_param_2 = _config9.reward_param_2;
                _data.vip = _config9.vip_double;
                break;
            }
            case 10:
            {
                sign_reward_9_config _config10 = (sign_reward_9_config) _CfgData;
                _data.entry = _config10.entry;
                _data.reward_gold_base = _config10.reward_gold_base;
                _data.reward_stone = _config10.reward_stone;
                _data.reward_type = _config10.reward_type;
                _data.reward_param_1 = _config10.reward_param_1;
                _data.reward_param_2 = _config10.reward_param_2;
                _data.vip = _config10.vip_double;
                break;
            }
            case 11:
            {
                sign_reward_10_config _config11 = (sign_reward_10_config) _CfgData;
                _data.entry = _config11.entry;
                _data.reward_gold_base = _config11.reward_gold_base;
                _data.reward_stone = _config11.reward_stone;
                _data.reward_type = _config11.reward_type;
                _data.reward_param_1 = _config11.reward_param_1;
                _data.reward_param_2 = _config11.reward_param_2;
                _data.vip = _config11.vip_double;
                break;
            }
            case 12:
            {
                sign_reward_11_config _config12 = (sign_reward_11_config) _CfgData;
                _data.entry = _config12.entry;
                _data.reward_gold_base = _config12.reward_gold_base;
                _data.reward_stone = _config12.reward_stone;
                _data.reward_type = _config12.reward_type;
                _data.reward_param_1 = _config12.reward_param_1;
                _data.reward_param_2 = _config12.reward_param_2;
                _data.vip = _config12.vip_double;
                break;
            }
            default:
                _data.entry = 0;
                _data.reward_gold_base = 0;
                _data.reward_stone = 0;
                _data.reward_type = 0;
                _data.reward_param_1 = 0;
                _data.reward_param_2 = 0;
                _data.vip = -1;
                Debug.LogWarning("Unknown month " + month);
                break;
        }
    }

    public void HideBtn(bool _show)
    {
        base.gameObject.transform.FindChild("PickBtn").gameObject.SetActive(_show);
    }

    private void HideTips()
    {
        TweenScale.Begin(this._TipsDiag.gameObject, 0.2f, new Vector3(0.0001f, 0.0001f, 0.0001f));
        TweenAlpha.Begin(this._TipsDiag.gameObject, 0.2f, 0f);
        this.DelayCallBack(0.1f, () => this._TipsDiag.gameObject.SetActive(false));
    }

    private void InitCfgList()
    {
        CommonFunc.DeleteChildItem(this.Grid.transform);
        this.CreateBuffer.Clear();
        ArrayList cfgByMonth = this.GetCfgByMonth();
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        GameObject obj2 = null;
        IEnumerator enumerator = cfgByMonth.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                if ((num == 0) || ((num % 7) == 0))
                {
                    obj2 = new GameObject();
                    obj2.name = (num2 + 1).ToString();
                    obj2.transform.parent = this.Grid.transform;
                    obj2.transform.localPosition = Vector3.zero;
                    obj2.transform.localScale = Vector3.one;
                    num = 0;
                    num2++;
                }
                GameObject go = UnityEngine.Object.Instantiate(this.SignInObj) as GameObject;
                go.transform.parent = obj2.transform;
                go.transform.localPosition = new Vector3((float) (-301 + (num * 0x65)), 0f, 0f);
                go.transform.localScale = Vector3.one;
                GUIDataHolder.setData(go, current);
                UIEventListener.Get(go).onPress = new UIEventListener.BoolDelegate(this.OnClickIcon);
                this.UpdateSignInData(go, current);
                if (((ActorData.getInstance().RegistrationCount + 1) == (num3 + 1)) && (ActorData.getInstance().RegistrationCount != cfgByMonth.Count))
                {
                    this.UpdateRewardData(current);
                }
                this.CreateBuffer.Add(go);
                num++;
                num3++;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        this.Grid.repositionNow = true;
    }

    private void OnClickIcon(GameObject obj, bool isPress)
    {
        if (isPress)
        {
            object obj2 = GUIDataHolder.getData(obj);
            UILabel component = obj.transform.FindChild("Day").GetComponent<UILabel>();
            this.SetTipsInfo(obj2, component.text);
            TweenScale.Begin(this._TipsDiag.gameObject, 0.2f, Vector3.one).method = UITweener.Method.SpringIn;
            TweenAlpha.Begin(this._TipsDiag.gameObject, 0.2f, 1f);
            this._TipsDiag.gameObject.SetActive(true);
        }
        else
        {
            this.HideTips();
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(true);
        }
    }

    public override void OnInitialize()
    {
        this._TipsDiag = base.transform.FindChild("Tips");
        this.InitCfgList();
        if (ActorData.getInstance().RegistrationReward >= 1)
        {
            this.HideBtn(false);
        }
        if (GameDefine.getInstance().IsCanShowQQVIP())
        {
            base.transform.FindChild("QQRewardTips").gameObject.SetActive(ActorData.getInstance().UserInfo.qq_vip_type > 0);
        }
        else
        {
            base.transform.FindChild("QQRewardTips").gameObject.SetActive(false);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(false);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.m_time += Time.deltaTime;
        if (this.m_time > this.m_updateInterval)
        {
            this.m_time = 0f;
            if (((TimeMgr.Instance.ServerDateTime.Hour == 0) && (TimeMgr.Instance.ServerDateTime.Minute == 0)) && (TimeMgr.Instance.ServerDateTime.Second == 0))
            {
                this.HideBtn(false);
                SocketMgr.Instance.RequestGetUserInfo();
            }
        }
    }

    private void SetRewardTips(object cfg)
    {
        SignRewardData data = new SignRewardData();
        this.GetCfgDataByMonth(cfg, out data);
        UILabel component = base.gameObject.transform.FindChild("PickDesc/Label").GetComponent<UILabel>();
        if (ActorData.getInstance().RegistrationReward >= 1)
        {
            if ((ActorData.getInstance().RegistrationCount - 1) == data.entry)
            {
                component.text = ConfigMgr.getInstance().GetWord(0xa037d3);
            }
            else
            {
                component.text = string.Format(ConfigMgr.getInstance().GetWord(0xa037d4), data.entry + 1);
            }
        }
        else if (ActorData.getInstance().RegistrationCount == data.entry)
        {
            component.text = ConfigMgr.getInstance().GetWord(0xa037d3);
        }
        else
        {
            component.text = string.Format(ConfigMgr.getInstance().GetWord(0xa037d4), data.entry + 1);
        }
    }

    private void SetTipsInfo(object _cfg, string day)
    {
        SignRewardData data = new SignRewardData();
        this.GetCfgDataByMonth(_cfg, out data);
        UILabel component = this._TipsDiag.FindChild("Name").GetComponent<UILabel>();
        UILabel label2 = this._TipsDiag.FindChild("Desc").GetComponent<UILabel>();
        UITexture texture = this._TipsDiag.FindChild("SignInItem/Icon").GetComponent<UITexture>();
        UISprite sprite = this._TipsDiag.FindChild("SignInItem/frame").GetComponent<UISprite>();
        Transform transform = this._TipsDiag.transform.FindChild("SignInItem/Patch");
        UILabel label3 = this._TipsDiag.FindChild("Gold").GetComponent<UILabel>();
        Transform transform2 = this._TipsDiag.transform.FindChild("Have");
        UILabel label4 = this._TipsDiag.FindChild("Have/Count").GetComponent<UILabel>();
        UILabel label5 = this._TipsDiag.FindChild("Day").GetComponent<UILabel>();
        UISprite sprite2 = this._TipsDiag.FindChild("Border").GetComponent<UISprite>();
        RewardType type = (RewardType) data.reward_type;
        if (type == RewardType.Card)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(data.reward_param_1);
            if (_config == null)
            {
                return;
            }
            texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            texture.gameObject.SetActive(true);
            CommonFunc.SetEquipQualityBorder(sprite, _config.quality, false);
            component.text = _config.name;
            label2.text = _config.describe;
            label5.text = string.Format(ConfigMgr.getInstance().GetWord(0x4f0), day);
            label3.gameObject.SetActive(false);
            transform.gameObject.SetActive(false);
            transform2.gameObject.SetActive(false);
        }
        else if (type == RewardType.Item)
        {
            <SetTipsInfo>c__AnonStorey21E storeye = new <SetTipsInfo>c__AnonStorey21E {
                ic = ConfigMgr.getInstance().getByEntry<item_config>(data.reward_param_1)
            };
            if (storeye.ic == null)
            {
                return;
            }
            texture.mainTexture = BundleMgr.Instance.CreateItemIcon(storeye.ic.icon);
            texture.gameObject.SetActive(true);
            CommonFunc.SetEquipQualityBorder(sprite, storeye.ic.quality, false);
            label2.text = storeye.ic.describe;
            label5.text = string.Format(ConfigMgr.getInstance().GetWord(0x4f0), day);
            component.text = storeye.ic.name;
            label3.text = storeye.ic.sell_price.ToString();
            label3.gameObject.SetActive(true);
            if ((storeye.ic.type == 3) || (storeye.ic.type == 2))
            {
                transform.gameObject.SetActive(true);
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
            Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeye.<>m__45B));
            if (item != null)
            {
                label4.text = string.Format(ConfigMgr.getInstance().GetWord(0x4ef), item.num);
            }
            else
            {
                label4.text = string.Empty;
            }
            transform2.gameObject.SetActive(true);
        }
        else
        {
            texture.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
            component.text = ConfigMgr.getInstance().GetWord(0x31b);
            label2.text = ConfigMgr.getInstance().GetWord(0x3e2);
            label5.text = string.Format(ConfigMgr.getInstance().GetWord(0x4f0), day);
            transform.gameObject.SetActive(false);
            label3.gameObject.SetActive(false);
            transform2.gameObject.SetActive(false);
        }
        label5.transform.localPosition = new Vector3(label5.transform.localPosition.x, (float) (-35 - label2.height), label5.transform.localPosition.z);
        sprite2.height = (210 + label2.height) - 30;
    }

    private void UpdateCfgData(GameObject obj, object _cfg, int index)
    {
        UILabel component = obj.transform.FindChild("Name/Label").GetComponent<UILabel>();
        UITexture texture = obj.transform.FindChild("Icon/Image").GetComponent<UITexture>();
        UISprite sprite = obj.transform.FindChild("Icon/frame").GetComponent<UISprite>();
        GameObject gameObject = obj.transform.FindChild("Icon/Patch").gameObject;
        UILabel label2 = obj.transform.FindChild("Icon/Label").GetComponent<UILabel>();
        UISprite sprite2 = obj.transform.FindChild("Icon/QQVip").GetComponent<UISprite>();
        SignRewardData data = new SignRewardData();
        this.GetCfgDataByMonth(_cfg, out data);
        switch (index)
        {
            case 1:
                component.text = data.reward_gold_base.ToString();
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(this.strGoldIcon);
                obj.SetActive(true);
                label2.text = string.Empty;
                break;

            case 2:
                component.text = data.reward_stone.ToString();
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(this.strStoneIcon);
                obj.SetActive(true);
                label2.text = string.Empty;
                if (!GameDefine.getInstance().IsCanShowQQVIP())
                {
                    sprite2.gameObject.SetActive(false);
                    break;
                }
                if (ActorData.getInstance().UserInfo.qq_vip_type > 0)
                {
                    sprite2.gameObject.SetActive(true);
                    object[] objArray1 = new object[] { data.reward_stone, GameConstant.DefaultTextRedColor, " +", (int) (data.reward_stone * (((float) GameConstValues.QQVIP_SIGN_STONE_ADD) / 10000f)) };
                    component.text = string.Concat(objArray1);
                }
                break;

            case 3:
                label2.text = data.reward_param_2.ToString();
                switch (((RewardType) data.reward_type))
                {
                    case RewardType.Card:
                    {
                        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(data.reward_param_1);
                        if (_config != null)
                        {
                            texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                            texture.gameObject.SetActive(true);
                            CommonFunc.SetQualityColor(sprite, _config.quality);
                            obj.SetActive(true);
                            component.text = _config.name;
                            gameObject.SetActive(false);
                        }
                        return;
                    }
                    case RewardType.Item:
                    {
                        item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(data.reward_param_1);
                        if (_config2 != null)
                        {
                            texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                            texture.gameObject.SetActive(true);
                            CommonFunc.SetQualityColor(sprite, _config2.quality);
                            obj.SetActive(true);
                            component.text = _config2.name;
                            if ((_config2.type == 3) || (_config2.type == 2))
                            {
                                gameObject.SetActive(true);
                            }
                            else
                            {
                                gameObject.SetActive(false);
                            }
                        }
                        return;
                    }
                }
                obj.SetActive(false);
                break;
        }
    }

    public void UpdateCheckState()
    {
        foreach (GameObject obj2 in this.CreateBuffer)
        {
            GameObject gameObject = obj2.transform.FindChild("Tag").gameObject;
            object obj4 = GUIDataHolder.getData(obj2);
            this.UpdateSignInData(obj2, obj4);
            SignRewardData data = new SignRewardData();
        }
    }

    private void UpdateRewardData(object _cfg)
    {
        this.SetRewardTips(_cfg);
        SignRewardData data = new SignRewardData();
        this.GetCfgDataByMonth(_cfg, out data);
        GameObject gameObject = base.gameObject.transform.FindChild("PickDesc/Reward1").gameObject;
        GameObject obj3 = base.gameObject.transform.FindChild("PickDesc/Reward2").gameObject;
        gameObject.gameObject.SetActive(false);
        obj3.gameObject.SetActive(false);
        bool flag = false;
        if (data.reward_stone > 0)
        {
            this.UpdateCfgData(gameObject, _cfg, 2);
            flag = true;
        }
        if (!flag)
        {
            this.UpdateCfgData(gameObject, _cfg, 3);
        }
        else
        {
            this.UpdateCfgData(obj3, _cfg, 3);
        }
        GameObject obj4 = base.gameObject.transform.FindChild("PickDesc/RewardVIP").gameObject;
        float y = !flag ? 21.23f : -80.8f;
        obj4.transform.localPosition = new Vector3(obj4.transform.localPosition.x, y, obj4.transform.localPosition.z);
        if ((data.vip >= 0) && (data.vip <= ActorData.getInstance().VipType))
        {
            obj4.SetActive(true);
            this.UpdateCfgData(obj4, _cfg, 3);
        }
        else
        {
            obj4.SetActive(false);
        }
    }

    public void UpdateSignBtn()
    {
        this.InitCfgList();
        if (ActorData.getInstance().RegistrationReward >= 1)
        {
            this.HideBtn(false);
        }
        else
        {
            this.HideBtn(true);
        }
    }

    private void UpdateSignInData(GameObject obj, object _cfg)
    {
        SignRewardData data = new SignRewardData();
        this.GetCfgDataByMonth(_cfg, out data);
        UILabel component = obj.transform.FindChild("Label").GetComponent<UILabel>();
        UITexture texture = obj.transform.FindChild("Icon").GetComponent<UITexture>();
        UISprite sprite = obj.transform.FindChild("Icon1").GetComponent<UISprite>();
        GameObject gameObject = obj.transform.FindChild("Tag").gameObject;
        GameObject obj3 = obj.transform.FindChild("Patch").gameObject;
        GameObject obj4 = obj.transform.FindChild("vip").gameObject;
        obj.transform.FindChild("Day").GetComponent<UILabel>().text = (data.entry + 1).ToString();
        UITexture texture2 = obj.transform.FindChild("frame").GetComponent<UITexture>();
        UISprite sprite2 = obj.transform.FindChild("bg").GetComponent<UISprite>();
        bool flag = true;
        if (ActorData.getInstance().RegistrationCount >= (data.entry + 1))
        {
            gameObject.SetActive(true);
            sprite2.color = (Color) new Color32(0xa1, 0x8a, 0x79, 0xff);
            nguiTextureGrey.doChangeEnableGrey(texture, true);
        }
        else
        {
            gameObject.SetActive(false);
            sprite2.color = (Color) new Color32(0xef, 0xe7, 210, 0xff);
            nguiTextureGrey.doChangeEnableGrey(texture, false);
            flag = false;
        }
        if (data.reward_type >= 0)
        {
            component.text = "X" + data.reward_param_2.ToString();
        }
        else
        {
            component.text = "X" + data.reward_stone.ToString();
        }
        RewardType type = (RewardType) data.reward_type;
        if (type == RewardType.Card)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(data.reward_param_1);
            if (_config == null)
            {
                return;
            }
            texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            texture.gameObject.SetActive(true);
            if (flag)
            {
                nguiTextureGrey.doChangeEnableGrey(texture2, true);
            }
            else
            {
                texture2.mainTexture = BundleMgr.Instance.CreateItemQuality("Ui_Zhuangbei_Frame_" + _config.quality);
                nguiTextureGrey.doChangeEnableGrey(texture2, false);
            }
            texture.width = 100;
            texture.height = 100;
            texture2.gameObject.SetActive(true);
            sprite.gameObject.SetActive(false);
        }
        else if (type == RewardType.Item)
        {
            item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(data.reward_param_1);
            if (_config2 == null)
            {
                return;
            }
            texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
            if (_config2.type == 3)
            {
                texture.width = 100;
                texture.height = 100;
            }
            else
            {
                texture.width = 80;
                texture.height = 80;
            }
            texture.gameObject.SetActive(true);
            if (flag)
            {
                nguiTextureGrey.doChangeEnableGrey(texture2, true);
            }
            else
            {
                texture2.mainTexture = BundleMgr.Instance.CreateItemQuality("Ui_Zhuangbei_Frame_" + _config2.quality);
                nguiTextureGrey.doChangeEnableGrey(texture2, false);
            }
            texture2.gameObject.SetActive(true);
            if ((_config2.type == 3) || (_config2.type == 2))
            {
                obj3.SetActive(true);
            }
            else
            {
                obj3.SetActive(false);
            }
            sprite.gameObject.SetActive(false);
        }
        else
        {
            if (flag)
            {
                texture2.color = (Color) new Color32(0xa5, 0x91, 0x80, 0xff);
            }
            texture.gameObject.SetActive(true);
            texture.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
            sprite.gameObject.SetActive(false);
        }
        if (data.vip >= 0)
        {
            obj4.SetActive(true);
            UISprite sprite3 = obj4.transform.FindChild("LevelNum").GetComponent<UISprite>();
            int num = data.vip + 1;
            if (num <= 1)
            {
                sprite3.spriteName = "Ui_Main_Icon_1";
            }
            else
            {
                sprite3.spriteName = "Ui_Main_Icon_" + num;
            }
        }
        else
        {
            obj4.SetActive(false);
        }
    }

    [CompilerGenerated]
    private sealed class <SetTipsInfo>c__AnonStorey21E
    {
        internal item_config ic;

        internal bool <>m__45B(Item e)
        {
            return (e.entry == this.ic.entry);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SignRewardData
    {
        public int entry;
        public int reward_gold_base;
        public int reward_stone;
        public int reward_type;
        public int reward_param_1;
        public int reward_param_2;
        public int vip;
    }
}

