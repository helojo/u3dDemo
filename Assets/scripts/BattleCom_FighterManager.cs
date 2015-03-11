using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleCom_FighterManager : BattleCom_Base
{
    public bool IsMonsterMoveing;
    public List<Vector3> teamPos = new List<Vector3>();

    public BattleCom_FighterManager()
    {
        this.fighters = new List<BattleFighter>(new BattleFighter[BattleGlobal.FighterNumberMax * 2]);
    }

    private void AddFighter(CombatDetailActor actor, int posIndex, int serverIndex)
    {
        if ((actor != null) && (actor.entry != -1))
        {
            if (actor.isCard)
            {
                this.createFighter(actor.entry, posIndex, 1f, actor, serverIndex, false);
            }
            else
            {
                this.createMonsterFighter(actor.entry, posIndex, actor, serverIndex);
            }
        }
    }

    public void ClearDeadFighter()
    {
        for (int i = 0; i < this.fighters.Count; i++)
        {
            if ((this.fighters[i] != null) && this.fighters[i].IsDead())
            {
                this.DestoryFighter(this.fighters[i]);
                this.fighters[i] = null;
            }
        }
    }

    public void ClearFighters()
    {
        for (int i = 0; i < this.fighters.Count; i++)
        {
            if (this.fighters[i] != null)
            {
                this.DestoryFighter(this.fighters[i]);
                this.fighters[i] = null;
            }
        }
    }

    public void ClearMonsterFighter()
    {
        for (int i = 0; i < this.fighters.Count; i++)
        {
            if ((this.fighters[i] != null) && !this.fighters[i].isPlayer)
            {
                this.DestoryFighter(this.fighters[i], 5f);
                this.fighters[i] = null;
            }
        }
    }

    public GameObject createFighter(int entry, int pos, float scale, CombatDetailActor detailActor, int serverIdx, bool isBigBoss = false)
    {
        if (entry < 0)
        {
            return null;
        }
        bool flag = pos < BattleGlobal.FighterNumberOneSide;
        GameObject obj2 = new GameObject {
            name = "fighter " + pos.ToString()
        };
        BattleFighter fighter = obj2.AddComponent<BattleFighter>();
        fighter.PosIndex = pos;
        fighter.Init(base.battleGameData, scale, flag, entry, detailActor);
        fighter.IsBigBoss = isBigBoss;
        fighter.ServerIdx = serverIdx;
        this.fighters[pos] = fighter;
        this.UpdateFighterWorldPosAndRot(pos);
        return obj2;
    }

    public GameObject createMonsterFighter(int entry, int pos, CombatDetailActor detailActor, int serverIdx)
    {
        monster_config _config = ConfigMgr.getInstance().getByEntry<monster_config>(entry);
        if (_config == null)
        {
            Debug.LogWarning("Can't find monster config ID: " + entry.ToString());
            _config = ConfigMgr.getInstance().getByEntry<monster_config>(0);
        }
        if (_config != null)
        {
            return this.createFighter(_config.card_entry, pos, _config.zoom, detailActor, serverIdx, _config.type == 2);
        }
        return null;
    }

    private void DestoryFighter(BattleFighter obj)
    {
        obj.OnDestory();
        UnityEngine.Object.Destroy(obj.gameObject);
    }

    public void DestoryFighter(int index)
    {
        this.DestoryFighter(this.fighters[index]);
        this.fighters[index] = null;
    }

    private void DestoryFighter(BattleFighter obj, float time)
    {
        obj.OnDestory();
        UnityEngine.Object.Destroy(obj.gameObject, time);
    }

    public void DoToAllFighter(Action<BattleFighter, int> func)
    {
        for (int i = 0; i < (BattleGlobal.FighterNumberMax * 2); i++)
        {
            if (this.fighters[i] != null)
            {
                func(this.fighters[i], i);
            }
        }
    }

    public void DoToMonsterAllFighter(Action<BattleFighter, int> func)
    {
        for (int i = BattleGlobal.FighterNumberMax; i < (BattleGlobal.FighterNumberOneSide + BattleGlobal.FighterNumberMax); i++)
        {
            if (this.fighters[i] != null)
            {
                func(this.fighters[i], i);
            }
        }
    }

    public void DoToPlayerAllFighter(Action<BattleFighter, int> func)
    {
        for (int i = 0; i < BattleGlobal.FighterNumberOneSide; i++)
        {
            if (this.fighters[i] != null)
            {
                func(this.fighters[i], i);
            }
        }
    }

    public List<BattleFighter> GetAllFighters()
    {
        List<BattleFighter> list = new List<BattleFighter>();
        for (int i = 0; i < (BattleGlobal.FighterNumberMax * 2); i++)
        {
            if (this.fighters[i] != null)
            {
                list.Add(this.fighters[i]);
            }
        }
        return list;
    }

    public BattleFighter GetFighter(int index)
    {
        return (((index >= 0) && (index < this.fighters.Count)) ? this.fighters[index] : null);
    }

    public int GetFighterGameObjIndex(GameObject obj)
    {
        <GetFighterGameObjIndex>c__AnonStoreyDF ydf = new <GetFighterGameObjIndex>c__AnonStoreyDF {
            obj = obj
        };
        return this.fighters.FindIndex(new Predicate<BattleFighter>(ydf.<>m__41));
    }

    public int GetIndexAtLive(BattleFighter fighter)
    {
        int num = 0;
        int fighterNumberMax = BattleGlobal.FighterNumberMax;
        if (fighter.PosIndex >= BattleGlobal.FighterNumberMax)
        {
            num = BattleGlobal.FighterNumberMax;
            fighterNumberMax = BattleGlobal.FighterNumberMax * 2;
        }
        int num3 = 0;
        for (int i = num; i < fighterNumberMax; i++)
        {
            BattleFighter fighter2 = this.GetFighter(i);
            if (fighter2 == fighter)
            {
                return num3;
            }
            if ((fighter2 != null) && !fighter2.IsDead())
            {
                num3++;
            }
        }
        return num3;
    }

    public List<BattleFighter> GetMonsterFighters()
    {
        List<BattleFighter> list = new List<BattleFighter>();
        for (int i = BattleGlobal.FighterNumberMax; i < (BattleGlobal.FighterNumberMax * 2); i++)
        {
            if (this.fighters[i] != null)
            {
                list.Add(this.fighters[i]);
            }
        }
        return list;
    }

    public List<BattleFighter> GetPlayerFighters()
    {
        List<BattleFighter> list = new List<BattleFighter>();
        for (int i = 0; i < BattleGlobal.FighterNumberMax; i++)
        {
            if (this.fighters[i] != null)
            {
                list.Add(this.fighters[i]);
            }
        }
        return list;
    }

    public void InitFightersFormBattleDate()
    {
        if (base.battleGameData.attActor != null)
        {
            int posIndex = 0;
            foreach (CombatDetailActor actor in base.battleGameData.attActor)
            {
                this.AddFighter(actor, posIndex, posIndex);
                posIndex++;
            }
            this.OnFighterInitFinish();
        }
    }

    public void InitMonstersFormBattleDate()
    {
        if (base.battleGameData.defActor != null)
        {
            this.ClearMonsterFighter();
            if (base.battleGameData.CurBattlePhase < base.battleGameData.defActor.Count)
            {
                List<MonsterOneInfo> list = new List<MonsterOneInfo>();
                for (int i = 0; i < BattleGlobal.FighterNumberOneSide; i++)
                {
                    if (i < base.battleGameData.defActor[base.battleGameData.CurBattlePhase].actor.Count)
                    {
                        CombatDetailActor actor = base.battleGameData.defActor[base.battleGameData.CurBattlePhase].actor[i];
                        if ((actor != null) && (actor.entry >= 0))
                        {
                            MonsterOneInfo item = new MonsterOneInfo {
                                actorInfo = actor
                            };
                            if (base.battleGameData.drops != null)
                            {
                                int num2 = (base.battleGameData.CurBattlePhase * BattleGlobal.FighterNumberOneSide) + i;
                                if (num2 < base.battleGameData.drops.Count)
                                {
                                    item.dropInfo = base.battleGameData.drops[num2];
                                }
                                else
                                {
                                    item.dropInfo = null;
                                }
                            }
                            else
                            {
                                item.dropInfo = null;
                            }
                            item.index = i;
                            list.Add(item);
                        }
                    }
                }
                if (!base.battleGameData.IsBossBattle)
                {
                    list.Sort(new Comparison<MonsterOneInfo>(BattleCom_FighterManager.SortByPosition));
                }
                for (int j = 0; j < list.Count; j++)
                {
                    MonsterOneInfo info2 = list[j];
                    this.AddFighter(info2.actorInfo, j + BattleGlobal.FighterNumberMax, info2.index + BattleGlobal.FighterNumberMax);
                    BattleFighter fighter = this.GetFighter(j + BattleGlobal.FighterNumberMax);
                    if (fighter != null)
                    {
                        fighter.dropInfo = info2.dropInfo;
                    }
                }
            }
            if (base.battleGameData.OnMsgFighterChange != null)
            {
                base.battleGameData.OnMsgFighterChange();
            }
        }
    }

    public void InitPlayerTeam()
    {
        this.teamPos.Clear();
        GameObject parent = ObjectManager.CreateObj(!base.battleGameData.IsBossBattle ? BattleGlobal.DefaultTeamPosName : BattleGlobal.BossTeamPosName);
        for (int i = 0; i < (BattleGlobal.FighterNumberOneSide * 2); i++)
        {
            GameObject obj3 = BattleGlobalFunc.DeepFindChildObjectByName(parent, "pos_" + i.ToString());
            if (obj3 == null)
            {
                break;
            }
            this.teamPos.Add(obj3.transform.localPosition);
        }
        ObjectManager.DestoryObj(parent);
    }

    public override void OnCreateInit()
    {
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgEnter = (System.Action) Delegate.Combine(battleGameData.OnMsgEnter, new System.Action(this.OnMsgEnter));
        BattleData data2 = base.battleGameData;
        data2.OnMsgLeave = (System.Action) Delegate.Combine(data2.OnMsgLeave, new System.Action(this.OnMsgLeave));
        BattleData data3 = base.battleGameData;
        data3.OnMsgTimeScaleChange = (System.Action) Delegate.Combine(data3.OnMsgTimeScaleChange, new System.Action(this.OnMsgTimeScaleChange));
        BattleData data4 = base.battleGameData;
        data4.OnMsgGridGameFinishOneBattle = (Action<bool, bool, BattleNormalGameResult>) Delegate.Combine(data4.OnMsgGridGameFinishOneBattle, new Action<bool, bool, BattleNormalGameResult>(this.OnMsgGridGameFinishOneBattle));
        BattleData data5 = base.battleGameData;
        data5.OnMsgPhaseChange = (System.Action) Delegate.Combine(data5.OnMsgPhaseChange, new System.Action(this.OnMsgPhaseChange));
    }

    public void OnFighterInitFinish()
    {
    }

    private void OnMsgEnter()
    {
        if (!BattleSceneStarter.G_isTestEnable)
        {
            this.InitFightersFormBattleDate();
        }
    }

    private void OnMsgGridGameFinishOneBattle(bool isWin, bool isBreak, BattleNormalGameResult result)
    {
        this.ClearFighters();
    }

    private void OnMsgLeave()
    {
        this.ClearFighters();
    }

    private void OnMsgPhaseChange()
    {
        this.DoToAllFighter(delegate (BattleFighter arg1, int arg2) {
            if (arg1.IsDead())
            {
                this.DestoryFighter(arg2);
            }
        });
    }

    private void OnMsgTimeScaleChange()
    {
        this.SendMessageToAllFighter("OnMsgTimeScaleChange", null);
    }

    private void PlaceFighter(BattleFighter fighter, int posIndex, bool isPlayer)
    {
        Vector3 zero = Vector3.zero;
        Quaternion identity = Quaternion.identity;
        if (isPlayer)
        {
            zero = base.gameObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterStartPosByPhase(fighter.GetIndexAtLive());
            identity = Quaternion.LookRotation(base.gameObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase());
        }
        else
        {
            int monsterIndex = posIndex - BattleGlobal.FighterNumberMax;
            if ((monsterIndex >= 0) && (monsterIndex < BattleGlobal.FighterNumberOneSide))
            {
                if (fighter.IsBigBoss)
                {
                    if ((monsterIndex >= 0) && (monsterIndex < BattleGlobal.FighterNumberOneSide))
                    {
                        zero = base.gameObject.GetComponent<BattleCom_ScenePosManager>().GetMonsterInitPos(monsterIndex);
                        identity = base.gameObject.GetComponent<BattleCom_ScenePosManager>().GetMonsterInitRot(monsterIndex);
                    }
                }
                else
                {
                    zero = base.gameObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterEndPosByPhase(monsterIndex + BattleGlobal.FighterNumberOneSide);
                    identity = Quaternion.LookRotation(-base.gameObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase());
                }
            }
        }
        fighter.SetPosition(zero);
        fighter.transform.rotation = identity;
    }

    public void ResetPlayerPos()
    {
        for (int i = 0; i < BattleGlobal.FighterNumberOneSide; i++)
        {
            this.UpdateFighterWorldPosAndRot(i);
        }
    }

    private void SendMessageToAllFighter(string message, object param)
    {
        <SendMessageToAllFighter>c__AnonStoreyDE yde = new <SendMessageToAllFighter>c__AnonStoreyDE {
            message = message,
            param = param
        };
        this.fighters.ForEach(new Action<BattleFighter>(yde.<>m__40));
    }

    public void SetFighter(int index, BattleFighter fighter)
    {
        if ((index >= 0) && (index < this.fighters.Count))
        {
            this.fighters[index] = fighter;
        }
    }

    public static int SortByPosition(MonsterOneInfo card1, MonsterOneInfo card2)
    {
        int id = 0;
        int entry = 0;
        if (card1.actorInfo.isCard)
        {
            id = card1.actorInfo.entry;
        }
        else
        {
            monster_config _config = ConfigMgr.getInstance().getByEntry<monster_config>(card1.actorInfo.entry);
            if (_config != null)
            {
                id = _config.card_entry;
            }
        }
        if (card2.actorInfo.isCard)
        {
            entry = card2.actorInfo.entry;
        }
        else
        {
            monster_config _config2 = ConfigMgr.getInstance().getByEntry<monster_config>(card2.actorInfo.entry);
            if (_config2 != null)
            {
                entry = _config2.card_entry;
            }
        }
        card_config _config3 = ConfigMgr.getInstance().getByEntry<card_config>(id);
        card_config _config4 = ConfigMgr.getInstance().getByEntry<card_config>(entry);
        return (_config3.card_position - _config4.card_position);
    }

    private void Update()
    {
    }

    private void UpdateFighterWorldPosAndRot(int index)
    {
        if (this.fighters[index] != null)
        {
            this.PlaceFighter(this.fighters[index], index, this.fighters[index].isPlayer);
        }
    }

    private List<BattleFighter> fighters { get; set; }

    public GameObject playerTeamObj { get; private set; }

    public TeamMove teamMove { get; set; }

    [CompilerGenerated]
    private sealed class <GetFighterGameObjIndex>c__AnonStoreyDF
    {
        internal GameObject obj;

        internal bool <>m__41(BattleFighter fighter)
        {
            return ((fighter != null) && (fighter.gameObject == this.obj));
        }
    }

    [CompilerGenerated]
    private sealed class <SendMessageToAllFighter>c__AnonStoreyDE
    {
        internal string message;
        internal object param;

        internal void <>m__40(BattleFighter obj)
        {
            if (obj != null)
            {
                obj.SendMessage(this.message, this.param);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MonsterOneInfo
    {
        public CombatDetailActor actorInfo;
        public MonsterDrop dropInfo;
        public int index;
    }
}

