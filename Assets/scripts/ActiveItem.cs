using FastBuf;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ActiveItem : MonoBehaviour
{
    public UITexture activeBg;
    public int activeId;
    public GameObject black;
    public GameObject cur;
    private ActiveDetailState curActiveDetailState;
    public int curIndex;
    public GameObject fcur;
    public GameObject huobaojinxing;
    public ActiveInfo info = new ActiveInfo();
    public UILabel itemName;
    public GameObject jinxing;
    public GameObject newAct;
    public GameObject qita;
    public int sort;
    public UISprite spriteComplete;
    public UISprite typePic;
    private UIDragScrollView uidsv;

    private bool BoolTime()
    {
        return this.IsActivityEnable(this.info.startTime, this.info.holdOnTime, this.info.cdTime);
    }

    private bool ChickNew()
    {
        if (!SettingMgr.mInstance.GetActiveBool(string.Empty + this.info.activity_unid))
        {
            return true;
        }
        this.info.is_new = false;
        return false;
    }

    private bool IsActivityEnable(uint startTime, int duration, int cdTime)
    {
        long num = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
        if (num < startTime)
        {
            return false;
        }
        if (duration == -1)
        {
            return true;
        }
        if (cdTime == -1)
        {
            return (num < (startTime + (duration * 60)));
        }
        long num2 = (num - startTime) % ((long) ((duration + cdTime) * 60));
        return (num2 < (duration * 60));
    }

    public void OnClick()
    {
        ActivePanel.inst.lastSlot = this.curIndex;
        ActivePanel.inst.ResetInfo(this.info, true, this.BoolTime());
        if (this.info.is_new)
        {
            this.info.is_new = false;
            SettingMgr.mInstance.SetActiveBool(string.Empty + this.info.activity_unid);
        }
        SocketMgr.Instance.CheckActiveState(ActiveList.actives);
        this.ResetState(this.BoolTime(), false);
    }

    public void ResetCardInfo()
    {
        this.itemName.text = this.info.activity_name;
        this.ResetState(this.BoolTime(), true);
        this.sort = this.info.sort_priority;
    }

    private void ResetState(bool state, bool needNotToChange = true)
    {
        this.typePic.spriteName = "Ui__Active_Bg_left_" + ((int) this.info.showType);
        if (this.info.activity_PicName != null)
        {
            this.activeBg.mainTexture = BundleMgr.Instance.CreateActiveBackGround(this.info.activity_PicName);
        }
        if (this.info.is_new && this.ChickNew())
        {
            this.newAct.SetActive(true);
            this.jinxing.SetActive(false);
            this.huobaojinxing.SetActive(false);
            this.qita.SetActive(false);
            this.curActiveDetailState = ActiveDetailState.New;
        }
        else
        {
            this.newAct.SetActive(false);
            if (this.info.activity_type == ActivityType.e_tencent_activity_shop_package)
            {
                this.jinxing.SetActive(false);
                this.huobaojinxing.SetActive(state);
                if (state)
                {
                    this.curActiveDetailState = ActiveDetailState.Going;
                }
            }
            else
            {
                this.jinxing.SetActive(state);
                this.huobaojinxing.SetActive(false);
                if (state)
                {
                    this.curActiveDetailState = ActiveDetailState.Going;
                }
            }
            if (!state)
            {
                if (this.curActiveDetailState == ActiveDetailState.Will)
                {
                    this.qita.SetActive(!state);
                    if (!state)
                    {
                        this.curActiveDetailState = ActiveDetailState.Will;
                    }
                }
                else if (this.curActiveDetailState == ActiveDetailState.Going)
                {
                    this.jinxing.SetActive(!state);
                    this.qita.SetActive(state);
                    if (!state)
                    {
                        this.curActiveDetailState = ActiveDetailState.Going;
                    }
                }
                else
                {
                    this.qita.SetActive(!state);
                    if (!state)
                    {
                        this.curActiveDetailState = ActiveDetailState.Will;
                    }
                }
            }
            else
            {
                this.qita.SetActive(!state);
                if (!state)
                {
                    this.curActiveDetailState = ActiveDetailState.Will;
                }
            }
        }
    }

    public void SelectButton(bool curS)
    {
        this.cur.SetActive(!curS);
        this.fcur.SetActive(curS);
    }

    private int SortActiveList(ActiveInfo info1, ActiveInfo info2)
    {
        return (info1.sort_priority - info2.sort_priority);
    }

    private void Start()
    {
        this.uidsv = base.GetComponent<UIDragScrollView>();
        this.uidsv.scrollView = ActivePanel.inst.uisv;
        this.ResetCardInfo();
    }

    private enum ActiveDetailState
    {
        New,
        Going,
        Will
    }
}

