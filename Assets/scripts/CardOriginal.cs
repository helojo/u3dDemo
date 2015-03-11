using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CardOriginal : MonoBehaviour
{
    [HideInInspector]
    public card_config base_cfg;
    [HideInInspector]
    public bool dirty;
    [HideInInspector]
    public int dirty_flag;
    [HideInInspector]
    public HashSet<int> equip_lu_set = new HashSet<int>();
    [HideInInspector]
    public card_ex_config ex_cfg;
    [HideInInspector]
    public Card ori;

    public void CollectEquipMaterial()
    {
        if (this.ori != null)
        {
            int count = this.ori.equipInfo.Count;
            for (int i = 0; i != count; i++)
            {
                EquipInfo info = this.ori.equipInfo[i];
                if ((info != null) && (info.quality <= (this.ori.cardInfo.quality + 1)))
                {
                    break_equip_config _config = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
                    if ((_config != null) && (_config.break_equip_entry >= 0))
                    {
                        if ((_config.need_item_1 > 0) && (_config.need_num_1 > 0))
                        {
                            this.equip_lu_set.Add(_config.need_item_1);
                            item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(_config.need_item_1);
                            if ((_config2 != null) && (_config2.param_0 > 0))
                            {
                                this.equip_lu_set.Add(_config2.param_0);
                            }
                        }
                        if ((_config.need_item_2 > 0) && (_config.need_num_2 > 0))
                        {
                            this.equip_lu_set.Add(_config.need_item_2);
                            item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(_config.need_item_2);
                            if ((_config3 != null) && (_config3.param_0 > 0))
                            {
                                this.equip_lu_set.Add(_config3.param_0);
                            }
                        }
                        if ((_config.need_item_3 > 0) && (_config.need_num_3 > 0))
                        {
                            this.equip_lu_set.Add(_config.need_item_3);
                            item_config _config4 = ConfigMgr.getInstance().getByEntry<item_config>(_config.need_item_3);
                            if ((_config4 != null) && (_config4.param_0 > 0))
                            {
                                this.equip_lu_set.Add(_config4.param_0);
                            }
                        }
                    }
                }
            }
        }
    }

    public int NumOfExistedItem()
    {
        <NumOfExistedItem>c__AnonStorey17F storeyf = new <NumOfExistedItem>c__AnonStorey17F();
        if (this.ex_cfg == null)
        {
            return 0;
        }
        storeyf.item_entry = this.ex_cfg.item_entry;
        Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeyf.<>m__1D1));
        if (item == null)
        {
            return 0;
        }
        return item.num;
    }

    [CompilerGenerated]
    private sealed class <NumOfExistedItem>c__AnonStorey17F
    {
        internal int item_entry;

        internal bool <>m__1D1(Item e)
        {
            return (e.entry == this.item_entry);
        }
    }
}

