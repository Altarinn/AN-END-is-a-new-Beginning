using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ScoreSet : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI score, berry;
    public int totalBerries = 9;

    private void Awake()
    {
        score.text = $"Score: {ScoreManager.Instance.score}";
        berry.text = $"Berries: {GameController.Instance.nBerries} / {totalBerries}";
    }
}
