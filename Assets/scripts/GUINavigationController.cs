using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GUINavigationController
{
    private int baseMaxDepth = 140;
    private List<int> idBuffer = new List<int>();
    private int maxDepthOffset;
    public int minDepth = 120;
    private int ptrActivity = -1;

    public void Clear()
    {
        for (int i = 0; i != this.idBuffer.Count; i++)
        {
            GUIMgr.Instance.CloseGUIEntity(this.idBuffer[i]);
        }
        this.idBuffer.Clear();
        this.ptrActivity = -1;
        this.maxDepthOffset = 0;
    }

    private void DoNavigate(GUIEntity entity, Action<GUIEntity> actCompleted)
    {
        GUIEntity entity2 = this.TopestEntity();
        if (null != entity2)
        {
            this.FadeOutEntity(entity2);
        }
        else
        {
            this.maxDepthOffset = 0;
        }
        this.idBuffer.Add(entity.entity_id);
        this.ptrActivity = this.idBuffer.Count - 1;
        entity2 = this.TopestEntity();
        if (null != entity2)
        {
            this.PushEntity(entity2, actCompleted);
        }
        else
        {
            if (actCompleted != null)
            {
                actCompleted(entity);
            }
            GUIMgr.Instance.Camera3DEnabledOfMainScene = false;
        }
    }

    private void FadeInByPersistence(GUIPersistence pers, Action<GUIEntity> actCompleted)
    {
        <FadeInByPersistence>c__AnonStorey1F9 storeyf = new <FadeInByPersistence>c__AnonStorey1F9 {
            pers = pers,
            actCompleted = actCompleted,
            <>f__this = this
        };
        if ((storeyf.pers != null) && (null != storeyf.pers.entity))
        {
            storeyf.pers.entity.Hidden = false;
            storeyf.pers.entity.Depth = this.minDepth;
            storeyf.pers.entity.DeSerialization(storeyf.pers);
            GUINavigable navigable = this.UniNavigable(storeyf.pers.entity);
            if (null != navigable)
            {
                navigable.FadeIn(new System.Action(storeyf.<>m__33A));
            }
            else
            {
                storeyf.pers.entity.Depth = this.maxDepth;
                if (storeyf.actCompleted != null)
                {
                    storeyf.actCompleted(storeyf.pers.entity);
                }
                GUIMgr.Instance.Camera3DEnabledOfMainScene = false;
            }
        }
    }

    private void FadeOutEntity(GUIEntity entity)
    {
        <FadeOutEntity>c__AnonStorey1F6 storeyf = new <FadeOutEntity>c__AnonStorey1F6 {
            entity = entity,
            <>f__this = this
        };
        GUIPersistence gUIPersistences = GUIMgr.Instance.GetGUIPersistences(storeyf.entity.entity_id);
        if (gUIPersistences != null)
        {
            storeyf.entity.Serialization(gUIPersistences);
        }
        this.maxDepthOffset = storeyf.entity.RelativeDepthOffset;
        storeyf.entity.Depth = this.minDepth;
        GUINavigable navigable = this.UniNavigable(storeyf.entity);
        if (null != navigable)
        {
            navigable.FadeOut(new System.Action(storeyf.<>m__337));
        }
        else
        {
            storeyf.entity.Depth = this.maxDepth;
            storeyf.entity.Hidden = true;
        }
    }

    public bool Managed(int id)
    {
        return (-1 != this.idBuffer.BinarySearch(id));
    }

    private void MoveOutEntity(GUIEntity entity)
    {
        <MoveOutEntity>c__AnonStorey1F8 storeyf = new <MoveOutEntity>c__AnonStorey1F8 {
            entity = entity
        };
        GUINavigable navigable = this.UniNavigable(storeyf.entity);
        if (null != navigable)
        {
            navigable.MoveOut(new System.Action(storeyf.<>m__339));
        }
        else
        {
            storeyf.entity.Hidden = true;
        }
        GUIPersistence gUIPersistences = GUIMgr.Instance.GetGUIPersistences(storeyf.entity.entity_id);
        if (gUIPersistences != null)
        {
            storeyf.entity.Serialization(gUIPersistences);
        }
    }

    public void Pop(Action<GUIEntity> actCompleted)
    {
        <Pop>c__AnonStorey1FB storeyfb = new <Pop>c__AnonStorey1FB {
            actCompleted = actCompleted,
            <>f__this = this
        };
        int groupID = -1;
        int item = -1;
        GUIEntity entity = this.TopestEntity();
        if (null != entity)
        {
            item = entity.entity_id;
            groupID = entity.groupID;
            this.MoveOutEntity(entity);
        }
        if (groupID != -1)
        {
            List<int> list = new List<int>();
            foreach (int num3 in this.idBuffer)
            {
                if (item != num3)
                {
                    GUIPersistence gUIPersistences = GUIMgr.Instance.GetGUIPersistences(num3);
                    if (gUIPersistences == null)
                    {
                        list.Add(num3);
                    }
                    else if (groupID == gUIPersistences.idGroup)
                    {
                        if (null != gUIPersistences.entity)
                        {
                            foreach (GUIEntity entity2 in gUIPersistences.entity.SubEntities)
                            {
                                if (null != entity2)
                                {
                                    GUIMgr.Instance.CloseGUIEntity(entity2.entity_id);
                                }
                            }
                        }
                        list.Add(num3);
                    }
                }
            }
            foreach (int num4 in list)
            {
                this.idBuffer.Remove(num4);
            }
            this.ptrActivity = this.idBuffer.BinarySearch(item);
        }
        this.ptrActivity--;
        this.idBuffer.Remove(item);
        storeyfb.top_pers = this.TopestPersistence();
        if (storeyfb.top_pers != null)
        {
            storeyfb.top_pers.Fetch();
            if (null == storeyfb.top_pers.entity)
            {
                GUIMgr.Instance.CreateGUIEntity(storeyfb.top_pers.name, false, new Action<GUIEntity>(storeyfb.<>m__33C));
            }
            else
            {
                this.FadeInByPersistence(storeyfb.top_pers, storeyfb.actCompleted);
            }
        }
        else
        {
            if (storeyfb.actCompleted != null)
            {
                storeyfb.actCompleted(null);
            }
            GUIMgr.Instance.Camera3DEnabledOfMainScene = true;
        }
    }

    public void Push(string name, Action<GUIEntity> actCompleted)
    {
        <Push>c__AnonStorey1FA storeyfa = new <Push>c__AnonStorey1FA {
            actCompleted = actCompleted,
            <>f__this = this
        };
        GUIPersistence persistence = this.TopestPersistence();
        if ((persistence != null) && (persistence.name == name))
        {
            GUIMgr.Instance.UnLock();
        }
        else
        {
            int num = this.idBuffer.Count - 1;
            if ((this.ptrActivity < num) && (this.ptrActivity >= -1))
            {
                int index = this.ptrActivity + 1;
                GUIPersistence gUIPersistences = GUIMgr.Instance.GetGUIPersistences(this.idBuffer[index]);
                if (gUIPersistences != null)
                {
                    if (name == gUIPersistences.name)
                    {
                        index++;
                        gUIPersistences.Fetch();
                    }
                    else
                    {
                        gUIPersistences = null;
                    }
                }
                for (int i = index; i <= num; i++)
                {
                    int id = this.idBuffer[i];
                    GUIMgr.Instance.CloseGUIEntity(id);
                }
                if ((index <= num) && (index > 0))
                {
                    this.idBuffer.RemoveRange(index, (num - index) + 1);
                }
                if ((gUIPersistences != null) && (null != gUIPersistences.entity))
                {
                    this.idBuffer.Remove(gUIPersistences.entity.entity_id);
                    this.ptrActivity = this.idBuffer.Count - 1;
                    gUIPersistences.entity.OnReset();
                    gUIPersistences.entity.ResetAnimations();
                    gUIPersistences.entity.Hidden = false;
                    gUIPersistences.entity.SendFsmEvent(GUI_FSM_EVENT.ON_GUI_RESET);
                    gUIPersistences.entity.DeSerialization(gUIPersistences);
                    this.DoNavigate(gUIPersistences.entity, storeyfa.actCompleted);
                    return;
                }
            }
            if (this.idBuffer.Count > 10)
            {
                GUIMgr.Instance.CloseGUIEntity(this.idBuffer[0]);
                this.idBuffer.RemoveAt(0);
                this.ptrActivity--;
            }
            GUIMgr.Instance.CreateGUIEntity(name, false, new Action<GUIEntity>(storeyfa.<>m__33B));
        }
    }

    private void PushEntity(GUIEntity entity, Action<GUIEntity> actCompleted)
    {
        <PushEntity>c__AnonStorey1F7 storeyf = new <PushEntity>c__AnonStorey1F7 {
            actCompleted = actCompleted,
            entity = entity
        };
        storeyf.entity.Depth = this.maxDepth;
        storeyf.entity.BlendAnimations(new System.Action(storeyf.<>m__338), false, 1f);
    }

    public GUIEntity TopestEntity()
    {
        if (this.ptrActivity < 0)
        {
            return null;
        }
        if (this.ptrActivity > this.idBuffer.Count)
        {
            return null;
        }
        return GUIMgr.Instance.GetGUIEntity(this.idBuffer[this.ptrActivity]);
    }

    public GUIPersistence TopestPersistence()
    {
        if (this.ptrActivity < 0)
        {
            return null;
        }
        if (this.ptrActivity > this.idBuffer.Count)
        {
            return null;
        }
        return GUIMgr.Instance.GetGUIPersistences(this.idBuffer[this.ptrActivity]);
    }

    private GUINavigable UniNavigable(GUIEntity entity)
    {
        Component[] components = entity.GetComponents(typeof(GUINavigable));
        if (components.Length <= 0)
        {
            return null;
        }
        return (components[components.Length - 1] as GUINavigable);
    }

    public int Count
    {
        get
        {
            return Mathf.Max(0, this.ptrActivity + 1);
        }
    }

    public int FloatingDepth
    {
        get
        {
            return ((this.ptrActivity < 0) ? 0 : this.maxDepth);
        }
    }

    public int maxDepth
    {
        get
        {
            return (this.maxDepthOffset + this.baseMaxDepth);
        }
    }

    [CompilerGenerated]
    private sealed class <FadeInByPersistence>c__AnonStorey1F9
    {
        internal GUINavigationController <>f__this;
        internal Action<GUIEntity> actCompleted;
        internal GUIPersistence pers;

        internal void <>m__33A()
        {
            this.pers.entity.Depth = this.<>f__this.maxDepth;
            if (this.actCompleted != null)
            {
                this.actCompleted(this.pers.entity);
            }
            GUIMgr.Instance.Camera3DEnabledOfMainScene = false;
        }
    }

    [CompilerGenerated]
    private sealed class <FadeOutEntity>c__AnonStorey1F6
    {
        internal GUINavigationController <>f__this;
        internal GUIEntity entity;

        internal void <>m__337()
        {
            this.entity.Depth = this.<>f__this.maxDepth;
            this.entity.Hidden = true;
        }
    }

    [CompilerGenerated]
    private sealed class <MoveOutEntity>c__AnonStorey1F8
    {
        internal GUIEntity entity;

        internal void <>m__339()
        {
            this.entity.Hidden = true;
        }
    }

    [CompilerGenerated]
    private sealed class <Pop>c__AnonStorey1FB
    {
        internal GUINavigationController <>f__this;
        internal Action<GUIEntity> actCompleted;
        internal GUIPersistence top_pers;

        internal void <>m__33C(GUIEntity entity)
        {
            this.top_pers.entity = entity;
            this.<>f__this.FadeInByPersistence(this.top_pers, this.actCompleted);
        }
    }

    [CompilerGenerated]
    private sealed class <Push>c__AnonStorey1FA
    {
        internal GUINavigationController <>f__this;
        internal Action<GUIEntity> actCompleted;

        internal void <>m__33B(GUIEntity entity)
        {
            this.<>f__this.DoNavigate(entity, this.actCompleted);
            entity.DeSerialization(GUIMgr.Instance.GetGUIPersistences(entity.entity_id));
        }
    }

    [CompilerGenerated]
    private sealed class <PushEntity>c__AnonStorey1F7
    {
        internal Action<GUIEntity> actCompleted;
        internal GUIEntity entity;

        internal void <>m__338()
        {
            if (this.actCompleted != null)
            {
                this.actCompleted(this.entity);
            }
            GUIMgr.Instance.Camera3DEnabledOfMainScene = false;
        }
    }
}

