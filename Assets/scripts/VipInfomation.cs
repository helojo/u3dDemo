using FastBuf;
using System;
using System.Collections;
using UnityEngine;

public class VipInfomation : MonoBehaviour
{
    private UILabel _currentLV;
    private UILabel _needStone;
    private UILabel _nextLV;
    private UISlider _slider;

    public void Refresh()
    {
        int level = ActorData.getInstance().UserInfo.vip_level.level;
        int num2 = (int) ActorData.getInstance().UserInfo.vip_level.change_stone;
        if (level >= 0x10)
        {
            base.transform.FindChild("info").gameObject.SetActive(false);
            this.CurrentLV.text = "16";
            this.NextLV.text = "16";
            this.Slider.value = 1f;
            return;
        }
        int num4 = 0;
        float num5 = 0f;
        float f = 0f;
        IEnumerator enumerator = ConfigMgr.getInstance().getList<vip_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                vip_config current = (vip_config) enumerator.Current;
                if (current.entry == level)
                {
                    int exp = current.exp;
                    num5 = ((float) num2) / ((float) exp);
                    this.Slider.transform.FindChild("Label").GetComponent<UILabel>().text = num2 + "/" + exp;
                    f = ((float) (exp - num2)) / 10f;
                    num4 = Mathf.CeilToInt(f);
                    goto Label_0154;
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
    Label_0154:
        this.CurrentLV.text = level.ToString();
        this.NextLV.text = (level + 1).ToString();
        this.NeedStone.text = num4.ToString();
        this.Slider.value = num5;
    }

    private UILabel CurrentLV
    {
        get
        {
            if (null == this._currentLV)
            {
                this._currentLV = base.transform.FindChild("lv/value").GetComponent<UILabel>();
            }
            return this._currentLV;
        }
    }

    private UILabel NeedStone
    {
        get
        {
            if (null == this._needStone)
            {
                this._needStone = base.transform.FindChild("info/stone").GetComponent<UILabel>();
            }
            return this._needStone;
        }
    }

    private UILabel NextLV
    {
        get
        {
            if (null == this._nextLV)
            {
                this._nextLV = base.transform.FindChild("info/lv2/value").GetComponent<UILabel>();
            }
            return this._nextLV;
        }
    }

    private UISlider Slider
    {
        get
        {
            if (null == this._slider)
            {
                this._slider = base.transform.FindChild("progress").GetComponent<UISlider>();
            }
            return this._slider;
        }
    }
}

