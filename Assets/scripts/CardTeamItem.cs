using System;
using System.Collections.Generic;
using UnityEngine;

public class CardTeamItem : MonoBehaviour
{
    private int _job;
    private int _starLevel;
    public UISprite CardFrame;
    public UISprite CardJob;
    public UILabel CardLevel;
    public UITexture CardTexture;
    public UISprite Friend;
    public UIGrid StarGrid;
    public List<GameObject> StarList;

    public void InitRankItemData(string cardTextureName, int cardFrame, int cardLevel, int starLevel, int job)
    {
        this.CardTexture.mainTexture = BundleMgr.Instance.CreateHeadIcon(cardTextureName);
        CommonFunc.SetQualityBorder(this.CardFrame, cardFrame);
        this.CardLevel.text = cardLevel + string.Empty;
        this.StarLevel = starLevel;
        this.Job = job;
    }

    internal int Job
    {
        get
        {
            return this._job;
        }
        set
        {
            this._job = value;
            CommonFunc.SetCardJobIcon(this.CardJob, this._job);
        }
    }

    internal int StarLevel
    {
        get
        {
            return this._starLevel;
        }
        set
        {
            this._starLevel = value;
            for (int i = 0; i < this._starLevel; i++)
            {
                this.StarList[i].SetActive(true);
            }
            for (int j = this._starLevel; j < this.StarList.Count; j++)
            {
                if (null != this.StarList[j])
                {
                    this.StarList[j].SetActive(false);
                }
            }
            this.StarGrid.hideInactive = true;
            if (this.StarGrid.pivot != UIWidget.Pivot.Center)
            {
                this.StarGrid.pivot = UIWidget.Pivot.Center;
            }
            this.StarGrid.Reposition();
            this.StarGrid.transform.localPosition = new Vector3(6f, -15f, 0f);
        }
    }
}

