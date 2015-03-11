using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MovingJoystick
{
    public int fingerIndex;
    public EasyJoystick joystick;
    public Vector2 joystickAxis;
    public string joystickName;
    public Vector2 joystickValue;

    public float Axis2Angle(bool inDegree = true)
    {
        float num = Mathf.Atan2(this.joystickAxis.x, this.joystickAxis.y);
        if (inDegree)
        {
            return (num * 57.29578f);
        }
        return num;
    }
}

