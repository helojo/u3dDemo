using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VirtualScreen : MonoSingleton<VirtualScreen>
{
    public static float height = 768f;
    private float oldRealHeight;
    private float oldRealWidth;
    private float realHeight;
    private float realWidth;
    public float virtualHeight = 768f;
    public float virtualWidth = 1024f;
    public static float width = 1024f;
    public static float xRatio = 1f;
    public static float yRatio = 1f;

    public static  event On_ScreenResizeHandler On_ScreenResize;

    private void Awake()
    {
        this.realWidth = this.oldRealWidth = Screen.width;
        this.realHeight = this.oldRealHeight = Screen.height;
        this.ComputeScreen();
    }

    public void ComputeScreen()
    {
        VirtualScreen.width = this.virtualWidth;
        height = this.virtualHeight;
        xRatio = 1f;
        yRatio = 1f;
        float num = 0f;
        float width = 0f;
        if (Screen.width > Screen.height)
        {
            num = ((float) Screen.width) / ((float) Screen.height);
            width = VirtualScreen.width;
        }
        else
        {
            num = ((float) Screen.height) / ((float) Screen.width);
            width = height;
        }
        float num3 = 0f;
        num3 = width / num;
        if (Screen.width > Screen.height)
        {
            height = num3;
            xRatio = ((float) Screen.width) / VirtualScreen.width;
            yRatio = ((float) Screen.height) / height;
        }
        else
        {
            VirtualScreen.width = num3;
            xRatio = ((float) Screen.width) / VirtualScreen.width;
            yRatio = ((float) Screen.height) / height;
        }
    }

    public static void ComputeVirtualScreen()
    {
        MonoSingleton<VirtualScreen>.instance.ComputeScreen();
    }

    public static Rect GetRealRect(Rect rect)
    {
        return new Rect(rect.x * xRatio, rect.y * yRatio, rect.width * xRatio, rect.height * yRatio);
    }

    public static void SetGuiScaleMatrix()
    {
        GUI.matrix = Matrix4x4.Scale(new Vector3(xRatio, yRatio, 1f));
    }

    private void Update()
    {
        this.realWidth = Screen.width;
        this.realHeight = Screen.height;
        if ((this.realWidth != this.oldRealWidth) || (this.realHeight != this.oldRealHeight))
        {
            this.ComputeScreen();
            if (On_ScreenResize != null)
            {
                On_ScreenResize();
            }
        }
        this.oldRealWidth = this.realWidth;
        this.oldRealHeight = this.realHeight;
    }

    public delegate void On_ScreenResizeHandler();

    public enum ScreenResolution
    {
        IPhoneTall,
        IPhoneWide,
        IPhone4GTall,
        IPhone4GWide,
        IPadTall,
        IPadWide
    }
}

