using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public static SoundManager GetInstance() => Instance;

    public AudioSource backgroundMusicPlayer;
    public AudioSource UISEPlayer;
    public GameObject effectSoundPlayer;
    public UnityEngine.Audio.AudioMixerGroup audioMixer;
    [Header("°´Å¥ÒôÐ§ËØ²Ä")]
    public AudioClip moveOnSE;
    public AudioClip clickSE;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void PlayBGM(AudioClip bgm)
    {
        backgroundMusicPlayer.clip = bgm;
        backgroundMusicPlayer.Play();
    }

    public void PlaySE(AudioClip se)
    {
        StartCoroutine("SoundEffectOnce", se);
    }

    IEnumerator SoundEffectOnce(AudioClip se)
    {
        float startTime = Time.fixedTime;
        AudioSource tmpAS = effectSoundPlayer.AddComponent<AudioSource>();
        tmpAS.clip = se;
        tmpAS.outputAudioMixerGroup = audioMixer;
        tmpAS.Play();
        while (Time.fixedTime - startTime < se.length)
        {
            yield return new WaitForSeconds(1f);
        }
        Destroy(tmpAS);
    }

    public void PlayUISE(UISEType type)
    {
        switch (type)
        {
            case UISEType.Click:
                UISEPlayer.PlayOneShot(clickSE);
                break;
            case UISEType.MoveOn:
                UISEPlayer.PlayOneShot(moveOnSE);
                break;
        }
    }

    public enum UISEType
    {
        MoveOn,
        Click,
    }
}
