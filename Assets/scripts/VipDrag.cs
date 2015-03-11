using System;
using UnityEngine;

public class VipDrag : MonoBehaviour
{
    private void OnDrag(Vector2 delta)
    {
        VipDescriptGraft.inst.canClose = false;
        VipDescriptGraft.inst.enabled = true;
    }
}

