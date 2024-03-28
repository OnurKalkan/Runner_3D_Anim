using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int starCount;
    UIManager uimanager;
    GameManager gamemanager;
    float timeSpeed = 0.01f;
    public int levelScore, scoreIncreaseAmount = 1;

    private void Awake()
    {
        uimanager = GetComponent<UIManager>();
        gamemanager = GetComponent<GameManager>();
    }

    private void Start()
    {
        starCount = 0;
        uimanager.starCountText.text = starCount.ToString();
    }

    public void UpdateStarScore()
    {
        starCount++;
        uimanager.starCountText.text = starCount.ToString();
    }

    public IEnumerator DoubleXP()
    {
        scoreIncreaseAmount++;
        yield return new WaitForSeconds(3);
        scoreIncreaseAmount = 1;
    }

    public IEnumerator ScoreUpdate()
    {
        yield return new WaitForSeconds(timeSpeed);
        levelScore += scoreIncreaseAmount;
        uimanager.scoreText.text = levelScore.ToString();
        uimanager.winScoreText.text = "Level Score: " + (levelScore + (starCount * 20)).ToString();
        if (!gamemanager.levelFinished)
            StartCoroutine(ScoreUpdate());
    }
}
