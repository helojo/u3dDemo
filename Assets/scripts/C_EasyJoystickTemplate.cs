using System;
using UnityEngine;

public class C_EasyJoystickTemplate : MonoBehaviour
{
    private void On_JoystickDoubleTap(MovingJoystick move)
    {
    }

    private void On_JoystickMove(MovingJoystick move)
    {
    }

    private void On_JoystickMoveEnd(MovingJoystick move)
    {
    }

    private void On_JoystickMoveStart(MovingJoystick move)
    {
    }

    private void On_JoystickTap(MovingJoystick move)
    {
    }

    private void On_JoystickTouchStart(MovingJoystick move)
    {
    }

    private void On_JoystickTouchUp(MovingJoystick move)
    {
    }

    private void OnDestroy()
    {
        EasyJoystick.On_JoystickTouchStart -= new EasyJoystick.JoystickTouchStartHandler(this.On_JoystickTouchStart);
        EasyJoystick.On_JoystickMoveStart -= new EasyJoystick.JoystickMoveStartHandler(this.On_JoystickMoveStart);
        EasyJoystick.On_JoystickMove -= new EasyJoystick.JoystickMoveHandler(this.On_JoystickMove);
        EasyJoystick.On_JoystickMoveEnd -= new EasyJoystick.JoystickMoveEndHandler(this.On_JoystickMoveEnd);
        EasyJoystick.On_JoystickTouchUp -= new EasyJoystick.JoystickTouchUpHandler(this.On_JoystickTouchUp);
        EasyJoystick.On_JoystickTap -= new EasyJoystick.JoystickTapHandler(this.On_JoystickTap);
        EasyJoystick.On_JoystickDoubleTap -= new EasyJoystick.JoystickDoubleTapHandler(this.On_JoystickDoubleTap);
    }

    private void OnDisable()
    {
        EasyJoystick.On_JoystickTouchStart -= new EasyJoystick.JoystickTouchStartHandler(this.On_JoystickTouchStart);
        EasyJoystick.On_JoystickMoveStart -= new EasyJoystick.JoystickMoveStartHandler(this.On_JoystickMoveStart);
        EasyJoystick.On_JoystickMove -= new EasyJoystick.JoystickMoveHandler(this.On_JoystickMove);
        EasyJoystick.On_JoystickMoveEnd -= new EasyJoystick.JoystickMoveEndHandler(this.On_JoystickMoveEnd);
        EasyJoystick.On_JoystickTouchUp -= new EasyJoystick.JoystickTouchUpHandler(this.On_JoystickTouchUp);
        EasyJoystick.On_JoystickTap -= new EasyJoystick.JoystickTapHandler(this.On_JoystickTap);
        EasyJoystick.On_JoystickDoubleTap -= new EasyJoystick.JoystickDoubleTapHandler(this.On_JoystickDoubleTap);
    }

    private void OnEnable()
    {
        EasyJoystick.On_JoystickTouchStart += new EasyJoystick.JoystickTouchStartHandler(this.On_JoystickTouchStart);
        EasyJoystick.On_JoystickMoveStart += new EasyJoystick.JoystickMoveStartHandler(this.On_JoystickMoveStart);
        EasyJoystick.On_JoystickMove += new EasyJoystick.JoystickMoveHandler(this.On_JoystickMove);
        EasyJoystick.On_JoystickMoveEnd += new EasyJoystick.JoystickMoveEndHandler(this.On_JoystickMoveEnd);
        EasyJoystick.On_JoystickTouchUp += new EasyJoystick.JoystickTouchUpHandler(this.On_JoystickTouchUp);
        EasyJoystick.On_JoystickTap += new EasyJoystick.JoystickTapHandler(this.On_JoystickTap);
        EasyJoystick.On_JoystickDoubleTap += new EasyJoystick.JoystickDoubleTapHandler(this.On_JoystickDoubleTap);
    }
}

