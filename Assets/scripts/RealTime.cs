using System;
using UnityEngine;

public class RealTime : MonoBehaviour
{
    private static RealTime mInst;
    private float mRealDelta;
    private float mRealTime;

    private static void Spawn()
    {
        GameObject target = new GameObject("_RealTime");
        UnityEngine.Object.DontDestroyOnLoad(target);
        mInst = target.AddComponent<RealTime>();
        mInst.mRealTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        this.mRealDelta = Mathf.Clamp01(realtimeSinceStartup - this.mRealTime);
        this.mRealTime = realtimeSinceStartup;
    }

    public static float deltaTime
    {
        get
        {
            if (mInst == null)
            {
                Spawn();
            }
            return mInst.mRealDelta;
        }
    }

    public static float time
    {
        get
        {
            if (mInst == null)
            {
                Spawn();
            }
            return mInst.mRealTime;
        }
    }
}

