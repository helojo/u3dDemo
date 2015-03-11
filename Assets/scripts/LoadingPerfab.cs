using Holoville.HOTween;
using System;
using Toolbox;
using UnityEngine;

public class LoadingPerfab : MonoBehaviour
{
    private float curAlpha;
    private string[] loadText = new string[] { "loading1", "loading2", "loading3", "loading4" };
    public UIPanel panel;
    private int state = -1;
    private static float statrtTransStamp;
    private static LoadingPerfab trans;
    private static GameObject transSceneUI;

    public static void BeginTransition()
    {
        if (transSceneUI == null)
        {
            transSceneUI = BundleMgr.Instance.LoadResource("GUI/Prefab/LoadingPerfab", ".prefab", typeof(GameObject)) as GameObject;
        }
        if (trans == null)
        {
            trans = ((GameObject) UnityEngine.Object.Instantiate(transSceneUI)).GetComponent<LoadingPerfab>();
        }
        if (trans != null)
        {
            trans.transform.parent = UICamera.mainCamera.transform;
            trans.transform.localPosition = new Vector3(0f, 0f, 0f);
            trans.transform.localScale = Vector3.one;
            statrtTransStamp = Time.realtimeSinceStartup;
            trans.FadeIn();
        }
    }

    public static void EndTransition()
    {
        if (trans != null)
        {
            trans.FadeOut();
        }
    }

    public void FadeIn()
    {
        this.state = 1;
        this.curAlpha = 0f;
        this.panel.alpha = 0f;
    }

    public void FadeOut()
    {
        this.state = 2;
    }

    public static bool isInFadeState()
    {
        return (trans != null);
    }

    public static bool IsTransFinish()
    {
        if (trans != null)
        {
            UISlider component = trans.transform.FindChild("LoadingSlider").GetComponent<UISlider>();
            if (component != null)
            {
                bool flag = component.sliderValue > 0.99f;
                if (flag)
                {
                    Debug.Log("loading finish");
                }
                return flag;
            }
        }
        return true;
    }

    private void Start()
    {
        UITexture component = base.transform.FindChild("Texture").GetComponent<UITexture>();
        component.mainTexture = BundleMgr.Instance.CreateLoadTexture(GRandomer.RandomArray<string>(this.loadText));
        component.GetComponent<UIStretch>().uiCamera = GUIMgr.Instance.Root.transform.FindChild("TipsCamera").GetComponent<Camera>();
        Debug.Log(Screen.width + "-------------");
        Holoville.HOTween.HOTween.To(base.transform.FindChild("LoadingSlider").GetComponent<UISlider>(), 3f, "sliderValue", 1f);
        base.transform.FindChild("Tips").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetLoadingTips();
    }

    private void Update()
    {
        if (this.state != 0)
        {
            if (this.state == 1)
            {
                this.curAlpha = Mathf.Clamp01(this.curAlpha + 0.1f);
                this.panel.alpha = this.curAlpha;
                if (this.curAlpha == 1f)
                {
                    this.state = 0;
                }
            }
            else if (this.state == 2)
            {
                this.curAlpha = Mathf.Clamp01(this.curAlpha - 0.1f);
                this.panel.alpha = this.curAlpha;
                if (this.curAlpha == 0f)
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
        }
    }
}

