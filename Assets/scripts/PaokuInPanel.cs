using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class PaokuInPanel : GUIPanelEntity
{
    public static PaokuInPanel _instance;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache1E;
    private guildrun_character_config grchConfig = new guildrun_character_config();
    private guildrun_config guildrunConfig = new guildrun_config();
    private float hpValue;
    private bool isClick;
    private bool isDead = true;
    private float prValue;
    private IEnumerator refresher;

    private void Awake()
    {
        _instance = this;
    }

    public void BeginCoroutine()
    {
        if (this.refresher == null)
        {
            this.refresher = this.SkillCDAnim();
        }
        else
        {
            this.refresher = this.SkillCDAnim();
        }
        base.StartCoroutine(this.refresher);
    }

    public override void Initialize()
    {
        base.Initialize();
        this.grchConfig = ConfigMgr.getInstance().getByEntry<guildrun_character_config>(ActorData.getInstance().paokuCardEntry);
        this.guildrunConfig = ConfigMgr.getInstance().getByEntry<guildrun_config>(ActorData.getInstance().paokuMapEntry);
        if (<>f__am$cache1E == null)
        {
            <>f__am$cache1E = delegate (object u) {
                ParkourManager._instance.Pause(true);
                GUIMgr.Instance.DoModelGUI("PaokuPausePanel", null, null);
            };
        }
        this.btn_pause.OnUIMouseClick(<>f__am$cache1E);
        this.skill_group.OnUIMouseClick(delegate (object u) {
            if (this.isClick)
            {
                ParkourEvent._instance.UseSkill();
                this.isClick = false;
                this.ui_nuqi.gameObject.SetActive(false);
                this.ui_touxiang_star.gameObject.SetActive(false);
                this.Ui_nuqi_shunjian.gameObject.SetActive(false);
                this.skill_Border.gameObject.SetActive(true);
                this.skill_Border_hight.gameObject.SetActive(false);
                this.sp_Mask.gameObject.SetActive(true);
                this.BeginCoroutine();
            }
        });
        if (this.grchConfig != null)
        {
            this.tt_skill_icon.mainTexture = BundleMgr.Instance.CreateSkillIcon(this.grchConfig.skill_icon);
            this.tt_play_icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(this.grchConfig.character_icon);
            this.sp_Mask.gameObject.SetActive(true);
            this.BeginCoroutine();
        }
        if (this.grchConfig != null)
        {
            this.UpdataHp((float) this.grchConfig.character_hp);
        }
        this.UpdateBoxAndGold(ParkourEvent._instance.baoxiangCount, ParkourEvent._instance.goldCoinCount);
        this.isDead = true;
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.btn_pause = base.FindChild<UIButton>("btn_pause");
        this.HP_Progress = base.FindChild<UISlider>("HP_Progress");
        this.hp_Foreground1 = base.FindChild<UISprite>("hp_Foreground1");
        this.PrSider = base.FindChild<UISlider>("PrSider");
        this.sp_Foreground = base.FindChild<UISprite>("sp_Foreground");
        this.pr_bg = base.FindChild<UISprite>("pr_bg");
        this.player = base.FindChild<Transform>("player");
        this.sp_paly_Faction = base.FindChild<UISprite>("sp_paly_Faction");
        this.tt_play_icon = base.FindChild<UITexture>("tt_play_icon");
        this.sp_play_Kuang = base.FindChild<UISprite>("sp_play_Kuang");
        this.Box = base.FindChild<Transform>("Box");
        this.lb_box_count = base.FindChild<UILabel>("lb_box_count");
        this.skill_group = base.FindChild<TweenRotation>("skill_group");
        this.tt_skill_icon = base.FindChild<UITexture>("tt_skill_icon");
        this.skill_Border_hight = base.FindChild<UISprite>("skill_Border_hight");
        this.sp_Mask = base.FindChild<UISprite>("sp_Mask");
        this.ui_touxiang_star = base.FindChild<Transform>("ui_touxiang_star");
        this.ui_nuqi = base.FindChild<RenderQueueSetter>("ui_nuqi");
        this.Ui_nuqi_shunjian = base.FindChild<Transform>("Ui_nuqi_shunjian");
        this.skill_Border = base.FindChild<UISprite>("skill_Border");
        this.Gold = base.FindChild<Transform>("Gold");
        this.lb_gold_count = base.FindChild<UILabel>("lb_gold_count");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.grchConfig != null)
        {
            this.hp_Foreground1.fillAmount = this.hpValue / ((float) this.grchConfig.character_hp);
            if (this.hp_Foreground1.fillAmount <= 0f)
            {
                if (this.isDead)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186d6));
                }
                this.isDead = false;
            }
        }
        this.sp_Foreground.fillAmount = this.prValue;
        float num = this.sp_Foreground.width * this.sp_Foreground.fillAmount;
        this.player.transform.localPosition = new Vector3(num - 300f, 233f, 0f);
    }

    [DebuggerHidden]
    public IEnumerator PlayAngryIcon()
    {
        return new <PlayAngryIcon>c__Iterator82 { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator SkillCDAnim()
    {
        return new <SkillCDAnim>c__Iterator81 { <>f__this = this };
    }

    public void UpdataHp(float hpValue)
    {
        this.hpValue = hpValue;
    }

    public void UpdataProgress(float prValue)
    {
        this.prValue = prValue;
    }

    public void UpdateBoxAndGold(int boxCount, int goldCount)
    {
        if (this.guildrunConfig != null)
        {
            this.lb_box_count.text = boxCount.ToString() + "/" + this.guildrunConfig.max_box_num;
        }
        this.lb_gold_count.text = goldCount.ToString();
    }

    protected Transform Box { get; set; }

    protected UIButton btn_pause { get; set; }

    protected Transform Gold { get; set; }

    protected UISprite hp_Foreground1 { get; set; }

    protected UISlider HP_Progress { get; set; }

    protected UILabel lb_box_count { get; set; }

    protected UILabel lb_gold_count { get; set; }

    protected Transform player { get; set; }

    protected UISprite pr_bg { get; set; }

    protected UISlider PrSider { get; set; }

    protected UISprite skill_Border { get; set; }

    protected UISprite skill_Border_hight { get; set; }

    protected TweenRotation skill_group { get; set; }

    protected UISprite sp_Foreground { get; set; }

    protected UISprite sp_Mask { get; set; }

    protected UISprite sp_paly_Faction { get; set; }

    protected UISprite sp_play_Kuang { get; set; }

    protected UITexture tt_play_icon { get; set; }

    protected UITexture tt_skill_icon { get; set; }

    protected RenderQueueSetter ui_nuqi { get; set; }

    protected Transform Ui_nuqi_shunjian { get; set; }

    protected Transform ui_touxiang_star { get; set; }

    [CompilerGenerated]
    private sealed class <PlayAngryIcon>c__Iterator82 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal PaokuInPanel <>f__this;

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
                    if (this.<>f__this.refresher != null)
                    {
                        this.<>f__this.StopCoroutine(this.<>f__this.refresher);
                    }
                    this.<>f__this.isClick = true;
                    this.<>f__this.skill_Border.gameObject.SetActive(false);
                    this.<>f__this.skill_Border_hight.gameObject.SetActive(true);
                    this.<>f__this.ui_nuqi.gameObject.SetActive(true);
                    this.<>f__this.ui_touxiang_star.gameObject.SetActive(true);
                    this.<>f__this.Ui_nuqi_shunjian.gameObject.SetActive(true);
                    this.<>f__this.sp_Mask.gameObject.SetActive(false);
                    TweenAlpha.Begin(this.<>f__this.tt_skill_icon.gameObject, 0.2f, 0f);
                    TweenScale.Begin(this.<>f__this.tt_skill_icon.gameObject, 0.2f, new Vector3(1.5f, 1.5f, 1.5f)).method = UITweener.Method.EaseInOut;
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    goto Label_01BA;

                case 1:
                    TweenAlpha.Begin(this.<>f__this.tt_skill_icon.gameObject, 0.1f, 1f).method = UITweener.Method.Linear;
                    TweenScale.Begin(this.<>f__this.tt_skill_icon.gameObject, 0.1f, Vector3.one).method = UITweener.Method.Linear;
                    this.$current = new WaitForSeconds(0.3f);
                    this.$PC = 2;
                    goto Label_01BA;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_01BA:
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

    [CompilerGenerated]
    private sealed class <SkillCDAnim>c__Iterator81 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal PaokuInPanel <>f__this;
        internal float <cdTime>__0;

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
                    if (this.<>f__this.grchConfig == null)
                    {
                        goto Label_00AF;
                    }
                    this.<cdTime>__0 = this.<>f__this.grchConfig.skillcd_time;
                    break;

                case 1:
                    break;

                default:
                    goto Label_00CD;
            }
            if (this.<cdTime>__0 > 0f)
            {
                this.<cdTime>__0 -= Time.deltaTime;
                this.<>f__this.sp_Mask.fillAmount = this.<cdTime>__0 / ((float) this.<>f__this.grchConfig.skillcd_time);
                this.$current = 0;
                this.$PC = 1;
                return true;
            }
        Label_00AF:
            this.<>f__this.StartCoroutine(this.<>f__this.PlayAngryIcon());
            this.$PC = -1;
        Label_00CD:
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

