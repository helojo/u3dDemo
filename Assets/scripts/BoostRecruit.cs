using FastBuf;
using Newbie;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class BoostRecruit
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache9;
    [CompilerGenerated]
    private static System.Action <>f__am$cacheA;
    [CompilerGenerated]
    private static System.Action <>f__am$cacheB;
    [CompilerGenerated]
    private static System.Action <>f__am$cacheC;
    private GameObject animDummy;
    private GameObject[] avatar_list;
    private bool[] chip_list;
    private GameObject director1;
    private GameObject director2;
    private Texture2D[] frame_list;
    private Dictionary<int, LotteryCardDiscount> freeDic = new Dictionary<int, LotteryCardDiscount>();
    private Texture2D[] tex_list;
    public bool valid;

    private void ClearObjects()
    {
        GameObject obj2 = GameObject.Find("avatar");
        if (null != obj2)
        {
            int childCount = obj2.transform.childCount;
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i != childCount; i++)
            {
                Transform child = obj2.transform.GetChild(i);
                list.Add(child.gameObject);
            }
            foreach (GameObject obj3 in list)
            {
                obj3.transform.parent = null;
                UnityEngine.Object.Destroy(obj3);
            }
        }
    }

    public int FreeCount(int entry)
    {
        LotteryCardDiscount discount = null;
        if (!this.freeDic.TryGetValue(entry, out discount))
        {
            return 0;
        }
        return discount.free_count;
    }

    public bool FreeTime(int opt_entry, out long time)
    {
        time = 0L;
        if (this.freeDic.ContainsKey(opt_entry))
        {
            LotteryCardDiscount discount = this.freeDic[opt_entry];
            long num = discount.free_cd;
            if (num < 0L)
            {
                return false;
            }
            long num2 = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
            if (num <= num2)
            {
                return true;
            }
            time = num - num2;
        }
        return false;
    }

    public void FreeTime(int entry, int cd, int free_count)
    {
        LotteryCardDiscount discount = null;
        if (this.freeDic.TryGetValue(entry, out discount))
        {
            discount.free_cd = cd;
            discount.free_count = free_count;
        }
    }

    private void HangupHandEffect(Transform hang_point, string effect)
    {
        GameObject obj2 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/" + effect);
        if (null != obj2)
        {
            obj2.transform.parent = hang_point;
            obj2.transform.localScale = Vector3.one;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.rotation = Quaternion.identity;
        }
    }

    public void HiddenLastResult()
    {
        <HiddenLastResult>c__AnonStorey254 storey = new <HiddenLastResult>c__AnonStorey254();
        this.ClearObjects();
        storey.go_group = GameObject.Find("point");
        if (null != storey.go_group)
        {
            GameObject obj2 = GameObject.Find("shadow");
            if (null != obj2)
            {
                for (int j = 0; j != 10; j++)
                {
                    Transform transform = obj2.transform.FindChild(j.ToString());
                    if (null != transform)
                    {
                        transform.gameObject.SetActive(false);
                    }
                }
            }
            storey.ori_position = storey.go_group.transform.position;
            object[] args = new object[] { "islocal", false, "position", storey.ori_position - new Vector3(0f, 5f, 0f), "time", 0.3f, "easetype", iTween.EaseType.easeInQuart };
            iTween.MoveTo(storey.go_group, iTween.Hash(args));
            for (int i = 0; i != storey.go_group.transform.childCount; i++)
            {
                Transform child = storey.go_group.transform.GetChild(i);
                TextMesh component = child.FindChild("num").GetComponent<TextMesh>();
                if (null != component)
                {
                    component.text = string.Empty;
                }
                Vector3 localPosition = child.transform.localPosition;
                child.transform.localPosition = new Vector3(localPosition.x, 2.3398f, localPosition.z);
            }
            ScheduleMgr.Schedule(0.48f, new System.Action(storey.<>m__53F));
        }
    }

    public void OnQuit()
    {
        EasyTouch.On_LongTapStart -= new EasyTouch.LongTapStartHandler(this.OnTouchDown);
        EasyTouch.On_LongTapEnd -= new EasyTouch.LongTapEndHandler(this.OnTouchUp);
    }

    private void OnTouchDown(Gesture gesture)
    {
        Ray ray = Camera.main.ScreenPointToRay((Vector3) gesture.position);
        GameObject obj2 = GameObject.Find("point");
        if (((null != obj2) && (GUIMgr.Instance.GetGUIEntity<MessageBox>() == null)) && (GUIMgr.Instance.GetGUIEntity<SharePanel>() == null))
        {
            RaycastHit hit;
            if (GUIMgr.Instance.GetGUIEntity<ItemInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
            }
            if ((Physics.Raycast(ray, out hit, 1000f) && (null != hit.collider)) && (null == UICamera.hoveredObject))
            {
                if (hit.collider.transform.parent == obj2.transform)
                {
                    <OnTouchDown>c__AnonStorey255 storey = new <OnTouchDown>c__AnonStorey255 {
                        ri_entry = hit.collider.GetComponent<RecruitItemEntry>()
                    };
                    if (null != storey.ri_entry)
                    {
                        GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storey.<>m__540), null);
                    }
                }
                else
                {
                    <OnTouchDown>c__AnonStorey256 storey2 = new <OnTouchDown>c__AnonStorey256 {
                        rc_entry = hit.collider.transform.parent.GetComponent<RecruitCardEntry>()
                    };
                    if (null != storey2.rc_entry)
                    {
                        GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storey2.<>m__541), null);
                    }
                }
            }
        }
    }

    private void OnTouchUp(Gesture gesture)
    {
        GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
    }

    public void RefreshAllSaleRemainTimes(List<LotteryCardDiscount> discounts)
    {
        int count = discounts.Count;
        for (int i = 0; i != count; i++)
        {
            LotteryCardDiscount discount = discounts[i];
            if (!this.freeDic.ContainsKey(discount.entry))
            {
                this.freeDic.Add(discount.entry, null);
            }
            this.freeDic[discount.entry] = discount;
        }
        this.valid = true;
    }

    public void RegisterTouchEvent()
    {
        EasyTouch.SetlongTapTime(0.05f);
        EasyTouch.On_LongTapStart += new EasyTouch.LongTapStartHandler(this.OnTouchDown);
        EasyTouch.On_LongTapEnd += new EasyTouch.LongTapEndHandler(this.OnTouchUp);
    }

    public void Reset()
    {
        this.freeDic.Clear();
        this.valid = false;
    }

    public void ShowRecruitResult(List<int> card_list, List<int> item_list, List<int> item_num_list, List<int> morph_list, List<int> morph_num_list, bool idle)
    {
        <ShowRecruitResult>c__AnonStorey257 storey = new <ShowRecruitResult>c__AnonStorey257 {
            idle = idle,
            card_list = card_list,
            <>f__this = this
        };
        GameDataMgr.Instance.boostRecruit.valid = true;
        if (storey.idle)
        {
            this.ClearObjects();
        }
        float num = 0f;
        float num2 = 0f;
        float num3 = 0f;
        storey.anim_idle = string.Empty;
        storey.anim_action = string.Empty;
        storey.multi_action = (storey.card_list.Count + item_list.Count) > 1;
        switch (RecruitPanel.actived_function)
        {
            case RecruitPanel.function.stone:
                num = 0f;
                storey.anim_idle = "zhanli02";
                storey.anim_action = !storey.multi_action ? "chouka01" : "chouka02";
                break;

            case RecruitPanel.function.gold:
                num = 1f;
                storey.anim_idle = "zhanli01";
                storey.anim_action = !storey.multi_action ? "chouka01" : "chouka02";
                break;

            case RecruitPanel.function.soul:
                num = 0.3f;
                num2 = -1f;
                num3 = 0.5f;
                storey.anim_idle = "zhanli02";
                storey.anim_action = "chouka03";
                break;
        }
        storey.go_group = GameObject.Find("point");
        if (null != storey.go_group)
        {
            this.OnQuit();
            this.RegisterTouchEvent();
            if (!storey.idle)
            {
                this.director1 = GameObject.Find("1/CK_xjl");
                this.director2 = GameObject.Find("2/Ck_d");
            }
            if ((null != this.director1) && (null != this.director2))
            {
                GameObject obj2 = null;
                switch (RecruitPanel.actived_function)
                {
                    case RecruitPanel.function.stone:
                    case RecruitPanel.function.soul:
                        this.director2.SetActive(false);
                        obj2 = this.director1;
                        break;

                    default:
                        this.director1.SetActive(false);
                        obj2 = this.director2;
                        break;
                }
                storey.hand_r = obj2.transform.FindChild("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/Dummy001");
                storey.hand_l = obj2.transform.FindChild("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand/Dummy002");
                storey.anim = obj2.GetComponent<Animation>();
                if (null != storey.anim)
                {
                    storey.state = storey.anim[storey.anim_action];
                    float length = storey.state.clip.length;
                    ScheduleMgr.Schedule(1f, new System.Action(storey.<>m__542));
                    int num5 = storey.card_list.Count + item_list.Count;
                    this.tex_list = new Texture2D[num5];
                    this.frame_list = new Texture2D[num5];
                    this.avatar_list = new GameObject[num5];
                    this.chip_list = new bool[num5];
                    int index = 0;
                    foreach (int num7 in storey.card_list)
                    {
                        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(num7);
                        if (_config != null)
                        {
                            GameObject obj3 = CardPlayer.CreateCardPlayer(_config.entry, null, CardPlayerStateType.Normal, _config.quality);
                            obj3.SetActive(false);
                            this.chip_list[index] = false;
                            this.avatar_list[index++] = obj3;
                            RecruitCardEntry component = obj3.GetComponent<RecruitCardEntry>();
                            if (null == component)
                            {
                                component = obj3.AddComponent<RecruitCardEntry>();
                            }
                            component.entry = num7;
                            int num8 = morph_list.IndexOf(num7);
                            if (num8 >= 0)
                            {
                                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(num7, 1);
                                if (cardExCfg != null)
                                {
                                    component.morph_entry = cardExCfg.item_entry;
                                    component.morph_num = morph_num_list[num8];
                                }
                            }
                            int childCount = obj3.transform.childCount;
                            for (int i = 0; i != childCount; i++)
                            {
                                Transform child = obj3.transform.GetChild(i);
                                SkinnedMeshRenderer renderer = child.GetComponent<SkinnedMeshRenderer>();
                                if ((null != renderer) && (null != renderer.sharedMesh))
                                {
                                    MeshCollider collider = child.gameObject.AddComponent<MeshCollider>();
                                    collider.sharedMesh = renderer.sharedMesh;
                                    collider.convex = true;
                                }
                            }
                        }
                    }
                    int num11 = 0;
                    foreach (int num12 in item_list)
                    {
                        item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(num12);
                        if (_config3 != null)
                        {
                            Texture2D textured = null;
                            if (_config3.type == 3)
                            {
                                textured = BundleMgr.Instance.CreateHeadIcon(_config3.icon);
                            }
                            else
                            {
                                textured = BundleMgr.Instance.CreateItemIcon(_config3.icon);
                            }
                            this.tex_list[index] = textured;
                            this.chip_list[index] = (_config3.type == 3) || (2 == _config3.type);
                            GameObject obj4 = RecruitAnimation.FindHangPoint(index, item_list.Count, storey.go_group);
                            RecruitItemEntry entry2 = obj4.GetComponent<RecruitItemEntry>();
                            if (null == entry2)
                            {
                                entry2 = obj4.AddComponent<RecruitItemEntry>();
                            }
                            entry2.entry = num12;
                            TextMesh mesh = obj4.transform.FindChild("num").GetComponent<TextMesh>();
                            if ((null != mesh) && (num11 < item_num_list.Count))
                            {
                                mesh.text = item_num_list[num11].ToString();
                            }
                            string str = string.Empty;
                            switch (_config3.quality)
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
                            num11++;
                            this.frame_list[index++] = textured;
                        }
                    }
                    if ((item_list.Count + storey.card_list.Count) <= 1)
                    {
                        ScheduleMgr.Schedule(0.8f + num, new System.Action(storey.<>m__543));
                    }
                    else
                    {
                        if (<>f__am$cache9 == null)
                        {
                            <>f__am$cache9 = delegate {
                                string str = (RecruitPanel.actived_function != RecruitPanel.function.soul) ? "choukatexiao_2_1" : "choujiang3_tx";
                                GameObject obj2 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/" + str);
                                if (null != obj2)
                                {
                                    obj2.transform.position = new Vector3(0f, 2f, 7f);
                                    obj2.transform.rotation = Quaternion.identity;
                                    obj2.transform.localScale = Vector3.one;
                                    UnityEngine.Object.Destroy(obj2, 10f);
                                }
                            };
                        }
                        ScheduleMgr.Schedule(((2.2f + num) + num2) + num3, <>f__am$cache9);
                        if (RecruitPanel.actived_function == RecruitPanel.function.soul)
                        {
                            ScheduleMgr.Schedule(1f + num, new System.Action(storey.<>m__545));
                        }
                        else
                        {
                            if (<>f__am$cacheA == null)
                            {
                                <>f__am$cacheA = delegate {
                                    GameObject obj2 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/choukatexiao_2_3");
                                    GameObject obj3 = GameObject.Find("effect/0");
                                    if ((null != obj2) && (null != obj3))
                                    {
                                        obj2.transform.parent = obj3.transform;
                                        obj2.transform.localPosition = Vector3.zero;
                                        obj2.transform.rotation = Quaternion.identity;
                                        obj2.transform.localScale = Vector3.one;
                                    }
                                };
                            }
                            ScheduleMgr.Schedule((3.8f + num) + num2, <>f__am$cacheA);
                            if (<>f__am$cacheB == null)
                            {
                                <>f__am$cacheB = delegate {
                                    GameObject obj2 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/choukatexiao_2_3");
                                    GameObject obj3 = GameObject.Find("effect/1");
                                    if ((null != obj2) && (null != obj3))
                                    {
                                        obj2.transform.parent = obj3.transform;
                                        obj2.transform.localPosition = Vector3.zero;
                                        obj2.transform.rotation = Quaternion.identity;
                                        obj2.transform.localScale = Vector3.one;
                                    }
                                };
                            }
                            ScheduleMgr.Schedule((4.3f + num) + num2, <>f__am$cacheB);
                            if (<>f__am$cacheC == null)
                            {
                                <>f__am$cacheC = delegate {
                                    GameObject obj2 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/choukatexiao_2_3");
                                    GameObject obj3 = GameObject.Find("effect/2");
                                    if ((null != obj2) && (null != obj3))
                                    {
                                        obj2.transform.parent = obj3.transform;
                                        obj2.transform.localPosition = Vector3.zero;
                                        obj2.transform.rotation = Quaternion.identity;
                                        obj2.transform.localScale = Vector3.one;
                                    }
                                };
                            }
                            ScheduleMgr.Schedule((4.8f + num) + num2, <>f__am$cacheC);
                        }
                    }
                    ScheduleMgr.Schedule(((length * 0.45f) + 1f) + num, new System.Action(storey.<>m__549));
                    ScheduleMgr.Schedule(((length + num) + num3) + 1.5f, new System.Action(storey.<>m__54A));
                    ScheduleMgr.Schedule(length + 1f, new System.Action(storey.<>m__54B));
                }
            }
        }
    }

    private RecruitAnimation Animator
    {
        get
        {
            if (null == this.animDummy)
            {
                this.animDummy = new GameObject();
                UnityEngine.Object.DontDestroyOnLoad(this.animDummy);
                return this.animDummy.AddComponent<RecruitAnimation>();
            }
            return this.animDummy.GetComponent<RecruitAnimation>();
        }
    }

    [CompilerGenerated]
    private sealed class <HiddenLastResult>c__AnonStorey254
    {
        internal GameObject go_group;
        internal Vector3 ori_position;

        internal void <>m__53F()
        {
            for (int i = 0; i != 10; i++)
            {
                Transform transform = this.go_group.transform.FindChild(i.ToString());
                if (null != transform)
                {
                    transform.gameObject.SetActive(false);
                }
            }
            this.go_group.transform.position = this.ori_position;
        }
    }

    [CompilerGenerated]
    private sealed class <OnTouchDown>c__AnonStorey255
    {
        internal RecruitItemEntry ri_entry;

        internal void <>m__540(GUIEntity entity)
        {
            ItemInfoPanel panel = entity as ItemInfoPanel;
            Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(this.ri_entry.entry);
            panel.UpdateData(itemByEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <OnTouchDown>c__AnonStorey256
    {
        internal RecruitCardEntry rc_entry;

        internal void <>m__541(GUIEntity entity)
        {
            (entity as ItemInfoPanel).ShowCardInfo(this.rc_entry.entry, 1, true);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowRecruitResult>c__AnonStorey257
    {
        internal BoostRecruit <>f__this;
        internal Animation anim;
        internal string anim_action;
        internal string anim_idle;
        internal List<int> card_list;
        internal GameObject go_group;
        internal Transform hand_l;
        internal Transform hand_r;
        internal bool idle;
        internal bool multi_action;
        internal AnimationState state;

        internal void <>m__542()
        {
            if (this.idle)
            {
                this.anim.CrossFade(this.anim_action);
            }
            else
            {
                this.anim.Play(this.anim_action);
            }
            SoundManager.mInstance.PlaySFX("sound_choujiang");
            if ((null != this.hand_r) && (null != this.hand_l))
            {
                switch (RecruitPanel.actived_function)
                {
                    case RecruitPanel.function.stone:
                        if (!this.multi_action)
                        {
                            this.<>f__this.HangupHandEffect(this.hand_r, "chouka_r_hand");
                            break;
                        }
                        this.<>f__this.HangupHandEffect(this.hand_l, "chouka_l_hand");
                        this.<>f__this.HangupHandEffect(this.hand_r, "chouka_r_hand");
                        break;

                    case RecruitPanel.function.gold:
                        if (!this.multi_action)
                        {
                            this.<>f__this.HangupHandEffect(this.hand_l, "chouka_l_hand");
                            break;
                        }
                        this.<>f__this.HangupHandEffect(this.hand_l, "chouka_l_hand");
                        this.<>f__this.HangupHandEffect(this.hand_r, "chouka_r_hand");
                        break;

                    case RecruitPanel.function.soul:
                        this.<>f__this.HangupHandEffect(this.hand_r, "choujiang3_tx_Lhand");
                        break;
                }
            }
        }

        internal void <>m__543()
        {
            GameObject gameObject = this.go_group.transform.FindChild("5").gameObject;
            if (null != gameObject)
            {
                GameObject obj3 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/choukatexiao");
                if (null != obj3)
                {
                    obj3.transform.position = gameObject.transform.position;
                    obj3.transform.rotation = Quaternion.identity;
                    obj3.transform.localScale = Vector3.one;
                    UnityEngine.Object.Destroy(obj3, 10f);
                }
            }
        }

        internal void <>m__545()
        {
            GameObject gameObject = this.go_group.transform.FindChild("5").gameObject;
            if (null != gameObject)
            {
                GameObject obj3 = BundleMgr.Instance.CreateEffectObject("EffectPrefabs/choukatexiao");
                if (null != obj3)
                {
                    obj3.transform.position = gameObject.transform.position;
                    obj3.transform.rotation = Quaternion.identity;
                    obj3.transform.localScale = Vector3.one;
                    UnityEngine.Object.Destroy(obj3, 10f);
                }
            }
        }

        internal void <>m__549()
        {
            this.<>f__this.Animator.ToDo(this.<>f__this.tex_list, this.<>f__this.frame_list, this.<>f__this.avatar_list, this.<>f__this.chip_list);
        }

        internal void <>m__54A()
        {
            GUIMgr.Instance.DoModelGUI("RecruitResultPanel", delegate (GUIEntity obj) {
                RecruitResultPanel panel = (RecruitResultPanel) obj;
                panel._ShareBtn.SetActive((this.card_list.Count >= 1) && SharePanel.IsCanShare());
                if (GuideSystem.MatchEvent(GuideEvent.GoldRecruit) || GuideSystem.MatchEvent(GuideEvent.StoneRecruit))
                {
                    GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Recruit.tag_recruit_comeback, panel.CloseButton);
                }
            }, null);
        }

        internal void <>m__54B()
        {
            this.state = this.anim[this.anim_idle];
            this.state.wrapMode = WrapMode.Loop;
            this.anim.Blend(this.anim_idle, 0.3f);
        }

        internal void <>m__54C(GUIEntity obj)
        {
            RecruitResultPanel panel = (RecruitResultPanel) obj;
            panel._ShareBtn.SetActive((this.card_list.Count >= 1) && SharePanel.IsCanShare());
            if (GuideSystem.MatchEvent(GuideEvent.GoldRecruit) || GuideSystem.MatchEvent(GuideEvent.StoneRecruit))
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Recruit.tag_recruit_comeback, panel.CloseButton);
            }
        }
    }

    private class ExpandInfo
    {
        [HideInInspector]
        public int boost;
        [HideInInspector]
        public int need;
    }
}

