using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Transform UIContainer;

    private static UIManager _instance = null;

    private UIManager()
    {
        _instance = this;
    }

    public static UIManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new UIManager();
        }
        return _instance;
    }

    private void Awake()
    {
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
}