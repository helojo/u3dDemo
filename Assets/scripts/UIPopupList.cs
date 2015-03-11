using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/Popup List")]
public class UIPopupList : UIWidgetContainer
{
    private const float animSpeed = 0.15f;
    public UIAtlas atlas;
    public Color backgroundColor = Color.white;
    public string backgroundSprite;
    public UIFont bitmapFont;
    public static UIPopupList current;
    [SerializeField, HideInInspector]
    private GameObject eventReceiver;
    [HideInInspector, SerializeField]
    private UIFont font;
    public int fontSize = 0x10;
    public FontStyle fontStyle;
    [SerializeField, HideInInspector]
    private string functionName = "OnSelectionChange";
    public Color highlightColor = new Color(0.8823529f, 0.7843137f, 0.5882353f, 1f);
    public string highlightSprite;
    public bool isAnimated = true;
    public bool isLocalized;
    public List<string> items = new List<string>();
    private UISprite mBackground;
    private float mBgBorder;
    private GameObject mChild;
    private UISprite mHighlight;
    private UILabel mHighlightedLabel;
    private List<UILabel> mLabelList = new List<UILabel>();
    private LegacyEvent mLegacyEvent;
    private UIPanel mPanel;
    [SerializeField, HideInInspector]
    private string mSelectedItem;
    private bool mTweening;
    private bool mUseDynamicFont;
    public List<EventDelegate> onChange = new List<EventDelegate>();
    public Vector2 padding = new Vector3(4f, 4f);
    public Position position;
    public Color textColor = Color.white;
    [SerializeField, HideInInspector]
    private UILabel textLabel;
    [HideInInspector, SerializeField]
    private float textScale;
    public Font trueTypeFont;

    private void Animate(UIWidget widget, bool placeAbove, float bottom)
    {
        this.AnimateColor(widget);
        this.AnimatePosition(widget, placeAbove, bottom);
    }

    private void AnimateColor(UIWidget widget)
    {
        Color color = widget.color;
        widget.color = new Color(color.r, color.g, color.b, 0f);
        TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
    }

    private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
    {
        Vector3 localPosition = widget.cachedTransform.localPosition;
        Vector3 vector2 = !placeAbove ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z);
        widget.cachedTransform.localPosition = vector2;
        TweenPosition.Begin(widget.gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
    }

    private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
    {
        GameObject gameObject = widget.gameObject;
        Transform cachedTransform = widget.cachedTransform;
        float num = (this.activeFontSize * this.activeFontScale) + (this.mBgBorder * 2f);
        cachedTransform.localScale = new Vector3(1f, num / ((float) widget.height), 1f);
        TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
        if (placeAbove)
        {
            Vector3 localPosition = cachedTransform.localPosition;
            cachedTransform.localPosition = new Vector3(localPosition.x, (localPosition.y - widget.height) + num, localPosition.z);
            TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
        }
    }

    public void Close()
    {
        if (this.mChild != null)
        {
            this.mLabelList.Clear();
            this.handleEvents = false;
            if (this.isAnimated)
            {
                UIWidget[] componentsInChildren = this.mChild.GetComponentsInChildren<UIWidget>();
                int index = 0;
                int length = componentsInChildren.Length;
                while (index < length)
                {
                    UIWidget widget = componentsInChildren[index];
                    Color color = widget.color;
                    color.a = 0f;
                    TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
                    index++;
                }
                Collider[] colliderArray = this.mChild.GetComponentsInChildren<Collider>();
                int num3 = 0;
                int num4 = colliderArray.Length;
                while (num3 < num4)
                {
                    colliderArray[num3].enabled = false;
                    num3++;
                }
                UnityEngine.Object.Destroy(this.mChild, 0.15f);
            }
            else
            {
                UnityEngine.Object.Destroy(this.mChild);
            }
            this.mBackground = null;
            this.mHighlight = null;
            this.mChild = null;
        }
    }

    private Vector3 GetHighlightPosition()
    {
        if (this.mHighlightedLabel == null)
        {
            return Vector3.zero;
        }
        UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
        if (atlasSprite == null)
        {
            return Vector3.zero;
        }
        float pixelSize = this.atlas.pixelSize;
        float num2 = atlasSprite.borderLeft * pixelSize;
        float y = atlasSprite.borderTop * pixelSize;
        return (this.mHighlightedLabel.cachedTransform.localPosition + new Vector3(-num2, y, 1f));
    }

    private void Highlight(UILabel lbl, bool instant)
    {
        if (this.mHighlight != null)
        {
            this.mHighlightedLabel = lbl;
            if (this.mHighlight.GetAtlasSprite() != null)
            {
                Vector3 highlightPosition = this.GetHighlightPosition();
                if (instant || !this.isAnimated)
                {
                    this.mHighlight.cachedTransform.localPosition = highlightPosition;
                }
                else
                {
                    TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
                    if (!this.mTweening)
                    {
                        this.mTweening = true;
                        base.StartCoroutine(this.UpdateTweenPosition());
                    }
                }
            }
        }
    }

