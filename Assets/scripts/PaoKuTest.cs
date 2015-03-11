using System;
using UnityEngine;

public class PaoKuTest : MonoBehaviour
{
    public AnimFSM animFsm;
    private CharacterFSMManager chaFSMManager;

    private void Awake()
    {
        this.Init();
    }

    private void Init()
    {
        AnimFSMInfoManager.Instance();
        this.chaFSMManager = CharacterFSMManager.Instance();
    }

    private void LateUpdate()
    {
    }

    private void Start()
    {
        this.animFsm.SetStateTable("paoku");
        Debug.Log(this.animFsm.PlayAnim("move01", 1f, 0f, true));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(this.animFsm.PlayAnim(this.chaFSMManager.GetStateInfo(CharacterType.ATTACK).animName, 1f, 0f, false));
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(this.animFsm.PlayAnim("fangyu", 1f, 0f, false));
        }
    }
}

