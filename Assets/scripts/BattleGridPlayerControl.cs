using System;
using UnityEngine;

public class BattleGridPlayerControl : BattlePlayerControlImplBase
{
    private bool isCanControlCamera = true;
    private Vector2 mStartPos;

    public override void Init()
    {
        base.battleGameData.InitBattleValue();
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgEnter = (System.Action) Delegate.Combine(battleGameData.OnMsgEnter, new System.Action(this.OnMsgEnter));
        BattleData data2 = base.battleGameData;
        data2.OnMsgStart = (System.Action) Delegate.Combine(data2.OnMsgStart, new System.Action(this.OnMsgStart));
        BattleData data3 = base.battleGameData;
        data3.OnMsgLeave = (System.Action) Delegate.Combine(data3.OnMsgLeave, new System.Action(this.OnMsgLeave));
        BattleData data4 = base.battleGameData;
        data4.OnMsgGridGameFinishOneBattle = (Action<bool, bool, BattleNormalGameResult>) Delegate.Combine(data4.OnMsgGridGameFinishOneBattle, new Action<bool, bool, BattleNormalGameResult>(this.OnMsgGridGameFinishOneBattle));
    }

    private void On_DoubleTap(Gesture gesture)
    {
        if (this.isCanControlCamera)
        {
            base.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().BeginRestoring();
        }
    }

    private void On_PinchIn(Gesture gesture)
    {
        if (this.isCanControlCamera)
        {
            BattleCom_CameraManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>();
            if (component != null)
            {
                component.ZoomCamera(gesture.deltaPinch);
            }
        }
        else
        {
            base.battleGameData.battleComObject.GetComponent<BattleGridGameMapControl>().cameraObj.GetComponent<CameraChanger>().Zoom(gesture.deltaPinch);
        }
    }

    private void On_PinchOut(Gesture gesture)
    {
        if (this.isCanControlCamera)
        {
            BattleCom_CameraManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>();
            if (component != null)
            {
                component.ZoomCamera(-gesture.deltaPinch);
            }
        }
        else
        {
            base.battleGameData.battleComObject.GetComponent<BattleGridGameMapControl>().cameraObj.GetComponent<CameraChanger>().Zoom(-gesture.deltaPinch);
        }
    }

    private void On_Swipe(Gesture gesture)
    {
        if (this.isCanControlCamera)
        {
            BattleCom_CameraManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>();
            if ((gesture.touchCount == 1) && (component != null))
            {
                Vector2 vector = gesture.position - this.mStartPos;
                component.TurnCameraX(vector.x * 0.1f);
                component.TurnCameraY(vector.y * 0.1f);
                this.mStartPos = gesture.position;
            }
        }
    }

    private void On_SwipeEnd(Gesture gesture)
    {
    }

    private void On_SwipeStart(Gesture gesture)
    {
        this.mStartPos = gesture.startPosition;
    }

    private void OnMsgEnter()
    {
        this.isCanControlCamera = false;
        this.Subscribe();
        this.mStartPos = Vector2.zero;
    }

    private void OnMsgGridGameFinishOneBattle(bool isWin, bool isBreak, BattleNormalGameResult result)
    {
        this.isCanControlCamera = false;
    }

    private void OnMsgLeave()
    {
        this.isCanControlCamera = false;
        this.Unsubscribe();
    }

    private void OnMsgStart()
    {
        this.isCanControlCamera = true;
    }

    public override void QuestSkip()
    {
        if (base.battleGameData.round <= 1)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x198));
        }
    }

    private void Subscribe()
    {
        EasyTouch.On_PinchIn += new EasyTouch.PinchInHandler(this.On_PinchIn);
        EasyTouch.On_PinchOut += new EasyTouch.PinchOutHandler(this.On_PinchOut);
        EasyTouch.On_SwipeStart += new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe += new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd += new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_DoubleTap += new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
    }

    private void Unsubscribe()
    {
        EasyTouch.On_PinchIn -= new EasyTouch.PinchInHandler(this.On_PinchIn);
        EasyTouch.On_PinchOut -= new EasyTouch.PinchOutHandler(this.On_PinchOut);
        EasyTouch.On_SwipeStart -= new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd -= new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_DoubleTap += new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
    }
}

