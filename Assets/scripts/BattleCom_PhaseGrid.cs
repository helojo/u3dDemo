using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_PhaseGrid : BattleCom_PhaseManagerImplBase
{
    [DebuggerHidden]
    private IEnumerator BattleFinished(bool isWin)
    {
        return new <BattleFinished>c__Iterator12 { isWin = isWin, <$>isWin = isWin, <>f__this = this };
    }

    public override void BeginBattleFinished(bool isWin, bool isTimeOut)
    {
        base.owner.StartCoroutine(this.BattleFinished(isWin));
    }

    public override void BeginPhaseFinishing()
    {
    }

    public override void BeginPhaseStarting()
    {
        base.owner.StartCoroutine(this.PhaseStarting());
    }

    public override void BeginShowBattleResult(S2C_DuplicateEndReq res)
    {
    }

    [DebuggerHidden]
    private IEnumerator PhaseStarting()
    {
        return new <PhaseStarting>c__Iterator11 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <BattleFinished>c__Iterator12 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool <$>isWin;
        private static Action<BattleFighter, int> <>f__am$cache6;
        internal BattleCom_PhaseGrid <>f__this;
        internal BattleNormalGameResult <result>__0;
        internal bool isWin;

        private static void <>m__61(BattleFighter arg1, int arg2)
        {
            arg1.RestoreHPEnergy();
        }

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
                    BattleSecurityManager.Instance.RegisterPhaseEndTime();
                    if (!this.isWin)
                    {
                        break;
                    }
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    goto Label_016E;

                case 1:
                    if (<>f__am$cache6 == null)
                    {
                        <>f__am$cache6 = new Action<BattleFighter, int>(BattleCom_PhaseGrid.<BattleFinished>c__Iterator12.<>m__61);
                    }
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().DoToPlayerAllFighter(<>f__am$cache6);
                    this.$current = new WaitForSeconds(0.4f);
                    this.$PC = 2;
                    goto Label_016E;

                case 2:
                    break;

                case 3:
                    ObjectManager.HideTempObj();
                    this.<result>__0 = new BattleNormalGameResult();
                    this.<result>__0.ComputeResult(this.<>f__this.battleGameData);
                    if (this.<>f__this.battleGameData.OnMsgGridGameFinishOneBattle != null)
                    {
                        this.<>f__this.battleGameData.OnMsgGridGameFinishOneBattle(this.isWin, false, this.<result>__0);
                    }
                    this.$PC = -1;
                    goto Label_016C;

                default:
                    goto Label_016C;
            }
            SoundManager.mInstance.StopMusic(1f);
            if (this.isWin)
            {
                SoundManager.mInstance.PlaySFX("battle_win");
            }
            else
            {
                SoundManager.mInstance.PlaySFX("battle_failed");
            }
            this.$current = new WaitForSeconds(1f);
            this.$PC = 3;
            goto Label_016E;
        Label_016C:
            return false;
        Label_016E:
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
    private sealed class <PhaseStarting>c__Iterator11 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleCom_PhaseGrid <>f__this;
        internal BattleCom_FighterManager <fighterManager>__2;
        internal GameObject <girdCamera>__0;
        internal BattleUIGridImpl <uiControl>__1;

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
                    TransSceneUI.BeginTransition();
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    goto Label_0249;

                case 1:
                    this.<girdCamera>__0 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleGridGameMapControl>().cameraObj;
                    if (this.<girdCamera>__0 != null)
                    {
                        this.<girdCamera>__0.SetActive(false);
                    }
                    this.<uiControl>__1 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_UIControl>().impl as BattleUIGridImpl;
                    this.$current = this.<>f__this.owner.StartCoroutine(this.<uiControl>__1.StartCreateBattleGUI());
                    this.$PC = 2;
                    goto Label_0249;

                case 2:
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().InitFightersFormBattleDate();
                    BattleSecurityManager.Instance.AddNewBattlePhase(this.<>f__this.battleGameData.CurBattlePhase, BattleGameType.Grid, BattleNormalGameType.OutLandPk);
                    this.<fighterManager>__2 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                    this.<fighterManager>__2.ResetPlayerPos();
                    this.<fighterManager>__2.InitMonstersFormBattleDate();
                    this.<>f__this.battleGameData.OnMsgStart();
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().SetEnable(true);
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().GetCurCamera().UseLookAtInitProject = true;
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().BeginAttachToPlayerAndTeamDir(true);
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().GetCurCamera().UpdateCameraNow();
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 3;
                    goto Label_0249;

                case 3:
                    TransSceneUI.EndTransition();
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 4;
                    goto Label_0249;

                case 4:
                    BattleSecurityManager.Instance.RegisterPhaseStartTime();
                    if (this.<>f__this.battleGameData.OnMsgPhaseStartFinish != null)
                    {
                        this.<>f__this.battleGameData.OnMsgPhaseStartFinish();
                    }
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0249:
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
}

