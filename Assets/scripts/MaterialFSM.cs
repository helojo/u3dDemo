using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaterialFSM : MonoBehaviour
{
    [CompilerGenerated]
    private static Action<GameObject> <>f__am$cache12;
    public static readonly string AlphaMaterialFSMName = "alpha";
    private float alphaSmoothTime;
    private float colorSmoothTime;
    private Color colorV = Color.black;
    private Color curColor = Color.white;
    private Shader curShader;
    private MaterialFSMInfo curState;
    private List<GameObject> disableObjs = new List<GameObject>();
    private MateralFSMInfoTable info;
    private bool isOnlyCacheOnce;
    private bool isTempChanging;
    public static readonly string MainColorName = "_Color";
    public static readonly string MaterialBeHurt = "behurt";
    private MaterialFSMInfo oldState;
    public static readonly string ReflectColorName = "_ReflectColor";
    private Dictionary<Renderer, RendererMaterialInitInfo> rendererInitInfoes = new Dictionary<Renderer, RendererMaterialInitInfo>();
    private Color targetColor = Color.white;
    public static readonly string TintColorName = "_TintColor";

    private void AddRendererInitInfo(Renderer _renderer)
    {
        if (!this.rendererInitInfoes.ContainsKey(_renderer))
        {
            RendererMaterialInitInfo info = new RendererMaterialInitInfo {
                shader = _renderer.material.shader
            };
            if (_renderer.material.HasProperty(MainColorName))
            {
                info.color = _renderer.material.GetColor(MainColorName);
            }
            if (_renderer.material.HasProperty(ReflectColorName))
            {
                info.reflectColor = _renderer.material.GetColor(ReflectColorName);
            }
            if (_renderer.material.HasProperty(TintColorName))
            {
                info.TintColor = _renderer.material.GetColor(TintColorName);
            }
            this.rendererInitInfoes.Add(_renderer, info);
        }
    }

    [DebuggerHidden]
    private IEnumerator ChangeMaterialTemp(string name, float time)
    {
        return new <ChangeMaterialTemp>c__Iterator4F { name = name, time = time, <$>name = name, <$>time = time, <>f__this = this };
    }

    public void ClearTempNaterialChange()
    {
        if (this.isTempChanging)
        {
            base.StopCoroutine("ChangeMaterialTemp");
            this.OnChangeBackMaterial();
        }
    }

    private void FixedUpdate()
    {
        if (this.curColor != this.targetColor)
        {
            this.curColor.r = Mathf.SmoothDamp(this.curColor.r, this.targetColor.r, ref this.colorV.r, this.colorSmoothTime);
            this.curColor.g = Mathf.SmoothDamp(this.curColor.g, this.targetColor.g, ref this.colorV.g, this.colorSmoothTime);
            this.curColor.b = Mathf.SmoothDamp(this.curColor.b, this.targetColor.b, ref this.colorV.b, this.colorSmoothTime);
            this.curColor.a = Mathf.SmoothDamp(this.curColor.a, this.targetColor.a, ref this.colorV.a, this.alphaSmoothTime);
            this.SetColorAndAlpha(this.curColor);
        }
    }

    public void Init()
    {
        foreach (Renderer renderer in base.gameObject.GetComponentsInChildren<Renderer>())
        {
            if (renderer != null)
            {
                this.AddRendererInitInfo(renderer);
            }
        }
        this.isOnlyCacheOnce = true;
    }

    private bool IsCanChangeMaterial(string newMaterialName)
    {
        if (this.curState == null)
        {
            return true;
        }
        MaterialFSMInfo fSMInfo = this.info.GetFSMInfo(newMaterialName);
        if (fSMInfo == null)
        {
            return false;
        }
        return (fSMInfo.priority >= this.curState.priority);
    }

    private void OnChangeBackMaterial()
    {
        this.isTempChanging = false;
        if (this.oldState != null)
        {
            this.SetMaterial(this.oldState.name);
        }
        else
        {
            this.ResetMaterialSource();
        }
    }

    private void OnChangeMaterialAtRender(Renderer _renderer)
    {
        if ((_renderer != null) && !this.info.excludeNameList.Contains(_renderer.gameObject.name))
        {
            if (_renderer.gameObject.particleSystem != null)
            {
                if (this.curState.disableParticle)
                {
                    if (!this.disableObjs.Contains(_renderer.gameObject))
                    {
                        this.disableObjs.Add(_renderer.gameObject);
                    }
                    _renderer.gameObject.SetActive(false);
                }
            }
            else if (!this.curState.excludeTagNameList.Contains(_renderer.gameObject.tag))
            {
                this.AddRendererInitInfo(_renderer);
                _renderer.material.shader = this.curShader;
                this.OnSetMaterialParam(_renderer);
            }
        }
    }

    private void OnSetMaterialParam(Renderer _renderer)
    {
    }

    public void ResetColor()
    {
        this.curColor = this.targetColor = Color.white;
        this.SetColorAndAlpha(this.curColor);
    }

    public void ResetMaterial()
    {
        this.curColor = this.targetColor = Color.white;
        this.ResetMaterialSource();
    }

    public void ResetMaterialSource()
    {
        this.curState = null;
        foreach (KeyValuePair<Renderer, RendererMaterialInitInfo> pair in this.rendererInitInfoes)
        {
            Renderer key = pair.Key;
            if (key != null)
            {
                key.material.shader = pair.Value.shader;
            }
        }
        if (<>f__am$cache12 == null)
        {
            <>f__am$cache12 = delegate (GameObject obj) {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            };
        }
        this.disableObjs.ForEach(<>f__am$cache12);
        this.curColor.a = this.targetColor.a = 1f;
        this.SetColorAndAlpha(this.curColor);
        this.isTempChanging = false;
        base.StopAllCoroutines();
    }

    private void SetColorAndAlpha(Color _color)
    {
        foreach (Renderer renderer in base.gameObject.GetComponentsInChildren<Renderer>())
        {
            if (renderer != null)
            {
                RendererMaterialInitInfo info;
                if (!this.isOnlyCacheOnce)
                {
                    this.AddRendererInitInfo(renderer);
                }
                if (this.rendererInitInfoes.TryGetValue(renderer, out info))
                {
                    if (renderer.material.HasProperty(MainColorName))
                    {
                        Color color = info.color;
                        renderer.material.SetColor(MainColorName, _color * color);
                    }
                    if (renderer.material.HasProperty(ReflectColorName))
                    {
                        Color reflectColor = info.reflectColor;
                        renderer.material.SetColor(ReflectColorName, _color * reflectColor);
                    }
                    if (renderer.material.HasProperty(TintColorName))
                    {
                        Color tintColor = info.TintColor;
                        renderer.material.SetColor(TintColorName, _color * tintColor);
                    }
                }
            }
        }
    }

    public bool SetMaterial(string name)
    {
        this.TryInitDefault();
        if (!this.IsCanChangeMaterial(name))
        {
            return false;
        }
        if ((this.curState != null) && (this.curState.name == name))
        {
            return false;
        }
        this.curState = this.info.GetFSMInfo(name);
        if (this.curState == null)
        {
            return false;
        }
        this.curShader = Shader.Find(this.curState.shaderName);
        if (this.curShader != null)
        {
            foreach (Renderer renderer in base.gameObject.GetComponentsInChildren<Renderer>())
            {
                this.OnChangeMaterialAtRender(renderer);
            }
        }
        return true;
    }

    public void SetTable(string tableName)
    {
        this.info = MaterialFSMInfoManager.Instance().GetStateInfo(tableName);
    }

    public void StartAlphaChange(float time, float alpha)
    {
        this.SetMaterial(AlphaMaterialFSMName);
        this.targetColor.a = alpha;
        this.alphaSmoothTime = time;
    }

    public void StartChangeColor(Color _targetColor, float smoothTime)
    {
        this.targetColor.r = _targetColor.r;
        this.targetColor.g = _targetColor.g;
        this.targetColor.b = _targetColor.b;
        this.colorSmoothTime = smoothTime;
    }

    public void StartChangeMaterialTemp(string name, float time)
    {
        if ((((this.curState == null) || (this.curState.name != name)) && this.IsCanChangeMaterial(name)) && base.gameObject.activeSelf)
        {
            this.isTempChanging = true;
            base.StartCoroutine(this.ChangeMaterialTemp(name, time));
        }
    }

    private void TryInitDefault()
    {
        if (this.info == null)
        {
            this.info = MaterialFSMInfoManager.Instance().GetStateInfo("normal");
        }
    }

    [CompilerGenerated]
    private sealed class <ChangeMaterialTemp>c__Iterator4F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>name;
        internal float <$>time;
        internal MaterialFSM <>f__this;
        internal string name;
        internal float time;

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
                    this.<>f__this.oldState = this.<>f__this.curState;
                    if (this.<>f__this.SetMaterial(this.name))
                    {
                        this.$current = new WaitForSeconds(this.time);
                        this.$PC = 1;
                        return true;
                    }
                    break;

                case 1:
                    if ((this.<>f__this.curState == null) || !(this.<>f__this.curState.name != this.name))
                    {
                        if (this.<>f__this.isTempChanging)
                        {
                            this.<>f__this.OnChangeBackMaterial();
                        }
                        this.$PC = -1;
                        break;
                    }
                    break;
            }
            return false;
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

