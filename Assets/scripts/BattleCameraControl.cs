using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCameraControl : MonoBehaviour
{
    [CompilerGenerated]
    private static Action<CameraInfo> <>f__am$cache22;
    public List<CameraInfo> cameraInfo = new List<CameraInfo>();
    private CameraInfo curCameraInfo = new CameraInfo();
    private Vector3 curDir;
    private ShotState curShotState;
    private float dirXAcc;
    private float dirXOffset;
    private float dirYAcc;
    private float dirYOffset;
    private bool isRestoring;
    public bool IsSmoothMove = true;
    private Transform lookAt;
    private Vector3 lookAtInitDirNormal;
    private Vector3 lookAtInitPos;
    private Camera myCamera;
    private Vector3 oldLookAtPos;
    private Vector3 oldShakePos;
    private Quaternion rot = Quaternion.identity;
    private GameObject shakeObj;
    public float smoothTime = 0.35f;
    private Transform transContorl;
    private Vector3 turnSmoothVecAdd;
    private Vector3 turnSmoothVecAdd_Y;
    private Vector3 vec = Vector3.zero;
    private Vector3 vec2 = Vector3.zero;
    private Vector3 vecBase = Vector3.zero;
    private Vector3 vecBaseOffset = Vector3.zero;
    public float zoomAutoRevertDelayTime = 3f;
    public float zoomAutoRevertSpeed = 2f;
    public float zoomMaxValue = 4f;
    public float zoomMinValue;
    private float zoomSmoothBase;
    private float zoomSmoothVec;
    private float zoomValueCount;

    [DebuggerHidden]
    private IEnumerator _RealShakeCamera(Vector3 offset, float time)
    {
        return new <_RealShakeCamera>c__Iterator4 { offset = offset, time = time, <$>offset = offset, <$>time = time, <>f__this = this };
    }

    public void AddAnim(AnimationClip anim, string name)
    {
        this.myCamera.gameObject.animation.AddClip(anim, name);
        this.myCamera.gameObject.animation.Play(name);
    }

    public void BeginRestoring()
    {
        this.isRestoring = true;
    }

    public void BindLookAtToPlayerTeam()
    {
        if (<>f__am$cache22 == null)
        {
            <>f__am$cache22 = obj => obj.dir.Normalize();
        }
        this.cameraInfo.ForEach(<>f__am$cache22);
        if (this.cameraInfo.Count > 0)
        {
            this.curCameraInfo.Copy(this.cameraInfo[0]);
        }
        this.curDir = this.GetBaseDir();
        this.transContorl = base.transform;
        Transform transform = base.transform.FindChild("Camera");
        if (transform != null)
        {
            this.myCamera = transform.camera;
        }
    }

    private bool calDirTurn(ref float acc, ref float offset, ref Vector3 smoothValue, float minOffset, float maxOffset)
    {
        bool flag = false;
        if (acc != 0f)
        {
            offset += (acc * Time.deltaTime) * 10f;
            acc = Mathf.SmoothDamp(acc, 0f, ref smoothValue.x, 0.2f);
            offset = Mathf.Clamp(offset, minOffset, maxOffset);
            flag = true;
        }
        if ((offset != 0f) && this.isRestoring)
        {
            acc = 0f;
            if (offset != 0f)
            {
                offset = Mathf.SmoothDamp(offset, 0f, ref smoothValue.y, this.zoomAutoRevertSpeed);
                flag = true;
            }
        }
        return flag;
    }

    private void DoZoom(float delta)
    {
        float num = delta * Time.deltaTime;
        this.zoomValueCount += num;
        this.zoomValueCount = Mathf.Clamp(this.zoomValueCount, this.curCameraInfo.zoomMin, this.curCameraInfo.zoomMax);
    }

    private Vector3 GetBaseDir()
    {
        return this.curCameraInfo.dir;
    }

    private Vector3 GetBaseLookAtOffset()
    {
        return this.curCameraInfo.lookAtOffset;
    }

    private float GetBaseZoom()
    {
        return this.curCameraInfo.GetZoom();
    }

    private Vector3 GetCurDir()
    {
        return this.curDir;
    }

    private float GetCurZoom()
    {
        return (this.GetBaseZoom() + this.zoomValueCount);
    }

    private Vector3 GetLookAt()
    {
        if (this.lookAt == null)
        {
            return Vector3.zero;
        }
        Vector3 position = this.lookAt.position;
        if (this.UseLookAtInitProject)
        {
            Vector3 vector = this.lookAt.position - this.lookAtInitPos;
            position = this.lookAtInitPos + Vector3.Project(vector, this.lookAtInitDirNormal);
            position.y = this.lookAt.position.y;
        }
        Vector3 baseLookAtOffset = this.GetBaseLookAtOffset();
        float magnitude = baseLookAtOffset.magnitude;
        Vector3 normalized = baseLookAtOffset.normalized;
        normalized = this.TransformDir(normalized);
        return (position + ((Vector3) (normalized * magnitude)));
    }

    public Transform GetLookAtObjTrans()
    {
        return this.lookAt;
    }

    private Quaternion GetRot()
    {
        return this.rot;
    }

    public void InitLookAtInfo(Vector3 forward)
    {
        this.lookAtInitDirNormal = forward;
        this.lookAtInitPos = this.lookAt.position;
        this.rot = Quaternion.LookRotation(this.lookAtInitDirNormal);
    }

    private void modifyTargetPos(GameObject obj)
    {
        float num = 0.5625f;
        float num2 = ((float) Screen.width) / ((float) Screen.height);
        float num3 = num2 / num;
        Vector3 localPosition = obj.transform.localPosition;
        localPosition.x *= num3;
        obj.transform.localPosition = localPosition;
    }

    public void Reset()
    {
        this.zoomValueCount = 0f;
        this.zoomSmoothVec = 0f;
        this.zoomSmoothBase = 0f;
        this.dirXOffset = 0f;
        this.dirXAcc = 0f;
        this.turnSmoothVecAdd = Vector3.zero;
        this.dirYOffset = 0f;
        this.dirYAcc = 0f;
        this.turnSmoothVecAdd_Y = Vector3.zero;
    }

    public void SetLookAt(Transform _lookAt)
    {
        this.lookAt = _lookAt;
    }

    public void SetShotState(ShotState state, bool rightNow)
    {
        this.curShotState = state;
        if (rightNow)
        {
            if (this.curShotState < this.cameraInfo.Count)
            {
                this.curCameraInfo.Copy(this.cameraInfo[(int) this.curShotState]);
            }
            this.curDir = this.GetBaseDir();
            this.UpdateCameraNow();
        }
    }

    public void ShakeCamera(Vector3 offset, float time)
    {
        if (time > 0f)
        {
            base.StartCoroutine(this._RealShakeCamera(offset, time));
        }
    }

    private Vector3 TransformDir(Vector3 dir)
    {
        return (Vector3) (this.GetRot() * dir);
    }

    private Vector3 TransformPoint(Vector3 point)
    {
        return Matrix4x4.TRS(this.GetLookAt(), this.GetRot(), Vector3.one).MultiplyPoint(point);
    }

    public void TurnCameraX(float x)
    {
        this.isRestoring = false;
        this.dirXAcc += x;
    }

    public void TurnCameraY(float y)
    {
        this.isRestoring = false;
        this.dirYAcc += y;
    }

    private bool TurnDirTick()
    {
        bool flag = false;
        flag |= this.calDirTurn(ref this.dirXAcc, ref this.dirXOffset, ref this.turnSmoothVecAdd, this.curCameraInfo.dirOffsetMin.x, this.curCameraInfo.dirOffsetMax.x);
        return (flag | this.calDirTurn(ref this.dirYAcc, ref this.dirYOffset, ref this.turnSmoothVecAdd_Y, this.curCameraInfo.dirOffsetMin.y, this.curCameraInfo.dirOffsetMax.y));
    }

    private void Update()
    {
        this.UpdateBaseData();
        if ((this.zoomValueCount != 0f) && this.isRestoring)
        {
            this.zoomValueCount = Mathf.SmoothDamp(this.zoomValueCount, 0f, ref this.zoomSmoothVec, this.zoomAutoRevertSpeed);
        }
        bool flag = this.TurnDirTick();
        this.UpdateTurn();
        if (this.IsSmoothMove && (this.lookAt != null))
        {
            float smoothTime = !flag ? this.smoothTime : (this.smoothTime / 4f);
            Vector3 zero = Vector3.zero;
            if (this.shakeObj != null)
            {
                zero = this.shakeObj.transform.position;
            }
            Vector3 target = this.TransformPoint((Vector3) (this.GetCurDir() * this.GetCurZoom()));
            Vector3 current = this.transContorl.position - this.oldShakePos;
            Vector3 vector4 = Vector3.zero;
            vector4 = Vector3.SmoothDamp(current, target, ref this.vec, smoothTime) + zero;
            this.transContorl.position = vector4;
            this.oldLookAtPos = Vector3.SmoothDamp(this.oldLookAtPos, this.GetLookAt(), ref this.vec2, smoothTime);
            this.oldLookAtPos += zero;
            this.transContorl.LookAt(this.oldLookAtPos);
            this.oldLookAtPos -= zero;
            this.oldShakePos = zero;
        }
    }

    private void UpdateBaseData()
    {
        int curShotState = (int) this.curShotState;
        if (curShotState < this.cameraInfo.Count)
        {
            if (this.cameraInfo[curShotState].dir != this.curCameraInfo.dir)
            {
                this.curCameraInfo.dir = Vector3.SmoothDamp(this.curCameraInfo.dir, this.cameraInfo[curShotState].dir, ref this.vecBase, BattleGlobal.ScaleTime(this.smoothTime / 3f));
            }
            if (this.cameraInfo[curShotState].defaultZoom != this.curCameraInfo.defaultZoom)
            {
                this.curCameraInfo.defaultZoom = Mathf.SmoothDamp(this.curCameraInfo.defaultZoom, this.cameraInfo[curShotState].defaultZoom, ref this.zoomSmoothBase, BattleGlobal.ScaleTime(this.zoomAutoRevertSpeed / 3f));
            }
            if (this.cameraInfo[curShotState].lookAtOffset != this.curCameraInfo.lookAtOffset)
            {
                this.curCameraInfo.lookAtOffset = Vector3.SmoothDamp(this.curCameraInfo.lookAtOffset, this.cameraInfo[curShotState].lookAtOffset, ref this.vecBaseOffset, BattleGlobal.ScaleTime(this.smoothTime / 3f));
            }
            bool flag = false;
            if (this.cameraInfo[curShotState].zoomMin != this.curCameraInfo.zoomMin)
            {
                this.curCameraInfo.zoomMin = this.cameraInfo[curShotState].zoomMin;
                flag = true;
            }
            if (this.cameraInfo[curShotState].zoomMax != this.curCameraInfo.zoomMax)
            {
                this.curCameraInfo.zoomMax = this.cameraInfo[curShotState].zoomMax;
                flag = true;
            }
            if (flag)
            {
                this.zoomValueCount = Mathf.Clamp(this.zoomValueCount, this.curCameraInfo.zoomMin, this.curCameraInfo.zoomMax);
            }
        }
    }

    public void UpdateCameraNow()
    {
        Vector3 vector = this.TransformPoint((Vector3) (this.GetCurDir() * this.GetCurZoom()));
        this.transContorl.position = vector;
        this.oldLookAtPos = this.GetLookAt();
        this.transContorl.LookAt(this.GetLookAt());
        this.myCamera.fieldOfView = this.curCameraInfo.fov;
    }

    public void UpdateTurn()
    {
        Quaternion q = Quaternion.Euler(this.dirYOffset, this.dirXOffset, 0f);
        this.curDir = Matrix4x4.TRS(Vector3.zero, q, Vector3.one).MultiplyVector(this.GetBaseDir());
    }

    public void ZoomCamera(float delta)
    {
        this.isRestoring = false;
        if (((this.zoomValueCount > this.curCameraInfo.zoomMin) || (delta >= 0f)) && ((this.zoomValueCount < this.curCameraInfo.zoomMax) || (delta <= 0f)))
        {
            this.DoZoom(delta * 2f);
        }
    }

    public bool UseLookAtInitProject { get; set; }

    [CompilerGenerated]
    private sealed class <_RealShakeCamera>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Vector3 <$>offset;
        internal float <$>time;
        internal BattleCameraControl <>f__this;
        internal Vector3 offset;
        internal float time;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.shakeObj = new GameObject("shake");
                    this.<>f__this.shakeObj.transform.position = Vector3.zero;
                    iTween.ShakePosition(this.<>f__this.shakeObj, this.offset, this.time);
                    this.$current = new WaitForSeconds(this.time);
                    this.$PC = 1;
                    return true;

                case 1:
                    UnityEngine.Object.DestroyObject(this.<>f__this.shakeObj);
                    this.<>f__this.shakeObj = null;
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

