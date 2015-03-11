using System;
using UnityEngine;

public class GameStateMgr : MonoBehaviour
{
    private PlayMakerFSM fsmRoot;
    private static GameStateMgr instance;
    public static bool IsGameReturnLogin;
    public static bool IsGameReturnLogout;

    public void ChangeState(string transition)
    {
        this.ChangeStateWithParameter(transition, null);
    }

    public void ChangeStateWithParameter(string transition, string serl_para)
    {
        this.RootFSM.FsmVariables.FindFsmString("TransitionTrigger").Value = transition;
        this.RootFSM.FsmVariables.FindFsmString("TransitionParameter").Value = serl_para;
    }

    public void OnDestory()
    {
        instance = null;
        Debug.Log("GameStateMgr");
    }

    public static GameStateMgr Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject target = new GameObject {
                    name = "GameStateMgr_Container"
                };
                UnityEngine.Object.DontDestroyOnLoad(target);
                instance = target.AddComponent<GameStateMgr>();
            }
            return instance;
        }
    }

    private PlayMakerFSM RootFSM
    {
        get
        {
            if (null == this.fsmRoot)
            {
                GameObject obj2 = GameObject.Find("UI Root");
                if (null != obj2)
                {
                    this.fsmRoot = obj2.GetComponent<PlayMakerFSM>();
                }
            }
            return this.fsmRoot;
        }
    }
}

