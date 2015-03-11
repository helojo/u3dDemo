using Battle;
using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeBuffer
{
    [CompilerGenerated]
    private static Action<GameObject> <>f__am$cache7;
    public List<GameObject> attachedEffects;
    private RealTimeBufferInfo bufferData;
    public int bufferEntry;
    private StateBuffType bufferType;
    private BattleFighter casterFighter;
    private buff_config config;
    private BattleFighter ownerFighter;

    private void BreakAttackAway()
    {
        iTween.StopByName(this.ownerFighter.gameObject, "AttackAway");
        this.ownerFighter.moveControler.SetCollsionEnable(true);
    }

    public void Init(int _bufferEntry, int _casterID, BattleFighter _ownerFighter, BattleFighter _casterFighter)
    {
        this.bufferEntry = _bufferEntry;
        this.ownerFighter = _ownerFighter;
        this.casterFighter = _casterFighter;
        this.config = ConfigMgr.getInstance().getByEntry<buff_config>(_bufferEntry);
        this.bufferType = (StateBuffType) this.config.type;
        if ((this.config != null) && !string.IsNullOrEmpty(this.config.effect))
        {
            this.bufferData = RealSkillInfoManager.GetBufferData(this.config.effect);
        }
    }

    public void InitTest(string name, BattleFighter _ownerFighter)
    {
        this.ownerFighter = _ownerFighter;
        this.bufferData = RealSkillInfoManager.GetBufferData(name);
    }

    public bool isCanPastToNextPhase()
    {
        return (this.config.succeed_type == 1);
    }

    public bool IsNeedPauseAnim()
    {
        return (this.bufferType == StateBuffType.Freez);
    }

    public void OnAttach()
    {
        if (this.bufferType == StateBuffType.Cloaking)
        {
            MaterialFSM component = this.ownerFighter.GetAnimObj().GetComponent<MaterialFSM>();
            if (component != null)
            {
                component.StartAlphaChange(0.2f, 0.4f);
            }
            this.ownerFighter.moveControler.SetCollsionEnable(false);
        }
        else if (this.bufferType == StateBuffType.GoAway)
        {
            MaterialFSM lfsm2 = this.ownerFighter.GetAnimObj().GetComponent<MaterialFSM>();
            if (lfsm2 != null)
            {
                lfsm2.StartAlphaChange(0.2f, 0.2f);
            }
        }
        else if (this.bufferType == StateBuffType.Avatar)
        {
            this.ownerFighter.ChangeToAvatar();
        }
        else if (this.bufferType == StateBuffType.AttackAway)
        {
            this.StartAttackAway(this.config.effectValue1, this.config.effectValue2);
        }
        if (this.bufferData != null)
        {
            this.OnAttachEffect();
        }
        this.ownerFighter.OnStateBuffChange();
    }

    private void OnAttachEffect()
    {
        if (!string.IsNullOrEmpty(this.bufferData.avatarModelName))
        {
            this.ownerFighter.modelControler.ShowModel(this.bufferData.avatarModelName, true);
        }
        if (!string.IsNullOrEmpty(this.bufferData.sound))
        {
            SoundManager.mInstance.PlaySFX(this.bufferData.sound);
        }
        this.attachedEffects = new List<GameObject>();
        this.bufferData.effectInfos.ForEach(delegate (SkillEffectInfo data) {
            GameObject item = this.ownerFighter.modelControler.AttachByPrefab(data.effectPrefab, data.hangPoint, data.delayTime, data.offset, this.isCanPastToNextPhase());
            this.attachedEffects.Add(item);
        });
        if (this.bufferData.modelScale != 1f)
        {
            this.ownerFighter.AddScale(this.bufferData.modelScale);
        }
    }

    public void OnRemove()
    {
        if (this.bufferType == StateBuffType.Cloaking)
        {
            MaterialFSM component = this.ownerFighter.GetAnimObj().GetComponent<MaterialFSM>();
            if (component != null)
            {
                component.ResetMaterialSource();
            }
            this.ownerFighter.moveControler.SetCollsionEnable(true);
        }
        else if (this.bufferType == StateBuffType.GoAway)
        {
            MaterialFSM lfsm2 = this.ownerFighter.GetAnimObj().GetComponent<MaterialFSM>();
            if (lfsm2 != null)
            {
                lfsm2.ResetMaterialSource();
            }
        }
        else if (this.bufferType == StateBuffType.AttackAway)
        {
            this.ownerFighter.moveControler.SetCollsionEnable(true);
        }
        if (this.bufferData != null)
        {
            this.OnRemoveEffect();
        }
        this.ownerFighter.OnStateBuffChange();
    }

    private void OnRemoveEffect()
    {
        if (!string.IsNullOrEmpty(this.bufferData.avatarModelName))
        {
            this.ownerFighter.modelControler.DeleteModel(this.bufferData.avatarModelName);
        }
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = delegate (GameObject obj) {
                if (obj != null)
                {
                    ObjectManager.DestoryObj(obj);
                }
            };
        }
        this.attachedEffects.ForEach(<>f__am$cache7);
        this.attachedEffects.Clear();
        if (this.bufferData.modelScale != 1f)
        {
            this.ownerFighter.AddScale(1f / this.bufferData.modelScale);
        }
    }

    public void SetEffectActive(string cmpName, bool active)
    {
        foreach (GameObject obj2 in this.attachedEffects)
        {
            if (obj2 != null)
            {
                foreach (Transform transform in obj2.transform.GetComponentsInChildren<Transform>())
                {
                    if ((transform != null) && transform.name.Equals(cmpName))
                    {
                        transform.gameObject.SetActive(active);
                    }
                }
            }
        }
    }

    private void StartAttackAway(float dis, float height)
    {
        if (!this.ownerFighter.IsDead())
        {
            this.ownerFighter.moveControler.SetCollsionEnable(false);
            Vector3 position = this.ownerFighter.moveControler.GetPosition();
            Vector3 destPos = position;
            if (this.casterFighter != null)
            {
                this.casterFighter.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetActorHeadDir(this.casterFighter.PosIndex).Normalize();
                Vector3 animObjDir = this.casterFighter.GetAnimObjDir();
                Vector3 sceneFighterDirByPhase = this.casterFighter.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
                animObjDir = Vector3.Project(animObjDir, sceneFighterDirByPhase);
                animObjDir.Normalize();
                destPos += (Vector3) (animObjDir * dis);
                destPos = this.ownerFighter.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().ChangePosByBlock(position, destPos);
            }
            Vector3 vector6 = (Vector3) ((position + destPos) / 2f);
            vector6.y += height;
            List<Vector3> list = new List<Vector3> {
                position,
                vector6,
                destPos
            };
            object[] args = new object[] { "name", "AttackAway", "path", list.ToArray(), "easetype", iTween.EaseType.linear, "oncomplete", "OnAttackAwayFinish", "oncompletetarget", this.ownerFighter.gameObject, "speed", 10 };
            iTween.MoveTo(this.ownerFighter.gameObject, iTween.Hash(args));
        }
    }
}

