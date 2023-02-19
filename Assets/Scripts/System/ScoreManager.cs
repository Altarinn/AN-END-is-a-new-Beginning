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
    public Text scoreText;
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

    private void Update()
    {
        if (!ifActive) return;

        refreshTimer -= Time.deltaTime;
        if(refreshTimer <= 0)
        {
            refreshTimer = refreshFrequence;
            score -= 10;
            if(scoreText != null)
            {
                scoreText.text = "Score:" + score.ToString();
            }
        }
    }

    public void GetScore(int getScore)
    {
        score += getScore;
        scoreText.text = "Score:" + score.ToString();
    }

    public void LoseScore(int loseScore)
    {
        score -= loseScore;
        scoreText.text = "Score:" + score.ToString();
    }

    public void StartScore()
    {
        if (ifActive) return;

        ifActive = true;
        scoreText.gameObject.SetActive(true);
        refreshTimer = refreshFrequence;
        score = initScore;
        scoreText.text = "Score:" + score.ToString();
    }

    public void EndScore()
    {
        ifActive = false;
        scoreText.gameObject.SetActive(false);
    }
}
