using FastBuf;
using System;
using System.Collections;
using UnityEngine;

public class WorldBossRulePanel : GUIEntity
{
    private Color32[] FontColor = new Color32[] { new Color32(0xfe, 0x60, 0xfe, 0xff), new Color32(0, 0xe1, 0xfe, 0xff), new Color32(0xcc, 0xfe, 0, 0xff), new Color32(0xff, 0xff, 0xff, 0xff) };
    private Color32[] FrameColor = new Color32[] { new Color32(0x3f, 0x30, 40, 0xff), new Color32(0x36, 0x27, 0x20, 0xff) };
    private const int LineCount = 2;
    public GameObject RankAwarkObj;
    public GameObject Root;

    private unsafe void ChangeRankerColor(GameObject obj, int index)
    {
        UILabel component = obj.transform.FindChild("Label").GetComponent<UILabel>();
        component.text = string.Format(ConfigMgr.getInstance().GetWord(0x61), index);
        if (index == 1)
        {
            obj.transform.FindChild("1/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[0]));
            obj.transform.FindChild("2/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[0]));
            obj.transform.FindChild("3/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[0]));
            component.color = *((Color*) &(this.FontColor[0]));
        }
        else if (index == 2)
        {
            obj.transform.FindChild("1/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[1]));
            obj.transform.FindChild("2/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[1]));
            obj.transform.FindChild("3/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[1]));
            component.color = *((Color*) &(this.FontColor[1]));
        }
        else if (index == 3)
        {
            obj.transform.FindChild("1/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[2]));
            obj.transform.FindChild("2/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[2]));
            obj.transform.FindChild("3/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[2]));
            component.color = *((Color*) &(this.FontColor[2]));
        }
        else
        {
            obj.transform.FindChild("1/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[3]));
            obj.transform.FindChild("2/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[3]));
            obj.transform.FindChild("3/Label").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[3]));
            component.color = *((Color*) &(this.FontColor[3]));
        }
    }

    public override void OnInitialize()
    {
        ArrayList list = ConfigMgr.getInstance().getList<world_boss_reward_config>();
        base.gameObject.transform.FindChild("Scroll View/AtkGift/Desc").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4b2);
        base.gameObject.transform.FindChild("Scroll View/RankGift/Desc").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4b3);
        int num = 0;
        int num3 = 1;
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                world_boss_reward_config current = (world_boss_reward_config) enumerator.Current;
                if (current.entry != 0)
                {
                    GameObject obj3 = UnityEngine.Object.Instantiate(this.RankAwarkObj) as GameObject;
                    obj3.transform.parent = this.Root.transform;
                    obj3.transform.localPosition = new Vector3(0f, (float) (-num * 40), 0f);
                    obj3.transform.localScale = Vector3.one;
                    UISprite component = obj3.transform.FindChild("Sprite").GetComponent<UISprite>();
                    if ((num % 2) == 0)
                    {
                        component.gameObject.SetActive(true);
                    }
                    else
                    {
                        component.gameObject.SetActive(false);
                    }
                    obj3.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x61), num3);
                    num3++;
                    this.UpdataRewardData(obj3, current);
                    num++;
                }
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
    }

    private void UpdataRewardData(GameObject obj, world_boss_reward_config _data)
    {
        obj.transform.FindChild("1/Label").GetComponent<UILabel>().text = _data.gold.ToString();
        obj.transform.FindChild("2/Label").GetComponent<UILabel>().text = _data.stone.ToString();
        UITexture component = obj.transform.FindChild("3/HIcon").GetComponent<UITexture>();
        obj.transform.FindChild("3/Label").GetComponent<UILabel>().text = _data.key_num.ToString();
        if (_data.key_type == 5)
        {
            component.mainTexture = BundleMgr.Instance.CreateItemIcon("jm_icon_a03");
        }
        else if (_data.key_type == 6)
        {
            component.mainTexture = BundleMgr.Instance.CreateItemIcon("jm_icon_a02");
        }
        else if (_data.key_type == 7)
        {
            component.mainTexture = BundleMgr.Instance.CreateItemIcon("jm_icon_a01");
        }
    }
}

