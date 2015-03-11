using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UITableManager<T> where T: UITableItem, new()
{
    private List<T> _items;
    private int count;
    private UIGrid grid;
    private UITable table;

    public UITableManager() : this(0x40)
    {
    }

    public UITableManager(int defaultLenght)
    {
        this.Cache = true;
        this._items = new List<T>(defaultLenght);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return this._items.Take<T>(this.count).GetEnumerator();
    }

    public void InitFromGrid(UIGrid grid)
    {
        this.grid = grid;
        if (grid.transform.childCount > 0)
        {
            this.InitTable(grid.transform.GetChild(0));
        }
    }

    public void InitFromTable(UITable table)
    {
        this.table = table;
        if (table.transform.childCount > 0)
        {
            this.InitTable(table.transform.GetChild(0));
        }
    }

    public void InitTable(Transform root)
    {
        this.Root = root;
        this.Root.gameObject.SetActive(false);
    }

    public void RepositionLayout()
    {
        if (this.table != null)
        {
            this.table.Reposition();
        }
        if (this.grid != null)
        {
            this.grid.Reposition();
        }
    }

    public bool Cache { get; set; }

    public int Count
    {
        get
        {
            return this.count;
        }
        set
        {
            if (this.Root == null)
            {
                throw new Exception("Try to set a no init table manager!");
            }
            for (int i = 0; i < this._items.Count; i++)
            {
                T local3 = this._items[i];
                if (!local3.Root.gameObject.activeSelf)
                {
                    T local4 = this._items[i];
                    local4.Root.gameObject.SetActive(true);
                }
            }
            if (this._items.Count != value)
            {
                if (this._items.Count > value)
                {
                    if (this.Cache)
                    {
                        for (int j = value; j < this._items.Count; j++)
                        {
                            T local5 = this._items[j];
                            if (local5.Root.gameObject.activeSelf)
                            {
                                T local6 = this._items[j];
                                local6.Root.gameObject.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        Queue<T> queue = new Queue<T>();
                        for (int k = value; k < this._items.Count; k++)
                        {
                            queue.Enqueue(this._items[k]);
                        }
                        while (queue.Count > 0)
                        {
                            T item = queue.Dequeue();
                            UnityEngine.Object.Destroy(item.Root.gameObject);
                            this._items.Remove(item);
                        }
                    }
                }
                else
                {
                    for (int m = this._items.Count; m < value; m++)
                    {
                        T local2 = Activator.CreateInstance<T>();
                        GameObject obj2 = UnityEngine.Object.Instantiate(this.Root.gameObject) as GameObject;
                        obj2.name = string.Format("{0}_{1:000}", this.Root.gameObject.name, m);
                        obj2.transform.parent = this.Root.parent;
                        obj2.transform.localScale = Vector3.one;
                        obj2.SetActive(true);
                        local2.Init(obj2.transform);
                        local2.OnCreate();
                        this._items.Add(local2);
                    }
                }
            }
            this.count = value;
            this.RepositionLayout();
        }
    }

    public T FristItemNoHide
    {
        get
        {
            foreach (T local in this._items)
            {
                if (!local.Hide)
                {
                    return local;
                }
            }
            return null;
        }
    }

    public T this[int key]
    {
        get
        {
            if ((this.count < key) || (key < 0))
            {
                throw new IndexOutOfRangeException("Out of index");
            }
            return this._items[key];
        }
    }

    private Transform Root { get; set; }

    public int TotalCountNoHide
    {
        get
        {
            int num = 0;
            IEnumerator<T> enumerator = this.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;
                    if (!current.Hide)
                    {
                        num++;
                    }
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            return num;
        }
    }
}

