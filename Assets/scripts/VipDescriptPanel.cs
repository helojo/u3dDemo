using FastBuf;
using Holoville.HOTween;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using Toolbox;
using UnityEngine;

public class VipDescriptPanel : GUIEntity
{
    private UIGrid _grid;
    private UILabel _nextLV;
    private UILabel _preLV;
    private UIScrollView _scrollView;
    private UILabel _titleLV;
    public float cellWidth = 658.1f;
    private UICenterOnChild ctr;
    private UIGrid grid;
    public static VipDescriptPanel inst;
    public GameObject itemPrefab;
    private float lastClickTime;
    public UITexture leftB;
    public GameObject leftBtn;
    public int num = 1;
    public UIPanel panel;
    public UITexture rightB;
    public GameObject rightBtn;

    private void Awake()
    {
        inst = this;
    }

    private void InitPage(int vipLv)
    {
        this.ctr.enabled = false;
        this.num = vipLv - 1;
        int num = vipLv - 1;
        float x = num * -this.cellWidth;
        Vector3 vector = new Vector3(x, this.panel.transform.localPosition.y, this.panel.transform.localPosition.z);
        Vector2 vector2 = new Vector2(-x, this.panel.clipOffset.y);
        Holoville.HOTween.HOTween.To(this.panel.transform, 0f, "localPosition", vector).easeType = Holoville.HOTween.EaseType.EaseOutBack;
        Holoville.HOTween.HOTween.To(this.panel, 0f, "clipOffset", vector2).easeType = Holoville.HOTween.EaseType.EaseOutBack;
        this.DelayCallBack(0.2f, () => this.ctr.enabled = true);
        this.ReSetPage();
    }

    private void OnClickLeftBtn(GameObject go)
    {
        this.TurnPage(true);
    }

    private void OnClickRightBtn(GameObject go)
    {
        this.TurnPage(false);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        VipInfomation component = base.transform.FindChild("VipInfomation").GetComponent<VipInfomation>();
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        component.Refresh();
        SpringPanel panel = this.panel.GetComponent<SpringPanel>();
        if (panel != null)
        {
            panel.enabled = false;
        }
        this.InitPage(ActorData.getInstance().UserInfo.vip_level.level);
        GUIMgr.Instance.FloatTitleBar();
    }