    private void OnClick()
    {
        if (((base.enabled && NGUITools.GetActive(base.gameObject)) && ((this.mChild == null) && (this.atlas != null))) && (this.isValid && (this.items.Count > 0)))
        {
            this.mLabelList.Clear();
            if (this.mPanel == null)
            {
                this.mPanel = UIPanel.Find(base.transform);
                if (this.mPanel == null)
                {
                    return;
                }
            }
            this.handleEvents = true;
            Transform content = base.transform;
            Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(content.parent, content);
            this.mChild = new GameObject("Drop-down List");
            this.mChild.layer = base.gameObject.layer;
            Transform transform = this.mChild.transform;
            transform.parent = content.parent;
            transform.localPosition = bounds.min;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
            this.mBackground.pivot = UIWidget.Pivot.TopLeft;
            this.mBackground.depth = NGUITools.CalculateNextDepth(this.mPanel.gameObject);
            this.mBackground.color = this.backgroundColor;
            Vector4 border = this.mBackground.border;
            this.mBgBorder = border.y;
            this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
            this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
            this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
            this.mHighlight.color = this.highlightColor;
            UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
            if (atlasSprite != null)
            {
                float borderTop = atlasSprite.borderTop;
                float activeFontSize = this.activeFontSize;
                float activeFontScale = this.activeFontScale;
                float num4 = activeFontSize * activeFontScale;
                float a = 0f;
                float y = -this.padding.y;
                int num7 = (this.bitmapFont == null) ? this.fontSize : this.bitmapFont.defaultSize;
                List<UILabel> list = new List<UILabel>();
                int num8 = 0;
                int count = this.items.Count;
                while (num8 < count)
                {
                    string key = this.items[num8];
                    UILabel item = NGUITools.AddWidget<UILabel>(this.mChild);
                    item.name = num8.ToString();
                    item.pivot = UIWidget.Pivot.TopLeft;
                    item.bitmapFont = this.bitmapFont;
                    item.trueTypeFont = this.trueTypeFont;
                    item.fontSize = num7;
                    item.fontStyle = this.fontStyle;
                    item.text = !this.isLocalized ? key : Localization.Get(key);
                    item.color = this.textColor;
                    item.cachedTransform.localPosition = new Vector3(border.x + this.padding.x, y, -1f);
                    item.overflowMethod = UILabel.Overflow.ResizeFreely;
                    item.MakePixelPerfect();
                    if (activeFontScale != 1f)
                    {
                        item.cachedTransform.localScale = (Vector3) (Vector3.one * activeFontScale);
                    }
                    list.Add(item);
                    y -= num4;
                    y -= this.padding.y;
                    a = Mathf.Max(a, item.printedSize.x);
                    UIEventListener listener = UIEventListener.Get(item.gameObject);
                    listener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
                    listener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
                    listener.onClick = new UIEventListener.VoidDelegate(this.OnItemClick);
                    listener.parameter = key;
                    if ((this.mSelectedItem == key) || ((num8 == 0) && string.IsNullOrEmpty(this.mSelectedItem)))
                    {
                        this.Highlight(item, true);
                    }
                    this.mLabelList.Add(item);
                    num8++;
                }
                a = Mathf.Max(a, (bounds.size.x * activeFontScale) - ((border.x + this.padding.x) * 2f));
                float x = a / activeFontScale;
                Vector3 vector2 = new Vector3(x * 0.5f, -activeFontSize * 0.5f, 0f);
                Vector3 vector3 = new Vector3(x, (num4 + this.padding.y) / activeFontScale, 1f);
                int num11 = 0;
                int num12 = list.Count;
                while (num11 < num12)
                {
                    UILabel label2 = list[num11];
                    NGUITools.AddWidgetCollider(label2.gameObject);
                    BoxCollider component = label2.GetComponent<BoxCollider>();
                    if (component != null)
                    {
                        vector2.z = component.center.z;
                        component.center = vector2;
                        component.size = vector3;
                    }
                    else
                    {
                        BoxCollider2D colliderd = label2.GetComponent<BoxCollider2D>();
                        colliderd.center = vector2;
                        colliderd.size = vector3;
                    }
                    num11++;
                }
                a += (border.x + this.padding.x) * 2f;
                y -= border.y;
                this.mBackground.width = Mathf.RoundToInt(a);
                this.mBackground.height = Mathf.RoundToInt(-y + border.y);
                float num13 = 2f * this.atlas.pixelSize;
                float f = (a - ((border.x + this.padding.x) * 2f)) + (atlasSprite.borderLeft * num13);
                float num15 = num4 + (borderTop * num13);
                this.mHighlight.width = Mathf.RoundToInt(f);
                this.mHighlight.height = Mathf.RoundToInt(num15);
                bool placeAbove = this.position == Position.Above;
                if (this.position == Position.Auto)
                {
                    UICamera camera = UICamera.FindCameraForLayer(base.gameObject.layer);
                    if (camera != null)
                    {
                        placeAbove = camera.cachedCamera.WorldToViewportPoint(content.position).y < 0.5f;
                    }
                }
                if (this.isAnimated)
                {
                    float bottom = y + num4;
                    this.Animate(this.mHighlight, placeAbove, bottom);
                    int num17 = 0;
                    int num18 = list.Count;
                    while (num17 < num18)
                    {
                        this.Animate(list[num17], placeAbove, bottom);
                        num17++;
                    }
                    this.AnimateColor(this.mBackground);
                    this.AnimateScale(this.mBackground, placeAbove, bottom);
                }
                if (placeAbove)
                {
                    transform.localPosition = new Vector3(bounds.min.x, (bounds.max.y - y) - border.y, bounds.min.z);
                }
            }
        }
        else
        {
            this.OnSelect(false);
        }
    }

