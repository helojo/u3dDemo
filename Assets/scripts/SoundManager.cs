using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private string curMusicName = string.Empty;
    private Dictionary<string, AudioClip> m_bgmClipCache = new Dictionary<string, AudioClip>();
    private GameObject m_bgmObj;
    private float m_bgmVolume = 1f;
    private Dictionary<string, AudioClip> m_sfxclipCache = new Dictionary<string, AudioClip>();
    private float m_sfxVolume = 1f;
    private int maxCreateSfxNum;
    public static SoundManager mInstance;
    private static float SoundTransDurationSecond = 3f;

    private void Awake()
    {
        mInstance = this;
    }

    [DebuggerHidden]
    private IEnumerator DoPlayBGM(AudioClip ac, float duration)
    {
        return new <DoPlayBGM>c__IteratorB2 { duration = duration, ac = ac, <$>duration = duration, <$>ac = ac, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator DoResumeBGM(float duration)
    {
        return new <DoResumeBGM>c__IteratorB4 { duration = duration, <$>duration = duration, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator DoStopBGM(float duration)
    {
        return new <DoStopBGM>c__IteratorB3 { duration = duration, <$>duration = duration, <>f__this = this };
    }

    private static float linear(float time, float start, float delta, float duration)
    {
        return (((delta * time) / duration) + start);
    }

    private void OnDestroy()
    {
        mInstance = null;
    }

    public void PlayMusic(string name)
    {
        if (this.m_bgmObj == null)
        {
            this.m_bgmObj = new GameObject("music_bgm");
            this.m_bgmObj.transform.parent = base.transform;
        }
        if (this.curMusicName != name)
        {
            this.curMusicName = name;
            AudioClip clip = null;
            this.m_bgmClipCache.TryGetValue(name, out clip);
            if (clip == null)
            {
                clip = BundleMgr.Instance.LoadResource("BGM/" + name, ".mp3", typeof(AudioClip)) as AudioClip;
                this.m_bgmClipCache[name] = clip;
            }
            base.StartCoroutine(this.DoPlayBGM(clip, SoundTransDurationSecond));
        }
    }

    public AudioSource PlaySFX(string name)
    {
        return this.PlaySFX(name, "Sound", ".wav", true, false);
    }

    public AudioSource PlaySFX(string name, bool autoDestroy)
    {
        return this.PlaySFX(name, "Sound", ".wav", autoDestroy, false);
    }

    public AudioSource PlaySFX(string name, bool autoDestroy, bool cache)
    {
        return this.PlaySFX(name, "Sound", ".wav", autoDestroy, cache);
    }

    private AudioSource PlaySFX(string name, string folder, string soundType, bool autoDestroy, bool cache)
    {
        <PlaySFX>c__AnonStorey28E storeye = new <PlaySFX>c__AnonStorey28E();
        if (this.m_sfxVolume == 0f)
        {
            return null;
        }
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        AudioClip clip = null;
        this.m_sfxclipCache.TryGetValue(name, out clip);
        if (clip == null)
        {
            clip = BundleMgr.Instance.LoadResource(folder + "/" + name, ".wav", typeof(AudioClip)) as AudioClip;
            if (clip == null)
            {
                Debug.LogWarning("Sound Load Failed:" + name);
                return null;
            }
            this.m_sfxclipCache[name] = clip;
        }
        if (cache)
        {
            return null;
        }
        GameObject obj2 = new GameObject("sfx" + this.maxCreateSfxNum++);
        storeye.src = obj2.AddComponent<AudioSource>();
        storeye.src.clip = clip;
        storeye.src.volume = this.m_sfxVolume;
        storeye.src.Play();
        if (autoDestroy)
        {
            obj2.AddComponent<AutoDestroyObject>().m_match = new Predicate<AutoDestroyObject>(storeye.<>m__5ED);
        }
        return storeye.src;
    }

    public AudioSource PlayVoice(string name)
    {
        return this.PlaySFX(name, "Voice", ".ogg", true, false);
    }

    public void ResumeMusic(float duration)
    {
        base.StartCoroutine(this.DoResumeBGM(duration));
    }

    public void StopMusic(float duration)
    {
        base.StartCoroutine(this.DoStopBGM(duration));
    }

    [DebuggerHidden]
    private static IEnumerator transVolume(AudioSource src, float duration, float currentVol, float targetVol)
    {
        return new <transVolume>c__IteratorB1 { targetVol = targetVol, currentVol = currentVol, duration = duration, src = src, <$>targetVol = targetVol, <$>currentVol = currentVol, <$>duration = duration, <$>src = src };
    }

    public float BGMVolume
    {
        get
        {
            return this.m_bgmVolume;
        }
        set
        {
            this.m_bgmVolume = Mathf.Lerp(0f, 1f, Mathf.Clamp01(value));
            if (this.m_bgmObj != null)
            {
                this.m_bgmObj.GetComponent<AudioSource>().volume = this.m_bgmVolume;
            }
        }
    }

    public bool Mute
    {
        set
        {
            AudioListener.volume = !value ? ((float) 1) : ((float) 0);
        }
    }

    public float SFXVolume
    {
        get
        {
            return this.m_sfxVolume;
        }
        set
        {
            this.m_sfxVolume = value;
        }
    }

    [CompilerGenerated]
    private sealed class <DoPlayBGM>c__IteratorB2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal AudioClip <$>ac;
        internal float <$>duration;
        internal SoundManager <>f__this;
        internal IEnumerator <newVolumeEnum>__2;
        internal AudioSource <src>__0;
        internal IEnumerator <volumeEnum>__1;
        internal AudioClip ac;
        internal float duration;

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
                    this.<src>__0 = this.<>f__this.m_bgmObj.GetComponent<AudioSource>();
                    if (this.<src>__0 != null)
                    {
                        this.<volumeEnum>__1 = SoundManager.transVolume(this.<src>__0, this.duration, this.<src>__0.volume, 0f);
                        break;
                    }
                    this.<src>__0 = this.<>f__this.m_bgmObj.AddComponent<AudioSource>();
                    goto Label_00B6;

                case 1:
                    break;

                case 2:
                    goto Label_012D;

                default:
                    goto Label_0144;
            }
            while (this.<volumeEnum>__1.MoveNext())
            {
                this.$current = null;
                this.$PC = 1;
                goto Label_0146;
            }
        Label_00B6:
            this.<src>__0.clip = this.ac;
            this.<src>__0.volume = 0f;
            this.<src>__0.loop = true;
            this.<src>__0.Play();
            this.<newVolumeEnum>__2 = SoundManager.transVolume(this.<src>__0, this.duration, 0f, this.<>f__this.m_bgmVolume);
        Label_012D:
            while (this.<newVolumeEnum>__2.MoveNext())
            {
                this.$current = null;
                this.$PC = 2;
                goto Label_0146;
            }
            this.$PC = -1;
        Label_0144:
            return false;
        Label_0146:
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
    private sealed class <DoResumeBGM>c__IteratorB4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>duration;
        internal SoundManager <>f__this;
        internal AudioSource <src>__0;
        internal IEnumerator <volumeEnum>__1;
        internal float duration;

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
                    this.<src>__0 = this.<>f__this.m_bgmObj.GetComponent<AudioSource>();
                    if (this.<src>__0 == null)
                    {
                        goto Label_0097;
                    }
                    this.<volumeEnum>__1 = SoundManager.transVolume(this.<src>__0, this.duration, 0f, this.<>f__this.m_bgmVolume);
                    break;

                case 1:
                    break;

                default:
                    goto Label_009E;
            }
            if (this.<volumeEnum>__1.MoveNext())
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
        Label_0097:
            this.$PC = -1;
        Label_009E:
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

    [CompilerGenerated]
    private sealed class <DoStopBGM>c__IteratorB3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>duration;
        internal SoundManager <>f__this;
        internal AudioSource <src>__0;
        internal IEnumerator <volumeEnum>__1;
        internal float duration;

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
                    this.<src>__0 = this.<>f__this.m_bgmObj.GetComponent<AudioSource>();
                    if (this.<src>__0 == null)
                    {
                        goto Label_0097;
                    }
                    this.<volumeEnum>__1 = SoundManager.transVolume(this.<src>__0, this.duration, this.<src>__0.volume, 0f);
                    break;

                case 1:
                    break;

                default:
                    goto Label_009E;
            }
            if (this.<volumeEnum>__1.MoveNext())
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
        Label_0097:
            this.$PC = -1;
        Label_009E:
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

    [CompilerGenerated]
    private sealed class <PlaySFX>c__AnonStorey28E
    {
        internal AudioSource src;

        internal bool <>m__5ED(AutoDestroyObject o1)
        {
            return !this.src.isPlaying;
        }
    }

    [CompilerGenerated]
    private sealed class <transVolume>c__IteratorB1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>currentVol;
        internal float <$>duration;
        internal AudioSource <$>src;
        internal float <$>targetVol;
        internal float <delta>__0;
        internal float <t>__1;
        internal float <v>__2;
        internal float currentVol;
        internal float duration;
        internal AudioSource src;
        internal float targetVol;

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
                    this.<delta>__0 = this.targetVol - this.currentVol;
                    this.<t>__1 = 0f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_00DC;
            }
            if (this.<t>__1 <= this.duration)
            {
                this.<t>__1 += Time.deltaTime;
                this.<v>__2 = SoundManager.linear(this.<t>__1, this.currentVol, this.<delta>__0, this.duration);
                this.<v>__2 = Mathf.Clamp(this.<v>__2, 0f, Mathf.Max(this.currentVol, this.targetVol));
                this.src.volume = this.<v>__2;
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            this.$PC = -1;
        Label_00DC:
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

