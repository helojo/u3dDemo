using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleDarkScene : MonoBehaviour
{
    private Color changeV = Color.black;
    private Color curValue = Color.white;
    private List<MaterialInfo> materials = new List<MaterialInfo>();
    private List<ParticleInfo> particles = new List<ParticleInfo>();
    private float smoothTime = 1f;
    private Color targetFogColor;
    private Color targetValue = Color.white;

    public void changeColor(Color col)
    {
        foreach (MaterialInfo info in this.materials)
        {
            if (info.myMaterial.HasProperty("_Color"))
            {
                info.myMaterial.SetColor("_Color", col * info.initColor);
            }
        }
        foreach (ParticleInfo info2 in this.particles)
        {
            info2.myParticle.startColor = col * info2.initColor;
        }
        RenderSettings.fogColor = this.targetFogColor * col;
    }

    public void ChangeColor(Color targetColor, float _smoothTime)
    {
        this.smoothTime = _smoothTime;
        this.targetValue = targetColor;
    }

    private void FixedUpdate()
    {
        if (this.curValue != this.targetValue)
        {
            this.curValue.r = Mathf.SmoothDamp(this.curValue.r, this.targetValue.r, ref this.changeV.r, this.smoothTime);
            this.curValue.g = Mathf.SmoothDamp(this.curValue.g, this.targetValue.g, ref this.changeV.g, this.smoothTime);
            this.curValue.b = Mathf.SmoothDamp(this.curValue.b, this.targetValue.b, ref this.changeV.b, this.smoothTime);
            this.changeColor(this.curValue);
        }
    }

    public static BattleDarkScene GetGlobalDarkMB()
    {
        GameObject obj2 = GameObject.Find("/RootGroup");
        if (obj2 == null)
        {
            return null;
        }
        BattleDarkScene component = obj2.GetComponent<BattleDarkScene>();
        if (component == null)
        {
            component = obj2.AddComponent<BattleDarkScene>();
        }
        return component;
    }

    private void getMats()
    {
        this.targetFogColor = RenderSettings.fogColor;
        foreach (MeshRenderer renderer in base.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            <getMats>c__AnonStoreyE5 ye = new <getMats>c__AnonStoreyE5();
            Material[] sharedMaterials = renderer.sharedMaterials;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                ye.mat = sharedMaterials[i];
                if ((((ye.mat != null) && (this.materials.Find(new Predicate<MaterialInfo>(ye.<>m__4F)) == null)) && ((ye.mat.shader != null) && !ye.mat.shader.name.Contains("Rain"))) && ye.mat.HasProperty("_Color"))
                {
                    MaterialInfo item = new MaterialInfo {
                        myMaterial = ye.mat,
                        initColor = ye.mat.color
                    };
                    this.materials.Add(item);
                }
            }
        }
    }

    private void getParticles()
    {
        foreach (ParticleSystem system in base.gameObject.GetComponentsInChildren<ParticleSystem>())
        {
            ParticleInfo item = new ParticleInfo {
                myParticle = system,
                initColor = system.startColor
            };
            this.particles.Add(item);
        }
    }

    private void OnDestroy()
    {
        this.changeColor(Color.white);
    }

    private void Start()
    {
        this.getMats();
        this.getParticles();
    }

    public static void StartChangeColor(Color targetColor, float _smoothTime)
    {
        BattleDarkScene globalDarkMB = GetGlobalDarkMB();
        if (globalDarkMB != null)
        {
            globalDarkMB.ChangeColor(targetColor, _smoothTime);
        }
    }

    [CompilerGenerated]
    private sealed class <getMats>c__AnonStoreyE5
    {
        internal Material mat;

        internal bool <>m__4F(BattleDarkScene.MaterialInfo obj)
        {
            return (obj.myMaterial == this.mat);
        }
    }

    public class MaterialInfo
    {
        public Color initColor;
        public Material myMaterial;
    }

    public class ParticleInfo
    {
        public Color initColor;
        public ParticleSystem myParticle;
    }
}

