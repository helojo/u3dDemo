using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class PlayMakerCollectionProxy : MonoBehaviour
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map4;
    public string addEvent;
    public bool condensedView;
    public int contentPreviewMaxRows = 10;
    public int contentPreviewStartIndex;
    public bool enablePlayMakerEvents;
    public bool liveUpdate;
    public List<AudioClip> preFillAudioClipList = new List<AudioClip>();
    public List<bool> preFillBoolList = new List<bool>();
    public List<Color> preFillColorList = new List<Color>();
    public int preFillCount;
    public List<float> preFillFloatList = new List<float>();
    public List<GameObject> preFillGameObjectList = new List<GameObject>();
    public List<int> preFillIntList = new List<int>();
    public List<string> preFillKeyList = new List<string>();
    public List<Material> preFillMaterialList = new List<Material>();
    public List<UnityEngine.Object> preFillObjectList = new List<UnityEngine.Object>();
    public int preFillObjectTypeIndex;
    public List<Quaternion> preFillQuaternionList = new List<Quaternion>();
    public List<Rect> preFillRectList = new List<Rect>();
    public List<string> preFillStringList = new List<string>();
    public List<Texture2D> preFillTextureList = new List<Texture2D>();
    public VariableEnum preFillType;
    public List<Vector2> preFillVector2List = new List<Vector2>();
    public List<Vector3> preFillVector3List = new List<Vector3>();
    public string referenceName = string.Empty;
    public string removeEvent;
    public string setEvent;
    public bool showContent;
    public bool showEvents;
    public bool TextureElementSmall;

    protected PlayMakerCollectionProxy()
    {
    }

    public void cleanPrefilledLists()
    {
        if (this.preFillKeyList.Count > this.preFillCount)
        {
            this.preFillKeyList.RemoveRange(this.preFillCount, this.preFillKeyList.Count - this.preFillCount);
        }
        if (this.preFillBoolList.Count > this.preFillCount)
        {
            this.preFillBoolList.RemoveRange(this.preFillCount, this.preFillBoolList.Count - this.preFillCount);
        }
        if (this.preFillColorList.Count > this.preFillCount)
        {
            this.preFillColorList.RemoveRange(this.preFillCount, this.preFillColorList.Count - this.preFillCount);
        }
        if (this.preFillFloatList.Count > this.preFillCount)
        {
            this.preFillFloatList.RemoveRange(this.preFillCount, this.preFillFloatList.Count - this.preFillCount);
        }
        if (this.preFillIntList.Count > this.preFillCount)
        {
            this.preFillIntList.RemoveRange(this.preFillCount, this.preFillIntList.Count - this.preFillCount);
        }
        if (this.preFillMaterialList.Count > this.preFillCount)
        {
            this.preFillMaterialList.RemoveRange(this.preFillCount, this.preFillMaterialList.Count - this.preFillCount);
        }
        if (this.preFillGameObjectList.Count > this.preFillCount)
        {
            this.preFillGameObjectList.RemoveRange(this.preFillCount, this.preFillGameObjectList.Count - this.preFillCount);
        }
        if (this.preFillObjectList.Count > this.preFillCount)
        {
            this.preFillObjectList.RemoveRange(this.preFillCount, this.preFillObjectList.Count - this.preFillCount);
        }
        if (this.preFillQuaternionList.Count > this.preFillCount)
        {
            this.preFillQuaternionList.RemoveRange(this.preFillCount, this.preFillQuaternionList.Count - this.preFillCount);
        }
        if (this.preFillRectList.Count > this.preFillCount)
        {
            this.preFillRectList.RemoveRange(this.preFillCount, this.preFillRectList.Count - this.preFillCount);
        }
        if (this.preFillStringList.Count > this.preFillCount)
        {
            this.preFillStringList.RemoveRange(this.preFillCount, this.preFillStringList.Count - this.preFillCount);
        }
        if (this.preFillTextureList.Count > this.preFillCount)
        {
            this.preFillTextureList.RemoveRange(this.preFillCount, this.preFillTextureList.Count - this.preFillCount);
        }
        if (this.preFillVector2List.Count > this.preFillCount)
        {
            this.preFillVector2List.RemoveRange(this.preFillCount, this.preFillVector2List.Count - this.preFillCount);
        }
        if (this.preFillVector3List.Count > this.preFillCount)
        {
            this.preFillVector3List.RemoveRange(this.preFillCount, this.preFillVector3List.Count - this.preFillCount);
        }
        if (this.preFillAudioClipList.Count > this.preFillCount)
        {
            this.preFillAudioClipList.RemoveRange(this.preFillCount, this.preFillAudioClipList.Count - this.preFillCount);
        }
    }

    internal void dispatchEvent(string anEvent, object value, string type)
    {
        if (this.enablePlayMakerEvents)
        {
            string key = type;
            if (key != null)
            {
                int num;
                if (<>f__switch$map4 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(13);
                    dictionary.Add("bool", 0);
                    dictionary.Add("color", 1);
                    dictionary.Add("float", 2);
                    dictionary.Add("gameObject", 3);
                    dictionary.Add("int", 4);
                    dictionary.Add("material", 5);
                    dictionary.Add("object", 6);
                    dictionary.Add("quaternion", 7);
                    dictionary.Add("rect", 8);
                    dictionary.Add("string", 9);
                    dictionary.Add("texture", 10);
                    dictionary.Add("vector2", 11);
                    dictionary.Add("vector3", 12);
                    <>f__switch$map4 = dictionary;
                }
                if (<>f__switch$map4.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            Fsm.EventData.BoolData = (bool) value;
                            break;

                        case 1:
                            Fsm.EventData.ColorData = (Color) value;
                            break;

                        case 2:
                            Fsm.EventData.FloatData = (float) value;
                            break;

                        case 3:
                            Fsm.EventData.ObjectData = (GameObject) value;
                            break;

                        case 4:
                            Fsm.EventData.IntData = (int) value;
                            break;

                        case 5:
                            Fsm.EventData.MaterialData = (Material) value;
                            break;

                        case 6:
                            Fsm.EventData.ObjectData = (UnityEngine.Object) value;
                            break;

                        case 7:
                            Fsm.EventData.QuaternionData = (Quaternion) value;
                            break;

                        case 8:
                            Fsm.EventData.RectData = (Rect) value;
                            break;

                        case 9:
                            Fsm.EventData.StringData = (string) value;
                            break;

                        case 10:
                            Fsm.EventData.TextureData = (Texture) value;
                            break;

                        case 11:
                            Fsm.EventData.Vector3Data = (Vector3) value;
                            break;

                        case 12:
                            Fsm.EventData.Vector3Data = (Vector3) value;
                            break;
                    }
                }
            }
            FsmEventTarget eventTarget = new FsmEventTarget {
                target = FsmEventTarget.EventTarget.BroadcastAll
            };
            List<Fsm> list = new List<Fsm>(Fsm.FsmList);
            if (list.Count > 0)
            {
                list[0].Event(eventTarget, anEvent);
            }
        }
    }

    internal string getFsmVariableType(VariableType _type)
    {
        return _type.ToString();
    }

    public enum VariableEnum
    {
        GameObject,
        Int,
        Float,
        String,
        Bool,
        Vector3,
        Rect,
        Quaternion,
        Color,
        Material,
        Texture,
        Vector2,
        AudioClip
    }
}

