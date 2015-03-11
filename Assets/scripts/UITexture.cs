using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Texture"), ExecuteInEditMode]
public class UITexture : UIBasicSprite
{
    [HideInInspector, SerializeField]
    private Vector4 mBorder = Vector4.zero;
    [SerializeField, HideInInspector]
    private Material mMat;
    [NonSerialized]
    private int mPMA = -1;
    [HideInInspector, SerializeField]
    private Rect mRect = new Rect(0f, 0f, 1f, 1f);
    [HideInInspector, SerializeField]
    private Shader mShader;
    [SerializeField, HideInInspector]
    private Texture mTexture;

    public override void MakePixelPerfect()
    {
        base.MakePixelPerfect();
        if (base.mType != UIBasicSprite.Type.Tiled)
        {
            Texture mainTexture = this.mainTexture;
            if ((mainTexture != null) && ((((base.mType == UIBasicSprite.Type.Simple) || (base.mType == UIBasicSprite.Type.Filled)) || !base.hasBorder) && (mainTexture != null)))
            {
                int width = mainTexture.width;
                int height = mainTexture.height;
                if ((width & 1) == 1)
                {
                    width++;
                }
                if ((height & 1) == 1)
                {
                    height++;
                }
                base.width = width;
                base.height = height;
            }
        }
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            Rect outer = new Rect(this.mRect.x * mainTexture.width, this.mRect.y * mainTexture.height, mainTexture.width * this.mRect.width, mainTexture.height * this.mRect.height);
            Rect inner = outer;
            Vector4 border = this.border;
            inner.xMin += border.x;
            inner.yMin += border.y;
            inner.xMax -= border.z;
            inner.yMax -= border.w;
            float num = 1f / ((float) mainTexture.width);
            float num2 = 1f / ((float) mainTexture.height);
            outer.xMin *= num;
            outer.xMax *= num;
            outer.yMin *= num2;
            outer.yMax *= num2;
            inner.xMin *= num;
            inner.xMax *= num;
            inner.yMin *= num2;
            inner.yMax *= num2;
            base.Fill(verts, uvs, cols, outer, inner);
        }
    }

    public override Vector4 border
    {
        get
        {
            return this.mBorder;
        }
        set
        {
            if (this.mBorder != value)
            {
                this.mBorder = value;
                this.MarkAsChanged();
            }
        }
    }

    public override Vector4 drawingDimensions
    {
        get
        {
            Vector2 pivotOffset = base.pivotOffset;
            float from = -pivotOffset.x * base.mWidth;
            float num2 = -pivotOffset.y * base.mHeight;
            float to = from + base.mWidth;
            float num4 = num2 + base.mHeight;
            if ((this.mTexture != null) && (base.mType != UIBasicSprite.Type.Tiled))
            {
                int width = this.mTexture.width;
                int height = this.mTexture.height;
                int num7 = 0;
                int num8 = 0;
                float num9 = 1f;
                float num10 = 1f;
                if (((width > 0) && (height > 0)) && ((base.mType == UIBasicSprite.Type.Simple) || (base.mType == UIBasicSprite.Type.Filled)))
                {
                    if ((width & 1) != 0)
                    {
                        num7++;
                    }
                    if ((height & 1) != 0)
                    {
                        num8++;
                    }
                    num9 = (1f / ((float) width)) * base.mWidth;
                    num10 = (1f / ((float) height)) * base.mHeight;
                }
                if ((base.mFlip == UIBasicSprite.Flip.Horizontally) || (base.mFlip == UIBasicSprite.Flip.Both))
                {
                    from += num7 * num9;
                }
                else
                {
                    to -= num7 * num9;
                }
                if ((base.mFlip == UIBasicSprite.Flip.Vertically) || (base.mFlip == UIBasicSprite.Flip.Both))
                {
                    num2 += num8 * num10;
                }
                else
                {
                    num4 -= num8 * num10;
                }
            }
            Vector4 border = this.border;
            float num11 = border.x + border.z;
            float num12 = border.y + border.w;
            float x = Mathf.Lerp(from, to - num11, this.mDrawRegion.x);
            float y = Mathf.Lerp(num2, num4 - num12, this.mDrawRegion.y);
            float z = Mathf.Lerp(from + num11, to, this.mDrawRegion.z);
            return new Vector4(x, y, z, Mathf.Lerp(num2 + num12, num4, this.mDrawRegion.w));
        }
    }

    public override Texture mainTexture
    {
        get
        {
            if (this.mTexture != null)
            {
                return this.mTexture;
            }
            if (this.mMat != null)
            {
                return this.mMat.mainTexture;
            }
            return null;
        }
        set
        {
            if (this.mTexture != value)
            {
                base.RemoveFromPanel();
                this.mTexture = value;
                this.mPMA = -1;
                this.MarkAsChanged();
            }
        }
    }

    public override Material material
    {
        get
        {
            return this.mMat;
        }
        set
        {
            if (this.mMat != value)
            {
                base.RemoveFromPanel();
                this.mShader = null;
                this.mMat = value;
                this.mPMA = -1;
                this.MarkAsChanged();
            }
        }
    }

    public override bool premultipliedAlpha
    {
        get
        {
            if (this.mPMA == -1)
            {
                Material material = this.material;
                this.mPMA = (((material == null) || (material.shader == null)) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1;
            }
            return (this.mPMA == 1);
        }
    }

    public override Shader shader
    {
        get
        {
            if (this.mMat != null)
            {
                return this.mMat.shader;
            }
            if (this.mShader == null)
            {
                this.mShader = Shader.Find("Unlit/Transparent Colored");
            }
            return this.mShader;
        }
        set
        {
            if (this.mShader != value)
            {
                base.RemoveFromPanel();
                this.mShader = value;
                this.mPMA = -1;
                this.mMat = null;
                this.MarkAsChanged();
            }
        }
    }

    public Rect uvRect
    {
        get
        {
            return this.mRect;
        }
        set
        {
            if (this.mRect != value)
            {
                this.mRect = value;
                this.MarkAsChanged();
            }
        }
    }
}

