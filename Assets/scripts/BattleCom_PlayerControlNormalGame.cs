using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_PlayerControlNormalGame : BattlePlayerControlImplBase
{
    [CompilerGenerated]
    private static Predicate<BattleDropObject> <>f__am$cache3;
    private Vector2 cameraStartPos;
    private List<BattleDropObject> dropObjs = new List<BattleDropObject>();
    private bool isCanControlCamera = true;

    public override void Init()
    {
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgEnter = (System.Action) Delegate.Combine(battleGameData.OnMsgEnter, new System.Action(this.OnMsgEnter));
        BattleData data2 = base.battleGameData;
        data2.OnMsgLeave = (System.Action) Delegate.Combine(data2.OnMsgLeave, new System.Action(this.OnMsgLeave));
        BattleData data3 = base.battleGameData;
        data3.OnMsgBattleRunningChange = (Action<bool>) Delegate.Combine(data3.OnMsgBattleRunningChange, new Action<bool>(this.OnMsgBattleRunningChange));
        this.isCanControlCamera = false;
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
    }

    private void On_Swipe(Gesture gesture)
    {
        if (this.isCanControlCamera)
        {
            BattleCom_CameraManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>();
            if ((gesture.touchCount == 1) && (component != null))
            {
                Vector2 vector = gesture.position - this.cameraStartPos;
                component.TurnCameraX(vector.x * 0.1f);
                component.TurnCameraY(vector.y * 0.1f);
                this.cameraStartPos = gesture.position;
            }
        }
    }

    private void On_SwipeEnd(Gesture gesture)
    {
    }

    private void On_SwipeStart(Gesture gesture)
    {
        this.cameraStartPos = gesture.startPosition;
    }

    private void On_TouchDown(Gesture gesture)
    {
        RaycastHit hit;
        if ((null != Camera.main) && (Physics.Raycast(Camera.main.ScreenPointToRay((Vector3) gesture.position), out hit, 500f) && (UICamera.hoveredObject == null)))
        {
            this.OnPickDropObj(hit.collider.gameObject);
        }
    }

    private void On_TouchStart2Fingers(Gesture gesture)
    {
        EasyTouch.SetEnableTwist(false);
        EasyTouch.SetEnablePinch(true);
    }

    public override void OnDrop(MonsterDrop dropInfo, Vector3 pos)
    {
        bool flag = true;
        foreach (ItemProperty property in dropInfo.items)
        {
            Vector3 vector = pos;
            if (!flag)
            {
                vector.x += UnityEngine.Random.Range((float) -0.8f, (float) 0.8f);
                vector.z += UnityEngine.Random.Range((float) -0.8f, (float) 0.8f);
            }
            Vector3 sceneFighterDirByPhase = base.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
            BattleDropObject item = BattleDropObject.CreateDropObj(property, vector, Quaternion.Euler(sceneFighterDirByPhase) * Quaternion.Euler(0f, 0f, 0f));
            this.dropObjs.Add(item);
            flag = false;
        }
    }

    private void OnMsgBattleRunningChange(bool isRunning)
    {
        this.isCanControlCamera = isRunning;
    }

    private void OnMsgEnter()
    {
        this.isCanControlCamera = false;
        this.dropObjs.Clear();
        this.Subscribe();
    }

    private void OnMsgLeave()
    {
        this.Unsubscribe();
    }

    [DebuggerHidden]
    public override IEnumerator OnPhaseStarting()
    {
        return new <OnPhaseStarting>c__Iterator25 { <>f__this = this };
    }

    private void OnPickDropObj(GameObject pickObj)
    {
        <OnPickDropObj>c__AnonStorey154 storey = new <OnPickDropObj>c__AnonStorey154 {
            pickObj = pickObj
        };
        BattleDropObject obj2 = this.dropObjs.Find(new Predicate<BattleDropObject>(storey.<>m__11C));
        if (obj2 != null)
        {
            obj2.BePick(0f, true);
        }
    }

    private void Subscribe()
    {
        EasyTouch.On_TouchDown += new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchStart2Fingers += new EasyTouch.TouchStart2FingersHandler(this.On_TouchStart2Fingers);
        EasyTouch.On_PinchIn += new EasyTouch.PinchInHandler(this.On_PinchIn);
        EasyTouch.On_PinchOut += new EasyTouch.PinchOutHandler(this.On_PinchOut);
        EasyTouch.On_SwipeStart += new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe += new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd += new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_DoubleTap += new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
    }

    public override void Tick()
    {
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = delegate (BattleDropObject obj) {
                if ((obj != null) && obj.isFinish)
                {
                    ObjectManager.DestoryObj(obj.gameObject);
                    return true;
                }
                return false;
            };
        }
        this.dropObjs.RemoveAll(<>f__am$cache3);
    }

    private void Unsubscribe()
    {
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchStart2Fingers -= new EasyTouch.TouchStart2FingersHandler(this.On_TouchStart2Fingers);
        EasyTouch.On_PinchIn -= new EasyTouch.PinchInHandler(this.On_PinchIn);
        EasyTouch.On_PinchOut -= new EasyTouch.PinchOutHandler(this.On_PinchOut);
        EasyTouch.On_SwipeStart -= new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd -= new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_DoubleTap -= new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
    }

    [CompilerGenerated]
    private sealed class <OnPhaseStarting>c__Iterator25 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<BattleDropObject>.Enumerator <$s_275>__1;
        internal List<BattleDropObject>.Enumerator <$s_276>__5;
        internal BattleCom_PlayerControlNormalGame <>f__this;
        internal float <delayTime>__4;
        internal BattleDropObject <drop>__2;
        internal BattleDropObject <drop>__6;
        internal bool <hasDrop>__0;
        internal bool <hasDrop>__3;

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
                    this.<hasDrop>__0 = false;
                    this.<$s_275>__1 = this.<>f__this.dropObjs.GetEnumerator();
                    try
                    {
                        while (this.<$s_275>__1.MoveNext())
                        {
                            this.<drop>__2 = this.<$s_275>__1.Current;
                            if ((this.<drop>__2 != null) && !this.<drop>__2.isFinish)
                            {
                                this.<hasDrop>__0 = true;
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_275>__1.Dispose();
                    }
                    if (this.<hasDrop>__0)
                    {
                        this.$current = new WaitForSeconds(0.1f);
                        this.$PC = 1;
                        goto Label_01C3;
                    }
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_01BA;

                default:
                    goto Label_01C1;
            }
            this.<hasDrop>__3 = false;
            this.<delayTime>__4 = 0f;
            this.<$s_276>__5 = this.<>f__this.dropObjs.GetEnumerator();
            try
            {
                while (this.<$s_276>__5.MoveNext())
                {
                    this.<drop>__6 = this.<$s_276>__5.Current;
                    if ((this.<drop>__6 != null) && !this.<drop>__6.isFinish)
                    {
                        this.<drop>__6.BePick(this.<delayTime>__4, false);
                        this.<hasDrop>__3 = true;
                        this.<delayTime>__4 += 0.1f;
                        this.<delayTime>__4 = Mathf.Min(this.<delayTime>__4, 0.3f);
                    }
                }
            }
            finally
            {
                this.<$s_276>__5.Dispose();
            }
            if (this.<hasDrop>__3)
            {
                this.$current = new WaitForSeconds(0.4f);
                this.$PC = 2;
                goto Label_01C3;
            }
        Label_01BA:
            this.$PC = -1;
        Label_01C1:
            return false;
        Label_01C3:
            return true;
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

    [CompilerGenerated]
    private sealed class <OnPickDropObj>c__AnonStorey154
    {
        internal GameObject pickObj;

        internal bool <>m__11C(BattleDropObject obj)
        {
            return ((obj != null) && (obj.gameObject == this.pickObj));
        }
    }
}

