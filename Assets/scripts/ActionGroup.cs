using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class ActionGroup
{
    public float afterTime;
    public List<int> allTargetIDs;
    public int casterID;
    public List<RealTimeSkillActionBase> castingActions = new List<RealTimeSkillActionBase>();
    private bool isFinish;
    public List<RealTimeSkillActionBase> prepareActions = new List<RealTimeSkillActionBase>();
    public float time;

    public void ClearAll()
    {
        foreach (RealTimeSkillActionBase base2 in this.prepareActions)
        {
            base2.DoClear();
        }
        foreach (RealTimeSkillActionBase base3 in this.castingActions)
        {
            base3.DoClear();
        }
    }

    private int GetMainTargetID()
    {
        if ((this.allTargetIDs != null) && (this.allTargetIDs.Count > 0))
        {
            return this.allTargetIDs[0];
        }
        return -1;
    }

    public void Init()
    {
        this.isFinish = false;
    }

    public void InitAction(RealTimeSkillInfo info, RealTimeSkill skill, int _casterID)
    {
        this.casterID = _casterID;
        this.time = info.time;
        this.afterTime = info.afterTime;
        BattleFighter fighter = skill.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(this.casterID);
        bool flag = false;
        foreach (RealTimeSkillActionInfo info2 in info.actionInfoes)
        {
            if (info2.isCastBegin)
            {
                flag = true;
            }
            if ((info2.logicEffectInfoIndex >= 0) && !flag)
            {
                flag = true;
            }
            RealTimeSkillActionBase item = RealTimeSkillActionBase.CreateAction(skill, info2, skill.battleGameData, fighter, skill.casterID, this.allTargetIDs, skill.moveCollsionDis, info.oneByOne, info.isNeedTurn);
            if (flag)
            {
                this.castingActions.Add(item);
            }
            else
            {
                this.prepareActions.Add(item);
            }
        }
    }

    public bool IsFinish()
    {
        return this.isFinish;
    }

    [DebuggerHidden]
    public IEnumerator StartProcess(RealTimeSkill mb)
    {
        return new <StartProcess>c__Iterator29 { mb = mb, <$>mb = mb, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartTimeProcess(RealTimeSkill mb)
    {
        return new <StartTimeProcess>c__Iterator2A { mb = mb, <$>mb = mb, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <StartProcess>c__Iterator29 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkill <$>mb;
        internal List<RealTimeSkillActionBase>.Enumerator <$s_285>__0;
        internal List<int>.Enumerator <$s_286>__2;
        internal List<RealTimeSkillActionBase>.Enumerator <$s_287>__4;
        internal ActionGroup <>f__this;
        internal RealTimeSkillActionBase <action>__1;
        internal RealTimeSkillActionBase <action>__5;
        internal int <oneTargetID>__3;
        internal RealTimeSkill mb;

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
                        this.<$s_285>__0.Dispose();
                    }
                    break;

                case 2:
                    try
                    {
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_287>__4.Dispose();
                        }
                    }
                    finally
                    {
                        this.<$s_286>__2.Dispose();
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
                    if (this.<>f__this.time > 0f)
                    {
                        this.mb.StartCoroutine(this.<>f__this.StartTimeProcess(this.mb));
                    }
                    this.<$s_285>__0 = this.<>f__this.prepareActions.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_0128;

                default:
                    goto Label_0233;
            }
            try
            {
                while (this.<$s_285>__0.MoveNext())
                {
                    this.<action>__1 = this.<$s_285>__0.Current;
                    this.$current = this.mb.StartCoroutine(this.<action>__1.StartProcess(this.mb, this.<>f__this.GetMainTargetID(), this.mb.GetTarget(this.<>f__this.GetMainTargetID())));
                    this.$PC = 1;
                    flag = true;
                    goto Label_0235;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_285>__0.Dispose();
            }
            this.<$s_286>__2 = this.<>f__this.allTargetIDs.GetEnumerator();
            num = 0xfffffffd;
        Label_0128:
            try
            {
                switch (num)
                {
                    case 2:
                        goto Label_0163;
                }
                while (this.<$s_286>__2.MoveNext())
                {
                    this.<oneTargetID>__3 = this.<$s_286>__2.Current;
                    this.<$s_287>__4 = this.<>f__this.castingActions.GetEnumerator();
                    num = 0xfffffffd;
                Label_0163:
                    try
                    {
                        while (this.<$s_287>__4.MoveNext())
                        {
                            this.<action>__5 = this.<$s_287>__4.Current;
                            this.$current = this.mb.StartCoroutine(this.<action>__5.StartProcess(this.mb, this.<oneTargetID>__3, this.mb.GetTarget(this.<oneTargetID>__3)));
                            this.$PC = 2;
                            flag = true;
                            goto Label_0235;
                        }
                        continue;
                    }
                    finally
                    {
                        if (!flag)
                        {
                        }
                        this.<$s_287>__4.Dispose();
                    }
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_286>__2.Dispose();
            }
            this.<>f__this.isFinish = true;
            this.$PC = -1;
        Label_0233:
            return false;
        Label_0235:
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
    private sealed class <StartTimeProcess>c__Iterator2A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkill <$>mb;
        internal ActionGroup <>f__this;
        internal RealTimeSkill mb;

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
                    this.$current = this.mb.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this.<>f__this.time, new int?(this.<>f__this.casterID)));
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.isFinish = true;
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
}

