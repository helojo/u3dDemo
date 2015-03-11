using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite"), ExecuteInEditMode]
public class UI2DSprite : UIBasicSprite
{
    [HideInInspector, SerializeField]
    private Vector4 mBorder = Vector4.zero;
    [SerializeField, HideInInspector]
    private Material mMat;
    [NonSerialized]
    private int mPMA = -1;
    [SerializeField, HideInInspector]
    private Shader mShader;
    [SerializeField, HideInInspector]
    private UnityEngine.Sprite mSprite;
    public UnityEngine.Sprite nextSprite;

    public override void MakePixelPerfect()
    {
        base.MakePixelPerfect();
        if (base.mType != UIBasicSprite.Type.Tiled)
        {
            Texture mainTexture = this.mainTexture;
            if ((mainTexture != null) && ((((base.mType == UIBasicSprite.Type.Simple) || (base.mType == UIBasicSprite.Type.Filled)) || !base.hasBorder) && (mainTexture != null)))
            {
                Rect rect = this.mSprite.rect;
                int num = Mathf.RoundToInt(rect.width);
                int num2 = Mathf.RoundToInt(rect.height);
                if ((num & 1) == 1)
                {
                    num++;
                }
                if ((num2 & 1) == 1)
                {
                    num2++;
                }
                base.width = num;
                base.height = num2;
            }
        }
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            Rect textureRect = this.mSprite.textureRect;
            Rect inner = textureRect;
            Vector4 border = this.border;
            inner.xMin += border.x;
            inner.yMin += border.y;
            inner.xMax -= border.z;
            inner.yMax -= border.w;
            float num = 1f / ((float) mainTexture.width);
            float num2 = 1f / ((float) mainTexture.height);
            textureRect.xMin *= num;
            textureRect.xMax *= num;
            textureRect.yMin *= num2;
            textureRect.yMax *= num2;
            inner.xMin *= num;
            inner.xMax *= num;
            inner.yMin *= num2;
            inner.yMax *= num2;
            base.Fill(verts, uvs, cols, textureRect, inner);
        }
    }

    protected override void OnUpdate()
    {
        if (this.nextSprite != null)
        {
            if (this.nextSprite != this.mSprite)
            {
                this.sprite2D = this.nextSprite;
            }
            this.nextSprite = null;
        }
        base.OnUpdate();
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
            if ((this.mSprite != null) && (base.mType != UIBasicSprite.Type.Tiled))
            {
                int num5 = Mathf.RoundToInt(this.mSprite.rect.width);
                int num6 = Mathf.RoundToInt(this.mSprite.rect.height);
                int num7 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
                int num8 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
                int num9 = Mathf.RoundToInt((this.mSprite.rect.width - this.mSprite.textureRect.width) - this.mSprite.textureRectOffset.x);
                int num10 = Mathf.RoundToInt((this.mSprite.rect.height - this.mSprite.textureRect.height) - this.mSprite.textureRectOffset.y);
                float num11 = 1f;
                float num12 = 1f;
                if (((num5 > 0) && (num6 > 0)) && ((base.mType == UIBasicSprite.Type.Simple) || (base.mType == UIBasicSprite.Type.Filled)))
                {
                    if ((num5 & 1) != 0)
                    {
                        num9++;
                    }
                    if ((num6 & 1) != 0)
                    {
                        num10++;
                    }
                    num11 = (1f / ((float) num5)) * base.mWidth;
                    num12 = (1f / ((float) num6)) * base.mHeight;
                }
                if ((base.mFlip == UIBasicSprite.Flip.Horizontally) || (base.mFlip == UIBasicSprite.Flip.Both))
                {
                    from += num9 * num11;
                    to -= num7 * num11;
                }
                else
                {
                    from += num7 * num11;
                    to -= num9 * num11;
                }
                if ((base.mFlip == UIBasicSprite.Flip.Vertically) || (base.mFlip == UIBasicSprite.Flip.Both))
                {
                    num2 += num10 * num12;
                    num4 -= num8 * num12;
                }
                else
                {
                    num2 += num8 * num12;
                    num4 -= num10 * num12;
                }
            }
            Vector4 border = this.border;
            float num13 = border.x + border.z;
            float num14 = border.y + border.w;
            float x = Mathf.Lerp(from, to - num13, this.mDrawRegion.x);
            float y = Mathf.Lerp(num2, num4 - num14, this.mDrawRegion.y);
            float z = Mathf.Lerp(from + num13, to, this.mDrawRegion.z);
            return new Vector4(x, y, z, Mathf.Lerp(num2 + num14, num4, this.mDrawRegion.w));
        }
    }

    public override Texture mainTexture
    {
        get
        {
            if (this.mSprite != null)
            {
                return this.mSprite.texture;
            }
            if (this.mMat != null)
            {
                return this.mMat.mainTexture;
            }
            return null;
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
                Shader shader = this.shader;
                this.mPMA = ((shader == null) || !shader.name.Contains("Premultiplied")) ? 0 : 1;
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
                if (this.mMat == null)
                {
                    this.mPMA = -1;
                    this.MarkAsChanged();
                }
            }
        }
    }

    public UnityEngine.Sprite sprite2D
    {
        get
        {
            return this.mSprite;
        }
        set
        {
            if (this.mSprite != value)
            {
                base.RemoveFromPanel();
                this.mSprite = value;
                this.nextSprite = null;
                this.MarkAsChanged();
            }
        }
    }
}

