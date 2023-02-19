using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public Transform UIContainer;
    public GameObject tipsPrefab;

    public GameObject HPBar, HPBarPh;

    public Sprite mute, unmute, shake, unshake;
    public Image muteBtn, shakeBtn;
    public bool muted = false;
    public bool shaked = true;

    public GameObject timeup, dead, clear;
    public TMPro.TextMeshProUGUI time;

    public static UIManager GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(UIContainer.gameObject);
    }


    public void OpenMenu(GameObject openMenu)
    {
        openMenu?.SetActive(true);
    }

    public void CloseMenu(GameObject closeMenu)
    {
        closeMenu?.SetActive(false);
    }

    public void InstantiateMenu(GameObject openMenu)
    {
        Instantiate(openMenu, UIContainer);
    }

    public void DestroyMenu(GameObject destroyMenu)
    {
        Destroy(destroyMenu);
    }

    public void RefreshState()
    {
        HPBar.SetActive(!GameController.Instance.IsPhantom);
        HPBarPh.SetActive(GameController.Instance.IsPhantom);
    }

    public void TimeUp()
    {
        timeup.SetActive(true);
        DOVirtual.DelayedCall(0.5f, () => timeup.SetActive(false));
    }

    public void Dead()
    {
        dead.SetActive(true);
        DOVirtual.DelayedCall(0.5f, () => dead.SetActive(false));
    }

    public void Clear()
    {
        clear.SetActive(true);
        DOVirtual.DelayedCall(0.5f, () => clear.SetActive(false));
    }

    public void SetTime(int timeVal)
    {
        time.text = timeVal.ToString("D2");
    }

    public void ToggleMute()
    {
        muted = !muted;
        //SoundManager.Instance.backgroundMusicPlayer.mute = !SoundManager.Instance.backgroundMusicPlayer.mute;

        if(muted)
        {
            muteBtn.sprite = mute;
            AudioListener.volume = 0;
        }
        else
        {
            muteBtn.sprite = unmute;
            AudioListener.volume = 1;
        }
    }

    public void ToggleShake()
    {
        shaked = !shaked;
        if (shaked)
        {
            shakeBtn.sprite = shake;
        }
        else
        {
            shakeBtn.sprite = unshake;
        }
    }

    public void GoRetry()
    {
        GameController.Instance.RestartLevel();
    }

    public void GoRetryPlayer()
    {
        GameController.Instance.RestartLevelAsPlayer();
    }

    public GameObject InstantiateTips(string text, Vector2 pos)
    {
        GameObject tmp = Instantiate(tipsPrefab, Camera.main.WorldToScreenPoint(pos), new Quaternion(0, 0, 0, 0), UIContainer);
        tmp.GetComponent<Text>().text = text;
        return tmp;
    }
}