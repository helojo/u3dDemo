using HutongGames.PlayMaker;
using System;
using UnityEngine;

public class TipsDiag : MonoBehaviour
{
    public static void PushText(string text)
    {
        ShowTextByType(text, false, Vector2.zero);
    }

    public static void PushText(string text, Vector2 _pos)
    {
        ShowTextByType(text, false, _pos);
    }

    public static void SetText(string text)
    {
        ShowTextByType(text, true, Vector2.zero);
    }

    public static void ShowTextByType(string text, bool isNormal, Vector2 _pos)
    {
        GameObject obj2 = ObjectManager.CreateObj("Tips/TipsPrefab");
        if (obj2 != null)
        {
            obj2.transform.parent = GameObject.Find("UI Root/Camera").transform;
            obj2.transform.localScale = Vector3.one;
            PlayMakerFSM component = obj2.GetComponent<PlayMakerFSM>();
            if (component != null)
            {
                FsmString str = component.FsmVariables.FindFsmString("Content");
                FsmVector3 vector = component.FsmVariables.FindFsmVector3("FromPos");
                FsmVector3 vector2 = component.FsmVariables.FindFsmVector3("ToPos");
                if (!isNormal)
                {
                    Vector3 vector3 = new Vector3(_pos.x, _pos.y, 0f);
                    vector.Value = vector3;
                    vector2.Value = new Vector3(vector3.x, vector3.y + 100f, 0f);
                }
                UILabel label = obj2.transform.FindChild("Label").GetComponent<UILabel>();
                label.text = text;
                UISprite sprite = obj2.transform.FindChild("Background").GetComponent<UISprite>();
                UIWidget widget = label.GetComponent<UIWidget>();
                sprite.height = widget.height + 20;
                if (sprite.height < 0x52)
                {
                    sprite.height = 0x52;
                }
                component.SendEvent(!isNormal ? "CALL_ROLL_INFO_EVENT" : "CALL_TIPS_EVENT");
            }
        }
    }
}

