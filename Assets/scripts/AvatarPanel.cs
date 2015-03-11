using FastBuf;
using System;
using UnityEngine;

public class AvatarPanel : GUIEntity
{
    private int cardEntry;
    private int objIndex = -1;
    private bool showSkill;

    public override void OnDestroy()
    {
        Debug.Log("OnDestroy");
        FakeCharacter.GetInstance().DestroyCharater(this.objIndex);
    }

    public override void OnInitialize()
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!this.showSkill)
        {
            GameObject obj2 = FakeCharacter.GetInstance().FindFakeObjByIndex(this.objIndex);
            if (obj2 != null)
            {
                Transform transform = obj2.transform.parent.FindChild("target");
                if (transform == null)
                {
                    GameObject obj3 = new GameObject("target") {
                        transform = { parent = obj2.transform.parent, localPosition = new Vector3(-2.68f, 0.499f, -3.97f) }
                    };
                    new GameObject("Body") { transform = { parent = obj3.transform } }.transform.localPosition = Vector3.one;
                    transform = obj3.transform;
                    obj3.AddComponent<HangControler>();
                }
                this.showSkill = true;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(this.cardEntry);
                if (_config != null)
                {
                    skill_config _config2 = ConfigMgr.getInstance().getByEntry<skill_config>(_config.default_normal_skill);
                    if (_config2 != null)
                    {
                        RealTimeShowEffect.CreateNewShowEffect(_config2.effects, obj2, transform.gameObject).ToDoStart();
                    }
                }
            }
        }
    }

    public void SetModel(int _cardEntry)
    {
        this.cardEntry = _cardEntry;
        UITexture component = base.gameObject.transform.FindChild("Character").GetComponent<UITexture>();
        component.width = 0x470;
        component.height = 640;
        this.objIndex = FakeCharacter.GetInstance().SnapCardCharacter(this.cardEntry, 0, null, ModelViewType.side, component, 1);
    }
}

