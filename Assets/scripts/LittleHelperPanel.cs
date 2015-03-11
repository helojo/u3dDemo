using FastBuf;
using Holoville.HOTween;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LittleHelperPanel : GUIEntity
{
    private UIGrid _grid;
    private UISlider _slider;
    [CompilerGenerated]
    private static Comparison<LivenessTask> <>f__am$cache7;
    private GameObject activyObject;
    public GameObject itemPrefab;
    private int last_reward_entry;
    public GameObject rewardPrefab;
    private UISprite spriteActivyAward;

    private void DeleteAllItems()
    {
        CommonFunc.DeleteChildItem(this.itemGrid.transform);
    }

    private void ExitPanel()
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    private void MoveRewardList(bool _toLeft)
    {
        UIPanel component = base.transform.FindChild("Reward").GetComponent<UIPanel>();
        if (_toLeft)
        {
            Holoville.HOTween.HOTween.To(component.transform, 0.2f, "localPosition", new Vector3(5f, 0f, 0f));
            Holoville.HOTween.HOTween.To(component, 0.2f, "clipOffset", new Vector2(-5f, 0f));
        }
        else
        {
            Holoville.HOTween.HOTween.To(component.transform, 0.2f, "localPosition", new Vector3(-569f, 0f, 0f));
            Holoville.HOTween.HOTween.To(component, 0.2f, "clipOffset", new Vector2(569f, 0f));
        }
    }

    private void OnClickChestButton(GameObject go)
    {
        SocketMgr.Instance.RequestLivenessAward();
        GUIMgr.Instance.Lock();
    }

    private void OnClickLeftBtn(GameObject go)
    {
        this.MoveRewardList(true);
    }

    private void OnClickRightBtn(GameObject go)
    {
        this.MoveRewardList(false);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        SocketMgr.Instance.RequestLivenessList();
    }

    public override void OnInitialize()
    {
        UIEventListener.Get(base.transform.FindChild("leftBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLeftBtn);
        UIEventListener.Get(base.transform.FindChild("RightBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRightBtn);
    }

    public void Refresh(int liveness, int last_reward_entry)
    {
        this.DeleteAllItems();
        HashSet<int> set = new HashSet<int>();
        List<LivenessTask> livenessTaskList = ActorData.getInstance().LivenessTaskList;
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = delegate (LivenessTask l, LivenessTask r) {
                liveness_config _config = ConfigMgr.getInstance().getByEntry<liveness_config>(l.entry);
                liveness_config _config2 = ConfigMgr.getInstance().getByEntry<liveness_config>(r.entry);
                int num = (l.count < _config.max_count) ? 0 : 1;
                int num2 = (r.count < _config2.max_count) ? 0 : 1;
                return num - num2;
            };
        }
        livenessTaskList.Sort(<>f__am$cache7);
        int count = livenessTaskList.Count;
        int num2 = count / 2;
        if ((count % 2) != 0)
        {
            num2++;
        }
        int num3 = 0;
        for (int i = 0; i < num2; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this.itemPrefab) as GameObject;
            obj2.transform.parent = this.itemGrid.transform;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localScale = Vector3.one;
            Transform transform = obj2.transform.FindChild("Item1");
            Transform transform2 = obj2.transform.FindChild("Item2");
            if (num3 < count)
            {
                liveness_config cfg = ConfigMgr.getInstance().getByEntry<liveness_config>(livenessTaskList[num3].entry);
                if ((cfg == null) || set.Contains(livenessTaskList[num3].entry))
                {
                    continue;
                }
                set.Add(livenessTaskList[num3].entry);
                bool flag = livenessTaskList[num3].count >= cfg.max_count;
                this.SetItemInfo(transform, cfg, flag, livenessTaskList[num3]);
                num3++;
            }
            if (num3 < count)
            {
                liveness_config _config2 = ConfigMgr.getInstance().getByEntry<liveness_config>(livenessTaskList[num3].entry);
                if ((_config2 != null) && !set.Contains(livenessTaskList[num3].entry))
                {
                    set.Add(livenessTaskList[num3].entry);
                    bool flag2 = livenessTaskList[num3].count >= _config2.max_count;
                    this.SetItemInfo(transform2, _config2, flag2, livenessTaskList[num3]);
                    num3++;
                }
            }
            else
            {
                transform2.gameObject.SetActive(false);
            }
        }
        this.itemGrid.Reposition();
        this.RefreshReward(last_reward_entry, liveness);
        float a = ((float) liveness) / 100f;
        this.Slider.value = Mathf.Max(a, 0.01f);
        this.Slider.transform.FindChild("CurrValue").GetComponent<UILabel>().text = liveness.ToString();
    }

    private unsafe void RefreshReward(int last_re, int liveness)
    {
        List<int> list = new List<int>();
        for (int i = 0; i <= 8; i++)
        {
            list.Add(i);
        }
        this.last_reward_entry = last_re;
        list.Sort(new Comparison<int>(this.SortReward));
        CommonFunc.DeleteChildItem(base.transform.FindChild("Reward/Grid"));
        foreach (int num2 in list)
        {
            liveness_reward_config _config = ConfigMgr.getInstance().getByEntry<liveness_reward_config>(num2);
            if (_config != null)
            {
                int num3 = _config.reward_exp;
                GameObject obj2 = UnityEngine.Object.Instantiate(this.rewardPrefab) as GameObject;
                obj2.transform.parent = base.transform.FindChild("Reward/Grid");
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.localScale = Vector3.one;
                obj2.transform.rotation = Quaternion.identity;
                UILabel component = obj2.transform.FindChild("name").GetComponent<UILabel>();
                UILabel label2 = obj2.transform.FindChild("price").GetComponent<UILabel>();
                UISprite sprite = obj2.transform.FindChild("status").GetComponent<UISprite>();
                UISprite sprite2 = obj2.transform.FindChild("bg").GetComponent<UISprite>();
                UISprite sprite3 = obj2.transform.FindChild("bg2").GetComponent<UISprite>();
                UISprite sprite4 = obj2.transform.FindChild("line1").GetComponent<UISprite>();
                UISprite sprite5 = obj2.transform.FindChild("line2").GetComponent<UISprite>();
                UISprite sprite6 = obj2.transform.FindChild("frame").GetComponent<UISprite>();
                UITexture texture = obj2.transform.FindChild("icon").GetComponent<UITexture>();
                UILabel label3 = obj2.transform.FindChild("exp").GetComponent<UILabel>();
                UISprite sprite7 = obj2.transform.FindChild("complete").GetComponent<UISprite>();
                UISprite sprite8 = obj2.transform.FindChild("sprite").GetComponent<UISprite>();
                UILabel label4 = obj2.transform.FindChild("hydCount").GetComponent<UILabel>();
                UILabel label5 = obj2.transform.FindChild("Tips").GetComponent<UILabel>();
                label4.text = _config.liveness.ToString();
                if (_config.reward_exp > 0)
                {
                    label3.text = ConfigMgr.getInstance().GetWord(0x2f4) + _config.reward_exp.ToString();
                }
                switch (_config.reward_type)
                {
                    case 1:
                        sprite6.color = (Color) new Color32(0xa4, 0x84, 100, 0xff);
                        texture.mainTexture = BundleMgr.Instance.CreateTextureObject("GUI/Texture/Ui_Main_Icon_stone");
                        label3.text = string.Empty;
                        break;

                    case 2:
                        sprite6.color = (Color) new Color32(0xa4, 0x84, 100, 0xff);
                        texture.mainTexture = BundleMgr.Instance.CreateTextureObject("GUI/Texture/Ui_Main_Icon_coin");
                        component.text = ConfigMgr.getInstance().GetWord(0x89) + "X" + _config.reward_count;
                        break;

                    case 4:
                        sprite6.color = (Color) new Color32(0xa4, 0x84, 100, 0xff);
                        texture.mainTexture = BundleMgr.Instance.CreateTextureObject("GUI/Texture/Ui_Main_Icon_HP");
                        component.text = ConfigMgr.getInstance().GetWord(0x989682) + _config.reward_count;
                        break;

                    default:
                    {
                        int id = _config.reward_entry;
                        item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(id);
                        if (_config2 != null)
                        {
                            sprite6.color = *((Color*) &(GameConstant.ConstQuantityColor[_config2.quality]));
                            if (_config2.type == 3)
                            {
                                texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.icon);
                            }
                            else
                            {
                                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                                component.text = _config2.name + "X" + _config.reward_count;
                            }
                        }
                        break;
                    }
                }
                string str = string.Empty;
                if (_config.liveness <= 0)
                {
                    str = "[ffffff]" + ConfigMgr.getInstance().GetWord(0x989682) + _config.reward_count.ToString();
                }
                else
                {
                    str = "[ffffff]" + ConfigMgr.getInstance().GetWord(0x2bb) + _config.liveness.ToString();
                }
                if (num3 > 0)
                {
                    str = str + "  [fbd386]" + ConfigMgr.getInstance().GetWord(0x2f4) + num3.ToString();
                }
                sprite.gameObject.SetActive(this.last_reward_entry >= num2);
                if (liveness >= _config.liveness)
                {
                    sprite2.color = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    sprite3.color = (Color) new Color32(0xdb, 0xd1, 0xbd, 0xff);
                    sprite8.spriteName = "Ui_Daily_Icon_xiaohuoyue";
                    label4.color = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    label5.text = ConfigMgr.getInstance().GetWord(0x350);
                    label5.color = (Color) new Color32(0x8d, 0, 0, 0xff);
                    sprite4.color = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    sprite5.color = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                }
                else
                {
                    sprite2.color = (Color) new Color32(0xa1, 0x8f, 130, 0xff);
                    sprite3.color = (Color) new Color32(0xac, 150, 0x7e, 0xff);
                    sprite8.spriteName = "Ui_Daily_Icon_huoyuegrey";
                    label4.color = (Color) new Color32(0xe7, 0xc9, 0xaf, 0xff);
                    label5.text = ConfigMgr.getInstance().GetWord(0x351);
                    label5.color = (Color) new Color32(250, 0xf9, 0xea, 0xff);
                    sprite4.color = (Color) new Color32(0x62, 0x48, 0x39, 0xff);
                    sprite5.color = (Color) new Color32(0x62, 0x48, 0x39, 0xff);
                }
                if (num2 == (this.last_reward_entry + 1))
                {
                    this.spriteActivyAward = sprite7;
                    this.activyObject = obj2;
                }
                sprite7.gameObject.SetActive(false);
            }
        }
        base.transform.FindChild("Reward/Grid").GetComponent<UIGrid>().Reposition();
    }

    private void SetItemInfo(Transform item1, liveness_config cfg, bool is_finished, LivenessTask task)
    {
        UILabel component = item1.transform.FindChild("title").GetComponent<UILabel>();
        UILabel label2 = item1.transform.FindChild("reward").GetComponent<UILabel>();
        UISprite sprite = item1.transform.FindChild("status").GetComponent<UISprite>();
        UISprite sprite2 = item1.transform.FindChild("bg2").GetComponent<UISprite>();
        component.text = cfg.desc + string.Format("{0}/{1}", task.count, cfg.max_count);
        label2.text = "+" + cfg.gain_liveness.ToString();
        sprite.gameObject.SetActive(is_finished);
        if (is_finished)
        {
            sprite2.color = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
            label2.color = (Color) new Color32(0xf7, 0xfe, 0x39, 0xff);
        }
        else
        {
            sprite2.color = (Color) new Color32(0xba, 0xb0, 0xa1, 0xff);
            label2.color = (Color) new Color32(0xec, 0xe0, 0xd6, 0xff);
        }
        item1.transform.FindChild("FramePrefab/Border").GetComponent<UISprite>().color = !is_finished ? ((Color) new Color32(0xbc, 0xaf, 0xaf, 0xff)) : ((Color) new Color32(0xff, 0xff, 0xff, 0xff));
        item1.transform.FindChild("FramePrefab/Top").GetComponent<UISprite>().color = !is_finished ? ((Color) new Color32(0xbc, 0xaf, 0xaf, 0xff)) : ((Color) new Color32(0xff, 0xff, 0xff, 0xff));
        item1.transform.FindChild("FramePrefab/Bottom").GetComponent<UISprite>().color = !is_finished ? ((Color) new Color32(0xbc, 0xaf, 0xaf, 0xff)) : ((Color) new Color32(0xff, 0xff, 0xff, 0xff));
        item1.transform.FindChild("FramePrefab/Left").GetComponent<UISprite>().color = !is_finished ? ((Color) new Color32(0xbc, 0xaf, 0xaf, 0xff)) : ((Color) new Color32(0xff, 0xff, 0xff, 0xff));
        item1.transform.FindChild("FramePrefab/Right").GetComponent<UISprite>().color = !is_finished ? ((Color) new Color32(0xbc, 0xaf, 0xaf, 0xff)) : ((Color) new Color32(0xff, 0xff, 0xff, 0xff));
    }

    public void SetRewardEnable(bool enable)
    {
        if (null != this.spriteActivyAward)
        {
            this.spriteActivyAward.gameObject.SetActive(enable);
            if ((null != this.activyObject) && enable)
            {
                UIEventListener listener1 = UIEventListener.Get(this.activyObject);
                listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickChestButton));
            }
        }
    }

    private int SortReward(int id_l, int id_r)
    {
        int num = (this.last_reward_entry < id_l) ? 1 : 0;
        int num2 = (this.last_reward_entry < id_r) ? 1 : 0;
        if (num == num2)
        {
            return (id_l - id_r);
        }
        return (num - num2);
    }

    private UIGrid itemGrid
    {
        get
        {
            if (null == this._grid)
            {
                this._grid = base.transform.FindChild("Liveness/Grid").GetComponent<UIGrid>();
            }
            return this._grid;
        }
    }

    private UISlider Slider
    {
        get
        {
            if (null == this._slider)
            {
                this._slider = base.transform.FindChild("Progress").GetComponent<UISlider>();
            }
            return this._slider;
        }
    }
}

