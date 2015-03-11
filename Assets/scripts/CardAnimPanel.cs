using System;
using UnityEngine;

public class CardAnimPanel : GUIEntity
{
    public GameObject _3DCardCamera;
    private GameObject _animObj;
    private int FakeListIndex = -1;
    private GameObject mCardCamera;

    public override void OnDestroy()
    {
        if (this.FakeListIndex > 0)
        {
            FakeCharacter.GetInstance().DestroyCharater(this.FakeListIndex);
        }
        if (this.mCardCamera != null)
        {
            UnityEngine.Object.Destroy(this.mCardCamera);
        }
        ActorData.getInstance().mRequestCallCard = false;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        Transform transform = base.transform.FindChild("AminTf");
        this.PlayEffect(transform.gameObject);
    }

    private void PlayEffect(GameObject go)
    {
        GameObject obj2 = ObjectManager.CreateObj("EffectPrefabs/ui_yingxiongzhaohuan");
        ObjectManager.CreateTempObj(obj2, Vector3.zero, 2f);
        obj2.transform.parent = go.transform;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localPosition = Vector3.zero;
    }

    public void UpdateData(int _cardEntry, int _starLv, string _talkStr)
    {
        if (this.mCardCamera != null)
        {
            UnityEngine.Object.Destroy(this.mCardCamera);
        }
        this.mCardCamera = UnityEngine.Object.Instantiate(this._3DCardCamera) as GameObject;
        Transform transform = this.mCardCamera.transform.FindChild("Model");
        if (this._animObj != null)
        {
            CardPlayer.DestroyCardPlayer(this._animObj);
        }
        this._animObj = CardPlayer.CreateCardPlayer(_cardEntry, null, CardPlayerStateType.Normal, -1);
        Vector3 localScale = this._animObj.transform.localScale;
        this._animObj.transform.parent = transform;
        this._animObj.transform.localPosition = Vector3.zero;
        this._animObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        this._animObj.transform.localScale = localScale;
        base.transform.FindChild("Label").GetComponent<UILabel>().text = _talkStr;
        Transform transform2 = base.transform.FindChild("Star");
        for (int i = 0; i < 5; i++)
        {
            int num2 = i + 1;
            UISprite component = transform2.transform.FindChild(num2.ToString()).GetComponent<UISprite>();
            component.gameObject.SetActive(i < _starLv);
            component.transform.localPosition = new Vector3((float) (i * 0x1a), 0f, 0f);
        }
        transform2.localPosition = new Vector3(transform2.localPosition.x - ((_starLv - 1) * 13), transform2.localPosition.y, 0f);
        transform2.gameObject.SetActive(true);
    }
}

