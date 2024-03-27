using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Move move;
    UIManager uimanager;
    ScoreManager scoremanager;
    float uiOpeningTime = 0.75f;

    private void Awake()
    {
        move = GetComponent<Move>();
        uimanager = GameObject.Find("GameManager").GetComponent<UIManager>();
        scoremanager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            //move.AnimPlay("Celebrate");
            move.speed = 0;
            Invoke(nameof(WinCondition), uiOpeningTime);
        }
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
            scoremanager.UpdateStarScore();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            move.AnimPlay("Die");
            move.speed = 0;
            Invoke(nameof(FailCondition), uiOpeningTime);
        }
    }

    void FailCondition()
    {        
        uimanager.FailPanel();
    }

    void WinCondition()
    {
        uimanager.WinPanel();
    }
}
