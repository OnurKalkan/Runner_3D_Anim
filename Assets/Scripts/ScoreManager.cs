using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int starCount;
    UIManager uimanager;
    GameManager gamemanager;
    float timeSpeed = 0.01f;
    public int levelScore, scoreIncreaseAmount = 1, levelCoin;

    private void Awake()
    {
        uimanager = GetComponent<UIManager>();
        gamemanager = GetComponent<GameManager>();
    }

    private void Start()
    {
        starCount = 0;
    }

    public void UpdateStarScore()
    {
        starCount++;
        uimanager.starCountText.text = starCount.ToString();
    }

    public IEnumerator DoubleXP()
    {
        yield return new WaitForSeconds(0.0f);
        uimanager.starIcon.color = Color.white;
        scoreIncreaseAmount++;
        uimanager.starCountText.text = "x" + scoreIncreaseAmount.ToString();
        //uimanager.starIcon.fillAmount = 1;
        //for (float i = 1; i >= 0; i -= 0.01f)
        //{
        //    yield return new WaitForSeconds(0.06f);
        //    uimanager.starIcon.fillAmount = i;
        //}
        //scoreIncreaseAmount = 1;
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
