using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class BattleCom_PhaseManagerImplBase
{
    private GameObject blockObj;
    private bool isBlocking;

    protected BattleCom_PhaseManagerImplBase()
    {
    }

    public abstract void BeginBattleFinished(bool isWin, bool isTimeOut);
    public abstract void BeginPhaseFinishing();
    public abstract void BeginPhaseStarting();
    public abstract void BeginShowBattleResult(S2C_DuplicateEndReq res);
    public void BlockScene()
    {
        this.isBlocking = true;
        if (!this.battleGameData.IsBossBattle)
        {
            BattleFighter attackHeadFighter = this.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetAttackHeadFighter();
            BattleFighter defenderHeadFighter = this.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetDefenderHeadFighter();
            if ((attackHeadFighter != null) && (defenderHeadFighter != null))
            {
                this.owner.StartCoroutine(this.StartBlockScene((Vector3) ((attackHeadFighter.transform.position + defenderHeadFighter.transform.position) / 2f)));
            }
        }
    }

    public Vector3 ChangePosByBlock(Vector3 srcPos, Vector3 destPos)
    {
        if ((this.blockObj != null) && this.isBlocking)
        {
            Vector3 position = this.blockObj.transform.position;
            Vector3 sceneFighterDirByPhase = this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
            Vector3 vector = destPos - position;
            float magnitude = Vector3.Project(vector, sceneFighterDirByPhase).magnitude;
            float num2 = 7.5f;
            if (magnitude > num2)
            {
                Vector3 vector4 = destPos - srcPos;
                Vector3 vector6 = srcPos - position;
                magnitude = num2 - vector6.magnitude;
                destPos = srcPos + ((Vector3) (vector4.normalized * magnitude));
            }
        }
        return destPos;
    }

    public bool IsOverPos(Vector3 pos)
    {
        if (this.blockObj != null)
        {
            Vector3 position = this.blockObj.transform.position;
            Vector3 sceneFighterDirByPhase = this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
            Vector3 vector = pos - position;
            float magnitude = Vector3.Project(vector, sceneFighterDirByPhase).magnitude;
            float num2 = 5.5f;
            if (magnitude > num2)
            {
                return true;
            }
        }
        return false;
    }

    [DebuggerHidden]
    private IEnumerator StartBlockScene(Vector3 pos)
    {
        return new <StartBlockScene>c__Iterator6 { pos = pos, <$>pos = pos, <>f__this = this };
    }

    public void UnblockScene()
    {
        this.isBlocking = false;
        this.owner.StopCoroutine("StartBlockScene");
        if (this.blockObj != null)
        {
            this.blockObj.SetActive(false);
        }
    }

    public BattleData battleGameData { get; set; }

    public BattleCom_PhaseManager owner { get; set; }

    [CompilerGenerated]
    private sealed class <StartBlockScene>c__Iterator6 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Vector3 <$>pos;
        internal BattleCom_PhaseManagerImplBase <>f__this;
        internal Vector3 <dir>__0;
        internal Vector3 pos;

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
                    if (this.<>f__this.isBlocking)
                    {
                        if (this.<>f__this.blockObj == null)
                        {
                            this.<>f__this.blockObj = ObjectManager.CreateObj("BattlePrefabs/BlockObj");
                        }
                        this.<>f__this.blockObj.transform.position = this.pos;
                        this.<dir>__0 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
                        this.<>f__this.blockObj.transform.rotation = Quaternion.LookRotation(this.<dir>__0);
                        this.<>f__this.blockObj.SetActive(false);
                        this.$current = this.<>f__this.owner.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(2f, null));
                        this.$PC = 1;
                        return true;
                    }
                    break;

                case 1:
                    if (this.<>f__this.isBlocking)
                    {
                        this.<>f__this.blockObj.SetActive(true);
                        this.$PC = -1;
                        break;
                    }
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

