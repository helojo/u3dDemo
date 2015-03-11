using System;
using UnityEngine;

public class VipRemainTime : MonoBehaviour
{
    [HideInInspector]
    public bool isSuperVip;

    private void FixedUpdate()
    {
        int seconds = !this.isSuperVip ? ActorData.getInstance().VipRemainTime : ActorData.getInstance().SuperVipRemainTime;
        UILabel component = base.gameObject.GetComponent<UILabel>();
        if (null != component)
        {
            component.text = VipCardPanel.RemainTimeToString(seconds);
        }
    }
}

