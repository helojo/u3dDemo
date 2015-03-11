using HutongGames.PlayMaker;
using System;

[Tooltip("Sets the sprite value of a UISprite"), ActionCategory("NGUI")]
public class NguiSetSprite : FsmStateAction
{
    [Tooltip("The name of the new sprite"), RequiredField]
    public FsmString NewSpriteName;
    [RequiredField, Tooltip("NGUI Sprite to set")]
    public FsmOwnerDefault NguiSprite;

    private void DoSetSprite()
    {
        if (((this.NguiSprite != null) && (this.NewSpriteName != null)) && !string.IsNullOrEmpty(this.NewSpriteName.Value))
        {
            UISprite component = base.Fsm.GetOwnerDefaultTarget(this.NguiSprite).GetComponent<UISprite>();
            if (component != null)
            {
                component.spriteName = this.NewSpriteName.Value;
                component.MakePixelPerfect();
            }
        }
    }

    public override void OnUpdate()
    {
        this.DoSetSprite();
        base.Finish();
    }

    public override void Reset()
    {
        this.NguiSprite = null;
        this.NewSpriteName = null;
    }
}

