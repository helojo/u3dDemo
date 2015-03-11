using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

public class GUIPersistence
{
    private GUIEntity _entity;
    private BinaryFormatter formatter = new BinaryFormatter();
    public int id = -1;
    public int idGroup = -1;
    public string name;
    private MemoryStream stream = new MemoryStream();
    public bool uniqueness;

    public void Deserialization<T>(out T obj)
    {
        if ((this.stream.Length > 0L) && (this.stream.Position < this.stream.Length))
        {
            obj = (T) this.formatter.Deserialize(this.stream);
        }
        else
        {
            obj = default(T);
        }
    }

    public void Fetch()
    {
        GUIMgr.Instance.FetchGUIEntity(this);
    }

    public void Reset()
    {
        this.Seek2Begin();
        this.stream.SetLength(0L);
    }

    public void Seek2Begin()
    {
        this.stream.Seek(0L, SeekOrigin.Begin);
    }

    public void Serialization<T>(T obj)
    {
        this.formatter.Serialize(this.stream, obj);
    }

    public GUIEntity entity
    {
        get
        {
            return this._entity;
        }
        set
        {
            this._entity = value;
            if (null != this._entity)
            {
                this.idGroup = this._entity.groupID;
            }
        }
    }
}

