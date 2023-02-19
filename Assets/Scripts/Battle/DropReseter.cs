using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DropReseter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameController.Instance.player)
        {
            ScoreManager.Instance.LoseScore(ScoreManager.Instance.DropScore);
            GameController.Instance.player.GetComponent<ReplayableInput>().InputEnabled = false;
            DOVirtual.DelayedCall(0.5f, () => { GameController.Instance.RestartLevel(); });
        }
    }
}