    private void OnEnable()
    {
        if (EventDelegate.IsValid(this.onChange))
        {
            this.eventReceiver = null;
            this.functionName = null;
        }
        if (this.font != null)
        {
            if (this.font.isDynamic)
            {
                this.trueTypeFont = this.font.dynamicFont;
                this.fontStyle = this.font.dynamicFontStyle;
                this.mUseDynamicFont = true;
            }
            else if (this.bitmapFont == null)
            {
                this.bitmapFont = this.font;
                this.mUseDynamicFont = false;
            }
            this.font = null;
        }
        if (this.textScale != 0f)
        {
            this.fontSize = (this.bitmapFont == null) ? 0x10 : Mathf.RoundToInt(this.bitmapFont.defaultSize * this.textScale);
            this.textScale = 0f;
        }
        if (((this.trueTypeFont == null) && (this.bitmapFont != null)) && this.bitmapFont.isDynamic)
        {
            this.trueTypeFont = this.bitmapFont.dynamicFont;
            this.bitmapFont = null;
        }
    }

    private void OnItemClick(GameObject go)
    {
        this.Close();
    }

    private void OnItemHover(GameObject go, bool isOver)
    {
        if (isOver)
        {
            UILabel component = go.GetComponent<UILabel>();
            this.Highlight(component, false);
        }
    }

    private void OnItemPress(GameObject go, bool isPressed)
    {
        if (isPressed)
        {
            this.Select(go.GetComponent<UILabel>(), true);
        }
    }

    private void OnKey(KeyCode key)
    {
        if ((base.enabled && NGUITools.GetActive(base.gameObject)) && this.handleEvents)
        {
            int index = this.mLabelList.IndexOf(this.mHighlightedLabel);
            if (index == -1)
            {
                index = 0;
            }
            if (key == KeyCode.UpArrow)
            {
                if (index > 0)
                {
                    this.Select(this.mLabelList[--index], false);
                }
            }
            else if (key == KeyCode.DownArrow)
            {
                if ((index + 1) < this.mLabelList.Count)
                {
                    this.Select(this.mLabelList[++index], false);
                }
            }
            else if (key == KeyCode.Escape)
            {
                this.OnSelect(false);
            }
        }
    }

