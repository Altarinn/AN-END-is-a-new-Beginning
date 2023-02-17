using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public Transform UIContainer;
    public GameObject tipsPrefab;

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

    public GameObject InstantiateTips(string text, Vector2 pos)
    {
        GameObject tmp = Instantiate(tipsPrefab, Camera.main.WorldToScreenPoint(pos), new Quaternion(0, 0, 0, 0), UIContainer);
        tmp.GetComponent<Text>().text = text;
        return tmp;
    }
}