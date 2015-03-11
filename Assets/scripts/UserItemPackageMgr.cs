using FastBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Toolbox;

public class UserItemPackageMgr : XSingleton<UserItemPackageMgr>
{
    [CompilerGenerated]
    private static Func<UserItem, Item> <>f__am$cache1;
    private Dictionary<int, UserItem> userItems = new Dictionary<int, UserItem>();

    public void Clear()
    {
        this.userItems.Clear();
    }

    public void Each(ForeachCondition cond)
    {
        foreach (KeyValuePair<int, UserItem> pair in this.userItems)
        {
            if (cond(pair.Value))
            {
                break;
            }
        }
    }

    public bool Exists(ForeachCondition condtion)
    {
        foreach (KeyValuePair<int, UserItem> pair in this.userItems)
        {
            if (condtion(pair.Value))
            {
                return true;
            }
        }
        return false;
    }

    public Item GetItemByEntry(int entry)
    {
        UserItem item = this[entry];
        if (item == null)
        {
            return null;
        }
        return item.Item;
    }

    internal int GetItemCountByEntry(int itemId)
    {
        int num = 0;
        UserItem item = this[itemId];
        if (item != null)
        {
            num = item.Item.num;
        }
        return num;
    }

    internal List<Item> GetItemsByTypes(List<ItemCategory> types)
    {
        List<Item> list = new List<Item>();
        foreach (KeyValuePair<int, UserItem> pair in this.userItems)
        {
            for (int i = 0; i < types.Count; i++)
            {
                int num2 = types[i];
                if (pair.Value.Config.type == num2)
                {
                    list.Add(pair.Value.Item);
                }
            }
        }
        return list;
    }

    public UserItem GetUserItemByEntry(int entry)
    {
        UserItem item;
        if (this.userItems.TryGetValue(entry, out item))
        {
            return item;
        }
        return null;
    }

    internal void Update(Item _item)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_item.entry);
        if (_config.type == 6)
        {
            if (_config.param_0 == 0)
            {
                ActorData.getInstance().nBoxGoldKeys = _item.num;
            }
            else if (_config.param_0 == 1)
            {
                ActorData.getInstance().nBoxSliverKeys = _item.num;
            }
            else if (_config.param_0 == 2)
            {
                ActorData.getInstance().nBoxCopperKeys = _item.num;
            }
        }
        if (_item.num <= 0)
        {
            this.userItems.Remove(_item.entry);
        }
        else
        {
            UserItem item;
            if (this.userItems.TryGetValue(_item.entry, out item))
            {
                this.userItems[_item.entry].Item = _item;
            }
            else
            {
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(_item.entry);
                UserItem item2 = new UserItem {
                    Config = _config2,
                    Item = _item
                };
                this.userItems.Add(_item.entry, item2);
            }
        }
    }

    public UserItem this[int entry]
    {
        get
        {
            return this.GetUserItemByEntry(entry);
        }
    }

    [Obsolete("not a fast mothed")]
    public List<Item> Items
    {
        get
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = t => t.Item;
            }
            return this.userItems.Values.Select<UserItem, Item>(<>f__am$cache1).ToList<Item>();
        }
    }

    public delegate bool ForeachCondition(UserItemPackageMgr.UserItem item);

    public class UserItem
    {
        public item_config Config { get; set; }

        public FastBuf.Item Item { get; set; }
    }
}

