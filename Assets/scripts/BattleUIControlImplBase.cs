using System;
using System.Runtime.CompilerServices;

public class BattleUIControlImplBase
{
    public virtual void DoTalk(string talks)
    {
    }

    public virtual void Init()
    {
    }

    public virtual bool IsLoadFinish()
    {
        return true;
    }

    public virtual void OnPhaseEnding()
    {
    }

    public void OnTalkFinish()
    {
        this.Control.isTalking = false;
    }

    public virtual bool QuestCastSkill(int fighterIndex)
    {
        return false;
    }

    public virtual void QuestGotoNextPhase()
    {
    }

    public virtual void Tick()
    {
    }

    public BattleData battleGameData { get; set; }

    public BattleCom_UIControl Control { get; set; }
}

