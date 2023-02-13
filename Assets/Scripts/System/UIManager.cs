using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public Transform UIContainer;

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
}