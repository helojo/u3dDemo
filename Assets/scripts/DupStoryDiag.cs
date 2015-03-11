using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DupStoryDiag : GUIEntity
{
    public Transform _SelfGroup;
    public Transform _TargetGroup;
    private AudioSource curSound;
    private int mCurrDiagIdx;
    private List<duplicate_dialog_config> mDiagList = new List<duplicate_dialog_config>();
    private UIEventListener.VoidDelegate mOkCallBack;
    private int mSelfFakeListIndex = -1;
    private int mTargetFakeListIndex = -1;

    private int CreateModel(int _cardEntry, UITexture _Texture)
    {
        return FakeCharacter.GetInstance().SnapCardCharacter(_cardEntry, 0, null, ModelViewType.half, _Texture);
    }

    private int CreateModel(int cardEntry, int cardQuality, UITexture _Texture)
    {
        return FakeCharacter.GetInstance().SnapCardCharacter(cardEntry, cardQuality, null, ModelViewType.side, _Texture);
    }

    private void OnClickOkBtn(GameObject go)
    {
        this.mCurrDiagIdx++;
        if (this.mCurrDiagIdx < this.mDiagList.Count)
        {
            this.SetDiagInfo(this.mDiagList[this.mCurrDiagIdx]);
        }
        else if (this.mOkCallBack != null)
        {
            this.mOkCallBack(base.gameObject);
        }
    }

    public override void OnDestroy()
    {
        if (this.mSelfFakeListIndex > 0)
        {
            FakeCharacter.GetInstance().DestroyCharater(this.mSelfFakeListIndex);
        }
        if (this.mTargetFakeListIndex > 0)
        {
            FakeCharacter.GetInstance().DestroyCharater(this.mTargetFakeListIndex);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(base.transform.FindChild("Next").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOkBtn);
    }

    private void SetDiagInfo(duplicate_dialog_config dcc)
    {
        Transform transform = (dcc.type != 0) ? this._TargetGroup : this._SelfGroup;
        this._SelfGroup.gameObject.SetActive(dcc.type == 0);
        this._TargetGroup.gameObject.SetActive(dcc.type == 1);
        Transform transform2 = transform.FindChild("Hero");
        UITexture component = transform.FindChild("Icon").GetComponent<UITexture>();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(dcc.card_entry);
        if (_config != null)
        {
            component.gameObject.SetActive(false);
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            UITexture texture2 = transform.FindChild("Hero/Character").GetComponent<UITexture>();
            if (dcc.type == 0)
            {
                if (this.mSelfFakeListIndex > 0)
                {
                    FakeCharacter.GetInstance().DestroyCharater(this.mSelfFakeListIndex);
                }
                this.mSelfFakeListIndex = this.CreateModel(_config.entry, _config.quality, texture2);
            }
            else
            {
                if (this.mTargetFakeListIndex > 0)
                {
                    FakeCharacter.GetInstance().DestroyCharater(this.mTargetFakeListIndex);
                }
                this.mTargetFakeListIndex = this.CreateModel(_config.entry, _config.quality, texture2);
            }
            transform2.gameObject.SetActive(true);
        }
        else
        {
            transform2.gameObject.SetActive(false);
            component.gameObject.SetActive(false);
        }
        transform.FindChild("TalkLabel").GetComponent<UILabel>().text = dcc.dialog_content;
        this.StopSound();
        this.curSound = SoundManager.mInstance.PlaySFX(dcc.audioRes_id);
    }

    private void StopSound()
    {
        if (this.curSound != null)
        {
            this.curSound.Stop();
            this.curSound = null;
        }
    }

    public void UpdateData(int dialogId, UIEventListener.VoidDelegate _OkCallBack)
    {
        this.mOkCallBack = _OkCallBack;
        ArrayList list = ConfigMgr.getInstance().getList<duplicate_dialog_config>();
        this.mDiagList.Clear();
        this.mCurrDiagIdx = 0;
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                duplicate_dialog_config current = (duplicate_dialog_config) enumerator.Current;
                if (current.dia_id == dialogId)
                {
                    this.mDiagList.Add(current);
                }
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
        if (this.mDiagList.Count > 0)
        {
            this.SetDiagInfo(this.mDiagList[0]);
        }
    }
}