    public override void OnInitialize()
    {
        base.transform.FindChild("VipInfomation").GetComponent<VipInfomation>().Refresh();
        ArrayList list = ConfigMgr.getInstance().getList<vip_config>();
        int num = 8;
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                vip_config current = (vip_config) enumerator.Current;
                GameObject obj2 = UnityEngine.Object.Instantiate(this.itemPrefab) as GameObject;
                int num2 = 0x9d2ab7;
                if (Mathf.Max(0, current.entry - 1) == 0)
                {
                    num2 = 0x9d2ab8;
                }
                object[] args = new object[] { current.exp, Mathf.Max(0, current.entry - 1), current.dupsamsh_times, current.phyforce_buy_times, current.elite_buy_times, 0, current.courage_shop_refresh_times, current.warmmatch_buy_times };
                string str2 = string.Format(ConfigMgr.getInstance().GetWord(num2), args);
                string format = string.Empty;
                string str4 = string.Empty;
                if (current.exp != 0)
                {
                    format = ConfigMgr.getInstance().GetWord(0x9d2abf);
                    str4 = str4 + string.Format(format, current.exp / 10) + "\n";
                    num--;
                }
                if (Mathf.Max(0, current.entry) != 0)
                {
                    format = ConfigMgr.getInstance().GetWord(0x9d2ac0);
                    str4 = str4 + string.Format(format, Mathf.Max(0, current.entry)) + "\n";
                    num--;
                }
                if (current.dupsamsh_times != 0)
                {
                    format = ConfigMgr.getInstance().GetWord(0x9d2ac1);
                    str4 = str4 + string.Format(format, current.dupsamsh_times) + "\n";
                    num--;
                }
                if (current.phyforce_buy_times != 0)
                {
                    format = ConfigMgr.getInstance().GetWord(0x9d2ac2);
                    str4 = str4 + string.Format(format, current.phyforce_buy_times) + "\n";
                    num--;
                }
                if (current.elite_buy_times != 0)
                {
                    format = ConfigMgr.getInstance().GetWord(0x9d2ac3);
                    str4 = str4 + string.Format(format, current.elite_buy_times) + "\n";
                    num--;
                }
                if (current.arenaladder_buyattack_times > 0)
                {
                    format = ConfigMgr.getInstance().GetWord(0x9d2ac6);
                    str4 = str4 + string.Format(format, current.arenaladder_buyattack_times) + "\n";
                    num--;
                }
                if (current.shake_gold_count != 0)
                {
                    format = ConfigMgr.getInstance().GetWord(0x9d2ac7);
                    str4 = str4 + string.Format(format, current.shake_gold_count) + "\n";
                    num--;
                }
                str2 = str4;
                str2 = str2 + current.func_desc;
                obj2.transform.FindChild("text").GetComponent<UILabel>().text = str2;
                obj2.transform.parent = this.GridObj.transform;
                obj2.transform.localPosition = new Vector3(-130f, 72f, 0f);
                obj2.transform.localScale = Vector3.one;
                obj2.transform.rotation = Quaternion.identity;
                obj2.AddComponent<VipDescriptEntry>().entry = current.entry;
                UIEventListener.Get(base.transform.FindChild("left").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLeftBtn);
                UIEventListener.Get(base.transform.FindChild("right").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRightBtn);
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
        this.GridObj.Reposition();
        this.ctr = this.GridObj.GetComponent<UICenterOnChild>();
        this.grid = this.ctr.GetComponent<UIGrid>();
        this.cellWidth = this.grid.cellWidth;
        this.ctr.Recenter();
        this.InitPage(ActorData.getInstance().UserInfo.vip_level.level);
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    public void ReSetNum()
    {
        float x = this.panel.transform.localPosition.x;
        float num2 = -this.num * this.cellWidth;
        if (x < (num2 - (this.cellWidth * 0.5f)))
        {
            this.num++;
            if (this.num > 15)
            {
                this.num = 15;
            }
            this.ReSetPage();
        }
        else if (x > (num2 + (this.cellWidth * 0.5f)))
        {
            this.num--;
            if (this.num < 0)
            {
                this.num = 0;
            }
            this.ReSetPage();
        }
    }

    public void ReSetPage()
    {
        int num = this.num + 1;
        int num2 = Mathf.Max(num - 1, 0);
        int num3 = Mathf.Min(num + 1, 0x10);
        this.preLV.text = num2.ToString();
        this.NextLV.text = num3.ToString();
        this.TitleLV.text = num.ToString();
        switch (num)
        {
            case 1:
                this.leftBtn.SetActive(true);
                this.rightBtn.SetActive(false);
                nguiTextureGrey.doChangeEnableGrey(this.leftB, true);
                nguiTextureGrey.doChangeEnableGrey(this.rightB, false);
                break;

            case 0x10:
                this.rightBtn.SetActive(true);
                this.leftBtn.SetActive(false);
                nguiTextureGrey.doChangeEnableGrey(this.leftB, false);
                nguiTextureGrey.doChangeEnableGrey(this.rightB, true);
                break;

            default:
                this.leftBtn.SetActive(false);
                this.rightBtn.SetActive(false);
                nguiTextureGrey.doChangeEnableGrey(this.leftB, false);
                nguiTextureGrey.doChangeEnableGrey(this.rightB, false);
                break;
        }
    }

    private void TurnPage(bool _towardsLeft)
    {
        if ((null != this.ctr.centeredObject) && (Time.time >= (this.lastClickTime + 0.35f)))
        {
            this.ctr.enabled = false;
            int num = 1;
            if (!_towardsLeft)
            {
                num = -1;
            }
            this.num -= num;
            if (this.num < 0)
            {
                this.num = 0;
            }
            else if (this.num > 15)
            {
                this.num = 15;
            }
            float x = this.panel.transform.localPosition.x + (num * this.cellWidth);
            Vector3 vector = new Vector3(x, this.panel.transform.localPosition.y, this.panel.transform.localPosition.z);
            Vector2 vector2 = new Vector2(-x, this.panel.clipOffset.y);
            Holoville.HOTween.HOTween.To(this.panel.transform, 0.2f, "localPosition", vector).easeType = Holoville.HOTween.EaseType.EaseInQuad;
            Holoville.HOTween.HOTween.To(this.panel, 0.2f, "clipOffset", vector2).easeType = Holoville.HOTween.EaseType.EaseInQuad;
            this.DelayCallBack(0.2f, () => this.ctr.enabled = true);
            this.lastClickTime = Time.time;
            this.ReSetPage();
        }
    }

    private UIGrid GridObj
    {
        get
        {
            if (null == this._grid)
            {
                this._grid = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
            }
            return this._grid;
        }
    }

    private UILabel NextLV
    {
        get
        {
            if (null == this._nextLV)
            {
                this._nextLV = base.transform.FindChild("right/value").GetComponent<UILabel>();
            }
            return this._nextLV;
        }
    }

    private UILabel preLV
    {
        get
        {
            if (null == this._preLV)
            {
                this._preLV = base.transform.FindChild("left/value").GetComponent<UILabel>();
            }
            return this._preLV;
        }
    }

    private UIScrollView ScrollView
    {
        get
        {
            if (null == this._scrollView)
            {
                this._scrollView = base.transform.FindChild("List").GetComponent<UIScrollView>();
            }
            return this._scrollView;
        }
    }

    private UILabel TitleLV
    {
        get
        {
            if (null == this._titleLV)
            {
                this._titleLV = base.transform.FindChild("lv/value").GetComponent<UILabel>();
            }
            return this._titleLV;
        }
    }
}

