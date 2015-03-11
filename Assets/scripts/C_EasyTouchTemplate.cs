using System;
using UnityEngine;

public class C_EasyTouchTemplate : MonoBehaviour
{
    private void On_Cancel(Gesture gesture)
    {
    }

    private void On_DoubleTap(Gesture gesture)
    {
    }

    private void On_DoubleTap2Fingers(Gesture gesture)
    {
    }

    private void On_Drag(Gesture gesture)
    {
    }

    private void On_Drag2Fingers(Gesture gesture)
    {
    }

    private void On_DragEnd(Gesture gesture)
    {
    }

    private void On_DragEnd2Fingers(Gesture gesture)
    {
    }

    private void On_DragStart(Gesture gesture)
    {
    }

    private void On_DragStart2Fingers(Gesture gesture)
    {
    }

    private void On_LongTap(Gesture gesture)
    {
    }

    private void On_LongTap2Fingers(Gesture gesture)
    {
    }

    private void On_LongTapEnd(Gesture gesture)
    {
    }

    private void On_LongTapEnd2Fingers(Gesture gesture)
    {
    }

    private void On_LongTapStart(Gesture gesture)
    {
    }

    private void On_LongTapStart2Fingers(Gesture gesture)
    {
    }

    private void On_PinchEnd(Gesture gesture)
    {
    }

    private void On_PinchIn(Gesture gesture)
    {
    }

    private void On_PinchOut(Gesture gesture)
    {
    }

    private void On_SimpleTap(Gesture gesture)
    {
    }

    private void On_SimpleTap2Fingers(Gesture gesture)
    {
    }

    private void On_Swipe(Gesture gesture)
    {
    }

    private void On_Swipe2Fingers(Gesture gesture)
    {
    }

    private void On_SwipeEnd(Gesture gesture)
    {
    }

    private void On_SwipeEnd2Fingers(Gesture gesture)
    {
    }

    private void On_SwipeStart(Gesture gesture)
    {
    }

    private void On_SwipeStart2Fingers(Gesture gesture)
    {
    }

    private void On_TouchDown(Gesture gesture)
    {
    }

    private void On_TouchDown2Fingers(Gesture gesture)
    {
    }

    private void On_TouchStart(Gesture gesture)
    {
    }

    private void On_TouchStart2Fingers(Gesture gesture)
    {
    }

    private void On_TouchUp(Gesture gesture)
    {
    }

    private void On_TouchUp2Fingers(Gesture gesture)
    {
    }

    private void On_Twist(Gesture gesture)
    {
    }

    private void On_TwistEnd(Gesture gesture)
    {
    }

