using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    public static ScoreManager GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    [HideInInspector]
    public int score;
    public TMPro.TextMeshProUGUI scoreText;
    [Header("Score Paramater")]
    public int initScore;
    public int hurtLostScore = 50;
    public int timeLoseScore = 10;
    public int DropScore = 200;
    public int clearRoomScore = 500;
    public int StrawberryBonus = 200;

    private bool ifActive = false;
    private float refreshFrequence = 1f;
    private float refreshTimer = 0;

    bool timer = false;

    private void Update()
    {
        if (!ifActive) return;
        if (!timer) return;

        refreshTimer -= Time.deltaTime;
        if(refreshTimer <= 0)
        {
            refreshTimer = refreshFrequence;
            score -= timeLoseScore;
            if(scoreText != null)
            {
                SetScoreStr();
            }
        }
    }

    public void SetScoreStr()
    {
        scoreText.text = $"<mspace=0.9em>{score.ToString("D5")}</mspace>";
    }

    public void GetScore(int getScore)
    {
        score += getScore;
        SetScoreStr();
    }

    public void LoseScore(int loseScore)
    {
        score -= loseScore;
        SetScoreStr();
    }

    public void PauseTimer()
    {
        timer = false;
    }

    public void ResumeTimer()
    {
        timer = true;
    }

    public void StartScore()
    {
        if (ifActive) return;

        ifActive = true;
        scoreText.gameObject.SetActive(true);
        refreshTimer = refreshFrequence;
        score = initScore;
        SetScoreStr();
    }

    public void EndScore()
    {
        ifActive = false;
        scoreText.gameObject.SetActive(false);
    }
}
