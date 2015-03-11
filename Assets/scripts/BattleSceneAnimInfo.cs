using System;
using UnityEngine;

[Serializable]
public class BattleSceneAnimInfo
{
    public string animName;
    public BattleSceneAnimCameraActionType cameraAction;
    public AnimationClip cameraAnim;
    public float cameraShakeTime;
    public Vector3 cameraShakeValue;
    public float delayTime;
    public GameObject effect;
    public BattleSceneAnimEffectAttachType effectAttachType;
    public float effectLifeTime = 1f;
    public BattleSceneAnimFighterPlayAnimType fighterType;
    public HangPointType hangPoint;
    public BattleSceneAnimType type;

    public BattleSceneAnimInfo Clone()
    {
        return new BattleSceneAnimInfo { type = this.type, delayTime = this.delayTime, fighterType = this.fighterType, cameraAction = this.cameraAction, cameraAnim = this.cameraAnim, cameraShakeValue = this.cameraShakeValue, cameraShakeTime = this.cameraShakeTime, animName = this.animName, effect = this.effect, effectAttachType = this.effectAttachType, hangPoint = this.hangPoint, effectLifeTime = this.effectLifeTime };
    }
}

