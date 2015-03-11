using FastBuf;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_PhaseNormal : BattleCom_PhaseManagerImplBase
{
    private string curMoveSound = "sound_fight_shift_1";
    private static Vector3 towerOldPos = Vector3.zero;

    [DebuggerHidden]
    private IEnumerator BattleFinishing(bool isWin, bool isTimeOut)
    {
        return new <BattleFinishing>c__Iterator22 { isWin = isWin, isTimeOut = isTimeOut, <$>isWin = isWin, <$>isTimeOut = isTimeOut, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator BeforePhaseStarting()
    {
        return new <BeforePhaseStarting>c__Iterator1D { <>f__this = this };
    }

    public override void BeginBattleFinished(bool isWin, bool isTimeOut)
    {
        base.owner.StartCoroutine(this.BattleFinishing(isWin, isTimeOut));
    }

    private void BeginNextPhase()
    {
        base.owner.StartCoroutine(this.NextPhaseing());
    }

    public override void BeginPhaseFinishing()
    {
        base.owner.StartCoroutine(this.PhaseFinishing());
    }

    public override void BeginPhaseStarting()
    {
        base.owner.StartCoroutine(this.PhaseStarting());
    }

    public override void BeginShowBattleResult(S2C_DuplicateEndReq res)
    {
        base.owner.StartCoroutine(this.ShowBattleResult(res));
    }

    private bool HeroIsTeam(int _cardEntry)
    {
        <HeroIsTeam>c__AnonStorey152 storey = new <HeroIsTeam>c__AnonStorey152 {
            _cardEntry = _cardEntry
        };
        return ((storey._cardEntry == -1) || (base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetPlayerFighters().Find(new Predicate<BattleFighter>(storey.<>m__114)) != null));
    }

    private bool IsJumpMode()
    {
        return (!BattleCom_PhaseManager.G_moveEnable || (base.battleGameData.normalGameType == BattleNormalGameType.TowerPk));
    }

    [DebuggerHidden]
    private IEnumerator NextPhaseing()
    {
        return new <NextPhaseing>c__Iterator21 { <>f__this = this };
    }

    private void OnTowerBeforePhaseStarting()
    {
        GameObject target = GameObject.Find("RootGroup/tower");
        if (target != null)
        {
            towerOldPos = target.transform.position;
            iTween.MoveTo(target, towerOldPos + new Vector3(0f, -25f, 0f), 2f);
        }
    }

    private void OnTowerPhaseStarting()
    {
        GameObject target = GameObject.Find("RootGroup/tower");
        if (target != null)
        {
            iTween.Stop(target);
            target.transform.position = towerOldPos;
        }
    }

    [DebuggerHidden]
    private IEnumerator PhaseFinishing()
    {
        return new <PhaseFinishing>c__Iterator20 { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator PhaseStarting()
    {
        return new <PhaseStarting>c__Iterator1E { <>f__this = this };
    }

    private void playSceneSound()
    {
        if (base.battleGameData.CurBattlePhase == 0)
        {
            string str = null;
            if (base.battleGameData.IsDupBattle())
            {
                if (base.battleGameData.normalGameType == BattleNormalGameType.Dup)
                {
                    trench_normal_config _config = ConfigMgr.getInstance().getByEntry<trench_normal_config>(base.battleGameData.dupID);
                    if (_config != null)
                    {
                        str = _config.scene_sound;
                    }
                }
                else if (base.battleGameData.normalGameType == BattleNormalGameType.DupElite)
                {
                    trench_elite_config _config2 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(base.battleGameData.dupID);
                    if (_config2 != null)
                    {
                        str = _config2.scene_sound;
                    }
                }
            }
            else if (base.battleGameData.normalGameType == BattleNormalGameType.WorldBoss)
            {
                str = "Battle_Music01";
            }
            else if (((base.battleGameData.normalGameType == BattleNormalGameType.FriendPK) || (base.battleGameData.normalGameType == BattleNormalGameType.WorldCupPk)) || (base.battleGameData.normalGameType == BattleNormalGameType.WarmmatchPk))
            {
                str = "Battle_Music02";
            }
            else if (base.battleGameData.normalGameType == BattleNormalGameType.YuanZhengPk)
            {
                str = "Battle_Music01";
            }
            else if (base.battleGameData.normalGameType == BattleNormalGameType.ArenaLadder)
            {
                str = "Battle_Music01";
            }
            else if (base.battleGameData.normalGameType == BattleNormalGameType.GuildBattle)
            {
                guild_source_point_config _config3 = ConfigMgr.getInstance().getByEntry<guild_source_point_config>(ActorData.getInstance().m_resourceEntry);
                if (_config3 != null)
                {
                    str = _config3.scene_sound;
                }
            }
            if (GuideSimulate_Battle.sim_mode)
            {
                str = "Battle_Music01";
            }
            if (!string.IsNullOrEmpty(str))
            {
                SoundManager.mInstance.PlayMusic(str);
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator PlayStory()
    {
        return new <PlayStory>c__Iterator1F { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator ShowBattleResult(S2C_DuplicateEndReq res)
    {
        return new <ShowBattleResult>c__Iterator23 { res = res, <$>res = res, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <BattleFinishing>c__Iterator22 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool <$>isTimeOut;
        internal bool <$>isWin;
        internal List<BattleFighter>.Enumerator <$s_272>__3;
        internal List<BattleFighter>.Enumerator <$s_273>__6;
        private static Action<BattleFighter, int> <>f__am$cache10;
        internal BattleCom_PhaseNormal <>f__this;
        internal BattlePanel <bp>__1;
        internal BattlePausePanel <bpp>__0;
        internal BattleFighter <fighter>__4;
        internal BattleFighter <fighter>__7;
        internal BattleCom_FighterManager <fighterManager>__2;
        internal float <moveMaxTime>__5;
        internal BattleNormalGameResult <result>__8;
        internal bool isTimeOut;
        internal bool isWin;

        private static void <>m__119(BattleFighter arg1, int arg2)
        {
            arg1.RestoreHPEnergy();
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_273>__6.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    this.<bpp>__0 = GUIMgr.Instance.GetGUIEntity<BattlePausePanel>();
                    if (this.<bpp>__0 != null)
                    {
                        GUIMgr.Instance.ExitModelGUI("BattlePausePanel");
                    }
                    this.<bp>__1 = GUIMgr.Instance.GetGUIEntity<BattlePanel>();
                    if (this.<bp>__1 != null)
                    {
                        this.<bp>__1.FightIsOver = true;
                    }
                    this.<fighterManager>__2 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                    this.<$s_272>__3 = this.<fighterManager>__2.GetAllFighters().GetEnumerator();
                    try
                    {
                        while (this.<$s_272>__3.MoveNext())
                        {
                            this.<fighter>__4 = this.<$s_272>__3.Current;
                            if (!this.<fighter>__4.IsDead() && (this.<fighter>__4.GetAnimObj() != null))
                            {
                                this.<fighter>__4.GetAnimObj().GetComponent<AnimFSM>().StopCurAnimForce();
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_272>__3.Dispose();
                    }
                    this.<moveMaxTime>__5 = 5f;
                    this.<$s_273>__6 = this.<fighterManager>__2.GetAllFighters().GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                case 2:
                    if (<>f__am$cache10 == null)
                    {
                        <>f__am$cache10 = new Action<BattleFighter, int>(BattleCom_PhaseNormal.<BattleFinishing>c__Iterator22.<>m__119);
                    }
                    this.<fighterManager>__2.DoToPlayerAllFighter(<>f__am$cache10);
                    this.$current = new WaitForSeconds(0.4f);
                    this.$PC = 3;
                    goto Label_0383;

                case 3:
                    goto Label_0287;

                case 4:
                    SoundManager.mInstance.StopMusic(1f);
                    if (this.isWin)
                    {
                        SoundManager.mInstance.PlaySFX("battle_win");
                    }
                    else
                    {
                        SoundManager.mInstance.PlaySFX("battle_failed");
                    }
                    if (this.<>f__this.battleGameData.callBackFunc != null)
                    {
                        this.<result>__8 = new BattleNormalGameResult();
                        this.<result>__8.ComputeResult(this.<>f__this.battleGameData);
                        this.<result>__8.isTimeOut = this.isTimeOut;
                        this.<>f__this.battleGameData.callBackFunc(this.isWin, this.<>f__this.battleGameData.normalGameType, this.<result>__8);
                    }
                    this.$PC = -1;
                    goto Label_0381;

                default:
                    goto Label_0381;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        goto Label_01B6;
                }
                while (this.<$s_273>__6.MoveNext())
                {
                    this.<fighter>__7 = this.<$s_273>__6.Current;
                    if (!this.<fighter>__7.IsDead())
                    {
                        continue;
                    }
                Label_01B6:
                    while (!this.<fighter>__7.isDeadFinish && (this.<moveMaxTime>__5 > 0f))
                    {
                        this.<moveMaxTime>__5 -= Time.deltaTime;
                        this.$current = null;
                        this.$PC = 1;
                        flag = true;
                        goto Label_0383;
                    }
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_273>__6.Dispose();
            }
            ObjectManager.HideTempObj();
            if (this.isWin && (this.<>f__this.battleGameData.normalGameType == BattleNormalGameType.YuanZhengPk))
            {
                this.$current = new WaitForSeconds(0.2f);
                this.$PC = 2;
                goto Label_0383;
            }
        Label_0287:
            this.$current = this.<>f__this.owner.StartCoroutine(this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_PlayerControl>().OnPhaseStarting());
            this.$PC = 4;
            goto Label_0383;
        Label_0381:
            return false;
        Label_0383:
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
    private sealed class <BeforePhaseStarting>c__Iterator1D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<BattleFighter>.Enumerator <$s_266>__5;
        internal BattleCom_PhaseNormal <>f__this;
        internal BattleFighter <fighter>__6;
        internal BattleCom_FighterManager <fighterManager>__0;
        internal List<BattleFighter> <fighters>__1;
        internal Vector3 <moveVec>__4;
        internal BattleCom_ScenePosManager <scenePos>__2;
        internal List<Vector3> <startPath>__3;

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
                    if (!this.<>f__this.IsJumpMode() || (this.<>f__this.battleGameData.CurBattlePhase == 0))
                    {
                        goto Label_021F;
                    }
                    if (this.<>f__this.battleGameData.normalGameType == BattleNormalGameType.TowerPk)
                    {
                        this.<>f__this.OnTowerBeforePhaseStarting();
                    }
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().EndAttachToPlayer();
                    this.<fighterManager>__0 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                    this.<fighters>__1 = this.<fighterManager>__0.GetPlayerFighters();
                    this.<scenePos>__2 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>();
                    this.<startPath>__3 = this.<scenePos>__2.GetScenePathByPhase();
                    if (this.<startPath>__3 == null)
                    {
                        break;
                    }
                    this.<moveVec>__4 = this.<startPath>__3[this.<startPath>__3.Count - 1] - this.<startPath>__3[this.<startPath>__3.Count - 2];
                    this.<moveVec>__4.Normalize();
                    this.<moveVec>__4 = (Vector3) (this.<moveVec>__4 * 10f);
                    this.<$s_266>__5 = this.<fighters>__1.GetEnumerator();
                    try
                    {
                        while (this.<$s_266>__5.MoveNext())
                        {
                            this.<fighter>__6 = this.<$s_266>__5.Current;
                            if (!this.<fighter>__6.IsDead())
                            {
                                this.<fighter>__6.moveControler.StartMoveToPos(this.<fighter>__6.moveControler.GetPosition() + this.<moveVec>__4, 0f);
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_266>__5.Dispose();
                    }
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 1;
                    goto Label_0221;

                case 1:
                    TransSceneUI.BeginTransition();
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_021F;

                default:
                    goto Label_021F;
            }
            this.$current = new WaitForSeconds(1f);
            this.$PC = 2;
            goto Label_0221;
        Label_021F:
            return false;
        Label_0221:
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
    private sealed class <HeroIsTeam>c__AnonStorey152
    {
        internal int _cardEntry;

        internal bool <>m__114(BattleFighter e)
        {
            return (e.CardEntry == this._cardEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <NextPhaseing>c__Iterator21 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        private static Action<BattleFighter, int> <>f__am$cache4;
        internal BattleCom_PhaseNormal <>f__this;
        internal BattleCom_FighterManager <fightManager>__0;

        private static void <>m__118(BattleFighter arg1, int arg2)
        {
            if (arg1 != null)
            {
                arg1.ClearBufferOnPhase();
            }
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
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<fightManager>__0 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                    this.<fightManager>__0.ClearMonsterFighter();
                    if (<>f__am$cache4 == null)
                    {
                        <>f__am$cache4 = new Action<BattleFighter, int>(BattleCom_PhaseNormal.<NextPhaseing>c__Iterator21.<>m__118);
                    }
                    this.<fightManager>__0.DoToPlayerAllFighter(<>f__am$cache4);
                    this.<>f__this.battleGameData.CurBattlePhase++;
                    this.<>f__this.battleGameData.OnMsgPhaseChange();
                    if (this.<>f__this.battleGameData.isAuto)
                    {
                        this.<>f__this.BeginPhaseStarting();
                    }
                    else
                    {
                        this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_UIControl>().OnPhaseEnding();
                    }
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

    [CompilerGenerated]
    private sealed class <PhaseFinishing>c__Iterator20 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<BattleFighter>.Enumerator <$s_270>__1;
        internal List<BattleFighter>.Enumerator <$s_271>__4;
        internal BattleCom_PhaseNormal <>f__this;
        internal BattleFighter <fighter>__2;
        internal BattleFighter <fighter>__5;
        internal BattleCom_FighterManager <fighterManager>__0;
        internal float <moveMaxTime>__3;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_271>__4.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    Utility.NewbiestUnlock();
                    this.<fighterManager>__0 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                    this.<$s_270>__1 = this.<fighterManager>__0.GetAllFighters().GetEnumerator();
                    try
                    {
                        while (this.<$s_270>__1.MoveNext())
                        {
                            this.<fighter>__2 = this.<$s_270>__1.Current;
                            if (!this.<fighter>__2.IsDead() && (this.<fighter>__2.GetAnimObj() != null))
                            {
                                this.<fighter>__2.GetAnimObj().GetComponent<AnimFSM>().StopCurAnimForce();
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_270>__1.Dispose();
                    }
                    this.<moveMaxTime>__3 = 2f;
                    this.<$s_271>__4 = this.<fighterManager>__0.GetAllFighters().GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                default:
                    goto Label_01B8;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        goto Label_0152;
                }
                while (this.<$s_271>__4.MoveNext())
                {
                    this.<fighter>__5 = this.<$s_271>__4.Current;
                    if (!this.<fighter>__5.IsDead())
                    {
                        continue;
                    }
                Label_0152:
                    while (!this.<fighter>__5.isDeadFinish && (this.<moveMaxTime>__3 > 0f))
                    {
                        this.<moveMaxTime>__3 -= Time.deltaTime;
                        this.$current = null;
                        this.$PC = 1;
                        flag = true;
                        return true;
                    }
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_271>__4.Dispose();
            }
            ObjectManager.HideTempObj();
            this.<>f__this.BeginNextPhase();
            goto Label_01B8;
            this.$PC = -1;
        Label_01B8:
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
    private sealed class <PhaseStarting>c__Iterator1E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<BattleFighter>.Enumerator <$s_267>__5;
        internal List<BattleFighter>.Enumerator <$s_268>__9;
        internal List<BattleFighter>.Enumerator <$s_269>__11;
        private static Action<BattleFighter, int> <>f__am$cache10;
        internal BattleCom_PhaseNormal <>f__this;
        internal BattleFighter <fighter>__10;
        internal BattleFighter <fighter>__12;
        internal BattleFighter <fighter>__6;
        internal BattleCom_FighterManager <fighterManager>__0;
        internal List<BattleFighter> <fighters>__3;
        internal float <moveMaxTime>__7;
        internal float <moveTime>__8;
        internal float <overTime>__1;
        internal BattleCom_ScenePosManager <scenePos>__4;
        internal float <time>__2;

        private static void <>m__115(BattleFighter arg1, int arg2)
        {
            arg1.RestoreHPEnergy();
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 5:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_268>__9.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    BattleSecurityManager.Instance.AddNewBattlePhase(this.<>f__this.battleGameData.CurBattlePhase, BattleGameType.Normal, this.<>f__this.battleGameData.normalGameType);
                    this.<fighterManager>__0 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                    BattleSecurityManager.Instance.RegisterPhaseStartTime();
                    this.<>f__this.playSceneSound();
                    if (this.<>f__this.battleGameData.CurBattlePhase != 0)
                    {
                        if (<>f__am$cache10 == null)
                        {
                            <>f__am$cache10 = new Action<BattleFighter, int>(BattleCom_PhaseNormal.<PhaseStarting>c__Iterator1E.<>m__115);
                        }
                        this.<fighterManager>__0.DoToPlayerAllFighter(<>f__am$cache10);
                    }
                    this.$current = this.<>f__this.owner.StartCoroutine(this.<>f__this.BeforePhaseStarting());
                    this.$PC = 1;
                    goto Label_0644;

                case 1:
                    this.$current = this.<>f__this.owner.StartCoroutine(this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_PlayerControl>().OnPhaseStarting());
                    this.$PC = 2;
                    goto Label_0644;

                case 2:
                    this.<fighterManager>__0.ClearDeadFighter();
                    this.<overTime>__1 = 10f;
                    break;

                case 3:
                    this.<overTime>__1 -= 0.1f;
                    break;

                case 4:
                    goto Label_02E8;

                case 5:
                    goto Label_03FD;

                case 6:
                    if (this.<>f__this.battleGameData.OnMsgPhaseStartFinish != null)
                    {
                        this.<>f__this.battleGameData.OnMsgPhaseStartFinish();
                    }
                    this.$PC = -1;
                    goto Label_0642;

                default:
                    goto Label_0642;
            }
            if (!LoadingPerfab.IsTransFinish() && (this.<overTime>__1 > 0f))
            {
                this.$current = new WaitForSeconds(0.1f);
                this.$PC = 3;
                goto Label_0644;
            }
            if (this.<>f__this.IsJumpMode())
            {
                this.<fighterManager>__0.ResetPlayerPos();
                if (this.<>f__this.battleGameData.normalGameType == BattleNormalGameType.TowerPk)
                {
                    this.<>f__this.OnTowerPhaseStarting();
                }
            }
            TransSceneUI.EndTransition();
            LoadingPerfab.EndTransition();
            this.<fighterManager>__0.InitMonstersFormBattleDate();
            this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().BeginAttachToPlayerAndTeamDir(true);
            if (this.<>f__this.IsJumpMode() || (this.<>f__this.battleGameData.CurBattlePhase == 0))
            {
                this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().GetCurCamera().UpdateCameraNow();
            }
            this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().GetCurCamera().UseLookAtInitProject = false;
            if ((this.<>f__this.battleGameData.CurBattlePhase == 0) && !string.IsNullOrEmpty(this.<>f__this.battleGameData.startAnim))
            {
                this.<time>__2 = BattleSceneAnim.DoStartAnim(this.<>f__this.battleGameData.startAnim, this.<>f__this.battleGameData, this.<>f__this.owner);
                this.$current = new WaitForSeconds(this.<time>__2);
                this.$PC = 4;
                goto Label_0644;
            }
        Label_02E8:
            this.<fighters>__3 = this.<fighterManager>__0.GetPlayerFighters();
            this.<scenePos>__4 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>();
            this.<$s_267>__5 = this.<fighters>__3.GetEnumerator();
            try
            {
                while (this.<$s_267>__5.MoveNext())
                {
                    this.<fighter>__6 = this.<$s_267>__5.Current;
                    this.<fighter>__6.moveControler.SetSpeedScale(BattleGlobal.DefaultMoveSpeedOfOutBattleScale);
                    this.<fighter>__6.moveControler.StartMoveToPos(this.<scenePos>__4.GetSceneFighterEndPosByPhase(this.<fighter>__6.GetIndexAtLive()), 0f);
                    this.<fighter>__6.GetHang().AttachEffect("EffectPrefabs/yan_tx", "moveEffect", HangPointType.Feet);
                }
            }
            finally
            {
                this.<$s_267>__5.Dispose();
            }
            SoundManager.mInstance.PlaySFX(this.<>f__this.curMoveSound);
            this.<moveMaxTime>__7 = 8f;
            this.<moveTime>__8 = 0f;
            this.<$s_268>__9 = this.<fighters>__3.GetEnumerator();
            num = 0xfffffffd;
        Label_03FD:
            try
            {
                switch (num)
                {
                    case 5:
                        goto Label_047B;
                }
                while (this.<$s_268>__9.MoveNext())
                {
                    this.<fighter>__10 = this.<$s_268>__9.Current;
                Label_047B:
                    while (this.<fighter>__10.moveControler.IsMoveing())
                    {
                        if (this.<>f__this.battleGameData.timeScale_ShowTime > BattleGlobal.PauseTimeScale)
                        {
                            this.<moveTime>__8 += Time.deltaTime;
                        }
                        if (this.<moveTime>__8 > this.<moveMaxTime>__7)
                        {
                            break;
                        }
                        this.$current = null;
                        this.$PC = 5;
                        flag = true;
                        goto Label_0644;
                    }
                    this.<fighter>__10.moveControler.SetSpeedScale(1f);
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_268>__9.Dispose();
            }
            this.<$s_269>__11 = this.<fighters>__3.GetEnumerator();
            try
            {
                while (this.<$s_269>__11.MoveNext())
                {
                    this.<fighter>__12 = this.<$s_269>__11.Current;
                    this.<fighter>__12.GetHang().DetachEffect("moveEffect", HangPointType.Feet, 0.7f);
                    if (this.<moveTime>__8 > this.<moveMaxTime>__7)
                    {
                        this.<fighter>__12.moveControler.SetSpeedScale(1f);
                        this.<fighter>__12.moveControler.SetPosForce(this.<scenePos>__4.GetSceneFighterEndPosByPhase(this.<fighter>__12.GetIndexAtLive()));
                    }
                }
            }
            finally
            {
                this.<$s_269>__11.Dispose();
            }
            this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().GetCurCamera().UseLookAtInitProject = true;
            if (!this.<>f__this.IsJumpMode() && (this.<>f__this.battleGameData.CurBattlePhase != 0))
            {
                this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().BeginAttachToPlayerAndTeamDir(true);
            }
            this.$current = this.<>f__this.owner.StartCoroutine(this.<>f__this.PlayStory());
            this.$PC = 6;
            goto Label_0644;
        Label_0642:
            return false;
        Label_0644:
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
    private sealed class <PlayStory>c__Iterator1F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleCom_PhaseNormal <>f__this;
        internal BattlePanel <bp>__4;
        internal int <dialogId>__3;
        internal int <dupId>__1;
        internal int <isPlayDiagCount>__2;
        internal bool <playDiagIsOk>__0;
        internal trench_elite_config <tec>__6;
        internal trench_normal_config <tnc>__5;

        internal void <>m__116(GUIEntity obj)
        {
            ((DupStoryDiag) obj).UpdateData(this.<dialogId>__3, go => this.<playDiagIsOk>__0 = true);
            if (this.<bp>__4 != null)
            {
                this.<bp>__4.SetPosGroupStat(false);
            }
        }

        internal void <>m__117(GameObject go)
        {
            this.<playDiagIsOk>__0 = true;
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
                    if ((this.<>f__this.battleGameData.normalGameType != BattleNormalGameType.Dup) && (this.<>f__this.battleGameData.normalGameType != BattleNormalGameType.DupElite))
                    {
                        goto Label_0341;
                    }
                    this.<playDiagIsOk>__0 = false;
                    this.<dupId>__1 = this.<>f__this.battleGameData.dupID;
                    this.<isPlayDiagCount>__2 = (this.<>f__this.battleGameData.normalGameType != BattleNormalGameType.Dup) ? PlayerPrefs.GetInt(ActorData.getInstance().SessionInfo + "DupElite" + this.<dupId>__1, 0) : PlayerPrefs.GetInt(ActorData.getInstance().SessionInfo + "DupEntry" + this.<dupId>__1, 0);
                    this.<dialogId>__3 = -1;
                    this.<bp>__4 = GUIMgr.Instance.GetGUIEntity<BattlePanel>();
                    if (this.<>f__this.battleGameData.normalGameType != BattleNormalGameType.Dup)
                    {
                        if (this.<>f__this.battleGameData.normalGameType == BattleNormalGameType.DupElite)
                        {
                            this.<tec>__6 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(this.<dupId>__1);
                            if (((this.<tec>__6 != null) && this.<>f__this.HeroIsTeam(this.<tec>__6.hero_id)) && ((this.<isPlayDiagCount>__2 == 0) && (this.<tec>__6.node_id == this.<>f__this.battleGameData.CurBattlePhase)))
                            {
                                PlayerPrefs.SetInt(ActorData.getInstance().SessionInfo + "DupElite" + this.<dupId>__1, ++this.<isPlayDiagCount>__2);
                                this.<dialogId>__3 = this.<tec>__6.dialog_id;
                            }
                        }
                        break;
                    }
                    this.<tnc>__5 = ConfigMgr.getInstance().getByEntry<trench_normal_config>(this.<dupId>__1);
                    if (((this.<tnc>__5 != null) && this.<>f__this.HeroIsTeam(this.<tnc>__5.hero_id)) && ((this.<isPlayDiagCount>__2 == 0) && (this.<tnc>__5.node_id == this.<>f__this.battleGameData.CurBattlePhase)))
                    {
                        PlayerPrefs.SetInt(ActorData.getInstance().SessionInfo + "DupEntry" + this.<dupId>__1, ++this.<isPlayDiagCount>__2);
                        this.<dialogId>__3 = this.<tnc>__5.dialog_id;
                    }
                    break;

                case 1:
                    goto Label_02FB;

                default:
                    goto Label_0348;
            }
            if (this.<dialogId>__3 == -1)
            {
                goto Label_0341;
            }
            SoundManager.mInstance.StopMusic(1f);
            if (!this.<>f__this.battleGameData.IsBossBattle)
            {
                this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().EndAttachAndToStoryPoint();
            }
            GUIMgr.Instance.DoModelGUI("DupStoryDiag", new Action<GUIEntity>(this.<>m__116), null);
        Label_02FB:
            while (!this.<playDiagIsOk>__0)
            {
                this.$current = new WaitForSeconds(0.2f);
                this.$PC = 1;
                return true;
            }
            SoundManager.mInstance.ResumeMusic(1f);
            GUIMgr.Instance.ExitModelGUI("DupStoryDiag");
            if (this.<bp>__4 != null)
            {
                this.<bp>__4.SetPosGroupStat(true);
            }
        Label_0341:
            this.$PC = -1;
        Label_0348:
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
    private sealed class <ShowBattleResult>c__Iterator23 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal S2C_DuplicateEndReq <$>res;
        internal BattleCom_PhaseNormal <>f__this;
        internal BattleCom_FighterManager <fighterManager>__2;
        internal bool <hasLevelUp>__0;
        internal BattleCom_ScenePosManager <scenePos>__1;
        internal S2C_DuplicateEndReq res;

        internal void <>m__11A(BattleFighter arg1, int arg2)
        {
            <ShowBattleResult>c__AnonStorey153 storey = new <ShowBattleResult>c__AnonStorey153 {
                <>f__ref$35 = this,
                arg1 = arg1
            };
            if (!storey.arg1.IsDead())
            {
                Card card = this.res.cards.Find(new Predicate<Card>(storey.<>m__11B));
                if (card != null)
                {
                    Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) storey.arg1.CardEntry);
                    if ((cardByEntry != null) && (card.cardInfo.level > cardByEntry.cardInfo.level))
                    {
                        if (!this.<>f__this.battleGameData.IsBossBattle)
                        {
                            Vector3 looktarget = storey.arg1.gameObject.transform.position - Vector3.Cross(this.<scenePos>__1.GetSceneFighterDirByPhase(), Vector3.up);
                            iTween.LookTo(storey.arg1.gameObject, looktarget, 0.3f);
                        }
                        storey.arg1.GetHang().AttachEffect("EffectPrefabs/levelup", "test", HangPointType.Feet);
                        storey.arg1.GetAnimObj().GetComponent<AnimFSM>().PlayAnim(BattleGlobal.RestAnimName, 1f, 0f, false);
                        this.<hasLevelUp>__0 = true;
                    }
                }
            }
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
                    if (!this.res.is_pass)
                    {
                        break;
                    }
                    this.<hasLevelUp>__0 = false;
                    this.<scenePos>__1 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>();
                    this.<fighterManager>__2 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                    this.<fighterManager>__2.DoToPlayerAllFighter(new Action<BattleFighter, int>(this.<>m__11A));
                    if (!this.<hasLevelUp>__0)
                    {
                        break;
                    }
                    SoundManager.mInstance.PlaySFX("sound_fight_horelvup_1");
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 1;
                    return true;

                case 1:
                    break;

                default:
                    goto Label_00CE;
            }
            BattleStaticEntry.OnExitDupBattle(this.res);
            this.$PC = -1;
        Label_00CE:
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

        private sealed class <ShowBattleResult>c__AnonStorey153
        {
            internal BattleCom_PhaseNormal.<ShowBattleResult>c__Iterator23 <>f__ref$35;
            internal BattleFighter arg1;

            internal bool <>m__11B(Card obj)
            {
                return (obj.cardInfo.entry == this.arg1.CardEntry);
            }
        }
    }
}

