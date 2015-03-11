using FastBuf;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeArenaData
{
    [HideInInspector]
    public ArenaLadderEnemy activity_enemy;
    [HideInInspector]
    public int attack_buy_count;
    [HideInInspector]
    public int attack_count;
    [HideInInspector]
    public List<ArenaLadderEnemy> enemies = new List<ArenaLadderEnemy>();
    [HideInInspector]
    public bool[] enemy_activity_formation = new bool[8];
    [HideInInspector]
    public int modify_time;
    [HideInInspector]
    public int rank = -1;
    [HideInInspector]
    public int remain_attack_count;
    [HideInInspector]
    public int remain_attack_time;
    [HideInInspector]
    public bool[] self_activity_formation = new bool[8];
    [HideInInspector]
    public int top_rank = -1;

    public void ResetActivityFormation()
    {
        int length = this.self_activity_formation.Length;
        for (int i = 0; i != length; i++)
        {
            this.self_activity_formation[i] = true;
        }
        length = this.enemy_activity_formation.Length;
        for (int j = 0; j != length; j++)
        {
            this.enemy_activity_formation[j] = true;
        }
    }

    public int EnemyBanCount
    {
        get
        {
            int num = 0;
            int length = this.enemy_activity_formation.Length;
            for (int i = 0; i != length; i++)
            {
                if (!this.enemy_activity_formation[i])
                {
                    num++;
                }
            }
            return num;
        }
    }
}

