using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Root"), ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
    public bool adjustByDPI;
    public static List<UIRoot> list = new List<UIRoot>();
    public int manualHeight = 640;
    public int manualWidth = 960;
    public int maximumHeight = 0x600;
    public int minimumHeight = 320;
    private Transform mTrans;
    public Scaling scalingStyle;
    public bool shrinkPortraitUI;

    protected virtual void Awake()
    {
        this.mTrans = base.transform;
    }

    public static void Broadcast(string funcName)
    {
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            UIRoot root = list[num];
            if (root != null)
            {
                root.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
            }
            num++;
        }
    }

    public static void Broadcast(string funcName, object param)
    {
        if (param == null)
        {
            Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
        }
        else
        {
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                UIRoot root = list[num];
                if (root != null)
                {
                    root.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
                }
                num++;
            }
        }
    }

    public float GetPixelSizeAdjustment(int height)
    {
        height = Mathf.Max(2, height);
        if (this.scalingStyle == Scaling.FixedSize)
        {
            return (((float) this.manualHeight) / ((float) height));
        }
        if (this.scalingStyle == Scaling.FixedSizeOnMobiles)
        {
            return (((float) this.manualHeight) / ((float) height));
        }
        if (height < this.minimumHeight)
        {
            return (((float) this.minimumHeight) / ((float) height));
        }
        if (height > this.maximumHeight)
        {
            return (((float) this.maximumHeight) / ((float) height));
        }
        return 1f;
    }

    public static float GetPixelSizeAdjustment(GameObject go)
    {
        UIRoot root = NGUITools.FindInParents<UIRoot>(go);
        return ((root == null) ? 1f : root.pixelSizeAdjustment);
    }

    protected virtual void OnDisable()
    {
        list.Remove(this);
    }

    protected virtual void OnEnable()
    {
        list.Add(this);
    }

    protected virtual void Start()
    {
        UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
        if (componentInChildren != null)
        {
            Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
            Camera component = componentInChildren.gameObject.GetComponent<Camera>();
            componentInChildren.enabled = false;
            if (component != null)
            {
                component.orthographicSize = 1f;
            }
        }
        else
        {
            this.Update();
        }
    }

    private void Update()
    {
        if (this.mTrans != null)
        {
            float activeHeight = this.activeHeight;
            if (activeHeight > 0f)
            {
                float x = 2f / activeHeight;
                Vector3 localScale = this.mTrans.localScale;
                if (((Mathf.Abs((float) (localScale.x - x)) > float.Epsilon) || (Mathf.Abs((float) (localScale.y - x)) > float.Epsilon)) || (Mathf.Abs((float) (localScale.z - x)) > float.Epsilon))
                {
                    this.mTrans.localScale = new Vector3(x, x, x);
                }
            }
        }
    }

    public int activeHeight
    {
        get
        {
            int height = Screen.height;
            int minimumHeight = Mathf.Max(2, height);
            if (this.scalingStyle == Scaling.FixedSize)
            {
                float num3 = ((float) Screen.width) / ((float) Screen.height);
                if (num3 > 1.5f)
                {
                    return this.manualHeight;
                }
                return (int) (((float) this.manualWidth) / num3);
            }
            int width = Screen.width;
            if (this.scalingStyle == Scaling.FixedSizeOnMobiles)
            {
                return this.manualHeight;
            }
            if (minimumHeight < this.minimumHeight)
            {
                minimumHeight = this.minimumHeight;
            }
            if (minimumHeight > this.maximumHeight)
            {
                minimumHeight = this.maximumHeight;
            }
            if (this.shrinkPortraitUI && (height > width))
            {
                minimumHeight = Mathf.RoundToInt(minimumHeight * (((float) height) / ((float) width)));
            }
            return (!this.adjustByDPI ? minimumHeight : NGUIMath.AdjustByDPI((float) minimumHeight));
        }
    }

    public int activeWidth
    {
        get
        {
            float activeHeight = this.activeHeight;
            return Mathf.RoundToInt((activeHeight * Screen.width) / ((float) Screen.height));
        }
    }

    public float pixelSizeAdjustment
    {
        get
        {
            return this.GetPixelSizeAdjustment(Screen.height);
        }
    }

    public enum Scaling
    {
        PixelPerfect,
        FixedSize,
        FixedSizeOnMobiles
    }
}