    private void OnLocalize()
    {
        if (this.isLocalized)
        {
            this.TriggerCallbacks();
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (!isSelected)
        {
            this.Close();
        }
    }

    private void OnValidate()
    {
        Font trueTypeFont = this.trueTypeFont;
        UIFont bitmapFont = this.bitmapFont;
        this.bitmapFont = null;
        this.trueTypeFont = null;
        if ((trueTypeFont != null) && ((bitmapFont == null) || !this.mUseDynamicFont))
        {
            this.bitmapFont = null;
            this.trueTypeFont = trueTypeFont;
            this.mUseDynamicFont = true;
        }
        else if (bitmapFont != null)
        {
            if (bitmapFont.isDynamic)
            {
                this.trueTypeFont = bitmapFont.dynamicFont;
                this.fontStyle = bitmapFont.dynamicFontStyle;
                this.fontSize = bitmapFont.defaultSize;
                this.mUseDynamicFont = true;
            }
            else
            {
                this.bitmapFont = bitmapFont;
                this.mUseDynamicFont = false;
            }
        }
        else
        {
            this.trueTypeFont = trueTypeFont;
            this.mUseDynamicFont = true;
        }
    }

    private void Select(UILabel lbl, bool instant)
    {
        this.Highlight(lbl, instant);
        UIEventListener component = lbl.gameObject.GetComponent<UIEventListener>();
        this.value = component.parameter as string;
        UIPlaySound[] components = base.GetComponents<UIPlaySound>();
        int index = 0;
        int length = components.Length;
        while (index < length)
        {
            UIPlaySound sound = components[index];
            if (sound.trigger == UIPlaySound.Trigger.OnClick)
            {
                NGUITools.PlaySound(sound.audioClip, sound.volume, 1f);
            }
            index++;
        }
    }

    private void Start()
    {
        if (this.textLabel != null)
        {
            EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
            this.textLabel = null;
        }
        if (Application.isPlaying)
        {
            if (string.IsNullOrEmpty(this.mSelectedItem))
            {
                if (this.items.Count > 0)
                {
                    this.value = this.items[0];
                }
            }
            else
            {
                string mSelectedItem = this.mSelectedItem;
                this.mSelectedItem = null;
                this.value = mSelectedItem;
            }
        }
    }

    protected void TriggerCallbacks()
    {
        if (UIPopupList.current != this)
        {
            UIPopupList current = UIPopupList.current;
            UIPopupList.current = this;
            if (this.mLegacyEvent != null)
            {
                this.mLegacyEvent(this.mSelectedItem);
            }
            if (EventDelegate.IsValid(this.onChange))
            {
                EventDelegate.Execute(this.onChange);
            }
            else if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.functionName))
            {
                this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
            }
            UIPopupList.current = current;
        }
    }

    [DebuggerHidden]
    private IEnumerator UpdateTweenPosition()
    {
        return new <UpdateTweenPosition>c__Iterator0 { <>f__this = this };
    }

    private float activeFontScale
    {
        get
        {
            return (((this.trueTypeFont == null) && (this.bitmapFont != null)) ? (((float) this.fontSize) / ((float) this.bitmapFont.defaultSize)) : 1f);
        }
    }

    private int activeFontSize
    {
        get
        {
            return (((this.trueTypeFont == null) && (this.bitmapFont != null)) ? this.bitmapFont.defaultSize : this.fontSize);
        }
    }

    public UnityEngine.Object ambigiousFont
    {
        get
        {
            if (this.trueTypeFont != null)
            {
                return this.trueTypeFont;
            }
            if (this.bitmapFont != null)
            {
                return this.bitmapFont;
            }
            return this.font;
        }
        set
        {
            if (value is Font)
            {
                this.trueTypeFont = value as Font;
                this.bitmapFont = null;
                this.font = null;
            }
            else if (value is UIFont)
            {
                this.bitmapFont = value as UIFont;
                this.trueTypeFont = null;
                this.font = null;
            }
        }
    }

    private bool handleEvents
    {
        get
        {
            UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
            return ((component == null) || !component.enabled);
        }
        set
        {
            UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
            if (component != null)
            {
                component.enabled = !value;
            }
        }
    }

    public bool isOpen
    {
        get
        {
            return (this.mChild != null);
        }
    }

    private bool isValid
    {
        get
        {
            return ((this.bitmapFont != null) || (this.trueTypeFont != null));
        }
    }

    [Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
    public LegacyEvent onSelectionChange
    {
        get
        {
            return this.mLegacyEvent;
        }
        set
        {
            this.mLegacyEvent = value;
        }
    }

    [Obsolete("Use 'value' instead")]
    public string selection
    {
        get
        {
            return this.value;
        }
        set
        {
            this.value = value;
        }
    }

    public string value
    {
        get
        {
            return this.mSelectedItem;
        }
        set
        {
            this.mSelectedItem = value;
            if ((this.mSelectedItem != null) && (this.mSelectedItem != null))
            {
                this.TriggerCallbacks();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateTweenPosition>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal UIPopupList <>f__this;
        internal TweenPosition <tp>__0;

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
                    if ((this.<>f__this.mHighlight == null) || (this.<>f__this.mHighlightedLabel == null))
                    {
                        goto Label_00B2;
                    }
                    this.<tp>__0 = this.<>f__this.mHighlight.GetComponent<TweenPosition>();
                    break;

                case 1:
                    break;

                default:
                    goto Label_00C5;
            }
            if ((this.<tp>__0 != null) && this.<tp>__0.enabled)
            {
                this.<tp>__0.to = this.<>f__this.GetHighlightPosition();
                this.$current = null;
                this.$PC = 1;
                return true;
            }
        Label_00B2:
            this.<>f__this.mTweening = false;
            this.$PC = -1;
        Label_00C5:
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

    public delegate void LegacyEvent(string val);

    public enum Position
    {
        Auto,
        Above,
        Below
    }
}

