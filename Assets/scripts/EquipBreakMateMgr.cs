using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;

public class EquipBreakMateMgr : XSingleton<EquipBreakMateMgr>
{
    private Dictionary<long, Dictionary<int, BreakMate>> _cardQ = new Dictionary<long, Dictionary<int, BreakMate>>();

    public void Clear()
    {
        this._cardQ.Clear();
    }

    internal List<Card> GetCardListByLackOneItemByEquip(int itemEntry, int num)
    {
        <GetCardListByLackOneItemByEquip>c__AnonStorey289 storey = new <GetCardListByLackOneItemByEquip>c__AnonStorey289 {
            list = new List<long>()
        };
        foreach (KeyValuePair<long, Dictionary<int, BreakMate>> pair in this._cardQ)
        {
            bool flag = false;
            foreach (KeyValuePair<int, BreakMate> pair2 in pair.Value)
            {
                if (flag)
                {
                    break;
                }
                foreach (KeyValuePair<int, NeedNum> pair3 in pair2.Value.Mates)
                {
                    if ((pair3.Key == itemEntry) && (pair3.Value.Num <= num))
                    {
                        storey.list.Add(pair.Key);
                        flag = true;
                        break;
                    }
                }
            }
        }
        return ActorData.getInstance().CardList.FindAll(new Predicate<Card>(storey.<>m__5DA));
    }

    private void SearchNeedItems(item_config current, int needNum, Dictionary<int, NeedNum> items)
    {
        UserItemPackageMgr.UserItem item = XSingleton<UserItemPackageMgr>.Singleton[current.entry];
        int num = (item != null) ? item.Item.num : 0;
        int num2 = needNum - num;
        if ((current.type == 1) && ((item == null) || (item.Item.num < needNum)))
        {
            <>__AnonType0<int, int>[] typeArray = new <>__AnonType0<int, int>[] { new <>__AnonType0<int, int>(current.param_0, current.param_1), new <>__AnonType0<int, int>(current.param_2, current.param_3), new <>__AnonType0<int, int>(current.param_4, current.param_5) };
            foreach (<>__AnonType0<int, int> type in typeArray)
            {
                if (type.Entry >= 0)
                {
                    item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(type.Entry);
                    if (_config != null)
                    {
                        this.SearchNeedItems(_config, type.Num, items);
                    }
                }
            }
        }
        if (num < needNum)
        {
            NeedNum num4;
            if (items.TryGetValue(current.entry, out num4))
            {
                NeedNum local1 = items[current.entry];
                local1.Num += num2;
            }
            else
            {
                NeedNum num5 = new NeedNum {
                    Config = current,
                    Entry = current.entry,
                    Num = num2
                };
                items.Add(current.entry, num5);
            }
        }
    }

    public void Update(Card card, int index)
    {
        Dictionary<int, BreakMate> dictionary2;
        BreakMate mate2;
        EquipInfo info = card.equipInfo[index];
        break_equip_config _config = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
        <>__AnonType0<int, int>[] typeArray = new <>__AnonType0<int, int>[] { new <>__AnonType0<int, int>(_config.need_item_1, _config.need_num_1), new <>__AnonType0<int, int>(_config.need_item_2, _config.need_num_2), new <>__AnonType0<int, int>(_config.need_item_3, _config.need_num_3) };
        Dictionary<int, NeedNum> items = new Dictionary<int, NeedNum>();
        foreach (<>__AnonType0<int, int> type in typeArray)
        {
            if (type.Entry >= 0)
            {
                item_config current = ConfigMgr.getInstance().getByEntry<item_config>(type.Entry);
                if (current != null)
                {
                    this.SearchNeedItems(current, type.Num, items);
                }
            }
        }
        if (this._cardQ.TryGetValue(card.card_id, out dictionary2))
        {
            BreakMate mate;
            if (dictionary2.TryGetValue(index, out mate))
            {
                mate.Mates = items;
            }
            else
            {
                mate2 = new BreakMate {
                    Index = index,
                    Mates = items
                };
                dictionary2.Add(index, mate2);
            }
        }
        else
        {
            Dictionary<int, BreakMate> dictionary3 = new Dictionary<int, BreakMate>();
            mate2 = new BreakMate {
                Index = index,
                Mates = items
            };
            dictionary3.Add(index, mate2);
            this._cardQ.Add(card.card_id, dictionary3);
        }
    }

    [CompilerGenerated]
    private sealed class <GetCardListByLackOneItemByEquip>c__AnonStorey289
    {
        internal List<long> list;

        internal bool <>m__5DA(Card t)
        {
            return this.list.Contains(t.card_id);
        }
    }

    public class BreakMate
    {
        public int Index { get; set; }

        public Dictionary<int, EquipBreakMateMgr.NeedNum> Mates { get; set; }
    }

    public class NeedNum
    {
        public item_config Config { get; set; }

        public int Entry { get; set; }

        public int Num { get; set; }
    }
}

