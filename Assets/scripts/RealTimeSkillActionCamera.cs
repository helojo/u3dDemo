using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillActionCamera : RealTimeSkillActionBase
{
    [DebuggerHidden]
    protected override IEnumerator OnProcess(MonoBehaviour mb)
    {
        return new <OnProcess>c__Iterator3A { mb = mb, <$>mb = mb, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartCameraProcess(MonoBehaviour mb, BattleData battleGameData, float _delayTime, Vector3 _shakeValue, float _shakeTime)
    {
        return new <StartCameraProcess>c__Iterator3B { mb = mb, _delayTime = _delayTime, battleGameData = battleGameData, _shakeValue = _shakeValue, _shakeTime = _shakeTime, <$>mb = mb, <$>_delayTime = _delayTime, <$>battleGameData = battleGameData, <$>_shakeValue = _shakeValue, <$>_shakeTime = _shakeTime, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <OnProcess>c__Iterator3A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MonoBehaviour <$>mb;
        internal RealTimeSkillActionCamera <>f__this;
        internal MonoBehaviour mb;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            this.$PC = -1;
            if (this.$PC == 0)
            {
                if (this.<>f__this.info.shakeTime > 0f)
                {
                    this.mb.StartCoroutine(this.<>f__this.StartCameraProcess(this.mb, this.<>f__this.battleGameData, this.<>f__this.info.delayTime1, this.<>f__this.info.shakeValue, this.<>f__this.info.shakeTime));
                }
                if (this.<>f__this.info.shakeTime2 > 0f)
                {
                    this.mb.StartCoroutine(this.<>f__this.StartCameraProcess(this.mb, this.<>f__this.battleGameData, this.<>f__this.info.delayTime2, this.<>f__this.info.shakeValue2, this.<>f__this.info.shakeTime2));
                }
                if (this.<>f__this.info.shakeTime3 > 0f)
                {
                    this.mb.StartCoroutine(this.<>f__this.StartCameraProcess(this.mb, this.<>f__this.battleGameData, this.<>f__this.info.delayTime3, this.<>f__this.info.shakeValue3, this.<>f__this.info.shakeTime3));
                }
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

    [CompilerGenerated]
    private sealed class <StartCameraProcess>c__Iterator3B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float _delayTime;
        internal float _shakeTime;
        internal Vector3 _shakeValue;
        internal float <$>_delayTime;
        internal float <$>_shakeTime;
        internal Vector3 <$>_shakeValue;
        internal BattleData <$>battleGameData;
        internal MonoBehaviour <$>mb;
        internal RealTimeSkillActionCamera <>f__this;
        internal BattleData battleGameData;
        internal MonoBehaviour mb;

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
                    this.$current = this.mb.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this._delayTime, new int?(this.<>f__this.casterID)));
                    this.$PC = 1;
                    return true;

                case 1:
                    if (this.battleGameData == null)
                    {
                        if (Camera.main != null)
                        {
                            iTween.ShakePosition(Camera.main.gameObject, this._shakeValue, this._shakeTime);
                        }
                        break;
                    }
                    this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().ShakeCamera(this._shakeValue, this._shakeTime);
                    break;

                default:
                    goto Label_00C1;
            }
            this.$PC = -1;
        Label_00C1:
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

