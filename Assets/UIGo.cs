using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGo : MonoBehaviour
{
    public void Go(string sceneName)
    {
        GameController.Instance.ExitStage();
        UIManager.Instance.DestroyMenu(gameObject);
        GameController.Instance.EnterLevelAsync(sceneName);
    }
}
