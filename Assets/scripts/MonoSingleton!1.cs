using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T m_Instance;

    protected MonoSingleton()
    {
    }

    private void Awake()
    {
        if (MonoSingleton<T>.m_Instance == null)
        {
            MonoSingleton<T>.m_Instance = this as T;
        }
    }

    public virtual void Init()
    {
    }

    private void OnApplicationQuit()
    {
        MonoSingleton<T>.m_Instance = null;
    }

    public static T instance
    {
        get
        {
            if (MonoSingleton<T>.m_Instance == null)
            {
                MonoSingleton<T>.m_Instance = UnityEngine.Object.FindObjectOfType(typeof(T)) as T;
                if (MonoSingleton<T>.m_Instance == null)
                {
                    System.Type[] components = new System.Type[] { typeof(T) };
                    MonoSingleton<T>.m_Instance = new GameObject("Singleton of " + typeof(T).ToString(), components).GetComponent<T>();
                    MonoSingleton<T>.m_Instance.Init();
                }
            }
            return MonoSingleton<T>.m_Instance;
        }
    }
}

