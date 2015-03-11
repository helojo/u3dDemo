using System;
using UnityEngine;

public class ArenaHeroStatus : MonoBehaviour
{
    public int status_id = 0x7fffffff;
    public Type type;

    public enum Type
    {
        Enemy,
        Self
    }
}

