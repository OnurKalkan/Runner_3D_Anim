using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    Move move;
    UIManager uimanager;
    ScoreManager scoremanager;
    float uiOpeningTime = 0.75f;
    public Transform camLastPos, camLastPos2;
    GameManager gamemanager;
    bool isShieldActive;
    public GameObject shield;
    int shieldCounter = 0;

    private void Awake()
    {
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        uimanager = GameObject.Find("GameManager").GetComponent<UIManager>();
        scoremanager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            gamemanager.levelFinished = true;
            move.AnimPlay("Victory");
            move.speed = 0;
            Invoke(nameof(WinCondition), uiOpeningTime + 2);
            //
            Camera.main.transform.DOLocalRotate(camLastPos2.localEulerAngles, 1f).SetEase(Ease.Linear);
            Camera.main.transform.DOLocalMove(camLastPos2.localPosition, 1f).SetEase(Ease.Linear);
            //
            Camera.main.transform.DOLocalRotate(camLastPos.localEulerAngles, 1f).SetDelay(1).SetEase(Ease.Linear);
            Camera.main.transform.DOLocalMove(camLastPos.localPosition, 1f).SetDelay(1).SetEase(Ease.Linear);
        }
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Star)
                scoremanager.UpdateStarScore();
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.DoubleXP)
                StartCoroutine(scoremanager.DoubleXP());
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Shield)
            {
                isShieldActive = true;
                shieldCounter++;
                uimanager.shieldCountText.text = shieldCounter.ToString();
                shield.SetActive(true);
                uimanager.shiledIcon.color = Color.white;
                uimanager.shieldCountText.color = Color.white;
            }                
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isShieldActive)
        {
            gamemanager.levelFinished = true;
            move.AnimPlay("Die");
            move.speed = 0;
            Invoke(nameof(FailCondition), uiOpeningTime);
        }
        else if(collision.gameObject.CompareTag("Obstacle") && isShieldActive)
        {
            Destroy(collision.gameObject);            
            shieldCounter--;
            uimanager.shieldCountText.text = shieldCounter.ToString();
            if(shieldCounter == 0)
            {
                uimanager.shiledIcon.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                uimanager.shieldCountText.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                shield.SetActive(false);
                isShieldActive = false;
            }         
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
