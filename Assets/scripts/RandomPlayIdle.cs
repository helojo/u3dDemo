using System;
using UnityEngine;

public class RandomPlayIdle : MonoBehaviour
{
    private Animation anim;
    private AnimationType animationType;
    public bool bIsPlayBattleIdleAnim;
    public bool bStartPaly;
    private AnimationClip chusheng;
    private float curTime;
    private PlayType curType;
    private AnimationClip daiji;
    public string daijiChipName = "daiji";
    private float delayTime;
    private float fadeTime = 0.3f;
    public int randomData = 30;
    private AnimationClip xiuxian;
    public string xiuxianClipName = "xiuxian";
    public string zhanliClipName = "zhanli";

    public static RandomPlayIdle Begin(GameObject go, bool playIdle)
    {
        return Begin(go, playIdle, string.Empty, string.Empty, AnimationType.xiuxian);
    }

    public static RandomPlayIdle Begin(GameObject go, bool playIdle, AnimationType animType)
    {
        return Begin(go, playIdle, string.Empty, string.Empty, animType);
    }

    public static RandomPlayIdle Begin(GameObject go, bool playIdle, string zhanli, string xiuxian)
    {
        return Begin(go, playIdle, zhanli, xiuxian, AnimationType.xiuxian);
    }

    public static RandomPlayIdle Begin(GameObject go, bool playIdle, string zhanli, string xiuxian, AnimationType animType)
    {
        RandomPlayIdle component = go.GetComponent<RandomPlayIdle>();
        if (component == null)
        {
            component = go.AddComponent<RandomPlayIdle>();
        }
        else
        {
            component.enabled = true;
        }
        if (!string.IsNullOrEmpty(zhanli))
        {
            component.zhanliClipName = zhanli;
        }
        if (!string.IsNullOrEmpty(xiuxian))
        {
            component.xiuxianClipName = xiuxian;
        }
        component.StartPlay(playIdle, animType);
        return component;
    }

    private void FixedUpdate()
    {
        this.curTime += Time.deltaTime;
        if (((this.curType == PlayType.Null) || (this.curType == PlayType.Once)) && (this.curTime >= this.delayTime))
        {
            if (this.animationType == AnimationType.xiuxian)
            {
                this.pickRandomIdle();
            }
            else if (this.animationType == AnimationType.daiji)
            {
                this.pickDaiji();
            }
        }
    }

    private void OnEnable()
    {
        this.delayTime = 0f;
    }

    private void pickDaiji()
    {
        if (this.daiji != null)
        {
            this.anim.Blend(this.daiji.name, 0.3f);
            this.anim[this.daiji.name].wrapMode = WrapMode.Loop;
            this.delayTime = this.daiji.length;
            this.curTime = 0f;
        }
    }

    private void pickRandomIdle()
    {
        if ((UnityEngine.Random.Range(0, 100) > this.randomData) || (this.xiuxian == null))
        {
            if (this.daiji != null)
            {
                this.anim.Blend(this.daiji.name, 0.3f);
                this.anim[this.daiji.name].wrapMode = WrapMode.Loop;
                this.delayTime = this.daiji.length * UnityEngine.Random.Range(1, 3);
                this.curTime = 0f;
            }
        }
        else if (this.xiuxian != null)
        {
            this.anim.Blend(this.xiuxian.name, 0.3f);
            this.delayTime = this.xiuxian.length;
            this.curTime = 0f;
        }
    }

    public bool Play(string name, PlayType type)
    {
        this.curTime = 0f;
        this.curType = type;
        AnimationClip clip = this.anim.GetClip(name);
        if (clip == null)
        {
            return false;
        }
        this.anim.Blend(clip.name, 0.3f);
        this.anim[clip.name].wrapMode = WrapMode.Loop;
        if (type == PlayType.Once)
        {
            this.delayTime = clip.length;
        }
        return true;
    }

    public bool PlayAttackAnim()
    {
        for (int i = 1; i < 5; i++)
        {
            if (this.Play("gongji" + i.ToString(), PlayType.Once))
            {
                return true;
            }
        }
        return false;
    }

    public void PlayXiuxianAnim()
    {
        this.Play(this.xiuxianClipName, PlayType.Once);
    }

    private void Start()
    {
        if (this.bStartPaly)
        {
            this.StartPlay(true, AnimationType.xiuxian);
        }
    }

    public void StartPlay(bool playIdle, AnimationType _animType)
    {
        this.animationType = _animType;
        this.anim = base.GetComponent<Animation>();
        if (this.anim != null)
        {
            string name = string.Empty;
            this.daiji = this.anim.GetClip(this.zhanliClipName);
            if (this.daiji == null)
            {
                this.daiji = this.anim.GetClip(this.daijiChipName);
            }
            if (this.bIsPlayBattleIdleAnim)
            {
                this.xiuxian = this.anim.GetClip("xiuxian2");
                this.daiji = this.anim.GetClip("zhanli1");
            }
            else
            {
                this.xiuxian = this.anim.GetClip(this.xiuxianClipName);
            }
            this.chusheng = this.anim.GetClip("chusheng");
            if (playIdle && (this.chusheng != null))
            {
                this.anim.Play(this.chusheng.name, PlayMode.StopSameLayer);
                this.delayTime = this.chusheng.length;
                name = this.chusheng.name;
            }
            else if (this.daiji != null)
            {
                this.anim.Play(this.daiji.name, PlayMode.StopSameLayer);
                this.delayTime = this.daiji.length;
                name = this.daiji.name;
            }
            else
            {
                Debug.LogWarning("Can't Find Animation zhanli or daiji at:" + base.transform.name);
            }
            if (this.animationType == AnimationType.daiji)
            {
                this.anim[name].normalizedTime = UnityEngine.Random.Range((float) 0.1f, (float) 1f);
                this.anim[name].wrapMode = WrapMode.Loop;
            }
            this.curTime = 0f;
        }
    }

    public void Stop()
    {
        this.pickRandomIdle();
    }

    public enum AnimationType
    {
        xiuxian,
        daiji
    }

    public enum PlayType
    {
        Null,
        Once,
        Loop
    }
}

