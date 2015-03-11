using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/NGUI Label")]
public class UILabel : UIWidget
{
    public Crispness keepCrispWhenShrunk = Crispness.OnDesktop;
    [NonSerialized]
    private Font mActiveTTF;
    [SerializeField, HideInInspector]
    private NGUIText.Alignment mAlignment;
    [SerializeField, HideInInspector]
    private bool mApplyGradient;
    private Vector2 mCalculatedSize = Vector2.zero;
    private float mDensity = 1f;
    [SerializeField, HideInInspector]
    private Color mEffectColor = Color.black;
    [SerializeField, HideInInspector]
    private Vector2 mEffectDistance = Vector2.one;
    [SerializeField, HideInInspector]
    private Effect mEffectStyle;
    [SerializeField, HideInInspector]
    private bool mEncoding = true;
    [HideInInspector, SerializeField]
    private UIFont mFont;
    [HideInInspector, SerializeField]
    private int mFontSize = 0x10;
    [SerializeField, HideInInspector]
    private FontStyle mFontStyle;
    private static Dictionary<Font, int> mFontUsage = new Dictionary<Font, int>();
    [HideInInspector, SerializeField]
    private Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);
    [HideInInspector, SerializeField]
    private Color mGradientTop = Color.white;
    private int mLastHeight;
    private int mLastWidth;
    [HideInInspector, SerializeField]
    private float mLineWidth;
    private static BetterList<UILabel> mList = new BetterList<UILabel>();
    [SerializeField, HideInInspector]
    private Material mMaterial;
    [HideInInspector, SerializeField]
    private int mMaxLineCount;
    [SerializeField, HideInInspector]
    private int mMaxLineHeight;
    [SerializeField, HideInInspector]
    private int mMaxLineWidth;
    [HideInInspector, SerializeField]
    private bool mMultiline = true;
    [HideInInspector, SerializeField]
    private Overflow mOverflow;
    private bool mPremultiply;
    private int mPrintedSize;
    private string mProcessedText;
    private float mScale = 1f;
    private bool mShouldBeProcessed = true;
    [SerializeField, HideInInspector]
    private bool mShrinkToFit;
    [HideInInspector, SerializeField]
    private int mSpacingX;
    [HideInInspector, SerializeField]
    private int mSpacingY;
    [HideInInspector, SerializeField]
    private NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Normal;
    private static BetterList<int> mTempIndices = new BetterList<int>();
    private static BetterList<Vector3> mTempVerts = new BetterList<Vector3>();
    [Multiline(6), HideInInspector, SerializeField]
    private string mText = string.Empty;
    [HideInInspector, SerializeField]
    private Font mTrueTypeFont;

    protected Vector2 ApplyOffset(BetterList<Vector3> verts, int start)
    {
        Vector2 pivotOffset = base.pivotOffset;
        float f = Mathf.Lerp(0f, (float) -base.mWidth, pivotOffset.x);
        float num2 = Mathf.Lerp((float) base.mHeight, 0f, pivotOffset.y) + Mathf.Lerp(this.mCalculatedSize.y - base.mHeight, 0f, pivotOffset.y);
        f = Mathf.Round(f);
        num2 = Mathf.Round(num2);
        for (int i = start; i < verts.size; i++)
        {
            verts.buffer[i].x += f;
            verts.buffer[i].y += num2;
        }
        return new Vector2(f, num2);
    }

    private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
    {
        Color mEffectColor = this.mEffectColor;
        mEffectColor.a *= base.finalAlpha;
        Color32 color2 = ((this.bitmapFont == null) || !this.bitmapFont.premultipliedAlphaShader) ? mEffectColor : NGUITools.ApplyPMA(mEffectColor);
        for (int i = start; i < end; i++)
        {
            verts.Add(verts.buffer[i]);
            uvs.Add(uvs.buffer[i]);
            cols.Add(cols.buffer[i]);
            Vector3 vector = verts.buffer[i];
            vector.x += x;
            vector.y += y;
            verts.buffer[i] = vector;
            Color32 color3 = cols.buffer[i];
            if (color3.a == 0xff)
            {
                cols.buffer[i] = color2;
            }
            else
            {
                Color c = mEffectColor;
                c.a = (((float) color3.a) / 255f) * mEffectColor.a;
                cols.buffer[i] = ((this.bitmapFont == null) || !this.bitmapFont.premultipliedAlphaShader) ? c : NGUITools.ApplyPMA(c);
            }
        }
    }

    public void AssumeNaturalSize()
    {
        if (this.ambigiousFont != null)
        {
            base.mWidth = 0x186a0;
            base.mHeight = 0x186a0;
            this.ProcessText(false, true);
            base.mWidth = Mathf.RoundToInt(this.mCalculatedSize.x);
            base.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y);
            if ((base.mWidth & 1) == 1)
            {
                base.mWidth++;
            }
            if ((base.mHeight & 1) == 1)
            {
                base.mHeight++;
            }
            this.MarkAsChanged();
        }
    }

    public int CalculateOffsetToFit(string text)
    {
        this.UpdateNGUIText();
        NGUIText.encoding = false;
        NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
        return NGUIText.CalculateOffsetToFit(text);
    }

    [Obsolete("Use UILabel.GetCharacterAtPosition instead")]
    public int GetCharacterIndex(Vector2 localPos)
    {
        return this.GetCharacterIndexAtPosition(localPos);
    }

    [Obsolete("Use UILabel.GetCharacterAtPosition instead")]
    public int GetCharacterIndex(Vector3 worldPos)
    {
        return this.GetCharacterIndexAtPosition(worldPos);
    }

    public int GetCharacterIndex(int currentIndex, KeyCode key)
    {
        if (!this.isValid)
        {
            return currentIndex;
        }
        string processedText = this.processedText;
        if (string.IsNullOrEmpty(processedText))
        {
            return 0;
        }
        int defaultFontSize = this.defaultFontSize;
        this.UpdateNGUIText();
        NGUIText.PrintCharacterPositions(processedText, mTempVerts, mTempIndices);
        if (mTempVerts.size > 0)
        {
            this.ApplyOffset(mTempVerts, 0);
            for (int i = 0; i < mTempIndices.size; i++)
            {
                if (mTempIndices[i] == currentIndex)
                {
                    Vector2 pos = mTempVerts[i];
                    if (key == KeyCode.UpArrow)
                    {
                        pos.y += defaultFontSize + this.spacingY;
                    }
                    else if (key == KeyCode.DownArrow)
                    {
                        pos.y -= defaultFontSize + this.spacingY;
                    }
                    else if (key == KeyCode.Home)
                    {
                        pos.x -= 1000f;
                    }
                    else if (key == KeyCode.End)
                    {
                        pos.x += 1000f;
                    }
                    int closestCharacter = NGUIText.GetClosestCharacter(mTempVerts, pos);
                    closestCharacter = mTempIndices[closestCharacter];
                    if (closestCharacter != currentIndex)
                    {
                        mTempVerts.Clear();
                        mTempIndices.Clear();
                        return closestCharacter;
                    }
                    break;
                }
            }
            mTempVerts.Clear();
            mTempIndices.Clear();
        }
        if ((key == KeyCode.UpArrow) || (key == KeyCode.Home))
        {
            return 0;
        }
        if ((key != KeyCode.DownArrow) && (key != KeyCode.End))
        {
            return currentIndex;
        }
        return processedText.Length;
    }

    public int GetCharacterIndexAtPosition(Vector2 localPos)
    {
        if (this.isValid)
        {
            string processedText = this.processedText;
            if (string.IsNullOrEmpty(processedText))
            {
                return 0;
            }
            this.UpdateNGUIText();
            NGUIText.PrintCharacterPositions(processedText, mTempVerts, mTempIndices);
            if (mTempVerts.size > 0)
            {
                this.ApplyOffset(mTempVerts, 0);
                int closestCharacter = NGUIText.GetClosestCharacter(mTempVerts, localPos);
                closestCharacter = mTempIndices[closestCharacter];
                mTempVerts.Clear();
                mTempIndices.Clear();
                return closestCharacter;
            }
        }
        return 0;
    }

    public int GetCharacterIndexAtPosition(Vector3 worldPos)
    {
        Vector2 localPos = base.cachedTransform.InverseTransformPoint(worldPos);
        return this.GetCharacterIndexAtPosition(localPos);
    }

    public override Vector3[] GetSides(Transform relativeTo)
    {
        if (this.shouldBeProcessed)
        {
            this.ProcessText();
        }
        return base.GetSides(relativeTo);
    }

    public string GetUrlAtCharacterIndex(int characterIndex)
    {
        if ((characterIndex != -1) && (characterIndex < this.mText.Length))
        {
            int startIndex = this.mText.LastIndexOf("[url=", characterIndex);
            if (startIndex != -1)
            {
                startIndex += 5;
                int index = this.mText.IndexOf("]", startIndex);
                if (index != -1)
                {
                    return this.mText.Substring(startIndex, index - startIndex);
                }
            }
        }
        return null;
    }

    public string GetUrlAtPosition(Vector2 localPos)
    {
        return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos));
    }

    public string GetUrlAtPosition(Vector3 worldPos)
    {
        return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos));
    }

    public string GetWordAtCharacterIndex(int characterIndex)
    {
        if ((characterIndex != -1) && (characterIndex < this.mText.Length))
        {
            int startIndex = this.mText.LastIndexOf(' ', characterIndex) + 1;
            int index = this.mText.IndexOf(' ', characterIndex);
            if (index == -1)
            {
                index = this.mText.Length;
            }
            if (startIndex != index)
            {
                int length = index - startIndex;
                if (length > 0)
                {
                    return NGUIText.StripSymbols(this.mText.Substring(startIndex, length));
                }
            }
        }
        return null;
    }

    public string GetWordAtPosition(Vector2 localPos)
    {
        return this.GetWordAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos));
    }

    public string GetWordAtPosition(Vector3 worldPos)
    {
        return this.GetWordAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos));
    }

    public override void MakePixelPerfect()
    {
        if (this.ambigiousFont != null)
        {
            Vector3 localPosition = base.cachedTransform.localPosition;
            localPosition.x = Mathf.RoundToInt(localPosition.x);
            localPosition.y = Mathf.RoundToInt(localPosition.y);
            localPosition.z = Mathf.RoundToInt(localPosition.z);
            base.cachedTransform.localPosition = localPosition;
            base.cachedTransform.localScale = Vector3.one;
            if (this.mOverflow == Overflow.ResizeFreely)
            {
                this.AssumeNaturalSize();
            }
            else
            {
                int width = base.width;
                int height = base.height;
                Overflow mOverflow = this.mOverflow;
                if (mOverflow != Overflow.ResizeHeight)
                {
                    base.mWidth = 0x186a0;
                }
                base.mHeight = 0x186a0;
                this.mOverflow = Overflow.ShrinkContent;
                this.ProcessText(false, true);
                this.mOverflow = mOverflow;
                int a = Mathf.RoundToInt(this.mCalculatedSize.x);
                int num4 = Mathf.RoundToInt(this.mCalculatedSize.y);
                a = Mathf.Max(a, base.minWidth);
                num4 = Mathf.Max(num4, base.minHeight);
                base.mWidth = Mathf.Max(width, a);
                base.mHeight = Mathf.Max(height, num4);
                this.MarkAsChanged();
            }
        }
        else
        {
            base.MakePixelPerfect();
        }
    }

    public override void MarkAsChanged()
    {
        this.shouldBeProcessed = true;
        base.MarkAsChanged();
    }

    protected override void OnAnchor()
    {
        if (this.mOverflow == Overflow.ResizeFreely)
        {
            if (base.isFullyAnchored)
            {
                this.mOverflow = Overflow.ShrinkContent;
            }
        }
        else if (((this.mOverflow == Overflow.ResizeHeight) && (base.topAnchor.target != null)) && (base.bottomAnchor.target != null))
        {
            this.mOverflow = Overflow.ShrinkContent;
        }
        base.OnAnchor();
    }

    protected override void OnDisable()
    {
        this.SetActiveFont(null);
        mList.Remove(this);
        base.OnDisable();
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        if (this.isValid)
        {
            int size = verts.size;
            Color c = base.color;
            c.a = base.finalAlpha;
            if ((this.mFont != null) && this.mFont.premultipliedAlphaShader)
            {
                c = NGUITools.ApplyPMA(c);
            }
            string processedText = this.processedText;
            int start = verts.size;
            this.UpdateNGUIText();
            NGUIText.tint = c;
            NGUIText.Print(processedText, verts, uvs, cols);
            Vector2 vector = this.ApplyOffset(verts, start);
            if (((this.mFont == null) || !this.mFont.packedFontShader) && (this.effectStyle != Effect.None))
            {
                int end = verts.size;
                vector.x = this.mEffectDistance.x;
                vector.y = this.mEffectDistance.y;
                this.ApplyShadow(verts, uvs, cols, size, end, vector.x, -vector.y);
                if (this.effectStyle == Effect.Outline)
                {
                    size = end;
                    end = verts.size;
                    this.ApplyShadow(verts, uvs, cols, size, end, -vector.x, vector.y);
                    size = end;
                    end = verts.size;
                    this.ApplyShadow(verts, uvs, cols, size, end, vector.x, vector.y);
                    size = end;
                    end = verts.size;
                    this.ApplyShadow(verts, uvs, cols, size, end, -vector.x, -vector.y);
                }
            }
        }
    }

    private static void OnFontTextureChanged()
    {
        for (int i = 0; i < mList.size; i++)
        {
            UILabel label = mList[i];
            if (label != null)
            {
                Font trueTypeFont = label.trueTypeFont;
                if (trueTypeFont != null)
                {
                    trueTypeFont.RequestCharactersInTexture(label.mText, label.mPrintedSize, label.mFontStyle);
                    label.MarkAsChanged();
                }
            }
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        mList.Add(this);
        this.SetActiveFont(this.trueTypeFont);
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (this.mLineWidth > 0f)
        {
            this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
            this.mLineWidth = 0f;
        }
        if (!this.mMultiline)
        {
            this.mMaxLineCount = 1;
            this.mMultiline = true;
        }
        this.mPremultiply = ((this.material != null) && (this.material.shader != null)) && this.material.shader.name.Contains("Premultiplied");
        this.ProcessAndRequest();
    }

    public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
    {
        if (caret != null)
        {
            caret.Clear();
        }
        if (highlight != null)
        {
            highlight.Clear();
        }
        if (this.isValid)
        {
            string processedText = this.processedText;
            this.UpdateNGUIText();
            int size = caret.verts.size;
            Vector2 item = new Vector2(0.5f, 0.5f);
            float finalAlpha = base.finalAlpha;
            if ((highlight != null) && (start != end))
            {
                int num3 = highlight.verts.size;
                NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, highlight.verts);
                if (highlight.verts.size > num3)
                {
                    this.ApplyOffset(highlight.verts, num3);
                    Color32 color = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * finalAlpha);
                    for (int j = num3; j < highlight.verts.size; j++)
                    {
                        highlight.uvs.Add(item);
                        highlight.cols.Add(color);
                    }
                }
            }
            else
            {
                NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, null);
            }
            this.ApplyOffset(caret.verts, size);
            Color32 color2 = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * finalAlpha);
            for (int i = size; i < caret.verts.size; i++)
            {
                caret.uvs.Add(item);
                caret.cols.Add(color2);
            }
        }
    }

    private void ProcessAndRequest()
    {
        if (this.ambigiousFont != null)
        {
            this.ProcessText();
        }
    }

    private void ProcessText()
    {
        this.ProcessText(false, true);
    }

    private void ProcessText(bool legacyMode, bool full)
    {
        if (this.isValid)
        {
            base.mChanged = true;
            this.shouldBeProcessed = false;
            NGUIText.rectWidth = !legacyMode ? base.width : ((this.mMaxLineWidth == 0) ? 0xf4240 : this.mMaxLineWidth);
            NGUIText.rectHeight = !legacyMode ? base.height : ((this.mMaxLineHeight == 0) ? 0xf4240 : this.mMaxLineHeight);
            this.mPrintedSize = Mathf.Abs(!legacyMode ? this.defaultFontSize : Mathf.RoundToInt(base.cachedTransform.localScale.x));
            this.mScale = 1f;
            if ((NGUIText.rectWidth < 1) || (NGUIText.rectHeight < 0))
            {
                this.mProcessedText = string.Empty;
            }
            else
            {
                bool flag = this.trueTypeFont != null;
                if (flag && this.keepCrisp)
                {
                    UIRoot root = base.root;
                    if (root != null)
                    {
                        this.mDensity = (root == null) ? 1f : root.pixelSizeAdjustment;
                    }
                }
                else
                {
                    this.mDensity = 1f;
                }
                if (full)
                {
                    this.UpdateNGUIText();
                }
                if (this.mOverflow == Overflow.ResizeFreely)
                {
                    NGUIText.rectWidth = 0xf4240;
                }
                if ((this.mOverflow == Overflow.ResizeFreely) || (this.mOverflow == Overflow.ResizeHeight))
                {
                    NGUIText.rectHeight = 0xf4240;
                }
                if (this.mPrintedSize > 0)
                {
                    bool keepCrisp = this.keepCrisp;
                    for (int i = this.mPrintedSize; i > 0; i--)
                    {
                        if (keepCrisp)
                        {
                            this.mPrintedSize = i;
                            NGUIText.fontSize = this.mPrintedSize;
                        }
                        else
                        {
                            this.mScale = ((float) i) / ((float) this.mPrintedSize);
                            NGUIText.fontScale = !flag ? ((((float) this.mFontSize) / ((float) this.mFont.defaultSize)) * this.mScale) : this.mScale;
                        }
                        NGUIText.Update(false);
                        bool flag3 = NGUIText.WrapText(this.mText, out this.mProcessedText, true);
                        if ((this.mOverflow == Overflow.ShrinkContent) && !flag3)
                        {
                            if (--i <= 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (this.mOverflow == Overflow.ResizeFreely)
                            {
                                this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
                                base.mWidth = Mathf.Max(this.minWidth, Mathf.RoundToInt(this.mCalculatedSize.x));
                                base.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
                                if ((base.mWidth & 1) == 1)
                                {
                                    base.mWidth++;
                                }
                                if ((base.mHeight & 1) == 1)
                                {
                                    base.mHeight++;
                                }
                            }
                            else if (this.mOverflow == Overflow.ResizeHeight)
                            {
                                this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
                                base.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
                                if ((base.mHeight & 1) == 1)
                                {
                                    base.mHeight++;
                                }
                            }
                            else
                            {
                                this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
                            }
                            if (legacyMode)
                            {
                                base.width = Mathf.RoundToInt(this.mCalculatedSize.x);
                                base.height = Mathf.RoundToInt(this.mCalculatedSize.y);
                                base.cachedTransform.localScale = Vector3.one;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    base.cachedTransform.localScale = Vector3.one;
                    this.mProcessedText = string.Empty;
                    this.mScale = 1f;
                }
            }
        }
    }

    protected void SetActiveFont(Font fnt)
    {
        if (this.mActiveTTF != fnt)
        {
            if (this.mActiveTTF != null)
            {
                int num;
                if (mFontUsage.TryGetValue(this.mActiveTTF, out num))
                {
                    num = Mathf.Max(0, --num);
                    if (num == 0)
                    {
                        this.mActiveTTF.textureRebuildCallback = null;
                        mFontUsage.Remove(this.mActiveTTF);
                    }
                    else
                    {
                        mFontUsage[this.mActiveTTF] = num;
                    }
                }
                else
                {
                    this.mActiveTTF.textureRebuildCallback = null;
                }
            }
            this.mActiveTTF = fnt;
            if (this.mActiveTTF != null)
            {
                int num2 = 0;
                if (!mFontUsage.TryGetValue(this.mActiveTTF, out num2))
                {
                    this.mActiveTTF.textureRebuildCallback = new Font.FontTextureRebuildCallback(UILabel.OnFontTextureChanged);
                }
                mFontUsage[this.mActiveTTF] = ++num2;
            }
        }
    }

    public void SetCurrentPercent()
    {
        if (UIProgressBar.current != null)
        {
            this.text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
        }
    }

    public void SetCurrentProgress()
    {
        if (UIProgressBar.current != null)
        {
            this.text = UIProgressBar.current.value.ToString("F");
        }
    }

    public void SetCurrentSelection()
    {
        if (UIPopupList.current != null)
        {
            this.text = !UIPopupList.current.isLocalized ? UIPopupList.current.value : Localization.Get(UIPopupList.current.value);
        }
    }

    public void UpdateNGUIText()
    {
        Font trueTypeFont = this.trueTypeFont;
        bool flag = trueTypeFont != null;
        NGUIText.fontSize = this.mPrintedSize;
        NGUIText.fontStyle = this.mFontStyle;
        NGUIText.rectWidth = base.mWidth;
        NGUIText.rectHeight = base.mHeight;
        NGUIText.gradient = this.mApplyGradient && ((this.mFont == null) || !this.mFont.packedFontShader);
        NGUIText.gradientTop = this.mGradientTop;
        NGUIText.gradientBottom = this.mGradientBottom;
        NGUIText.encoding = this.mEncoding;
        NGUIText.premultiply = this.mPremultiply;
        NGUIText.symbolStyle = this.mSymbols;
        NGUIText.maxLines = this.mMaxLineCount;
        NGUIText.spacingX = this.mSpacingX;
        NGUIText.spacingY = this.mSpacingY;
        NGUIText.fontScale = !flag ? ((((float) this.mFontSize) / ((float) this.mFont.defaultSize)) * this.mScale) : this.mScale;
        if (this.mFont == null)
        {
            NGUIText.dynamicFont = trueTypeFont;
            NGUIText.bitmapFont = null;
        }
        else
        {
            NGUIText.bitmapFont = this.mFont;
            while (true)
            {
                UIFont replacement = NGUIText.bitmapFont.replacement;
                if (replacement == null)
                {
                    break;
                }
                NGUIText.bitmapFont = replacement;
            }
            if (NGUIText.bitmapFont.isDynamic)
            {
                NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
                NGUIText.bitmapFont = null;
            }
            else
            {
                NGUIText.dynamicFont = null;
            }
        }
        if (flag && this.keepCrisp)
        {
            UIRoot root = base.root;
            if (root != null)
            {
                NGUIText.pixelDensity = (root == null) ? 1f : root.pixelSizeAdjustment;
            }
        }
        else
        {
            NGUIText.pixelDensity = 1f;
        }
        if (this.mDensity != NGUIText.pixelDensity)
        {
            this.ProcessText(false, false);
            NGUIText.rectWidth = base.mWidth;
            NGUIText.rectHeight = base.mHeight;
        }
        if (this.alignment == NGUIText.Alignment.Automatic)
        {
            switch (base.pivot)
            {
                case UIWidget.Pivot.Left:
                case UIWidget.Pivot.TopLeft:
                case UIWidget.Pivot.BottomLeft:
                    NGUIText.alignment = NGUIText.Alignment.Left;
                    goto Label_0273;

                case UIWidget.Pivot.Right:
                case UIWidget.Pivot.TopRight:
                case UIWidget.Pivot.BottomRight:
                    NGUIText.alignment = NGUIText.Alignment.Right;
                    goto Label_0273;
            }
            NGUIText.alignment = NGUIText.Alignment.Center;
        }
        else
        {
            NGUIText.alignment = this.alignment;
        }
    Label_0273:
        NGUIText.Update();
    }

    protected override void UpgradeFrom265()
    {
        this.ProcessText(true, true);
        if (this.mShrinkToFit)
        {
            this.overflowMethod = Overflow.ShrinkContent;
            this.mMaxLineCount = 0;
        }
        if (this.mMaxLineWidth != 0)
        {
            base.width = this.mMaxLineWidth;
            this.overflowMethod = (this.mMaxLineCount <= 0) ? Overflow.ShrinkContent : Overflow.ResizeHeight;
        }
        else
        {
            this.overflowMethod = Overflow.ResizeFreely;
        }
        if (this.mMaxLineHeight != 0)
        {
            base.height = this.mMaxLineHeight;
        }
        if (this.mFont != null)
        {
            int defaultSize = this.mFont.defaultSize;
            if (base.height < defaultSize)
            {
                base.height = defaultSize;
            }
        }
        this.mMaxLineWidth = 0;
        this.mMaxLineHeight = 0;
        this.mShrinkToFit = false;
        NGUITools.UpdateWidgetCollider(base.gameObject, true);
    }

    public bool Wrap(string text, out string final)
    {
        return this.Wrap(text, out final, 0xf4240);
    }

    public bool Wrap(string text, out string final, int height)
    {
        this.UpdateNGUIText();
        NGUIText.rectHeight = height;
        return NGUIText.WrapText(text, out final);
    }

    public NGUIText.Alignment alignment
    {
        get
        {
            return this.mAlignment;
        }
        set
        {
            if (this.mAlignment != value)
            {
                this.mAlignment = value;
                this.shouldBeProcessed = true;
                this.ProcessAndRequest();
            }
        }
    }

    public UnityEngine.Object ambigiousFont
    {
        get
        {
            return ((this.mFont == null) ? ((UnityEngine.Object) this.mTrueTypeFont) : ((UnityEngine.Object) this.mFont));
        }
        set
        {
            UIFont font = value as UIFont;
            if (font != null)
            {
                this.bitmapFont = font;
            }
            else
            {
                this.trueTypeFont = value as Font;
            }
        }
    }

    public bool applyGradient
    {
        get
        {
            return this.mApplyGradient;
        }
        set
        {
            if (this.mApplyGradient != value)
            {
                this.mApplyGradient = value;
                this.MarkAsChanged();
            }
        }
    }

    public UIFont bitmapFont
    {
        get
        {
            return this.mFont;
        }
        set
        {
            if (this.mFont != value)
            {
                base.RemoveFromPanel();
                this.mFont = value;
                this.mTrueTypeFont = null;
                this.MarkAsChanged();
            }
        }
    }

    public int defaultFontSize
    {
        get
        {
            return ((this.trueTypeFont == null) ? ((this.mFont == null) ? 0x10 : this.mFont.defaultSize) : this.mFontSize);
        }
    }

    public override Vector4 drawingDimensions
    {
        get
        {
            if (this.shouldBeProcessed)
            {
                this.ProcessText();
            }
            return base.drawingDimensions;
        }
    }

    public Color effectColor
    {
        get
        {
            return this.mEffectColor;
        }
        set
        {
            if (this.mEffectColor != value)
            {
                this.mEffectColor = value;
                if (this.mEffectStyle != Effect.None)
                {
                    this.shouldBeProcessed = true;
                }
            }
        }
    }

    public Vector2 effectDistance
    {
        get
        {
            return this.mEffectDistance;
        }
        set
        {
            if (this.mEffectDistance != value)
            {
                this.mEffectDistance = value;
                this.shouldBeProcessed = true;
            }
        }
    }

    public Effect effectStyle
    {
        get
        {
            return this.mEffectStyle;
        }
        set
        {
            if (this.mEffectStyle != value)
            {
                this.mEffectStyle = value;
                this.shouldBeProcessed = true;
            }
        }
    }

    [Obsolete("Use UILabel.bitmapFont instead")]
    public UIFont font
    {
        get
        {
            return this.bitmapFont;
        }
        set
        {
            this.bitmapFont = value;
        }
    }

    public int fontSize
    {
        get
        {
            return this.mFontSize;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 0x100);
            if (this.mFontSize != value)
            {
                this.mFontSize = value;
                this.shouldBeProcessed = true;
                this.ProcessAndRequest();
            }
        }
    }

    public FontStyle fontStyle
    {
        get
        {
            return this.mFontStyle;
        }
        set
        {
            if (this.mFontStyle != value)
            {
                this.mFontStyle = value;
                this.shouldBeProcessed = true;
                this.ProcessAndRequest();
            }
        }
    }

    public Color gradientBottom
    {
        get
        {
            return this.mGradientBottom;
        }
        set
        {
            if (this.mGradientBottom != value)
            {
                this.mGradientBottom = value;
                if (this.mApplyGradient)
                {
                    this.MarkAsChanged();
                }
            }
        }
    }

    public Color gradientTop
    {
        get
        {
            return this.mGradientTop;
        }
        set
        {
            if (this.mGradientTop != value)
            {
                this.mGradientTop = value;
                if (this.mApplyGradient)
                {
                    this.MarkAsChanged();
                }
            }
        }
    }

    public int intValue
    {
        get
        {
            return int.Parse(this.mText);
        }
        set
        {
            this.text = value.ToString();
        }
    }

    public override bool isAnchoredHorizontally
    {
        get
        {
            return (base.isAnchoredHorizontally || (this.mOverflow == Overflow.ResizeFreely));
        }
    }

    public override bool isAnchoredVertically
    {
        get
        {
            return ((base.isAnchoredVertically || (this.mOverflow == Overflow.ResizeFreely)) || (this.mOverflow == Overflow.ResizeHeight));
        }
    }

    private bool isValid
    {
        get
        {
            return ((this.mFont != null) || (this.mTrueTypeFont != null));
        }
    }

    private bool keepCrisp
    {
        get
        {
            return (((this.trueTypeFont != null) && (this.keepCrispWhenShrunk != Crispness.Never)) && (this.keepCrispWhenShrunk == Crispness.Always));
        }
    }

    [Obsolete("Use 'height' instead")]
    public int lineHeight
    {
        get
        {
            return base.height;
        }
        set
        {
            base.height = value;
        }
    }

    [Obsolete("Use 'width' instead")]
    public int lineWidth
    {
        get
        {
            return base.width;
        }
        set
        {
            base.width = value;
        }
    }

    public override Vector3[] localCorners
    {
        get
        {
            if (this.shouldBeProcessed)
            {
                this.ProcessText();
            }
            return base.localCorners;
        }
    }

    public override Vector2 localSize
    {
        get
        {
            if (this.shouldBeProcessed)
            {
                this.ProcessText();
            }
            return base.localSize;
        }
    }

    public override Material material
    {
        get
        {
            if (this.mMaterial != null)
            {
                return this.mMaterial;
            }
            if (this.mFont != null)
            {
                return this.mFont.material;
            }
            if (this.mTrueTypeFont != null)
            {
                return this.mTrueTypeFont.material;
            }
            return null;
        }
        set
        {
            if (this.mMaterial != value)
            {
                this.MarkAsChanged();
                this.mMaterial = value;
                this.MarkAsChanged();
            }
        }
    }

    public int maxLineCount
    {
        get
        {
            return this.mMaxLineCount;
        }
        set
        {
            if (this.mMaxLineCount != value)
            {
                this.mMaxLineCount = Mathf.Max(value, 0);
                this.shouldBeProcessed = true;
                if (this.overflowMethod == Overflow.ShrinkContent)
                {
                    this.MakePixelPerfect();
                }
            }
        }
    }

    public bool multiLine
    {
        get
        {
            return (this.mMaxLineCount != 1);
        }
        set
        {
            if ((this.mMaxLineCount != 1) != value)
            {
                this.mMaxLineCount = !value ? 1 : 0;
                this.shouldBeProcessed = true;
            }
        }
    }

    public Overflow overflowMethod
    {
        get
        {
            return this.mOverflow;
        }
        set
        {
            if (this.mOverflow != value)
            {
                this.mOverflow = value;
                this.shouldBeProcessed = true;
            }
        }
    }

    public Vector2 printedSize
    {
        get
        {
            if (this.shouldBeProcessed)
            {
                this.ProcessText();
            }
            return this.mCalculatedSize;
        }
    }

    public string processedText
    {
        get
        {
            if ((this.mLastWidth != base.mWidth) || (this.mLastHeight != base.mHeight))
            {
                this.mLastWidth = base.mWidth;
                this.mLastHeight = base.mHeight;
                this.mShouldBeProcessed = true;
            }
            if (this.shouldBeProcessed)
            {
                this.ProcessText();
            }
            return this.mProcessedText;
        }
    }

    private bool shouldBeProcessed
    {
        get
        {
            return this.mShouldBeProcessed;
        }
        set
        {
            if (value)
            {
                base.mChanged = true;
                this.mShouldBeProcessed = true;
            }
            else
            {
                this.mShouldBeProcessed = false;
            }
        }
    }

    [Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
    public bool shrinkToFit
    {
        get
        {
            return (this.mOverflow == Overflow.ShrinkContent);
        }
        set
        {
            if (value)
            {
                this.overflowMethod = Overflow.ShrinkContent;
            }
        }
    }

    public int spacingX
    {
        get
        {
            return this.mSpacingX;
        }
        set
        {
            if (this.mSpacingX != value)
            {
                this.mSpacingX = value;
                this.MarkAsChanged();
            }
        }
    }

    public int spacingY
    {
        get
        {
            return this.mSpacingY;
        }
        set
        {
            if (this.mSpacingY != value)
            {
                this.mSpacingY = value;
                this.MarkAsChanged();
            }
        }
    }

    public bool supportEncoding
    {
        get
        {
            return this.mEncoding;
        }
        set
        {
            if (this.mEncoding != value)
            {
                this.mEncoding = value;
                this.shouldBeProcessed = true;
            }
        }
    }

    public NGUIText.SymbolStyle symbolStyle
    {
        get
        {
            return this.mSymbols;
        }
        set
        {
            if (this.mSymbols != value)
            {
                this.mSymbols = value;
                this.shouldBeProcessed = true;
            }
        }
    }

    public string text
    {
        get
        {
            return this.mText;
        }
        set
        {
            if (this.mText != value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (!string.IsNullOrEmpty(this.mText))
                    {
                        this.mText = string.Empty;
                        this.MarkAsChanged();
                        this.ProcessAndRequest();
                    }
                }
                else if (this.mText != value)
                {
                    this.mText = value;
                    this.MarkAsChanged();
                    this.ProcessAndRequest();
                }
                if (base.autoResizeBoxCollider)
                {
                    base.ResizeCollider();
                }
            }
        }
    }

    public Font trueTypeFont
    {
        get
        {
            if (this.mTrueTypeFont != null)
            {
                return this.mTrueTypeFont;
            }
            return ((this.mFont == null) ? null : this.mFont.dynamicFont);
        }
        set
        {
            if (this.mTrueTypeFont != value)
            {
                this.SetActiveFont(null);
                base.RemoveFromPanel();
                this.mTrueTypeFont = value;
                this.shouldBeProcessed = true;
                this.mFont = null;
                this.SetActiveFont(value);
                this.ProcessAndRequest();
                if (this.mActiveTTF != null)
                {
                    base.MarkAsChanged();
                }
            }
        }
    }

    public override Vector3[] worldCorners
    {
        get
        {
            if (this.shouldBeProcessed)
            {
                this.ProcessText();
            }
            return base.worldCorners;
        }
    }

    public enum Crispness
    {
        Never,
        OnDesktop,
        Always
    }

    public enum Effect
    {
        None,
        Shadow,
        Outline
    }

    public enum Overflow
    {
        ShrinkContent,
        ClampContent,
        ResizeFreely,
        ResizeHeight
    }
}

