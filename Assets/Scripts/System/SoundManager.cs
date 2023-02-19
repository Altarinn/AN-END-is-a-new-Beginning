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
    [Header("��ť��Ч�ز�")]
    public AudioClip moveOnSE;
    public AudioClip clickSE;
    public AudioClip bgm1;
    public AudioClip bgm2;
    public AudioClip bgm3;
    public bool silence;
    string lastroom = "";


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        PlayBGM();
    }

    private void Update()
    {
        
    }

    void PlayBGM()
    {
        if (GameController.Instance.currentRoom != null) {
            if (GameController.Instance.currentRoom.key != lastroom) {
                lastroom = GameController.Instance.currentRoom.key;
                if (lastroom.Contains('F')) {
                    backgroundMusicPlayer.Stop();
                    backgroundMusicPlayer.clip = bgm3;
                }
                else {
                    backgroundMusicPlayer.clip = GameController.Instance.IsPhantom? bgm2:bgm1;
                }
            }
        }

        if (!backgroundMusicPlayer.isPlaying)
        { 
            Debug.Log(lastroom);
            if(!silence) {
                backgroundMusicPlayer.Play(); 
            }
            
        }
        Invoke("PlayBGM", 1); 
    }

    public void StopBGM()
    {
        if(backgroundMusicPlayer.isPlaying)
        {
            backgroundMusicPlayer.Stop();
        }
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
