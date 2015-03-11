using System;
using UnityEngine;

public class CameraSliderController : MonoBehaviour
{
    private int currentState = 1;
    private static float dragCameraDurationSecond = 0.8f;
    private float halfScreen;
    private Vector3 lastPosition = Vector3.zero;
    private float lastStartXPos;
    private float lastSwipeTime;
    private float lerpPercent;
    public Transform[] lookPath;
    public Transform lookTarget;
    public Transform[] movePath;
    public float percentage;
    public float[] percentages;

    private float getPercentByStage(int stage)
    {
        if (stage < 0)
        {
            return 0f;
        }
        if (stage >= this.percentages.Length)
        {
            return 1f;
        }
        return this.percentages[stage];
    }

    private void On_Swipe(Gesture gesture)
    {
        float num = gesture.position.x - this.lastStartXPos;
        this.lastStartXPos = gesture.position.x;
        if (num != 0f)
        {
            Debug.Log(string.Concat(new object[] { "lastStartXPos:", this.lastStartXPos, "  position:", gesture.position.x, " delta:", num }));
        }
        this.lerpPercent -= num * 0.1f;
    }

    private void On_SwipeEnd(Gesture gesture)
    {
        this.lerpPercent = this.percentage;
        Debug.Log(string.Concat(new object[] { "Last swipe : ", gesture.swipe.ToString(), " /  vector : ", gesture.swipeVector }));
    }

    private void On_SwipeStart(Gesture gesture)
    {
        if (gesture.fingerIndex == 0)
        {
            this.lastStartXPos = gesture.position.x;
            iTween.Stop(base.gameObject);
            this.lastSwipeTime = 0f;
        }
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvent();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvent();
    }

    private void OnDrawGizmos()
    {
        iTween.DrawPath(this.movePath, Color.magenta);
        iTween.DrawPath(this.lookPath, Color.cyan);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(base.transform.position, this.lookTarget.position);
    }

    private void OnEnable()
    {
        this.SubscribeEvent();
    }

    private void OnGUI()
    {
        this.lerpPercent = Mathf.Clamp01(this.lerpPercent);
        float num2 = 0f;
        float a = this.percentage - this.lerpPercent;
        if (this.percentage > this.lerpPercent)
        {
            num2 = -Mathf.Min(a, 0.002f);
        }
        else if (this.percentage < this.lerpPercent)
        {
            num2 = Mathf.Min(-a, 0.002f);
        }
        this.percentage += num2;
        iTween.PutOnPath(base.gameObject, this.movePath, this.percentage);
        iTween.PutOnPath(this.lookTarget, this.lookPath, this.percentage);
        base.transform.LookAt(iTween.PointOnPath(this.lookPath, this.percentage));
    }

    private void SlidePercentage(float p)
    {
        this.lerpPercent = p;
    }

    private void Start()
    {
        this.percentage = this.lerpPercent = 0.5f;
        this.halfScreen = ((float) Screen.width) / 2f;
    }

    private void SubscribeEvent()
    {
        EasyTouch.On_SwipeStart += new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe += new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd += new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
    }

    private void UnsubscribeEvent()
    {
        EasyTouch.On_SwipeStart -= new EasyTouch.SwipeStartHandler(this.On_SwipeStart);
        EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SwipeEnd -= new EasyTouch.SwipeEndHandler(this.On_SwipeEnd);
    }
}

