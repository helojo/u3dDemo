using System;
using UnityEngine;

public class GameJoySDK : MonoBehaviour
{
    public bool enableOnStart = true;
    public static GameJoySDK gameJoy;
    private AndroidJavaClass mQMiObj;
    private AndroidJavaObject playerActivityContext;

    private void Awake()
    {
        Debug.Log("GameJoy Awake");
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj == null)
        {
            Debug.Log("GameJoy: Java class not accessible from C#.");
        }
        else
        {
            this.InitializeRenderCamera("Pre");
            this.InitializeRenderCamera("Post");
        }
    }

    public int BeginDraw()
    {
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            return this.mQMiObj.CallStatic<int>("beginDraw", new object[0]);
        }
        Debug.Log("BeginDraw mQMiObj = null");
        return 0;
    }

    public int EndDraw()
    {
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            return this.mQMiObj.CallStatic<int>("endDraw", new object[0]);
        }
        Debug.Log("EndDraw mQMiObj = null");
        return 0;
    }

    private AndroidJavaObject getActivityContext()
    {
        if (this.playerActivityContext == null)
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (class2 == null)
            {
                Debug.Log("Get UnityPlayer Class failed");
                return null;
            }
            this.playerActivityContext = class2.GetStatic<AndroidJavaObject>("currentActivity");
            if (this.playerActivityContext == null)
            {
                Debug.Log("get context failed");
                return null;
            }
        }
        return this.playerActivityContext;
    }

    private void GetGameEngineType()
    {
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            string str = "Unity3D_" + Application.unityVersion;
            object[] args = new object[] { str };
            this.mQMiObj.CallStatic("setGameEngineType", args);
            Debug.Log("GetGameEngineType unityVersion = " + str);
        }
        else
        {
            Debug.Log("GetGameEngineType mQMiObj = null");
        }
    }

    public static GameJoySDK getGameJoyInstance()
    {
        if (gameJoy == null)
        {
            gameJoy = new GameJoySDK();
        }
        return gameJoy;
    }

    private void InitializeRenderCamera(string type)
    {
        if ((type.Equals("Pre") || type.Equals("Post")) && (GameObject.Find("GameJoy" + type + "Camera") == null))
        {
            GameObject target = new GameObject();
            Camera camera = (Camera) target.AddComponent("Camera");
            camera.name = "GameJoy" + type + "Camera";
            camera.clearFlags = CameraClearFlags.Nothing;
            camera.cullingMask = 0;
            if (type.Equals("Pre"))
            {
                camera.depth = float.MinValue;
            }
            else
            {
                camera.depth = float.MaxValue;
            }
            Debug.Log("InitializeRenderCamera start add gamejoy render");
            camera.gameObject.AddComponent("GameJoyAndroid" + type + "Render");
            Debug.Log("InitializeRenderCamera start SetActive");
            target.SetActive(true);
            UnityEngine.Object.DontDestroyOnLoad(target);
        }
    }

    private AndroidJavaClass mQMiObjJavaClass()
    {
        if (this.mQMiObj == null)
        {
            this.mQMiObj = new AndroidJavaClass("com.tencent.qqgamemi.QmiSdkApi");
        }
        if (this.mQMiObj == null)
        {
            Debug.Log("GameJoy: Unable to find GameJoy java class.");
        }
        return this.mQMiObj;
    }

    private void OnApplicationFocus()
    {
        Debug.Log("gamejoy start OnApplicationFocus");
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            this.mQMiObj.CallStatic("onGameEnterForeground", new object[0]);
        }
        else
        {
            Debug.Log("OnApplicationFocus mQMiObj = null");
        }
    }

    private void OnApplicationPause()
    {
        Debug.Log("gamejoy start OnApplicationPause");
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            this.mQMiObj.CallStatic("onGameEnterBackground", new object[0]);
        }
        else
        {
            Debug.Log("OnApplicationPause mQMiObj = null");
        }
    }

    private void OnApplicationQuit()
    {
        this.StopRecord();
        this.StopQMi();
    }

    public void ScollToSide()
    {
        Debug.Log("gamejoy start ScollToSide");
        if (this.mQMiObj == null)
        {
            Debug.Log("gamejoy ScollToSide mqmiobj = null");
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            this.playerActivityContext = this.getActivityContext();
            if (this.playerActivityContext == null)
            {
                Debug.Log("ScollToSide get context fail");
            }
            else
            {
                Debug.Log("gamejoy call ScollToSide");
                object[] args = new object[] { this.playerActivityContext };
                this.mQMiObj.CallStatic("scollToSide", args);
            }
        }
        else
        {
            Debug.Log("ScollToSide get mQMiObj fail");
        }
    }

    private void Start()
    {
        Debug.Log("gamejousdk start");
        if (this.enableOnStart)
        {
            Debug.Log("gamejousdk start qmi");
            this.StartQMi();
            Debug.Log("end gamejousdk start qmi");
        }
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    public void StartQMi()
    {
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            Debug.Log("gamejousdk StartQMi new qmioubj");
            this.playerActivityContext = this.getActivityContext();
            if (this.playerActivityContext == null)
            {
                Debug.Log("startqmi get context failed");
            }
            else
            {
                Debug.Log("gamejousdk StartQMi start call show qim");
                object[] args = new object[] { this.playerActivityContext, "Unity3D_412" };
                this.mQMiObj.CallStatic("showQMi", args);
                Debug.Log("gamejousdk StartQMi end call show qim");
            }
        }
        else
        {
            Debug.Log("gamejousdk StartQMi mqmiobj = null");
        }
    }

    public void StartRecord()
    {
        Debug.Log("gamejoy start StartRecord");
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            this.mQMiObj.CallStatic("onStartRecordVideo", new object[0]);
        }
        else
        {
            Debug.Log("StartRecord mQMiObj = null");
        }
    }

    public void StopQMi()
    {
        Debug.Log("gamejoy start hidQMi");
        if (this.mQMiObj == null)
        {
            Debug.Log("gamejoy hidqmi mqmiobj = null");
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            this.playerActivityContext = this.getActivityContext();
            if (this.playerActivityContext == null)
            {
                Debug.Log("stop qmi get context fail");
            }
            else
            {
                Debug.Log("gamejoy call hidQMi");
                object[] args = new object[] { this.playerActivityContext };
                this.mQMiObj.CallStatic("hideQMi", args);
            }
        }
        else
        {
            Debug.Log("StopQMi get mQMiObj fail");
        }
    }

    public void StopRecord()
    {
        Debug.Log("gamejoy start StopRecord");
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            this.mQMiObj.CallStatic("onStopRecordVideo", new object[0]);
        }
        else
        {
            Debug.Log("StopRecord mQMiObj = null");
        }
    }

    public void Update()
    {
        if (this.mQMiObj == null)
        {
            this.mQMiObj = this.mQMiObjJavaClass();
        }
        if (this.mQMiObj != null)
        {
            this.mQMiObj.CallStatic("onUpdateVideoFrame", new object[0]);
        }
        else
        {
            Debug.Log("Update mQMiObj = null");
        }
    }
}

