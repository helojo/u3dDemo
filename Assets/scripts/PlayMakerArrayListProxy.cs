using System;
using System.Collections;
using System.Runtime.InteropServices;

public class PlayMakerArrayListProxy : PlayMakerCollectionProxy
{
    public ArrayList _arrayList;
    private ArrayList _snapShot;

    public void Add(object value, string type, bool silent = false)
    {
        this.arrayList.Add(value);
        if (!silent)
        {
            base.dispatchEvent(base.addEvent, value, type);
        }
    }

    public int AddRange(ICollection collection, string type)
    {
        this.arrayList.AddRange(collection);
        return this.arrayList.Count;
    }

    public void Awake()
    {
        this._arrayList = new ArrayList();
        this.PreFillArrayList();
        this.TakeSnapShot();
    }

    public void InspectorEdit(int index)
    {
        base.dispatchEvent(base.setEvent, index, "int");
    }

    public bool isCollectionDefined()
    {
        return (this.arrayList != null);
    }

    private void PreFillArrayList()
    {
        switch (base.preFillType)
        {
            case PlayMakerCollectionProxy.VariableEnum.GameObject:
                this.arrayList.InsertRange(0, base.preFillGameObjectList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Int:
                this.arrayList.InsertRange(0, base.preFillIntList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Float:
                this.arrayList.InsertRange(0, base.preFillFloatList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.String:
                this.arrayList.InsertRange(0, base.preFillStringList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Bool:
                this.arrayList.InsertRange(0, base.preFillBoolList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Vector3:
                this.arrayList.InsertRange(0, base.preFillVector3List);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Rect:
                this.arrayList.InsertRange(0, base.preFillRectList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Quaternion:
                this.arrayList.InsertRange(0, base.preFillQuaternionList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Color:
                this.arrayList.InsertRange(0, base.preFillColorList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Material:
                this.arrayList.InsertRange(0, base.preFillMaterialList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Texture:
                this.arrayList.InsertRange(0, base.preFillTextureList);
                break;

            case PlayMakerCollectionProxy.VariableEnum.Vector2:
                this.arrayList.InsertRange(0, base.preFillVector2List);
                break;

            case PlayMakerCollectionProxy.VariableEnum.AudioClip:
                this.arrayList.InsertRange(0, base.preFillAudioClipList);
                break;
        }
    }

    public bool Remove(object value, string type, bool silent = false)
    {
        if (!this.arrayList.Contains(value))
        {
            return false;
        }
        this.arrayList.Remove(value);
        if (!silent)
        {
            base.dispatchEvent(base.removeEvent, value, type);
        }
        return true;
    }

    public void RevertToSnapShot()
    {
        this._arrayList = new ArrayList();
        this._arrayList.AddRange(this._snapShot);
    }

    public void Set(int index, object value, string type)
    {
        this.arrayList[index] = value;
        base.dispatchEvent(base.setEvent, index, "int");
    }

    public void TakeSnapShot()
    {
        this._snapShot = new ArrayList();
        this._snapShot.AddRange(this._arrayList);
    }

    public ArrayList arrayList
    {
        get
        {
            return this._arrayList;
        }
    }
}

