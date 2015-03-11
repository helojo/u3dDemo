using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParkourMap
{
    public GameObject mapObj;
    public GameObject obstacleObj;
    public List<PropInfo> props = new List<PropInfo>();

    public ParkourMap(GameObject _mapObj, GameObject _obstacleObj, int mapCount, int mapOffect)
    {
        this.mapObj = UnityEngine.Object.Instantiate(_mapObj, new Vector3(0f, 0f, (float) ((200 * mapCount) + mapOffect)), Quaternion.identity) as GameObject;
        this.mapObj.name = this.mapObj.name + "|" + mapCount;
        this.obstacleObj = _obstacleObj;
    }

    public void DestoryMapObj()
    {
        this.props.Clear();
        UnityEngine.Object.Destroy(this.mapObj);
        UnityEngine.Object.Destroy(this.obstacleObj);
    }

    public void SetActive(bool isActive)
    {
        this.mapObj.SetActive(isActive);
        this.obstacleObj.SetActive(isActive);
    }
}

