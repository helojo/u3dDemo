using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleSceneAnim : MonoBehaviour
{
    [CompilerGenerated]
    private static Predicate<BattleFighter> <>f__am$cache0;

    [DebuggerHidden]
    private static IEnumerator DoSceneAnim(BattleSceneAnimInfo info, string name, BattleData _battleGameData)
    {
        return new <DoSceneAnim>c__Iterator43 { info = info, _battleGameData = _battleGameData, name = name, <$>info = info, <$>_battleGameData = _battleGameData, <$>name = name };
    }

    public static float DoStartAnim(string name, BattleData _battleGameData, MonoBehaviour _control)
    {
        BattleSceneAnimData data = BattleSceneAnimDataManager.GetInstance().GetData(name);
        if (data == null)
        {
            return 0f;
        }
        foreach (BattleSceneAnimInfo info in data.infoes)
        {
            _control.StartCoroutine(DoSceneAnim(info, name, _battleGameData));
        }
        return data.time;
    }

    private static BattleFighter GetFighter(BattleData data, BattleSceneAnimFighterPlayAnimType type)
    {
        if (type != BattleSceneAnimFighterPlayAnimType.Boss)
        {
            return null;
        }
        List<BattleFighter> monsterFighters = data.battleComObject.GetComponent<BattleCom_FighterManager>().GetMonsterFighters();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = obj => obj.IsBigBoss;
        }
        BattleFighter fighter = monsterFighters.Find(<>f__am$cache0);
        if ((fighter == null) && (monsterFighters.Count > 0))
        {
            fighter = monsterFighters[0];
        }
        return fighter;
    }

    [CompilerGenerated]
    private sealed class <DoSceneAnim>c__Iterator43 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleData _battleGameData;
        internal BattleData <$>_battleGameData;
        internal BattleSceneAnimInfo <$>info;
        internal string <$>name;
        internal BattleCom_CameraManager <cameraControl>__0;
        internal BattleCom_CameraManager <cameraControl>__1;
        internal BattleFighter <fighter>__2;
        internal BattleFighter <fighter>__3;
        internal HangControler <hang>__4;
        internal BattleSceneAnimInfo info;
        internal string name;

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
                    this.$current = new WaitForSeconds(this.info.delayTime);
                    this.$PC = 1;
                    goto Label_0261;

                case 1:
                    if (this.info.type != BattleSceneAnimType.cameraAnim)
                    {
                        if (this.info.type == BattleSceneAnimType.FighterPlayAnim)
                        {
                            this.<fighter>__2 = BattleSceneAnim.GetFighter(this._battleGameData, this.info.fighterType);
                            if (this.<fighter>__2 != null)
                            {
                                this.<fighter>__2.GetAnimObj().GetComponent<AnimFSM>().PlayAnim(this.info.animName, 1f, 0f, false);
                            }
                        }
                        else if (this.info.type == BattleSceneAnimType.Effect)
                        {
                            if (this.info.effectAttachType == BattleSceneAnimEffectAttachType.Fighter)
                            {
                                this.<fighter>__3 = BattleSceneAnim.GetFighter(this._battleGameData, this.info.fighterType);
                                if (this.<fighter>__3 != null)
                                {
                                    this.<hang>__4 = this.<fighter>__3.GetAnimObj().GetComponent<HangControler>();
                                    if (this.<hang>__4 != null)
                                    {
                                        this.<hang>__4.AttachByPrefab(this.info.effect, this.info.hangPoint, this.info.effectLifeTime, 0f, Vector3.zero, true, false);
                                    }
                                }
                            }
                            else if (this.info.effectAttachType == BattleSceneAnimEffectAttachType.Screen)
                            {
                                BattleGlobalFunc.AttachEffectToScreen(this.info.effect, 0f, this.info.effectLifeTime);
                            }
                        }
                        break;
                    }
                    if (this.info.cameraAction != BattleSceneAnimCameraActionType.Anim)
                    {
                        if (this.info.cameraAction == BattleSceneAnimCameraActionType.Shark)
                        {
                            this.<cameraControl>__1 = this._battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>();
                            this.<cameraControl>__1.ShakeCamera(this.info.cameraShakeValue, this.info.cameraShakeTime);
                        }
                        break;
                    }
                    this.<cameraControl>__0 = this._battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>();
                    this.<cameraControl>__0.GetCurCamera().AddAnim(this.info.cameraAnim, this.name);
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_025F;

                default:
                    goto Label_025F;
            }
            this.$current = null;
            this.$PC = 2;
            goto Label_0261;
        Label_025F:
            return false;
        Label_0261:
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

