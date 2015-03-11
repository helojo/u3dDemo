using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeShowEffect : MonoBehaviour
{
    private List<RealTimeSkillActionBase> actions;
    private BattleFighter actor;
    private int actorID;
    private GameObject casterModel;
    private GameObject targetModel;

    public void Clear()
    {
        base.StopAllCoroutines();
    }

    public static RealTimeShowEffect CreateNewShowEffect(string effectName, int _actorID, BattleData battleGameData)
    {
        RealTimeShowEffect effect = new GameObject { name = "ShowEffect " + effectName }.AddComponent<RealTimeShowEffect>();
        effect.Init(effectName, _actorID, battleGameData);
        return effect;
    }

    public static RealTimeShowEffect CreateNewShowEffect(string effectName, GameObject _casterModel, GameObject _targetModel)
    {
        RealTimeShowEffect effect = new GameObject { name = "ShowEffect " + effectName }.AddComponent<RealTimeShowEffect>();
        effect.Init(effectName, _casterModel, _targetModel);
        return effect;
    }

    [DebuggerHidden]
    private IEnumerator DoStart()
    {
        return new <DoStart>c__Iterator28 { <>f__this = this };
    }

    public void Init(string effectName, int _actorID, BattleData battleGameData)
    {
        RealTimeSkillInfo skillData = RealSkillInfoManager.GetSkillData(effectName);
        this.actorID = _actorID;
        if (skillData != null)
        {
            this.actor = battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(this.actorID);
            this.actions = new List<RealTimeSkillActionBase>();
            foreach (RealTimeSkillActionInfo info2 in skillData.actionInfoes)
            {
                this.actions.Add(RealTimeSkillActionBase.CreateAction(null, info2, battleGameData, this.actor, this.actorID, null, 0f, skillData.oneByOne, skillData.isNeedTurn));
            }
        }
    }

    public void Init(string effectName, GameObject _casterModel, GameObject _targetModel)
    {
        RealTimeSkillInfo skillData = RealSkillInfoManager.GetSkillData(effectName);
        this.casterModel = _casterModel;
        this.targetModel = _targetModel;
        if (skillData != null)
        {
            this.actions = new List<RealTimeSkillActionBase>();
            foreach (RealTimeSkillActionInfo info2 in skillData.actionInfoes)
            {
                this.actions.Add(RealTimeSkillActionBase.CreateAction(null, info2, null, null, -1, null, 0f, skillData.oneByOne, skillData.isNeedTurn));
            }
        }
    }

    public void ToDoStart()
    {
        base.StartCoroutine(this.DoStart());
    }

    [CompilerGenerated]
    private sealed class <DoStart>c__Iterator28 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<RealTimeSkillActionBase>.Enumerator <$s_283>__0;
        internal RealTimeShowEffect <>f__this;
        internal RealTimeSkillActionBase <action>__1;

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
                        this.<$s_283>__0.Dispose();
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
                    if (this.<>f__this.actions == null)
                    {
                        goto Label_0115;
                    }
                    this.<$s_283>__0 = this.<>f__this.actions.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                default:
                    goto Label_011C;
            }
            try
            {
                while (this.<$s_283>__0.MoveNext())
                {
                    this.<action>__1 = this.<$s_283>__0.Current;
                    if (this.<action>__1 != null)
                    {
                        this.<action>__1.casterModel = this.<>f__this.casterModel;
                        this.<action>__1.targetModel = this.<>f__this.targetModel;
                        this.$current = this.<>f__this.StartCoroutine(this.<action>__1.StartProcess(this.<>f__this, this.<>f__this.actorID, this.<>f__this.actor));
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
                this.<$s_283>__0.Dispose();
            }
        Label_0115:
            this.$PC = -1;
        Label_011C:
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

