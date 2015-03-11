using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_Base : MonoBehaviour
{
    public virtual void OnCreateInit()
    {
    }

    public void OnMsgCreateInit(BattleData _gameData)
    {
        this.battleGameData = _gameData;
        this.OnCreateInit();
    }

    public BattleData battleGameData { get; private set; }
}

