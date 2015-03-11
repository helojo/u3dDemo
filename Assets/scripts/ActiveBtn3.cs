using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using UnityEngine;

public class ActiveBtn3 : MonoBehaviour
{
    public Act_Rech_Reward arr;
    public ActiveBtnType btnType = ActiveBtnType.chongzhi;
    public UniversialRewardDrawFlag flag = UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW;
    private float lastTime;
    public UILabel[] uil = new UILabel[4];
    public UISprite uis;
    public UITexture uit;

    private void OnClick()
    {
        if (((this.flag != UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_START) && (this.flag != UniversialRewardDrawFlag.E_UNIREWARD_FLAG_HAVE_DRAW)) && (Time.time >= (this.lastTime + 1f)))
        {
            ActivePanel.inst.ButtonEvent_LingQu(this.arr.slotId);
            this.lastTime = Time.time;
        }
    }

    public void ResetBtn()
    {
        for (int i = 0; i < this.uil.Length; i++)
        {
            if (i == this.flag)
            {
                this.uil[i].gameObject.SetActive(true);
            }
            else
            {
                this.uil[i].gameObject.SetActive(false);
            }
        }
        if (this.uit != null)
        {
            if ((this.flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_NOT_START) || (this.flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_HAVE_DRAW))
            {
                nguiTextureGrey.doChangeEnableGrey(this.uit, true);
            }
            else
            {
                nguiTextureGrey.doChangeEnableGrey(this.uit, false);
            }
        }
        else if (this.uis != null)
        {
            if (this.flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW)
            {
                this.uis.spriteName = "Button_lan";
                base.GetComponent<UIButton>().normalSprite = this.uis.spriteName;
            }
            else
            {
                this.uis.spriteName = "Button_langrey";
                base.GetComponent<UIButton>().normalSprite = this.uis.spriteName;
            }
        }
    }
}

