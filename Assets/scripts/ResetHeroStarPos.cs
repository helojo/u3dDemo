using System;
using System.Collections.Generic;
using UnityEngine;

public class ResetHeroStarPos : MonoBehaviour
{
    public List<GameObject> StarList = new List<GameObject>();

    public void ResetPos(int _star)
    {
        if (_star == 1)
        {
            this.StarList[0].transform.localPosition = Vector3.zero;
        }
        else if (_star == 2)
        {
            this.StarList[0].transform.localPosition = new Vector3(-8f, 0f, 0f);
            this.StarList[1].transform.localPosition = new Vector3(8f, 0f, 0f);
        }
        else if (_star == 3)
        {
            this.StarList[0].transform.localPosition = new Vector3(-16f, 0f, 0f);
            this.StarList[1].transform.localPosition = new Vector3(0f, 0f, 0f);
            this.StarList[2].transform.localPosition = new Vector3(16f, 0f, 0f);
        }
        else if (_star == 4)
        {
            this.StarList[0].transform.localPosition = new Vector3(-24f, 0f, 0f);
            this.StarList[1].transform.localPosition = new Vector3(-8f, 0f, 0f);
            this.StarList[2].transform.localPosition = new Vector3(8f, 0f, 0f);
            this.StarList[3].transform.localPosition = new Vector3(24f, 0f, 0f);
        }
    }
}

