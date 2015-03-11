using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Image Effects/Blur"), ExecuteInEditMode]
public class BlurEffect : MonoBehaviour
{
    private static RenderTexture _backBlurRT;
    private static RenderTexture _blurRT;
    private static Camera _boardCamera;
    private static RenderTexture _frontBlurRT;
    private static Material _matEffect;
    private static RenderTexture _tinyRT;
    private Action<Texture> actCompleted;
    private Camera camera;
    private bool Term;


    public static void Begin(List<Camera> lstCamera, List<GameObject> nudities, Action<Texture> actCompleted)
    {
        <Begin>c__AnonStorey278 storey = new <Begin>c__AnonStorey278 {
            nudities = nudities,
            actCompleted = actCompleted
        };
        int count = lstCamera.Count;
        if (storey.nudities != null)
        {
            foreach (GameObject obj2 in storey.nudities)
            {
                obj2.SetActive(false);
            }
        }
        SwapBlurRT();
        for (int i = 0; i != count; i++)
        {
            Camera camera = lstCamera[i];
            if (null != camera)
            {
                BlurEffect effect = camera.gameObject.AddComponent<BlurEffect>();
                effect.camera = camera;
                if (i >= (count - 1))
                {
                    effect.Term = true;
                    effect.actCompleted = new Action<Texture>(storey.<>m__5C6);
                }
                camera.targetTexture = tinyRT;
            }
        }
        Camera boardCamera = BoardCamera;
        boardCamera.enabled = true;
        boardCamera.gameObject.AddComponent<RTClearance>();
        boardCamera.targetTexture = tinyRT;
        boardCamera.Render();
        for (int j = 0; j != count; j++)
        {
            Camera camera3 = lstCamera[j];
            if (null != camera3)
            {
                camera3.Render();
            }
        }
    }

    private void OnPostRender()
    {
        if (this.Term)
        {
            EffectMaterial.SetFloat("_BlurLength", 1f);
            Graphics.Blit(tinyRT, blurRT, EffectMaterial, -1);
        }
        Camera component = base.GetComponent<Camera>();
        if (null != component)
        {
            component.targetTexture = null;
        }
        if (this.actCompleted != null)
        {
            this.actCompleted(blurRT);
        }
        UnityEngine.Object.Destroy(this);
    }

    public static bool Supported()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            return false;
        }
        return EffectMaterial.shader.isSupported;
    }

    private static void SwapBlurRT()
    {
        if (null == _frontBlurRT)
        {
            int width = tinyRT.width;
            int height = tinyRT.height;
            _frontBlurRT = new RenderTexture(width, height, 0);
            _frontBlurRT.generateMips = false;
            _frontBlurRT.name = "front";
        }
        if (null == _backBlurRT)
        {
            int num3 = tinyRT.width;
            int num4 = tinyRT.height;
            _backBlurRT = new RenderTexture(num3, num4, 0);
            _backBlurRT.generateMips = false;
            _backBlurRT.name = "back";
        }
        RenderTexture texture = _backBlurRT;
        _backBlurRT = _frontBlurRT;
        _frontBlurRT = texture;
    }

    private static RenderTexture blurRT
    {
        get
        {
            return _frontBlurRT;
        }
    }

    private static Camera BoardCamera
    {
        get
        {
            if (null == _boardCamera)
            {
                _boardCamera = new GameObject { name = "RT_Clearance_Container" }.AddComponent<Camera>();
                _boardCamera.enabled = false;
                _boardCamera.targetTexture = tinyRT;
            }
            return _boardCamera;
        }
    }

    private static Material EffectMaterial
    {
        get
        {
            if (null == _matEffect)
            {
                _matEffect = new Material(Shader.Find("MT/UI_Blur"));
            }
            return _matEffect;
        }
    }

    private static RenderTexture tinyRT
    {
        get
        {
            if (null == _tinyRT)
            {
                float width = Screen.width;
                float height = Screen.height;
                int num3 = 0x60;
                int num4 = Mathf.RoundToInt((num3 * height) / width);
                _tinyRT = new RenderTexture(num3, num4, 0x18);
                _tinyRT.generateMips = false;
                EffectMaterial.SetFloat("_TexelWidth", _tinyRT.texelSize.x);
                EffectMaterial.SetFloat("_TexelHeight", _tinyRT.texelSize.y);
            }
            return _tinyRT;
        }
    }

    [CompilerGenerated]
    private sealed class <Begin>c__AnonStorey278
    {
        internal Action<Texture> actCompleted;
        internal List<GameObject> nudities;

        internal void <>m__5C6(Texture tex)
        {
            if (this.nudities != null)
            {
                foreach (GameObject obj2 in this.nudities)
                {
                    obj2.SetActive(true);
                }
            }
            if (this.actCompleted != null)
            {
                this.actCompleted(tex);
            }
        }
    }
}

