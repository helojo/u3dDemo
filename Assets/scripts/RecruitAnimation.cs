using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RecruitAnimation : MonoBehaviour
{
    [DebuggerHidden]
    private IEnumerator DoAnimation(Texture2D[] tex_list, Texture2D[] frame_list, GameObject[] avatar_list, bool[] chip_list)
    {
        return new <DoAnimation>c__IteratorA2 { tex_list = tex_list, frame_list = frame_list, avatar_list = avatar_list, chip_list = chip_list, <$>tex_list = tex_list, <$>frame_list = frame_list, <$>avatar_list = avatar_list, <$>chip_list = chip_list, <>f__this = this };
    }

    public static GameObject FindHangPoint(int index, int count, GameObject group)
    {
        if (count <= 1)
        {
            return group.transform.FindChild("5").gameObject;
        }
        if (RecruitPanel.actived_function == RecruitPanel.function.soul)
        {
            index += 2;
        }
        return group.transform.FindChild(index.ToString()).gameObject;
    }

    public static void ItemObjectMorph(GameObject go, int entry, int num)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(entry);
        if (_config != null)
        {
            Texture2D textured = null;
            if (_config.type == 3)
            {
                textured = BundleMgr.Instance.CreateHeadIcon(_config.icon);
            }
            else
            {
                textured = BundleMgr.Instance.CreateItemIcon(_config.icon);
            }
            go.SetActive(true);
            Vector3 localPosition = go.transform.localPosition;
            go.transform.localPosition = new Vector3(localPosition.x, 2.3398f, localPosition.z);
            go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            bool flag = (_config.type == 2) || (3 == _config.type);
            go.transform.FindChild("chip").gameObject.SetActive(flag);
            go.transform.FindChild("chip/image").GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
            RecruitItemEntry component = go.GetComponent<RecruitItemEntry>();
            if (null == component)
            {
                component = go.AddComponent<RecruitItemEntry>();
            }
            component.entry = entry;
            MeshRenderer renderer = go.transform.FindChild("icon").GetComponent<MeshRenderer>();
            if (null != renderer)
            {
                if (null == textured)
                {
                    renderer.enabled = false;
                }
                else
                {
                    renderer.enabled = true;
                    renderer.material.mainTexture = textured;
                    float x = 0.8f;
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(entry);
                    if ((_config2 != null) && (_config2.type == 3))
                    {
                        x = 0.92f;
                    }
                    renderer.transform.localPosition = Vector3.zero;
                    renderer.transform.localScale = new Vector3(x, x, 1f);
                    renderer.transform.localRotation = Quaternion.identity;
                }
                TextMesh mesh = go.transform.FindChild("num").GetComponent<TextMesh>();
                if (null != mesh)
                {
                    mesh.text = (num <= 1) ? string.Empty : num.ToString();
                }
                string str = string.Empty;
                switch (_config.quality)
                {
                    case 0:
                        str = "Ui_Zhaomu_Frame_white";
                        break;

                    case 1:
                        str = "Ui_Zhaomu_Frame_green";
                        break;

                    case 2:
                    case 3:
                        str = "Ui_Zhaomu_Frame_blue";
                        break;

                    default:
                        str = "Ui_Zhaomu_Frame_purple";
                        break;
                }
                textured = BundleMgr.Instance.CreateTextureObject("GUI/Texture/" + str);
                renderer = go.transform.FindChild("frame").GetComponent<MeshRenderer>();
                if (null != renderer)
                {
                    renderer.material.mainTexture = textured;
                    OnMoveOver(go);
                }
            }
        }
    }

    private static void OnMoveOver(GameObject go)
    {
        GameObject obj2 = GameObject.Find("avatar");
        if (null != obj2)
        {
            RecruitItemEntry component = go.GetComponent<RecruitItemEntry>();
            if (null != component)
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(component.entry);
                if (_config != null)
                {
                    if (_config.type == 3)
                    {
                        GameObject obj3 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/chouka_juese");
                        obj3.transform.parent = obj2.transform;
                        obj3.transform.position = go.transform.position;
                        obj3.transform.localRotation = Quaternion.identity;
                        obj3.transform.localScale = Vector3.one;
                    }
                    else if (_config.quality >= 4)
                    {
                        GameObject obj4 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/chouka_wupin");
                        obj4.transform.parent = obj2.transform;
                        obj4.transform.position = go.transform.position;
                        obj4.transform.localRotation = Quaternion.identity;
                        obj4.transform.localScale = Vector3.one;
                    }
                }
            }
        }
    }

    private void OnMoveUp(GameObject go)
    {
        <OnMoveUp>c__AnonStorey251 storey = new <OnMoveUp>c__AnonStorey251 {
            go = go
        };
        Vector3 position = storey.go.transform.position;
        position.y = 2.34f;
        object[] args = new object[] { "islocal", false, "position", position, "time", 0.5f, "easetype", iTween.EaseType.easeInQuart };
        iTween.MoveTo(storey.go, iTween.Hash(args));
        object[] objArray2 = new object[] { "scale", new Vector3(1f, 1f, 1f), "time", 0.5f, "easetype", iTween.EaseType.easeInQuart };
        iTween.ScaleTo(storey.go, iTween.Hash(objArray2));
        ScheduleMgr.Schedule(0.5f, new System.Action(storey.<>m__535));
        ScheduleMgr.Schedule(0.7f, new System.Action(storey.<>m__536));
    }

    private void ShowCard(GameObject[] avatar_list)
    {
    }

    private GameObject ShowEffect(string name, float time)
    {
        <ShowEffect>c__AnonStorey252 storey = new <ShowEffect>c__AnonStorey252 {
            effect = BundleMgr.Instance.CreateEffectObject(name)
        };
        ScheduleMgr.Schedule(time, new System.Action(storey.<>m__537));
        return storey.effect;
    }

    [DebuggerHidden]
    private IEnumerator ShowShadow(Texture2D[] frame_list)
    {
        return new <ShowShadow>c__IteratorA1 { frame_list = frame_list, <$>frame_list = frame_list };
    }

    public void ToDo(Texture2D[] tex_list, Texture2D[] frame_list, GameObject[] avatar_list, bool[] chip_list)
    {
        base.StartCoroutine(this.DoAnimation(tex_list, frame_list, avatar_list, chip_list));
        base.StartCoroutine("ShowShadow", frame_list);
    }

    [CompilerGenerated]
    private sealed class <DoAnimation>c__IteratorA2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject[] <$>avatar_list;
        internal bool[] <$>chip_list;
        internal Texture2D[] <$>frame_list;
        internal Texture2D[] <$>tex_list;
        private static System.Action <>f__am$cache1E;
        internal RecruitAnimation <>f__this;
        internal GameObject <avatar>__7;
        internal GameObject <avt_group>__1;
        internal bool <chip>__8;
        internal int <count>__3;
        internal Texture2D <frame>__6;
        internal GameObject <go_effect>__18;
        internal GameObject <go_group>__0;
        internal GameObject <go>__9;
        internal int <i>__4;
        internal item_config <item_cfg>__15;
        internal RecruitItemEntry <item_entry>__14;
        internal Material <mat>__11;
        internal TextMesh <num_text>__16;
        internal int <num>__17;
        internal Vector3 <pos>__10;
        internal MeshRenderer <render>__12;
        internal float <scale>__13;
        internal Texture2D <tex>__5;
        internal int <total>__2;
        internal GameObject[] avatar_list;
        internal bool[] chip_list;
        internal Texture2D[] frame_list;
        internal Texture2D[] tex_list;

        internal void <>m__538()
        {
            this.<count>__3 = this.avatar_list.Length;
            for (int i = 0; i != this.<count>__3; i++)
            {
                <DoAnimation>c__AnonStorey253 storey = new <DoAnimation>c__AnonStorey253 {
                    <>f__ref$162 = this,
                    avatar = this.avatar_list[i]
                };
                if (null != storey.avatar)
                {
                    storey.go = RecruitAnimation.FindHangPoint(i, this.<count>__3, this.<go_group>__0);
                    storey.pos = storey.go.transform.position;
                    storey.pos.y -= 0.5f;
                    Vector3 pos = storey.pos;
                    pos.y--;
                    storey.avatar.transform.parent = GameObject.Find("avatar").transform;
                    storey.avatar.transform.position = pos;
                    storey.avatar.transform.localRotation = Quaternion.identity;
                    storey.avatar.SetActive(true);
                    RandomPlayIdle.Begin(storey.avatar, true);
                    object[] args = new object[] { "islocal", false, "position", storey.pos, "time", 1.2f, "easetype", iTween.EaseType.spring };
                    iTween.MoveTo(storey.avatar, iTween.Hash(args));
                    ScheduleMgr.Schedule(0.1f * i, new System.Action(storey.<>m__539));
                    storey.ri_entry = storey.avatar.GetComponent<RecruitCardEntry>();
                    if (((null != storey.ri_entry) && (storey.ri_entry.morph_entry >= 0)) && (storey.ri_entry.morph_num > 0))
                    {
                        ScheduleMgr.Schedule(0.9f, new System.Action(storey.<>m__53A));
                        ScheduleMgr.Schedule(1.7f, new System.Action(storey.<>m__53B));
                        ScheduleMgr.Schedule(1.8f, new System.Action(storey.<>m__53C));
                        ScheduleMgr.Schedule(2.2f, new System.Action(storey.<>m__53D));
                    }
                    if (<>f__am$cache1E == null)
                    {
                        <>f__am$cache1E = () => SoundManager.mInstance.PlaySFX("sound_danchou");
                    }
                    ScheduleMgr.Schedule(0.2f, <>f__am$cache1E);
                }
            }
        }

        private static void <>m__53E()
        {
            SoundManager.mInstance.PlaySFX("sound_danchou");
        }

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
                    this.<go_group>__0 = GameObject.Find("point");
                    if (null != this.<go_group>__0)
                    {
                        this.<avt_group>__1 = GameObject.Find("avatar");
                        if (null != this.<avt_group>__1)
                        {
                            this.<total>__2 = 0;
                            this.<count>__3 = this.tex_list.Length;
                            this.<i>__4 = 0;
                            while (this.<i>__4 != this.<count>__3)
                            {
                                this.<tex>__5 = this.tex_list[this.<i>__4];
                                this.<frame>__6 = this.frame_list[this.<i>__4];
                                this.<avatar>__7 = this.avatar_list[this.<i>__4];
                                this.<chip>__8 = this.chip_list[this.<i>__4];
                                if (null == this.<avatar>__7)
                                {
                                    this.<go>__9 = RecruitAnimation.FindHangPoint(this.<i>__4, this.<count>__3, this.<go_group>__0);
                                    if (null != this.<go>__9)
                                    {
                                        this.<pos>__10 = this.<go>__9.transform.position;
                                        this.<pos>__10.y -= 1.3f;
                                        this.<go>__9.transform.position = this.<pos>__10;
                                        this.<go>__9.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                                        this.<go>__9.SetActive(true);
                                        this.<go>__9.transform.FindChild("chip").gameObject.SetActive(this.<chip>__8);
                                        if (this.<chip>__8)
                                        {
                                            this.<mat>__11 = this.<go>__9.transform.FindChild("chip/image").GetComponent<Renderer>().material;
                                            this.<mat>__11.color = new Color(1f, 1f, 1f, 1f);
                                        }
                                        this.<render>__12 = this.<go>__9.transform.FindChild("icon").GetComponent<MeshRenderer>();
                                        if (null != this.<render>__12)
                                        {
                                            if (null == this.<tex>__5)
                                            {
                                                this.<render>__12.enabled = false;
                                            }
                                            else
                                            {
                                                this.<render>__12.enabled = true;
                                                this.<render>__12.material.mainTexture = this.<tex>__5;
                                                this.<scale>__13 = 0.8f;
                                                this.<item_entry>__14 = this.<go>__9.GetComponent<RecruitItemEntry>();
                                                if (null != this.<item_entry>__14)
                                                {
                                                    this.<item_cfg>__15 = ConfigMgr.getInstance().getByEntry<item_config>(this.<item_entry>__14.entry);
                                                    if ((this.<item_cfg>__15 != null) && (this.<item_cfg>__15.type == 3))
                                                    {
                                                        this.<scale>__13 = 0.92f;
                                                    }
                                                }
                                                this.<render>__12.transform.localPosition = Vector3.zero;
                                                this.<render>__12.transform.localScale = new Vector3(this.<scale>__13, this.<scale>__13, 1f);
                                                this.<render>__12.transform.localRotation = Quaternion.identity;
                                            }
                                            this.<num_text>__16 = this.<go>__9.transform.FindChild("num").GetComponent<TextMesh>();
                                            if ((null != this.<num_text>__16) && !string.IsNullOrEmpty(this.<num_text>__16.text))
                                            {
                                                this.<num>__17 = Convert.ToInt32(this.<num_text>__16.text);
                                                this.<num_text>__16.text = (this.<num>__17 <= 1) ? string.Empty : this.<num>__17.ToString();
                                            }
                                            this.<render>__12 = this.<go>__9.transform.FindChild("frame").GetComponent<MeshRenderer>();
                                            if (null != this.<render>__12)
                                            {
                                                this.<render>__12.material.mainTexture = this.<frame>__6;
                                                if (this.<count>__3 > 1)
                                                {
                                                    this.<go_effect>__18 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/choukatexiao_2_2");
                                                    if (null != this.<go_effect>__18)
                                                    {
                                                        this.<go_effect>__18.transform.parent = this.<avt_group>__1.transform;
                                                        this.<go_effect>__18.transform.position = this.<go>__9.transform.position;
                                                        this.<go_effect>__18.transform.rotation = Quaternion.identity;
                                                        this.<go_effect>__18.transform.localScale = Vector3.one;
                                                    }
                                                }
                                                this.<pos>__10.y += 2.6f;
                                                iTween.FadeFrom(this.<go>__9, 0f, 0.25f);
                                                object[] args = new object[] { "islocal", false, "position", this.<pos>__10, "time", 0.699999988079071 - (0.035 * this.<i>__4), "easetype", iTween.EaseType.easeOutQuint, "oncompletetarget", this.<>f__this.gameObject, "oncomplete", "OnMoveUp", "oncompleteparams", this.<go>__9 };
                                                iTween.MoveTo(this.<go>__9, iTween.Hash(args));
                                                object[] objArray2 = new object[] { "scale", new Vector3(1.4f, 1.4f, 1f), "time", 0.949999988079071 - (0.035 * this.<i>__4), "easetype", iTween.EaseType.easeOutQuart };
                                                iTween.ScaleTo(this.<go>__9, iTween.Hash(objArray2));
                                                SoundManager.mInstance.PlaySFX("sound_danchou");
                                                this.<total>__2++;
                                                this.$current = new WaitForSeconds(0.2f - (this.<i>__4 * 0.0155f));
                                                this.$PC = 1;
                                                goto Label_06B7;
                                            }
                                        }
                                    }
                                }
                            Label_065D:
                                this.<i>__4++;
                            }
                            ScheduleMgr.Schedule(0.1f * this.<total>__2, new System.Action(this.<>m__538));
                            this.$current = null;
                            this.$PC = 2;
                            goto Label_06B7;
                        }
                        break;
                    }
                    break;

                case 1:
                    goto Label_065D;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_06B7:
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

        private sealed class <DoAnimation>c__AnonStorey253
        {
            internal RecruitAnimation.<DoAnimation>c__IteratorA2 <>f__ref$162;
            internal GameObject avatar;
            internal GameObject go;
            internal Vector3 pos;
            internal RecruitCardEntry ri_entry;

            internal void <>m__539()
            {
                this.<>f__ref$162.<>f__this.ShowEffect("EffectPrefabs/chouka_tx_zhujiao", 3f).transform.position = this.pos;
            }

            internal void <>m__53A()
            {
                this.<>f__ref$162.<>f__this.ShowEffect("EffectPrefabs/chouka_tx_suipian", 3f).transform.position = new Vector3(this.pos.x, this.pos.y + 0.8f, this.pos.z + 1f);
            }

            internal void <>m__53B()
            {
                this.avatar.SetActive(false);
            }

            internal void <>m__53C()
            {
                this.<>f__ref$162.<>f__this.ShowEffect("EffectPrefabs/chouka_tx_suibian01", 3f).transform.position = this.pos;
            }

            internal void <>m__53D()
            {
                RecruitAnimation.ItemObjectMorph(this.go, this.ri_entry.morph_entry, this.ri_entry.morph_num);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnMoveUp>c__AnonStorey251
    {
        internal GameObject go;

        internal void <>m__535()
        {
            iTween.PunchRotation(this.go, new Vector3(10f, 0f, 10f), 1f);
            iTween.PunchPosition(this.go, new Vector3(0f, 0.3f, 0f), 0.8f);
        }

        internal void <>m__536()
        {
            RecruitAnimation.OnMoveOver(this.go);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowEffect>c__AnonStorey252
    {
        internal GameObject effect;

        internal void <>m__537()
        {
            UnityEngine.Object.Destroy(this.effect);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowShadow>c__IteratorA1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Texture2D[] <$>frame_list;
        internal int <count>__1;
        internal int <i>__2;
        internal GameObject <shadow_group>__0;
        internal GameObject <shadow>__3;
        internal Texture2D[] frame_list;

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
                    this.<shadow_group>__0 = GameObject.Find("shadow");
                    if (null != this.<shadow_group>__0)
                    {
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 1;
                        goto Label_0189;
                    }
                    goto Label_0187;

                case 1:
                    this.<count>__1 = this.frame_list.Length;
                    this.<i>__2 = 0;
                    goto Label_015C;

                case 2:
                    break;

                case 3:
                    this.$PC = -1;
                    goto Label_0187;

                default:
                    goto Label_0187;
            }
        Label_014E:
            this.<i>__2++;
        Label_015C:
            if (this.<i>__2 != this.<count>__1)
            {
                if (null != this.frame_list[this.<i>__2])
                {
                    this.<shadow>__3 = RecruitAnimation.FindHangPoint(this.<i>__2, this.<count>__1, this.<shadow_group>__0);
                    if (null == this.<shadow>__3)
                    {
                        goto Label_014E;
                    }
                    this.<shadow>__3.SetActive(true);
                    this.<shadow>__3.transform.localScale = new Vector3(0.001f, 0.001f, 1f);
                    iTween.ScaleTo(this.<shadow>__3, new Vector3(0.5f, 0.5f, 1f), 1.7f);
                }
                this.$current = new WaitForSeconds(0.2f - (this.<i>__2 * 0.0155f));
                this.$PC = 2;
            }
            else
            {
                this.$current = null;
                this.$PC = 3;
            }
            goto Label_0189;
        Label_0187:
            return false;
        Label_0189:
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

