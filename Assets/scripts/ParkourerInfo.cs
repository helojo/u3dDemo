using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParkourerInfo
{
    private int entry;
    private int hp;
    private int secondesLossHP;
    public System.Action skill;

    public ParkourerInfo(int _entry)
    {
        this.entry = _entry;
        this.hp = ConfigMgr.getInstance().getByEntry<guildrun_character_config>(this.entry).character_hp;
        this.secondesLossHP = ConfigMgr.getInstance().getByEntry<guildrun_character_config>(this.entry).hp_secondsloss;
        switch (this.entry)
        {
            case 0:
                this.skill = new System.Action(ParkourEvent._instance.MTSkill);
                break;

            case 1:
                this.skill = new System.Action(ParkourEvent._instance.DXJSkill);
                break;

            case 2:
                this.skill = new System.Action(ParkourEvent._instance.DDJSkill);
                break;
        }
    }

    public void ResetHP()
    {
        this.hp = ConfigMgr.getInstance().getByEntry<guildrun_character_config>(this.entry).character_hp;
    }

    [DebuggerHidden]
    public IEnumerator SecondesLossHP()
    {
        return new <SecondesLossHP>c__IteratorC4 { <>f__this = this };
    }

    public int UpdateHP(int value)
    {
        if (this.hp <= 0)
        {
            return -1;
        }
        if (value > 0)
        {
            this.hp = ((this.hp + value) <= ConfigMgr.getInstance().getByEntry<guildrun_character_config>(this.entry).character_hp) ? (this.hp + value) : ConfigMgr.getInstance().getByEntry<guildrun_character_config>(this.entry).character_hp;
        }
        else
        {
            this.hp = ((this.hp + value) >= 0) ? (this.hp + value) : 0;
        }
        if (PaokuInPanel._instance != null)
        {
            PaokuInPanel._instance.UpdataHp((float) this.hp);
        }
        if (this.hp <= 0)
        {
            ParkourEvent._instance.DeadAnim();
            ParkourEvent._instance.GameReward();
        }
        return this.hp;
    }

    public int GetHP
    {
        get
        {
            return this.hp;
        }
    }

    public float GetRunPercent
    {
        get
        {
            return (ParkourManager._instance.runDistance / ParkourInit._instance.maxRunDistance);
        }
    }

    [CompilerGenerated]
    private sealed class <SecondesLossHP>c__IteratorC4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ParkourerInfo <>f__this;

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
                case 1:
                    if ((this.<>f__this.hp > 0) && ParkourManager._instance.GameStart)
                    {
                        this.<>f__this.UpdateHP(-this.<>f__this.secondesLossHP);
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 1;
                        return true;
                    }
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