    private void OnDestroy()
    {
        EasyTouch.On_Cancel -= new EasyTouch.TouchCancelHandler(this.On_Cancel);
        EasyTouch.On_TouchStart -= new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.On_TouchUp);
        EasyTouch.On_SimpleTap -= new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
        EasyTouch.On_DoubleTap -= new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
        EasyTouch.On_LongTapStart -= new EasyTouch.LongTapStartHandler(this.On_LongTapStart);
        EasyTouch.On_LongTap -= new EasyTouch.LongTapHandler(this.On_LongTap);
        EasyTouch.On_LongTapEnd -= new EasyTouch.LongTapEndHandler(this.On_LongTapEnd);
        EasyTouch.On_DragStart -= new EasyTouch.DragStartHandler(this.On_DragStart);
        EasyTouch.On_Drag -= new EasyTouch.DragHandler(this.On_Drag);
        EasyTouch.On_DragEnd -= new EasyTouch.DragEndHandler(this.On_DragEnd);
        EasyTouch.On_SwipeStart -= new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd -= new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_TouchStart2Fingers -= new EasyTouch.TouchStart2FingersHandler(this.On_TouchStart2Fingers);
        EasyTouch.On_TouchDown2Fingers -= new EasyTouch.TouchDown2FingersHandler(this.On_TouchDown2Fingers);
        EasyTouch.On_TouchUp2Fingers -= new EasyTouch.TouchUp2FingersHandler(this.On_TouchUp2Fingers);
        EasyTouch.On_SimpleTap2Fingers -= new EasyTouch.SimpleTap2FingersHandler(this.On_SimpleTap2Fingers);
        EasyTouch.On_DoubleTap2Fingers -= new EasyTouch.DoubleTap2FingersHandler(this.On_DoubleTap2Fingers);
        EasyTouch.On_LongTapStart2Fingers -= new EasyTouch.LongTapStart2FingersHandler(this.On_LongTapStart2Fingers);
        EasyTouch.On_LongTap2Fingers -= new EasyTouch.LongTap2FingersHandler(this.On_LongTap2Fingers);
        EasyTouch.On_LongTapEnd2Fingers -= new EasyTouch.LongTapEnd2FingersHandler(this.On_LongTapEnd2Fingers);
        EasyTouch.On_Twist -= new EasyTouch.TwistHandler(this.On_Twist);
        EasyTouch.On_TwistEnd -= new EasyTouch.TwistEndHandler(this.On_TwistEnd);
        EasyTouch.On_PinchIn -= new EasyTouch.PinchInHandler(this.On_PinchIn);
        EasyTouch.On_PinchOut -= new EasyTouch.PinchOutHandler(this.On_PinchOut);
        EasyTouch.On_PinchEnd -= new EasyTouch.PinchEndHandler(this.On_PinchEnd);
        EasyTouch.On_DragStart2Fingers -= new EasyTouch.DragStart2FingersHandler(this.On_DragStart2Fingers);
        EasyTouch.On_Drag2Fingers -= new EasyTouch.Drag2FingersHandler(this.On_Drag2Fingers);
        EasyTouch.On_DragEnd2Fingers -= new EasyTouch.DragEnd2FingersHandler(this.On_DragEnd2Fingers);
        EasyTouch.On_SwipeStart2Fingers -= new EasyTouch.SwipeStart2FingersHandler(this.On_SwipeStart2Fingers);
        EasyTouch.On_Swipe2Fingers -= new EasyTouch.Swipe2FingersHandler(this.On_Swipe2Fingers);
        EasyTouch.On_SwipeEnd2Fingers -= new EasyTouch.SwipeEnd2FingersHandler(this.On_SwipeEnd2Fingers);
    }

    private void OnDisable()
    {
        EasyTouch.On_Cancel -= new EasyTouch.TouchCancelHandler(this.On_Cancel);
        EasyTouch.On_TouchStart -= new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchUp -= new EasyTouch.TouchUpHandler(this.On_TouchUp);
        EasyTouch.On_SimpleTap -= new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
        EasyTouch.On_DoubleTap -= new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
        EasyTouch.On_LongTapStart -= new EasyTouch.LongTapStartHandler(this.On_LongTapStart);
        EasyTouch.On_LongTap -= new EasyTouch.LongTapHandler(this.On_LongTap);
        EasyTouch.On_LongTapEnd -= new EasyTouch.LongTapEndHandler(this.On_LongTapEnd);
        EasyTouch.On_DragStart -= new EasyTouch.DragStartHandler(this.On_DragStart);
        EasyTouch.On_Drag -= new EasyTouch.DragHandler(this.On_Drag);
        EasyTouch.On_DragEnd -= new EasyTouch.DragEndHandler(this.On_DragEnd);
        EasyTouch.On_SwipeStart -= new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd -= new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_TouchStart2Fingers -= new EasyTouch.TouchStart2FingersHandler(this.On_TouchStart2Fingers);
        EasyTouch.On_TouchDown2Fingers -= new EasyTouch.TouchDown2FingersHandler(this.On_TouchDown2Fingers);
        EasyTouch.On_TouchUp2Fingers -= new EasyTouch.TouchUp2FingersHandler(this.On_TouchUp2Fingers);
        EasyTouch.On_SimpleTap2Fingers -= new EasyTouch.SimpleTap2FingersHandler(this.On_SimpleTap2Fingers);
        EasyTouch.On_DoubleTap2Fingers -= new EasyTouch.DoubleTap2FingersHandler(this.On_DoubleTap2Fingers);
        EasyTouch.On_LongTapStart2Fingers -= new EasyTouch.LongTapStart2FingersHandler(this.On_LongTapStart2Fingers);
        EasyTouch.On_LongTap2Fingers -= new EasyTouch.LongTap2FingersHandler(this.On_LongTap2Fingers);
        EasyTouch.On_LongTapEnd2Fingers -= new EasyTouch.LongTapEnd2FingersHandler(this.On_LongTapEnd2Fingers);
        EasyTouch.On_Twist -= new EasyTouch.TwistHandler(this.On_Twist);
        EasyTouch.On_TwistEnd -= new EasyTouch.TwistEndHandler(this.On_TwistEnd);
        EasyTouch.On_PinchIn -= new EasyTouch.PinchInHandler(this.On_PinchIn);
        EasyTouch.On_PinchOut -= new EasyTouch.PinchOutHandler(this.On_PinchOut);
        EasyTouch.On_PinchEnd -= new EasyTouch.PinchEndHandler(this.On_PinchEnd);
        EasyTouch.On_DragStart2Fingers -= new EasyTouch.DragStart2FingersHandler(this.On_DragStart2Fingers);
        EasyTouch.On_Drag2Fingers -= new EasyTouch.Drag2FingersHandler(this.On_Drag2Fingers);
        EasyTouch.On_DragEnd2Fingers -= new EasyTouch.DragEnd2FingersHandler(this.On_DragEnd2Fingers);
        EasyTouch.On_SwipeStart2Fingers -= new EasyTouch.SwipeStart2FingersHandler(this.On_SwipeStart2Fingers);
        EasyTouch.On_Swipe2Fingers -= new EasyTouch.Swipe2FingersHandler(this.On_Swipe2Fingers);
        EasyTouch.On_SwipeEnd2Fingers -= new EasyTouch.SwipeEnd2FingersHandler(this.On_SwipeEnd2Fingers);
    }

    private void OnEnable()
    {
        EasyTouch.On_Cancel += new EasyTouch.TouchCancelHandler(this.On_Cancel);
        EasyTouch.On_TouchStart += new EasyTouch.TouchStartHandler(this.On_TouchStart);
        EasyTouch.On_TouchDown += new EasyTouch.TouchDownHandler(this.On_TouchDown);
        EasyTouch.On_TouchUp += new EasyTouch.TouchUpHandler(this.On_TouchUp);
        EasyTouch.On_SimpleTap += new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
        EasyTouch.On_DoubleTap += new EasyTouch.DoubleTapHandler(this.On_DoubleTap);
        EasyTouch.On_LongTapStart += new EasyTouch.LongTapStartHandler(this.On_LongTapStart);
        EasyTouch.On_LongTap += new EasyTouch.LongTapHandler(this.On_LongTap);
        EasyTouch.On_LongTapEnd += new EasyTouch.LongTapEndHandler(this.On_LongTapEnd);
        EasyTouch.On_DragStart += new EasyTouch.DragStartHandler(this.On_DragStart);
        EasyTouch.On_Drag += new EasyTouch.DragHandler(this.On_Drag);
        EasyTouch.On_DragEnd += new EasyTouch.DragEndHandler(this.On_DragEnd);
        EasyTouch.On_SwipeStart += new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe += new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd += new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
        EasyTouch.On_TouchStart2Fingers += new EasyTouch.TouchStart2FingersHandler(this.On_TouchStart2Fingers);
        EasyTouch.On_TouchDown2Fingers += new EasyTouch.TouchDown2FingersHandler(this.On_TouchDown2Fingers);
        EasyTouch.On_TouchUp2Fingers += new EasyTouch.TouchUp2FingersHandler(this.On_TouchUp2Fingers);
        EasyTouch.On_SimpleTap2Fingers += new EasyTouch.SimpleTap2FingersHandler(this.On_SimpleTap2Fingers);
        EasyTouch.On_DoubleTap2Fingers += new EasyTouch.DoubleTap2FingersHandler(this.On_DoubleTap2Fingers);
        EasyTouch.On_LongTapStart2Fingers += new EasyTouch.LongTapStart2FingersHandler(this.On_LongTapStart2Fingers);
        EasyTouch.On_LongTap2Fingers += new EasyTouch.LongTap2FingersHandler(this.On_LongTap2Fingers);
        EasyTouch.On_LongTapEnd2Fingers += new EasyTouch.LongTapEnd2FingersHandler(this.On_LongTapEnd2Fingers);
        EasyTouch.On_Twist += new EasyTouch.TwistHandler(this.On_Twist);
        EasyTouch.On_TwistEnd += new EasyTouch.TwistEndHandler(this.On_TwistEnd);
        EasyTouch.On_PinchIn += new EasyTouch.PinchInHandler(this.On_PinchIn);
        EasyTouch.On_PinchOut += new EasyTouch.PinchOutHandler(this.On_PinchOut);
        EasyTouch.On_PinchEnd += new EasyTouch.PinchEndHandler(this.On_PinchEnd);
        EasyTouch.On_DragStart2Fingers += new EasyTouch.DragStart2FingersHandler(this.On_DragStart2Fingers);
        EasyTouch.On_Drag2Fingers += new EasyTouch.Drag2FingersHandler(this.On_Drag2Fingers);
        EasyTouch.On_DragEnd2Fingers += new EasyTouch.DragEnd2FingersHandler(this.On_DragEnd2Fingers);
        EasyTouch.On_SwipeStart2Fingers += new EasyTouch.SwipeStart2FingersHandler(this.On_SwipeStart2Fingers);
        EasyTouch.On_Swipe2Fingers += new EasyTouch.Swipe2FingersHandler(this.On_Swipe2Fingers);
        EasyTouch.On_SwipeEnd2Fingers += new EasyTouch.SwipeEnd2FingersHandler(this.On_SwipeEnd2Fingers);
    }
}

