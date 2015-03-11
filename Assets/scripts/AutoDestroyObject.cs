using System;
using UnityEngine;

public class AutoDestroyObject : MonoBehaviour
{
    public object m_data;
    public Predicate<AutoDestroyObject> m_match;

    private void Update()
    {
        if ((this.m_match == null) || this.m_match(this))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }
}

