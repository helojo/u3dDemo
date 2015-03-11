using HutongGames.PlayMaker;
using System;
using System.Runtime.CompilerServices;

public static class PlayMakerUtilsDotNetExtensions
{
    public static bool Contains(this VariableType[] target, VariableType vType)
    {
        if (target != null)
        {
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] == vType)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

