using System;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private GameObject attachObj;
    private Vector3 offset;

    public void AttachToObj(GameObject BattleFighterObj)
    {
        if (BattleFighterObj == null)
        {
            base.gameObject.SetActive(false);
        }
        else
        {
            GameObject obj2 = BattleGlobalFunc.DeepFindChildObjectByName(BattleFighterObj, "Top");
            if (obj2 != null)
            {
                this.attachObj = obj2;
                this.offset.y = 0.2f;
            }
            else
            {
                this.attachObj = BattleFighterObj;
                this.offset.y = 2.2f;
            }
        }
    }

    public void Update()
    {
        if (this.attachObj != null)
        {
            Vector3 pos = this.attachObj.transform.position + this.offset;
            base.transform.localPosition = BattleGlobalFunc.WorldToGUI(pos);
        }
    }
}

