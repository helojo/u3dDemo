using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_SceneEffect : BattleCom_Base
{
    private int curIndex;
    private bool isChanging;
    private Color resetColor = Color.white;
    private float resetSmoothTime = 0.1f;
    private float resetTime = -1f;

    public int ChangeSceneColor(Color targetColor, float smoothTime, float durTime, List<int> excludeObj)
    {
        <ChangeSceneColor>c__AnonStoreyE2 ye = new <ChangeSceneColor>c__AnonStoreyE2 {
            excludeObj = excludeObj,
            targetColor = targetColor,
            smoothTime = smoothTime
        };
        this.Reset();
        this.resetTime = durTime;
        this.isChanging = true;
        BattleDarkScene.StartChangeColor(ye.targetColor, ye.smoothTime);
        BattleCom_FighterManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        if (component != null)
        {
            component.DoToAllFighter(new Action<BattleFighter, int>(ye.<>m__4B));
        }
        this.curIndex++;
        return this.curIndex;
    }

    public override void OnCreateInit()
    {
    }

    private void Reset()
    {
        if (this.isChanging)
        {
            this.resetTime = -1f;
            BattleDarkScene.StartChangeColor(this.resetColor, this.resetSmoothTime);
            BattleCom_FighterManager manager = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
            if (manager != null)
            {
                manager.DoToAllFighter(delegate (BattleFighter arg1, int arg2) {
                    GameObject animObj = arg1.GetAnimObj();
                    if (animObj != null)
                    {
                        MaterialFSM component = animObj.GetComponent<MaterialFSM>();
                        if (component != null)
                        {
                            component.StartChangeColor(this.resetColor, this.resetSmoothTime);
                        }
                    }
                });
            }
        }
    }

    public void StopChangeSceneColor(int index)
    {
        if (this.curIndex == index)
        {
            this.resetTime = -1f;
            this.Reset();
        }
    }

    private void Update()
    {
        if (this.resetTime >= 0f)
        {
            this.resetTime -= BattleGlobal.ScaleSpeed(1f) * Time.deltaTime;
            if (this.resetTime < 0f)
            {
                this.Reset();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ChangeSceneColor>c__AnonStoreyE2
    {
        internal List<int> excludeObj;
        internal float smoothTime;
        internal Color targetColor;

        internal void <>m__4B(BattleFighter arg1, int arg2)
        {
            if (!this.excludeObj.Contains(arg2))
            {
                GameObject animObj = arg1.GetAnimObj();
                if (animObj != null)
                {
                    MaterialFSM component = animObj.GetComponent<MaterialFSM>();
                    if (component != null)
                    {
                        component.StartChangeColor(this.targetColor, this.smoothTime);
                    }
                }
            }
        }
    }
}

