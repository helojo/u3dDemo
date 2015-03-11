using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MTD/SceneNamedPos")]
public class SceneNamedPos : MonoBehaviour
{
    private Dictionary<string, ScenePathSlot> pathDict = new Dictionary<string, ScenePathSlot>();
    public ScenePathSlot[] pathSlots = new ScenePathSlot[1];
    private Dictionary<string, ScenePosSlot> posDict = new Dictionary<string, ScenePosSlot>();
    public ScenePosSlot[] slots = new ScenePosSlot[1];

    private void Awake()
    {
        foreach (ScenePosSlot slot in this.slots)
        {
            this.posDict.Add(slot.name, slot);
        }
        if (this.pathSlots != null)
        {
            foreach (ScenePathSlot slot2 in this.pathSlots)
            {
                if (slot2 != null)
                {
                    this.pathDict.Add(slot2.name, slot2);
                }
            }
        }
    }

    public List<Vector3> GetScenePath(string name)
    {
        ScenePathSlot slot;
        if (this.pathDict.TryGetValue(name, out slot))
        {
            return slot.nodes;
        }
        return null;
    }

    public Vector3 GetScenePathLastPos(string name)
    {
        ScenePathSlot slot;
        if (this.pathDict.TryGetValue(name, out slot))
        {
            return slot.nodes[slot.nodes.Count - 1];
        }
        return Vector3.zero;
    }

    public ScenePosSlot GetScenePosSlot(string name)
    {
        ScenePosSlot slot;
        if (this.posDict.TryGetValue(name, out slot))
        {
            return slot;
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (ScenePosSlot slot in this.slots)
        {
            Gizmos.DrawWireCube(slot.pos, Vector3.one);
        }
        foreach (ScenePathSlot slot2 in this.pathSlots)
        {
            iTween.DrawPathGizmos(slot2.nodes.ToArray(), Color.blue);
            foreach (Vector3 vector in slot2.nodes)
            {
                Gizmos.DrawWireCube(vector, Vector3.one);
            }
        }
    }

    [Serializable]
    public class ScenePathSlot
    {
        public string name;
        public List<Vector3> nodes = new List<Vector3> { Vector3.zero, Vector3.zero };
    }

    [Serializable]
    public class ScenePosSlot
    {
        public string name;
        public Vector3 pos = Vector3.zero;
        public Quaternion rot = Quaternion.identity;
    }
}

