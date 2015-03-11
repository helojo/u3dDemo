using System;
using UnityEngine;

public class FishPlayControllor : MonoBehaviour
{
    [SerializeField]
    public Animator FishAnimator;
    public bool WeaterOut;

    public void Start()
    {
        if (this.WeaterOut)
        {
            this.FishAnimator.SetTrigger("diaoqi");
        }
    }
}

