using System;
using System.Collections;

public class PlayMakerHashTableProxy : PlayMakerCollectionProxy
{
    public Hashtable _hashTable;
    private Hashtable _snapShot;

    public void Awake()
    {
        this._hashTable = new Hashtable();
        this.PreFillHashTable();
        this.TakeSnapShot();
    }

    public void InspectorEdit(int index)
    {
        base.dispatchEvent(base.setEvent, index, "int");
    }

    public bool isCollectionDefined()
    {
        return (this.hashTable != null);
    }

    private void PreFillHashTable()
    {
        for (int i = 0; i < base.preFillKeyList.Count; i++)
        {
            switch (base.preFillType)
            {
                case PlayMakerCollectionProxy.VariableEnum.GameObject:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillGameObjectList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Int:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillIntList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Float:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillFloatList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.String:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillStringList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Bool:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillBoolList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Vector3:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillVector3List[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Rect:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillRectList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Quaternion:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillQuaternionList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Color:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillColorList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Material:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillMaterialList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Texture:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillTextureList[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.Vector2:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillVector2List[i];
                    break;

                case PlayMakerCollectionProxy.VariableEnum.AudioClip:
                    this.hashTable[base.preFillKeyList[i]] = base.preFillAudioClipList[i];
                    break;
            }
        }
    }

    public void RevertToSnapShot()
    {
        this._hashTable = new Hashtable();
        IEnumerator enumerator = this._snapShot.Keys.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                this._hashTable[current] = this._snapShot[current];
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public void TakeSnapShot()
    {
        this._snapShot = new Hashtable();
        IEnumerator enumerator = this._hashTable.Keys.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                this._snapShot[current] = this._hashTable[current];
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public Hashtable hashTable
    {
        get
        {
            return this._hashTable;
        }
    }
}

