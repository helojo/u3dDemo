using FastBuf;
using System;

public class BattleNormalGameResult
{
    public BattleGameResultActorInfoes actorInfoes = new BattleGameResultActorInfoes();
    public int deadNumbers;
    public bool isTimeOut;
    public long worldBossHpChange;

    public void ComputeResult(BattleData _battleGameData)
    {
        this.worldBossHpChange = _battleGameData.worldBossInitHP - _battleGameData.worldBossFinishedHP;
        this.worldBossHpChange += _battleGameData.worldBossOverHurtHP;
        int num = 0;
        foreach (CombatDetailActor actor in _battleGameData.attActor)
        {
            if ((actor != null) && (actor.entry != -1))
            {
                num++;
            }
        }
        int num2 = 0;
        BattleCom_FighterManager component = _battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        for (int i = 0; i < BattleGlobal.FighterNumberOneSide; i++)
        {
            BattleFighter fighter = component.GetFighter(i);
            BattleGameResultActorInfo item = new BattleGameResultActorInfo();
            if (fighter != null)
            {
                item.cardEntry = fighter.CardEntry;
                item.hp = (long) fighter.HP;
                item.maxHp = (long) fighter.MaxHP;
                item.energy = (int) fighter.Energy;
                if (!fighter.IsDead())
                {
                    num2++;
                }
            }
            else
            {
                item.hp = 0L;
                item.energy = 0;
                item.cardEntry = -1;
            }
            this.actorInfoes.attackers.Add(item);
        }
        for (int j = 0; j < BattleGlobal.FighterNumberOneSide; j++)
        {
            BattleFighter fighter2 = component.GetFighter(j + BattleGlobal.FighterNumberMax);
            BattleGameResultActorInfo info2 = new BattleGameResultActorInfo();
            if (fighter2 != null)
            {
                info2.cardEntry = fighter2.CardEntry;
                info2.hp = (long) fighter2.HP;
                info2.maxHp = (long) fighter2.MaxHP;
                info2.energy = (int) fighter2.Energy;
            }
            else
            {
                info2.hp = 0L;
                info2.energy = 0;
                info2.cardEntry = -1;
            }
            this.actorInfoes.defenders.Add(info2);
        }
        this.deadNumbers = num - num2;
    }
}

