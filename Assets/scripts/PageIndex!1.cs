using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PageIndex<T> where T: IList
{
    private int currentPage;

    public PageIndex(T list) : this(list, 10)
    {
    }

    public PageIndex(T list, int maxCountPrePage)
    {
        this.MaxCountPrePage = maxCountPrePage;
        this.List = list;
        this.CurrentPage = 1;
    }

    public int Count
    {
        get
        {
            if (this.List == null)
            {
                return 0;
            }
            return this.List.Count;
        }
    }

    public int CurrentPage
    {
        get
        {
            if (this.Count == 0)
            {
                return 0;
            }
            return Mathf.Clamp(this.currentPage, 1, this.PageCount);
        }
        set
        {
            this.currentPage = Mathf.Clamp(value, 1, this.PageCount);
        }
    }

    public int End
    {
        get
        {
            return Mathf.Clamp(this.Start + this.MaxCountPrePage, 0, this.Count);
        }
    }

    public object this[int index]
    {
        get
        {
            if (this.Count <= index)
            {
                return null;
            }
            return this.List[index];
        }
    }

    public T List { get; private set; }

    public int MaxCountPrePage { get; set; }

    public int NextPage
    {
        get
        {
            return Mathf.Clamp(this.CurrentPage + 1, 1, this.PageCount);
        }
    }

    public int PageCount
    {
        get
        {
            return ((this.Count != 0) ? Mathf.CeilToInt(((float) this.Count) / ((float) this.MaxCountPrePage)) : 0);
        }
    }

    public int PageItemCount
    {
        get
        {
            return (this.End - this.Start);
        }
    }

    public int PrePage
    {
        get
        {
            return Mathf.Clamp(this.CurrentPage - 1, 1, this.PageCount);
        }
    }

    public int Start
    {
        get
        {
            return Mathf.Clamp((this.CurrentPage - 1) * this.MaxCountPrePage, 0, this.Count);
        }
    }
}

