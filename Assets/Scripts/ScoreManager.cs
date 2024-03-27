using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int starCount;
    UIManager uimanager;

    private void Awake()
    {
        uimanager = GetComponent<UIManager>();
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
}
