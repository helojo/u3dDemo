using System;
using System.Runtime.CompilerServices;

public class BattleCom_UIControl : BattleCom_Base
{
    public BattleUIControlImplBase impl;

    public void DoTalk(string talks)
    {
        this.impl.DoTalk(talks);
    }

    private void FixedUpdate()
    {
        if (base.battleGameData != null)
        {
            BattleCom_Runtime component = base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>();
            if ((component != null) && component.isRuning)
            {
                this.impl.Tick();
            }
        }
    }

    public bool IsLoadFinish()
    {
        return this.impl.IsLoadFinish();
    }

    public override void OnCreateInit()
    {
        if (base.battleGameData.gameType == BattleGameType.Normal)
        {
            this.impl = new BattleComUIControlNormalGame();
        }
        else if (base.battleGameData.gameType == BattleGameType.Grid)
        {
            this.impl = new BattleUIGridImpl();
        }
        this.impl.battleGameData = base.battleGameData;
        this.impl.Control = this;
        this.impl.Init();
    }

    public void OnPhaseEnding()
    {
        this.impl.OnPhaseEnding();
    }

    public bool isTalking { get; set; }
}

