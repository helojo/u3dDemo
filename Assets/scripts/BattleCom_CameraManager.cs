using System;
using UnityEngine;

public class BattleCom_CameraManager : BattleCom_Base
{
    private Vector3 attachPos;
    private BattleCameraControl cameraControl;
    public static bool isShowTimeing;
    private GameObject tempAttachObj;

    public void BeginAttachToPlayer()
    {
        this.GetCurCamera().enabled = true;
        BattleFighter attackHeadFighter = base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetAttackHeadFighter();
        if (attackHeadFighter == null)
        {
            for (int i = 0; i < BattleGlobal.FighterNumberMax; i++)
            {
                attackHeadFighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(i);
                if (attackHeadFighter != null)
                {
                    break;
                }
            }
        }
        if (attackHeadFighter != null)
        {
            this.GetCurCamera().SetLookAt(attackHeadFighter.transform);
            UnityEngine.Object.Destroy(this.tempAttachObj);
            this.tempAttachObj = null;
            this.attachPos = Vector3.zero;
        }
    }

    public void BeginAttachToPlayerAndTeamDir(bool isInitInfo)
    {
        this.BeginAttachToPlayer();
        if (isInitInfo)
        {
            this.GetCurCamera().UseLookAtInitProject = true;
            BattleCom_ScenePosManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>();
            this.GetCurCamera().InitLookAtInfo(-component.GetSceneFighterDirByPhase());
        }
    }

    public void BeginRestoring()
    {
        if (this.GetCurCamera() != null)
        {
            this.GetCurCamera().BeginRestoring();
        }
    }

    public void EndAttachAndToStoryPoint()
    {
        if (this.GetCurCamera().GetLookAtObjTrans() != null)
        {
            this.TryInitTempAttachObj();
            BattleFighter attackLastFighter = base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetAttackLastFighter();
            BattleFighter defenderLastFighter = base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetDefenderLastFighter();
            if ((attackLastFighter != null) && (defenderLastFighter != null))
            {
                this.attachPos = (Vector3) ((attackLastFighter.moveControler.GetPosition() + defenderLastFighter.moveControler.GetPosition()) / 2f);
                this.tempAttachObj.transform.position = this.attachPos;
                this.tempAttachObj.transform.rotation = this.GetCurCamera().GetLookAtObjTrans().rotation;
                this.GetCurCamera().SetLookAt(this.tempAttachObj.transform);
            }
        }
    }

    public void EndAttachToPlayer()
    {
        if (this.GetCurCamera().GetLookAtObjTrans() != null)
        {
            this.TryInitTempAttachObj();
            this.attachPos = this.GetCurCamera().GetLookAtObjTrans().position;
            this.tempAttachObj.transform.position = this.attachPos;
            this.tempAttachObj.transform.rotation = this.GetCurCamera().GetLookAtObjTrans().rotation;
            this.GetCurCamera().SetLookAt(this.tempAttachObj.transform);
        }
    }

    public void EndAttachToPlayerAndSetMiddlePos()
    {
        if (this.GetCurCamera().GetLookAtObjTrans() != null)
        {
            this.TryInitTempAttachObj();
            BattleFighter attackHeadFighter = base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetAttackHeadFighter();
            BattleFighter defenderHeadFighter = base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetDefenderHeadFighter();
            if ((attackHeadFighter != null) && (defenderHeadFighter != null))
            {
                this.attachPos = (Vector3) ((attackHeadFighter.moveControler.GetPosition() + defenderHeadFighter.moveControler.GetPosition()) / 2f);
                this.tempAttachObj.transform.position = this.attachPos;
                this.tempAttachObj.transform.rotation = this.GetCurCamera().GetLookAtObjTrans().rotation;
                this.GetCurCamera().SetLookAt(this.tempAttachObj.transform);
            }
        }
    }

    public BattleCameraControl GetCurCamera()
    {
        return this.cameraControl;
    }

    public void InitBindCamera()
    {
        if (this.cameraControl == null)
        {
            this.cameraControl = ObjectManager.CreateObj("BattlePrefabs/BattleCamera").GetComponent<BattleCameraControl>();
            this.cameraControl.name = "BattleCamera";
            this.cameraControl.BindLookAtToPlayerTeam();
        }
    }

    public override void OnCreateInit()
    {
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgEnter = (System.Action) Delegate.Combine(battleGameData.OnMsgEnter, new System.Action(this.OnMsgEnter));
        BattleData data2 = base.battleGameData;
        data2.OnMsgStart = (System.Action) Delegate.Combine(data2.OnMsgStart, new System.Action(this.OnMsgEnter));
        BattleData data3 = base.battleGameData;
        data3.OnMsgLeave = (System.Action) Delegate.Combine(data3.OnMsgLeave, new System.Action(this.OnMsgLeave));
    }

    private void OnMsgEnter()
    {
        BattleCameraControl curCamera = this.GetCurCamera();
        if (curCamera != null)
        {
            curCamera.SetShotState(base.battleGameData.cameraShotType, true);
        }
    }

    private void OnMsgLeave()
    {
        this.cameraControl = null;
        base.StopAllCoroutines();
    }

    public void SetEnable(bool enable)
    {
        BattleCameraControl curCamera = this.GetCurCamera();
        if (curCamera != null)
        {
            curCamera.gameObject.SetActive(enable);
        }
    }

    public void ShakeCamera(Vector3 offset, float time)
    {
        BattleCameraControl curCamera = this.GetCurCamera();
        if ((curCamera != null) && curCamera.gameObject.activeSelf)
        {
            curCamera.ShakeCamera(offset, time);
        }
        else if (Camera.main != null)
        {
            iTween.ShakePosition(Camera.main.gameObject, offset, time);
        }
    }

    private void TryInitTempAttachObj()
    {
        if (this.tempAttachObj == null)
        {
            this.tempAttachObj = new GameObject("tempCameraLookAt");
        }
    }

    public void TurnCameraX(float x)
    {
        if (this.GetCurCamera() != null)
        {
            this.GetCurCamera().TurnCameraX(x);
        }
    }

    public void TurnCameraY(float y)
    {
        if (this.GetCurCamera() != null)
        {
            this.GetCurCamera().TurnCameraY(y);
        }
    }

    public void ZoomCamera(float delta)
    {
        if (this.GetCurCamera() != null)
        {
            this.GetCurCamera().ZoomCamera(delta);
        }
    }
}

