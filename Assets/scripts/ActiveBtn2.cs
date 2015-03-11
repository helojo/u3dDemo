using System;
using UnityEngine;

public class ActiveBtn2 : MonoBehaviour
{
    private bool state;
    public ActTypeMgr typeMgr;
    public UILabel uil;

    public void InitBtnState(bool state__)
    {
        this.ResetBtn(state__);
    }

    private void OnClick()
    {
        this.ResetBtn(this.state);
    }

    private void ResetBtn(bool _state)
    {
        if (_state)
        {
            this.uil.text = "奖励";
            this.typeMgr.ResetRewardPanel(!this.state, this.typeMgr.showType, true);
            this.state = false;
        }
        else
        {
            this.uil.text = "规则";
            this.typeMgr.ResetRewardPanel(!_state, this.typeMgr.showType, true);
            this.state = true;
        }
    }

    private void Start()
    {
        this.state = false;
        this.ResetBtn(this.state);
    }
}

