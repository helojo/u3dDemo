using System;
using UnityEngine;

public class TransSceneUI : MonoBehaviour
{
    private float curAlpha;
    public UIPanel panel;
    private int state = -1;
    private static float statrtTransStamp;
    private static TransSceneUI trans;
    private static GameObject transSceneUI;

    private void battlePanelNvqi(bool isShow)
    {
        int num = !isShow ? 0 : 0xfa0;
        Debug.Log("battlePanelNvqi" + num);
        for (int i = 1; i <= 6; i++)
        {
            GameObject obj2 = GameObject.Find("/UI Root/Camera/BattlePanel/BottomCenter/Group/Pos" + i + "/nuqi");
            if (obj2 != null)
            {
                obj2.GetComponent<RenderQueueSetter>().ChangeQueue(num);
            }
        }
    }

    public static void BeginTransition()
    {
        if (transSceneUI == null)
        {
            transSceneUI = BundleMgr.Instance.LoadResource("GUI/Prefab/TransSceneUI", ".prefab", typeof(GameObject)) as GameObject;
        }
        if (trans == null)
        {
            trans = ((GameObject) UnityEngine.Object.Instantiate(transSceneUI)).GetComponent<TransSceneUI>();
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
        UISlider component = base.gameObject.transform.FindChild("Top/Battery").GetComponent<UISlider>();
        this.battlePanelNvqi(false);
    }

    public void FadeOut()
    {
        this.state = 2;
    }

    private void FixedUpdate()
    {
        if (this.state != 0)
        {
            if (this.state == 1)
            {
                this.curAlpha = Mathf.Clamp01(this.curAlpha + Time.deltaTime);
                this.panel.alpha = this.curAlpha;
                if (this.curAlpha == 1f)
                {
                    this.state = 0;
                }
            }
            else if (this.state == 2)
            {
                this.curAlpha = Mathf.Clamp01(this.curAlpha - Time.deltaTime);
                this.panel.alpha = this.curAlpha;
                if (this.curAlpha == 0f)
                {
                    this.battlePanelNvqi(true);
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
        }
    }

    public static bool isInFadeState()
    {
        return (trans != null);
    }

    private void Start()
    {
        base.transform.FindChild("Tips").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(UnityEngine.Random.Range(0x2328, 0x2332));
    }
}

