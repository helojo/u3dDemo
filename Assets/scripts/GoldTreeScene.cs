using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GoldTreeScene : MonoBehaviour
{
    public GameObject _HongBaoBg;
    public GameObject _HongBaoGroup;
    public GameObject _HongBaoTx;
    [SerializeField]
    public Animator AnimatorControllor;
    public Transform[] bones;
    public Transform camearBone;
    public RenderTexture CurrentTexture;
    [SerializeField]
    public Camera goldTree;
    public GameObject ShakeOnce;
    public GameObject ShakeOnceCamear;
    public GameObject ShakeTen;
    public GameObject ShakeTenCamear;

    public void Actived(bool flag)
    {
        base.gameObject.SetActive(flag);
        if (flag)
        {
            this.ShakeOnce.SetActive(false);
            this.ShakeTen.SetActive(false);
            this.ShakeTenCamear.SetActive(false);
            this.ShakeOnceCamear.SetActive(false);
        }
    }

    private void Awake()
    {
        Current = this;
        this.CurrentTexture = new RenderTexture(540, 310, 1, RenderTextureFormat.ARGB32);
        this.Actived(false);
    }

    public void DoShakeOnce()
    {
        this.AnimatorControllor.SetTrigger("free01");
        this.ShowEffect(this.ShakeOnce, 2f);
        this.ShowCamear(this.ShakeOnceCamear, 2f);
    }

    public void DoShakeTen()
    {
        this.AnimatorControllor.SetTrigger("free");
        this.ShowEffect(this.ShakeTen, 3f);
        this.ShowCamear(this.ShakeTenCamear, 3f);
    }

    private void OnDestory()
    {
        Current = null;
    }

    [DebuggerHidden]
    private IEnumerator PlayHongBaoEffect()
    {
        return new <PlayHongBaoEffect>c__Iterator80 { <>f__this = this };
    }

    public void PlayHongBaoTx()
    {
        base.StartCoroutine(this.PlayHongBaoEffect());
    }

    public void SetHongBaoScene(bool showHongBaoBg)
    {
        this._HongBaoBg.gameObject.SetActive(showHongBaoBg);
        this._HongBaoGroup.gameObject.SetActive(showHongBaoBg);
        if (!showHongBaoBg)
        {
            this._HongBaoTx.gameObject.SetActive(false);
        }
    }

    public void ShowCamear(GameObject obj, float time)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate(obj) as GameObject;
        obj2.transform.parent = this.camearBone;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        obj2.SetActive(true);
        obj2.AddComponent<AutoDelayDestory>().delay = time;
    }

    private void ShowEffect(GameObject obj, float time)
    {
        foreach (Transform transform in this.bones)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(obj) as GameObject;
            obj2.transform.parent = transform;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localScale = Vector3.one;
            obj2.transform.localRotation = Quaternion.identity;
            obj2.SetActive(true);
            obj2.AddComponent<AutoDelayDestory>().delay = time;
        }
    }

    private void Start()
    {
        if (this.goldTree != null)
        {
            this.goldTree.targetTexture = this.CurrentTexture;
        }
    }

    private void Update()
    {
    }

    public static GoldTreeScene Current
    {
        [CompilerGenerated]
        get
        {
            return <Current>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            <Current>k__BackingField = value;
        }
    }

    [CompilerGenerated]
    private sealed class <PlayHongBaoEffect>c__Iterator80 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GoldTreeScene <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this._HongBaoTx.gameObject.SetActive(true);
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 1;
                    goto Label_0089;

                case 1:
                    this.<>f__this._HongBaoTx.gameObject.SetActive(false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0089;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0089:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

