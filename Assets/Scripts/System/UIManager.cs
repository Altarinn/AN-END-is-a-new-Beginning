using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public Transform UIContainer;
    public GameObject tipsPrefab;

    public GameObject HPBar, HPBarPh;

    public Sprite mute, unmute, shake, unshake;
    public Image muteBtn, shakeBtn;
    public bool muted = false;
    public bool shaked = true;

    public static UIManager GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(UIContainer.gameObject);
    }


    public void OpenMenu(GameObject openMenu)
    {
        openMenu.SetActive(true);
    }

    public void CloseMenu(GameObject closeMenu)
    {
        closeMenu.SetActive(false);
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

    public void ToggleMute()
    {
        muted = !muted;
        if(muted)
        {
            muteBtn.sprite = mute;
        }
        else
        {
            muteBtn.sprite = unmute;
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