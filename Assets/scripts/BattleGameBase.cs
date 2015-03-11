using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleGameBase : MonoBehaviour
{
    public virtual void Init()
    {
        this.battleGameData = new BattleData();
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnLeave()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public BattleData battleGameData { get; set; }
}

