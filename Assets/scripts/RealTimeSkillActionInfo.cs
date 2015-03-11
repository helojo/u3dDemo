using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RealTimeSkillActionInfo
{
    public SkillActionType actionType;
    public float afterTime;
    public SkillAnimType anim;
    public string animName;
    public float bulletDisCollision = 0.1f;
    public float bulletDispearTime;
    public GameObject bulleteffect;
    public SkillActionPlayerType bulletEnd = SkillActionPlayerType.target;
    public SkillEffectAttachType bulletEndAttachType;
    public HangPointType bulletEndHangPoint;
    public Vector3 bulletEndOffset;
    public float bulletHeight;
    public float bulletHeightPrecent = 0.5f;
    public float bulletMoveSpeed = 10f;
    public float bulletMoveTime = 0.5f;
    public SkillActionPlayerType bulletStart;
    public HangPointType bulletStartHangPoint;
    public float delayTime;
    public float delayTime1;
    public float delayTime2;
    public float delayTime3;
    public List<SkillEffectInfo> effectInfos = new List<SkillEffectInfo>();
    public bool hideModel;
    public bool isCastBegin;
    public bool isCastEnd;
    public bool isNeedStopAnim;
    public float logicDelayTime = 0.1f;
    public int logicEffectInfoIndex = -1;
    public float modelScale = 1f;
    public float modelScaleDurTime = 1f;
    public float moveAcceleration = -1f;
    public string moveAnim;
    public float moveDisCollision = 1f;
    public bool moveIsJump;
    public bool moveIsJumpRandom;
    public bool moveIsNeedMoveBack;
    public Vector3 moveJumpDir = new Vector3(0f, 0f, -1f);
    public float movePlayAnimSpeed = 1f;
    public float moveSpeed = 5f;
    public bool OneToOne;
    public bool playAnimLoop;
    public float playAnimSpeed = 1f;
    public float playAnimSpeedResetTime = -1f;
    public float playAnimStartTime;
    public SkillActionPlayerType playerType;
    public float sceneEffectDurTime = 1f;
    public float sceneEffectSmoothEndTime = 1f;
    public float sceneEffectSmoothTime = 1f;
    public BattleSceneEffectState sceneEffectState;
    public float shakeTime;
    public float shakeTime2;
    public float shakeTime3;
    public Vector3 shakeValue = Vector3.zero;
    public Vector3 shakeValue2 = Vector3.zero;
    public Vector3 shakeValue3 = Vector3.zero;
    public float soundDelayTime = -1f;
    public string soundName;
}

