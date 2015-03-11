using System;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("Image Effects/Bloom and Glow/Glow (Deprecated)")]
public class GlowEffect : MonoBehaviour
{
    public int blurIterations = 3;
    public Shader blurShader;
    public float blurSpread = 0.7f;
    public Shader compositeShader;
    public Shader downsampleShader;
    public float glowIntensity = 1.5f;
    public Color glowTint = new Color(1f, 1f, 1f, 0f);
    private Material m_BlurMaterial;
    private Material m_CompositeMaterial;
    private Material m_DownsampleMaterial;

    public void BlitGlow(RenderTexture source, RenderTexture dest)
    {
        this.compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(this.glowIntensity));
        Graphics.Blit(source, dest, this.compositeMaterial);
    }

    private void DownSample4x(RenderTexture source, RenderTexture dest)
    {
        this.downsampleMaterial.color = new Color(this.glowTint.r, this.glowTint.g, this.glowTint.b, this.glowTint.a / 4f);
        Graphics.Blit(source, dest, this.downsampleMaterial);
    }

    public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
        float x = 0.5f + (iteration * this.blurSpread);
        Vector2[] offsets = new Vector2[] { new Vector2(x, x), new Vector2(-x, x), new Vector2(x, -x), new Vector2(-x, -x) };
        Graphics.BlitMultiTap(source, dest, this.blurMaterial, offsets);
    }

    protected void OnDisable()
    {
        if (this.m_CompositeMaterial != null)
        {
            UnityEngine.Object.DestroyImmediate(this.m_CompositeMaterial);
        }
        if (this.m_BlurMaterial != null)
        {
            UnityEngine.Object.DestroyImmediate(this.m_BlurMaterial);
        }
        if (this.m_DownsampleMaterial != null)
        {
            UnityEngine.Object.DestroyImmediate(this.m_DownsampleMaterial);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        this.glowIntensity = Mathf.Clamp(this.glowIntensity, 0f, 10f);
        this.blurIterations = Mathf.Clamp(this.blurIterations, 0, 30);
        this.blurSpread = Mathf.Clamp(this.blurSpread, 0.5f, 1f);
        int width = source.width / 4;
        int height = source.height / 4;
        RenderTexture dest = RenderTexture.GetTemporary(width, height, 0);
        this.DownSample4x(source, dest);
        float num3 = Mathf.Clamp01((this.glowIntensity - 1f) / 4f);
        this.blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + num3);
        for (int i = 0; i < this.blurIterations; i++)
        {
            RenderTexture texture2 = RenderTexture.GetTemporary(width, height, 0);
            this.FourTapCone(dest, texture2, i);
            RenderTexture.ReleaseTemporary(dest);
            dest = texture2;
        }
        Graphics.Blit(source, destination);
        this.BlitGlow(dest, destination);
        RenderTexture.ReleaseTemporary(dest);
    }

    protected void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            base.enabled = false;
        }
        else if (this.downsampleShader == null)
        {
            Debug.Log("No downsample shader assigned! Disabling glow.");
            base.enabled = false;
        }
        else
        {
            if (!this.blurMaterial.shader.isSupported)
            {
                base.enabled = false;
            }
            if (!this.compositeMaterial.shader.isSupported)
            {
                base.enabled = false;
            }
            if (!this.downsampleMaterial.shader.isSupported)
            {
                base.enabled = false;
            }
        }
    }

    protected Material blurMaterial
    {
        get
        {
            if (this.m_BlurMaterial == null)
            {
                this.m_BlurMaterial = new Material(this.blurShader);
                this.m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return this.m_BlurMaterial;
        }
    }

    protected Material compositeMaterial
    {
        get
        {
            if (this.m_CompositeMaterial == null)
            {
                this.m_CompositeMaterial = new Material(this.compositeShader);
                this.m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return this.m_CompositeMaterial;
        }
    }

    protected Material downsampleMaterial
    {
        get
        {
            if (this.m_DownsampleMaterial == null)
            {
                this.m_DownsampleMaterial = new Material(this.downsampleShader);
                this.m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return this.m_DownsampleMaterial;
        }
    }
}

