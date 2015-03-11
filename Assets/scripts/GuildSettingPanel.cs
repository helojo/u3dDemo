using FastBuf;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildSettingPanel : GUIEntity
{
    private List<string> ApplyTypeList = new List<string>();
    private int CurLimitLev;
    private int CurType;
    public UILabel LevLabel;
    private float m_interval = 1f;
    private float m_timer;
    private bool PressBtn;
    private bool PressLeftBtn;
    public UILabel TypeLabel;

    private void ClickLevLeft()
    {
        if (this.CurLimitLev > GameConstValues.GUILD_MIN_APPLY_LEVEL)
        {
            this.CurLimitLev--;
            this.UpdateLev(this.CurLimitLev);
        }
        else if (this.CurLimitLev <= GameConstValues.GUILD_MIN_APPLY_LEVEL)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa65292));
        }
    }

    private void ClickLevRight()
    {
        if (this.CurLimitLev >= CommonFunc.GetUserMaxLv())
        {
            TipsDiag.SetText("已经是最大设置等级");
        }
        else
        {
            this.CurLimitLev++;
        }
        this.UpdateLev(this.CurLimitLev);
    }

    private void ClickOk()
    {
        SocketMgr.Instance.RequestGuildSetNotice(ActorData.getInstance().mGuildData.notice, this.CurType, this.CurLimitLev);
    }

    private void ClickTypeLeft()
    {
        if (this.CurType > 0)
        {
            this.CurType--;
            this.UpdateType((GuildApplyType) this.CurType);
        }
        else if (this.CurType <= 0)
        {
            this.CurType = 2;
            this.UpdateType((GuildApplyType) this.CurType);
        }
    }

    private void ClickTypeRight()
    {
        if (this.CurType >= 2)
        {
            this.CurType = 0;
            this.UpdateType((GuildApplyType) this.CurType);
        }
        else
        {
            this.CurType++;
            this.UpdateType((GuildApplyType) this.CurType);
        }
    }

    private void OnClickLeftLvBtn(GameObject obj, bool _state)
    {
        this.PressBtn = _state;
        this.PressLeftBtn = true;
        if (!_state)
        {
            this.m_timer = 0f;
        }
    }

    private void OnClickRightLvBtn(GameObject obj, bool _state)
    {
        this.PressBtn = _state;
        this.PressLeftBtn = false;
        if (!_state)
        {
            this.m_timer = 0f;
        }
    }

    public override void OnInitialize()
    {
        this.ApplyTypeList.Add(ConfigMgr.getInstance().GetWord(0x9d2ab0));
        this.ApplyTypeList.Add(ConfigMgr.getInstance().GetWord(0x9d2ab1));
        this.ApplyTypeList.Add(ConfigMgr.getInstance().GetWord(0x9d2ab2));
        this.CurType = ActorData.getInstance().mGuildData.apply_type;
        this.CurLimitLev = ActorData.getInstance().mGuildData.apply_level;
        GameObject gameObject = base.gameObject.transform.FindChild("Limit/RButton").gameObject;
        GameObject go = base.gameObject.transform.FindChild("Limit/LButton").gameObject;
        UIEventListener.Get(gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickRightLvBtn);
        UIEventListener.Get(go).onPress = new UIEventListener.BoolDelegate(this.OnClickLeftLvBtn);
        this.UpdateType((GuildApplyType) this.CurType);
        this.UpdateLev(this.CurLimitLev);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.PressBtn)
        {
            this.m_timer += Time.deltaTime;
            if (this.m_timer > this.m_interval)
            {
                if (this.PressLeftBtn)
                {
                    if (this.CurLimitLev > GameConstValues.GUILD_MIN_APPLY_LEVEL)
                    {
                        this.CurLimitLev--;
                    }
                }
                else if (this.CurLimitLev < CommonFunc.GetUserMaxLv())
                {
                    this.CurLimitLev++;
                }
                this.UpdateLev(this.CurLimitLev);
            }
        }
    }

    private void UpdateLev(int _LimitLev)
    {
        this.LevLabel.text = _LimitLev.ToString();
    }

    private void UpdateType(GuildApplyType _type)
    {
        this.TypeLabel.text = this.ApplyTypeList[(int) _type];
    }
}

