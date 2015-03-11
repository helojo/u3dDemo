using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CombineAnimation : MonoBehaviour
{
    public System.Action action_finish;
    private static Vector3 born_position = new Vector3(-5.449452f, 3.078305f, -22.03369f);
    private static Vector3 focus_position = new Vector3(-4.59f, 1.3f, 1.25f);
    private static Vector3 focus_rotation = new Vector3(-7.5f, -180f, 0f);
    private static Vector3 siphon_position = new Vector3(-8.275261f, 6.245327f, -13.77073f);
    private static Vector3 stand_position = new Vector3(-4.826851f, 1.747196f, -13.77073f);

    [DebuggerHidden]
    private IEnumerator AsyncDoAnimation(int entry)
    {
        return new <AsyncDoAnimation>c__Iterator5A { entry = entry, <$>entry = entry, <>f__this = this };
    }

    public void DoAnimaition(int entry)
    {
        base.StartCoroutine(this.AsyncDoAnimation(entry));
    }

    private float PlayDoorAnimation(string name, Animation anim)
    {
        AnimationState state = anim[name];
        if (null == state)
        {
            return 0f;
        }
        float length = state.clip.length;
        anim.Play(name, PlayMode.StopAll);
        return length;
    }

    [CompilerGenerated]
    private sealed class <AsyncDoAnimation>c__Iterator5A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>entry;
        internal CombineAnimation <>f__this;
        internal Animation <anim_door>__2;
        internal Vector3 <break_position>__25;
        internal Camera <camera>__0;
        internal Animation <card_anim>__5;
        internal GameObject <card_obj>__4;
        internal card_config <cfg>__3;
        internal Vector3 <cross>__15;
        internal Vector3 <dir>__14;
        internal float <dur>__19;
        internal GameObject <effect_break>__24;
        internal GameObject <effect_door>__20;
        internal GameObject <effect_siphon>__26;
        internal bool <enable_3dcamera>__8;
        internal GameObject <go_3drole>__7;
        internal GameObject <go_door>__1;
        internal float <len>__17;
        internal UIRoot <list_root>__10;
        internal Vector3[] <path>__18;
        internal Vector3 <pos_3drole>__6;
        internal Vector3 <pos>__12;
        internal Quaternion <rot>__13;
        internal AnimationState <run_state>__21;
        internal float <stand_dur>__23;
        internal AnimationState <stand_state>__22;
        internal TweenAlpha <twn_alpha>__11;
        internal UIRoot <ui_root>__9;
        internal Vector3 <wei>__16;
        internal int entry;

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
                    this.<camera>__0 = GUIMgr.Instance.Camera3D;
                    if (null != this.<camera>__0)
                    {
                        this.<go_door>__1 = GameObject.Find("GameObject/cj_xzjm2/jiuguan/men");
                        if (null != this.<go_door>__1)
                        {
                            this.<anim_door>__2 = this.<go_door>__1.GetComponent<Animation>();
                            if (null != this.<anim_door>__2)
                            {
                                this.<cfg>__3 = ConfigMgr.getInstance().getByEntry<card_config>(this.entry);
                                if (this.<cfg>__3 != null)
                                {
                                    this.<card_obj>__4 = CardPlayer.CreateCardPlayer(this.entry, null, CardPlayerStateType.Normal, this.<cfg>__3.quality);
                                    if (null != this.<card_obj>__4)
                                    {
                                        this.<card_obj>__4.SetActive(false);
                                        this.<card_anim>__5 = this.<card_obj>__4.GetComponent<Animation>();
                                        if (null != this.<card_anim>__5)
                                        {
                                            this.<pos_3drole>__6 = Vector3.zero;
                                            this.<go_3drole>__7 = GameObject.Find("juese/3DRole");
                                            if (null != this.<go_3drole>__7)
                                            {
                                                this.<pos_3drole>__6 = this.<go_3drole>__7.transform.position;
                                                this.<go_3drole>__7.transform.position = new Vector3(-1000f, -1000f, -1000f);
                                            }
                                            GUIMgr.Instance.NGUICamera.FlexAnimationLocked = true;
                                            GUIMgr.Instance.NGUIListCamera.FlexAnimationLocked = true;
                                            EasyTouch.instance.FlexAnimationLocked = true;
                                            this.<enable_3dcamera>__8 = this.<camera>__0.enabled;
                                            this.<camera>__0.enabled = true;
                                            this.<ui_root>__9 = GUIMgr.Instance.Root;
                                            this.<list_root>__10 = GUIMgr.Instance.ListRoot;
                                            this.<twn_alpha>__11 = TweenAlpha.Begin(this.<ui_root>__9.gameObject, 1f, 0f);
                                            this.<twn_alpha>__11.from = 1f;
                                            this.<twn_alpha>__11.to = 0f;
                                            this.<twn_alpha>__11 = TweenAlpha.Begin(this.<list_root>__10.gameObject, 1f, 0f);
                                            this.<twn_alpha>__11.from = 1f;
                                            this.<twn_alpha>__11.to = 0f;
                                            this.$current = new WaitForSeconds(0.7f);
                                            this.$PC = 1;
                                            goto Label_0A15;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                    break;

                case 1:
                {
                    this.<pos>__12 = this.<camera>__0.transform.position;
                    this.<rot>__13 = this.<camera>__0.transform.rotation;
                    Vector3 vector = CombineAnimation.focus_position - this.<pos>__12;
                    this.<dir>__14 = vector.normalized;
                    this.<cross>__15 = Vector3.Cross(Vector3.up, this.<dir>__14).normalized;
                    this.<wei>__16 = Vector3.Lerp(this.<pos>__12, CombineAnimation.focus_position, 0.35f);
                    Vector3 vector3 = this.<wei>__16 - this.<pos>__12;
                    this.<len>__17 = vector3.magnitude * 0.15f;
                    this.<wei>__16 += (Vector3) (this.<cross>__15 * this.<len>__17);
                    this.<path>__18 = new Vector3[] { this.<pos>__12, this.<wei>__16, CombineAnimation.focus_position };
                    object[] args = new object[] { "islocal", false, "path", this.<path>__18, "time", 2.2f, "easetype", iTween.EaseType.easeInOutQuad };
                    iTween.MoveTo(this.<camera>__0.gameObject, iTween.Hash(args));
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 2;
                    goto Label_0A15;
                }
                case 2:
                    this.<dur>__19 = this.<>f__this.PlayDoorAnimation("kai", this.<anim_door>__2);
                    this.<effect_door>__20 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/jiuguang_light_zhaohuan");
                    this.<effect_door>__20.transform.position = this.<go_door>__1.transform.position;
                    this.<effect_door>__20.transform.rotation = Quaternion.identity;
                    this.<effect_door>__20.transform.localScale = Vector3.one;
                    this.<card_obj>__4.SetActive(true);
                    this.<card_obj>__4.transform.position = CombineAnimation.born_position;
                    this.$current = new WaitForSeconds(this.<dur>__19 - 0.1f);
                    this.$PC = 3;
                    goto Label_0A15;

                case 3:
                {
                    this.<anim_door>__2.Stop();
                    this.<run_state>__21 = this.<card_anim>__5["xingzou"];
                    this.<run_state>__21.wrapMode = WrapMode.Loop;
                    this.<run_state>__21.speed = 0.5f;
                    this.<card_anim>__5.CrossFade("xingzou");
                    object[] objArray2 = new object[] { "islocal", false, "position", CombineAnimation.stand_position, "time", 1.5f, "easetype", iTween.EaseType.linear };
                    iTween.MoveTo(this.<card_obj>__4, iTween.Hash(objArray2));
                    this.$current = new WaitForSeconds(1.5f);
                    this.$PC = 4;
                    goto Label_0A15;
                }
                case 4:
                    UnityEngine.Object.Destroy(this.<effect_door>__20);
                    this.<stand_state>__22 = this.<card_anim>__5["xiuxian"];
                    this.<stand_state>__22.wrapMode = WrapMode.Once;
                    this.<stand_dur>__23 = this.<stand_state>__22.clip.length;
                    this.<card_anim>__5.CrossFade("xiuxian");
                    this.$current = new WaitForSeconds(this.<stand_dur>__23 - 0.1f);
                    this.$PC = 5;
                    goto Label_0A15;

                case 5:
                    this.<stand_state>__22 = this.<card_anim>__5["daiji"];
                    this.<stand_state>__22.wrapMode = WrapMode.Loop;
                    this.<card_anim>__5.Play("daiji");
                    this.$current = new WaitForSeconds(0.3f);
                    this.$PC = 6;
                    goto Label_0A15;

                case 6:
                    this.<effect_break>__24 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/chouka_tx_suipian");
                    this.<effect_break>__24.transform.parent = this.<card_obj>__4.transform;
                    this.<effect_break>__24.transform.localPosition = new Vector3(0f, 0.4795697f, 0.6913967f);
                    this.<effect_break>__24.transform.localScale = Vector3.one;
                    this.<effect_break>__24.transform.rotation = Quaternion.identity;
                    this.<break_position>__25 = this.<effect_break>__24.transform.position;
                    this.$current = new WaitForSeconds(1.1f);
                    this.$PC = 7;
                    goto Label_0A15;

                case 7:
                {
                    this.<effect_siphon>__26 = BundleMgr.Instance.CreateEffectObject("BattlePrefabs/Diaoluo_Tx");
                    this.<effect_siphon>__26.transform.position = this.<break_position>__25;
                    this.<effect_siphon>__26.transform.localScale = Vector3.one;
                    this.<effect_siphon>__26.transform.rotation = Quaternion.identity;
                    object[] objArray3 = new object[] { "islocal", false, "position", CombineAnimation.siphon_position, "time", 0.7f, "easetype", iTween.EaseType.easeOutCirc };
                    iTween.MoveTo(this.<effect_siphon>__26, iTween.Hash(objArray3));
                    this.$current = new WaitForSeconds(0.1f);
                    this.$PC = 8;
                    goto Label_0A15;
                }
                case 8:
                    UnityEngine.Object.Destroy(this.<card_obj>__4);
                    UnityEngine.Object.Destroy(this.<effect_break>__24);
                    this.$current = new WaitForSeconds(0.3f);
                    this.$PC = 9;
                    goto Label_0A15;

                case 9:
                    this.<twn_alpha>__11 = TweenAlpha.Begin(this.<ui_root>__9.gameObject, 1f, 1f);
                    this.<twn_alpha>__11.from = 0f;
                    this.<twn_alpha>__11.to = 1f;
                    this.<twn_alpha>__11 = TweenAlpha.Begin(this.<list_root>__10.gameObject, 1f, 1f);
                    this.<twn_alpha>__11.from = 0f;
                    this.<twn_alpha>__11.to = 1f;
                    this.$current = new WaitForSeconds(0.4f);
                    this.$PC = 10;
                    goto Label_0A15;

                case 10:
                {
                    UnityEngine.Object.Destroy(this.<effect_siphon>__26);
                    object[] objArray4 = new object[] { "islocal", false, "position", this.<pos>__12, "time", 1f, "easetype", iTween.EaseType.easeInOutQuad };
                    iTween.MoveTo(this.<camera>__0.gameObject, iTween.Hash(objArray4));
                    this.$current = new WaitForSeconds(0.7f);
                    this.$PC = 11;
                    goto Label_0A15;
                }
                case 11:
                    this.<camera>__0.enabled = this.<enable_3dcamera>__8;
                    GUIMgr.Instance.NGUICamera.FlexAnimationLocked = false;
                    GUIMgr.Instance.NGUIListCamera.FlexAnimationLocked = false;
                    EasyTouch.instance.FlexAnimationLocked = false;
                    if (null != this.<go_3drole>__7)
                    {
                        this.<go_3drole>__7.transform.position = this.<pos_3drole>__6;
                    }
                    if (this.<>f__this.action_finish != null)
                    {
                        this.<>f__this.action_finish();
                    }
                    this.$current = null;
                    this.$PC = 12;
                    goto Label_0A15;

                case 12:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0A15:
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
}

