using System;
using UnityEngine;

public class LookTarget : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(base.transform.position, 0.25f);
    }
}

