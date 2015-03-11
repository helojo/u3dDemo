using System;
using UnityEngine;

public class LookPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(base.transform.position, 0.25f);
    }
}

