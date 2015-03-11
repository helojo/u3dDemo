using System;
using UnityEngine;

public class BattleGameShower : MonoBehaviour
{
    private BattleData battleGameData = new BattleData();

    public void Init()
    {
        GameObject gameObject = base.gameObject;
        this.battleGameData.battleComObject = gameObject;
        gameObject.AddComponent<BattleCom_ScenePosManager>();
        gameObject.AddComponent<BattleCom_FighterManager>();
        gameObject.AddComponent<BattleCom_Runtime>();
        gameObject.SendMessage("OnMsgCreateInit", this.battleGameData);
    }

    private void Update()
    {
    }
}

