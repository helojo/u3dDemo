using System;
using UnityEngine;

public class EasyTouchInput
{
    private bool bComplex;
    private Vector2 complexCenter;
    private Vector2 deltaFingerPosition;
    private float[] deltaTime = new float[2];
    private Vector2 oldFinger2Position;
    private Vector2[] oldMousePosition = new Vector2[2];
    private float[] startActionTime = new float[2];
    private int[] tapCount = new int[2];
    private float[] tapeTime = new float[2];

    private Vector2 GetComplex2finger()
    {
        Vector2 vector;
        vector.x = Input.mousePosition.x - this.deltaFingerPosition.x;
        vector.y = Input.mousePosition.y - this.deltaFingerPosition.y;
        this.complexCenter = new Vector2((Input.mousePosition.x + vector.x) / 2f, (Input.mousePosition.y + vector.y) / 2f);
        this.oldFinger2Position = vector;
        return vector;
    }

    public Finger GetMouseTouch(int fingerIndex, Finger myFinger)
    {
        Finger finger;
        if (myFinger != null)
        {
            finger = myFinger;
        }
        else
        {
            finger = new Finger {
                gesture = EasyTouch.GestureType.None
            };
        }
        if ((fingerIndex == 1) && ((Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(EasyTouch.instance.twistKey)) || (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(EasyTouch.instance.swipeKey))))
        {
            finger.fingerIndex = fingerIndex;
            finger.position = this.oldFinger2Position;
            finger.deltaPosition = finger.position - this.oldFinger2Position;
            finger.tapCount = this.tapCount[fingerIndex];
            finger.deltaTime = Time.realtimeSinceStartup - this.deltaTime[fingerIndex];
            finger.phase = TouchPhase.Ended;
            return finger;
        }
        if (Input.GetMouseButton(0))
        {
            finger.fingerIndex = fingerIndex;
            finger.position = this.GetPointerPosition(fingerIndex);
            if ((Time.realtimeSinceStartup - this.tapeTime[fingerIndex]) > 0.5)
            {
                this.tapCount[fingerIndex] = 0;
            }
            if (Input.GetMouseButtonDown(0) || ((fingerIndex == 1) && ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(EasyTouch.instance.twistKey)) || (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(EasyTouch.instance.swipeKey)))))
            {
                finger.position = this.GetPointerPosition(fingerIndex);
                finger.deltaPosition = Vector2.zero;
                this.tapCount[fingerIndex]++;
                finger.tapCount = this.tapCount[fingerIndex];
                this.startActionTime[fingerIndex] = Time.realtimeSinceStartup;
                this.deltaTime[fingerIndex] = this.startActionTime[fingerIndex];
                finger.deltaTime = 0f;
                finger.phase = TouchPhase.Began;
                if (fingerIndex == 1)
                {
                    this.oldFinger2Position = finger.position;
                }
                else
                {
                    this.oldMousePosition[fingerIndex] = finger.position;
                }
                if (this.tapCount[fingerIndex] == 1)
                {
                    this.tapeTime[fingerIndex] = Time.realtimeSinceStartup;
                }
                return finger;
            }
            finger.deltaPosition = finger.position - this.oldMousePosition[fingerIndex];
            finger.tapCount = this.tapCount[fingerIndex];
            finger.deltaTime = Time.realtimeSinceStartup - this.deltaTime[fingerIndex];
            if (finger.deltaPosition.sqrMagnitude < 1f)
            {
                finger.phase = TouchPhase.Stationary;
            }
            else
            {
                finger.phase = TouchPhase.Moved;
            }
            this.oldMousePosition[fingerIndex] = finger.position;
            this.deltaTime[fingerIndex] = Time.realtimeSinceStartup;
            return finger;
        }
        if (Input.GetMouseButtonUp(0))
        {
            finger.fingerIndex = fingerIndex;
            finger.position = this.GetPointerPosition(fingerIndex);
            finger.deltaPosition = finger.position - this.oldMousePosition[fingerIndex];
            finger.tapCount = this.tapCount[fingerIndex];
            finger.deltaTime = Time.realtimeSinceStartup - this.deltaTime[fingerIndex];
            finger.phase = TouchPhase.Ended;
            this.oldMousePosition[fingerIndex] = finger.position;
            return finger;
        }
        return null;
    }

    private Vector2 GetPinchTwist2Finger()
    {
        Vector2 vector;
        if (this.complexCenter == Vector2.zero)
        {
            vector.x = (((float) Screen.width) / 2f) - (Input.mousePosition.x - (((float) Screen.width) / 2f));
            vector.y = (((float) Screen.height) / 2f) - (Input.mousePosition.y - (((float) Screen.height) / 2f));
        }
        else
        {
            vector.x = this.complexCenter.x - (Input.mousePosition.x - this.complexCenter.x);
            vector.y = this.complexCenter.y - (Input.mousePosition.y - this.complexCenter.y);
        }
        this.oldFinger2Position = vector;
        return vector;
    }

    private Vector2 GetPointerPosition(int index)
    {
        if (index == 0)
        {
            return Input.mousePosition;
        }
        return this.GetSecondFingerPosition();
    }

    public Vector2 GetSecondFingerPosition()
    {
        Vector2 vector = new Vector2(-1f, -1f);
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(EasyTouch.instance.twistKey)) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(EasyTouch.instance.swipeKey)))
        {
            if (!this.bComplex)
            {
                this.bComplex = true;
                this.deltaFingerPosition = Input.mousePosition - this.oldFinger2Position;
            }
            return this.GetComplex2finger();
        }
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(EasyTouch.instance.twistKey))
        {
            vector = this.GetPinchTwist2Finger();
            this.bComplex = false;
            return vector;
        }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(EasyTouch.instance.swipeKey))
        {
            vector = this.GetComplex2finger();
            this.bComplex = false;
        }
        return vector;
    }

    private int getTouchCount(bool realTouch)
    {
        int num = 0;
        if (realTouch || EasyTouch.instance.enableRemote)
        {
            return Input.touchCount;
        }
        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
        {
            return num;
        }
        num = 1;
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(EasyTouch.instance.twistKey)) || (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(EasyTouch.instance.swipeKey)))
        {
            num = 2;
        }
        if ((!Input.GetKeyUp(KeyCode.LeftAlt) && !Input.GetKeyUp(EasyTouch.instance.twistKey)) && (!Input.GetKeyUp(KeyCode.LeftControl) && !Input.GetKeyUp(EasyTouch.instance.swipeKey)))
        {
            return num;
        }
        return 2;
    }

    public int TouchCount()
    {
        return this.getTouchCount(true);
    }
}

