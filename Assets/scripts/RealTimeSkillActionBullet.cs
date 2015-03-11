using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillActionBullet : RealTimeSkillActionBase
{
    private GameObject bullet;
    private Vector3? lastPos;
    private Vector3 oldTargetPos;

    protected override void Clear()
    {
        ObjectManager.DestoryObj(this.bullet);
    }

    private GameObject GetEndObj()
    {
        return ((base.info.bulletEnd != SkillActionPlayerType.caster) ? base.GetTargetModelObj() : base.GetCasterModelObj());
    }

    private GameObject GetStartObj()
    {
        return ((base.info.bulletStart != SkillActionPlayerType.caster) ? base.GetTargetModelObj() : base.GetCasterModelObj());
    }

    private Vector3 GetTargetPos()
    {
        Vector3 position;
        if (!this.IsTargetOK())
        {
            return this.oldTargetPos;
        }
        if (base.info.bulletEndAttachType == SkillEffectAttachType.ONE_AttachTo)
        {
            float magnitude = base.info.bulletEndOffset.magnitude;
            Vector3 vector2 = (Vector3) (this.GetEndObj().transform.TransformDirection(base.info.bulletEndOffset.normalized) * magnitude);
            position = this.GetEndObj().GetComponent<HangControler>().GetHangPointPos(base.info.bulletEndHangPoint) + vector2;
        }
        else if (base.info.bulletEndAttachType == SkillEffectAttachType.ONE_HangPointPos)
        {
            float num2 = base.info.bulletEndOffset.magnitude;
            Vector3 vector3 = (Vector3) (this.GetStartObj().transform.TransformDirection(base.info.bulletEndOffset.normalized) * num2);
            position = this.GetEndObj().GetComponent<HangControler>().GetHangPointPos(base.info.bulletEndHangPoint) + vector3;
        }
        else
        {
            position = this.GetEndObj().transform.position;
        }
        this.oldTargetPos = position;
        return position;
    }

    private bool IsTargetOK()
    {
        return (base.target != null);
    }

    [DebuggerHidden]
    protected override IEnumerator OnProcess(MonoBehaviour mb)
    {
        return new <OnProcess>c__Iterator35 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <OnProcess>c__Iterator35 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkillActionBullet <>f__this;
        internal float <curPrecent>__7;
        internal Vector3 <dir>__2;
        internal GameObject <endObj>__15;
        internal Vector3 <firstPos>__0;
        internal float <moveSpeed>__8;
        internal float <moveStep>__12;
        internal float <needTime>__6;
        internal Vector3 <oldPos>__14;
        internal Quaternion <oldRot>__13;
        internal Vector3[] <path>__5;
        internal float <pathLength>__3;
        internal Vector3 <pos>__10;
        internal Vector3 <posNext>__11;
        internal Vector3 <secondPos>__4;
        internal Vector3 <targetPos>__1;
        internal float <timePrecent>__9;

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
                    if (SettingMgr.mInstance.IsEffectEnable && (((this.<>f__this.GetStartObj() != null) && (this.<>f__this.GetEndObj() != null)) && this.<>f__this.IsTargetOK()))
                    {
                        this.<>f__this.bullet = ObjectManager.InstantiateObj(this.<>f__this.info.bulleteffect);
                        if (this.<>f__this.bullet != null)
                        {
                            this.<>f__this.bullet.name = "bullet   " + this.<>f__this.bullet.name;
                            BattleGlobalFunc.SetObjTag(this.<>f__this.bullet, BattleGlobal.NoShaderChangeTag);
                            if (this.<>f__this.info.OneToOne && this.<>f__this.lastPos.HasValue)
                            {
                                this.<>f__this.bullet.transform.position = this.<>f__this.lastPos.Value;
                            }
                            else
                            {
                                this.<>f__this.GetStartObj().GetComponent<HangControler>().AttachToHangPoint(this.<>f__this.bullet, this.<>f__this.info.bulletStartHangPoint, Vector3.zero);
                            }
                            this.<>f__this.bullet.transform.parent = null;
                            this.<firstPos>__0 = this.<>f__this.bullet.transform.position;
                            this.<targetPos>__1 = this.<>f__this.GetTargetPos();
                            this.<dir>__2 = this.<targetPos>__1 - this.<firstPos>__0;
                            this.<pathLength>__3 = this.<dir>__2.magnitude;
                            this.<dir>__2.Normalize();
                            this.<secondPos>__4 = this.<firstPos>__0 + ((Vector3) ((this.<dir>__2 * this.<pathLength>__3) * this.<>f__this.info.bulletHeightPrecent));
                            this.<secondPos>__4.y = this.<firstPos>__0.y + this.<>f__this.info.bulletHeight;
                            this.<path>__5 = new Vector3[] { this.<>f__this.bullet.transform.position, this.<secondPos>__4, this.<targetPos>__1 };
                            this.<pathLength>__3 = iTween.PathLength(this.<path>__5);
                            this.<needTime>__6 = this.<pathLength>__3 / this.<>f__this.info.bulletMoveSpeed;
                            this.<curPrecent>__7 = 0f;
                            break;
                        }
                    }
                    goto Label_06A1;

                case 1:
                    break;

                default:
                    goto Label_06A1;
            }
            while (Vector3.Distance(this.<>f__this.bullet.transform.position, this.<>f__this.GetTargetPos()) > this.<>f__this.info.bulletDisCollision)
            {
                this.<targetPos>__1 = this.<>f__this.GetTargetPos();
                if (TimeManager.GetInstance().IsShowTimeObj(this.<>f__this.casterID))
                {
                    this.<moveSpeed>__8 = BattleGlobal.ScaleSpeed_ShowTime(this.<>f__this.info.bulletMoveSpeed);
                }
                else
                {
                    this.<moveSpeed>__8 = BattleGlobal.ScaleSpeed(this.<>f__this.info.bulletMoveSpeed);
                }
                if (this.<>f__this.info.bulletHeight > 0f)
                {
                    this.<timePrecent>__9 = Time.deltaTime / this.<needTime>__6;
                    this.<curPrecent>__7 += this.<timePrecent>__9;
                    this.<curPrecent>__7 = Mathf.Min(this.<curPrecent>__7, 1f);
                    Vector3[] path = new Vector3[] { this.<firstPos>__0, this.<secondPos>__4, this.<targetPos>__1 };
                    this.<pos>__10 = iTween.PointOnPath(path, this.<curPrecent>__7);
                    Vector3[] vectorArray3 = new Vector3[] { this.<firstPos>__0, this.<secondPos>__4, this.<targetPos>__1 };
                    this.<posNext>__11 = iTween.PointOnPath(vectorArray3, ((this.<curPrecent>__7 + this.<timePrecent>__9) <= 0.99f) ? (this.<curPrecent>__7 + this.<timePrecent>__9) : 1f);
                    this.<>f__this.bullet.transform.position = this.<pos>__10;
                    this.<>f__this.bullet.transform.LookAt(this.<posNext>__11);
                    if (this.<curPrecent>__7 > 0.99f)
                    {
                        this.<>f__this.bullet.transform.position = this.<>f__this.GetTargetPos();
                    }
                }
                else
                {
                    this.<>f__this.bullet.transform.LookAt(this.<targetPos>__1);
                    this.<moveStep>__12 = Time.deltaTime * this.<moveSpeed>__8;
                    this.<>f__this.bullet.transform.position = Vector3.MoveTowards(this.<>f__this.bullet.transform.position, this.<targetPos>__1, this.<moveStep>__12);
                }
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            this.<>f__this.lastPos = new Vector3?(this.<>f__this.bullet.transform.position);
            if (this.<>f__this.info.bulletEndAttachType == SkillEffectAttachType.ONE_AttachTo)
            {
                this.<oldRot>__13 = this.<>f__this.bullet.transform.rotation;
                this.<oldPos>__14 = this.<>f__this.bullet.transform.position;
                this.<endObj>__15 = this.<>f__this.GetEndObj();
                if (this.<endObj>__15 != null)
                {
                    this.<endObj>__15.GetComponent<HangControler>().AttachToHangPoint(this.<>f__this.bullet, this.<>f__this.info.bulletEndHangPoint, Vector3.zero);
                }
                this.<>f__this.bullet.transform.position = this.<oldPos>__14;
                this.<>f__this.bullet.transform.rotation = this.<oldRot>__13;
            }
            ObjectManager.DestoryObj(this.<>f__this.bullet, BattleGlobal.ScaleTime(this.<>f__this.info.bulletDispearTime));
            this.$PC = -1;
        Label_06A1:
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

