using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    Move move;
    UIManager uimanager;
    ScoreManager scoremanager;
    float uiOpeningTime = 1.5f;
    public Transform camLastPos, camLastPos2;
    GameManager gamemanager;
    public bool isShieldActive, isMagnetActive, onGround;
    public GameObject shield, leftShoe, rightShoe, hatsParent;
    int shieldCounter = 0;
    public AudioSource coinSound, magnetSound, starSound, wallHitSound, shieldSound, jumpSound;

    private void Awake()
    {
        move = GameObject.FindGameObjectWithTag("PlayerParent").GetComponent<Move>();
        uimanager = GameObject.Find("GameManager").GetComponent<UIManager>();
        scoremanager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        DOTween.SetTweensCapacity(500, 2000);
        if (PlayerPrefs.GetString("HatName") == "Cowboy")
            hatsParent.transform.Find("Cowboy").gameObject.SetActive(true);
        if (PlayerPrefs.GetInt("HatNo", 2) == 1)
            hatsParent.transform.GetChild(0).gameObject.SetActive(true);
        else if (PlayerPrefs.GetInt("HatNo", 2) == 2)
            hatsParent.transform.GetChild(1).gameObject.SetActive(true);
        else if (PlayerPrefs.GetInt("HatNo", 2) == 3)
            hatsParent.transform.GetChild(2).gameObject.SetActive(true);
    }

    IEnumerator MagnetActive()
    {
        uimanager.magnetIcon.fillAmount = 1;
        for (float i = 1; i >= 0; i -= 0.01f)
        {
            yield return new WaitForSeconds(0.06f);
            uimanager.magnetIcon.fillAmount = i;
        }
        isMagnetActive = false;
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
            if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Star)
                scoremanager.UpdateStarScore();
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.DoubleXP)
            {
                starSound.Play();
                StartCoroutine(scoremanager.DoubleXP());
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Coin)
            {             
                other.transform.DOKill();             
                coinSound.Play();
                scoremanager.levelCoin++;
                uimanager.coinCountText.text = scoremanager.levelCoin.ToString();
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Magnet)
            {
                magnetSound.Play();
                isMagnetActive = true;
                StartCoroutine(MagnetActive());
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Sneaker)
            {
                shieldSound.Play();
                leftShoe.SetActive(true);
                rightShoe.SetActive(true);
                //isMagnetActive = true;
                //StartCoroutine(MagnetActive());
                move.jumpHeight = 4f;
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Shield)
            {
                shieldSound.Play();
                isShieldActive = true;
                shieldCounter++;
                uimanager.shieldCountText.text = shieldCounter.ToString();                
                shield.SetActive(true);
                uimanager.shiledIcon.color = Color.white;
                uimanager.shieldCountText.color = Color.white;
            }
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isShieldActive)
        {
            wallHitSound.Play();
            gamemanager.levelFinished = true;
            move.AnimPlay("Die");
            move.speed = 0;
            Invoke(nameof(FailCondition), uiOpeningTime);
            try
            {
                collision.gameObject.transform.Find("WallCrack").gameObject.SetActive(true);
            }
            catch
            {
                print("no child object");
            }            
        }
        else if(collision.gameObject.CompareTag("Obstacle") && isShieldActive)
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;         
            wallHitSound.Play();                       
            shieldCounter--;
            uimanager.shieldCountText.text = shieldCounter.ToString();
            if(shieldCounter == 0)
            {
                uimanager.shiledIcon.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                uimanager.shieldCountText.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                shield.SetActive(false);
                isShieldActive = false;
            }            
                GameObject expEffect = collision.transform.Find("ExplosionEffect").gameObject;
                expEffect.GetComponent<ParticleSystem>().Play();
                expEffect.transform.parent = null;
                StartCoroutine(ObjectDestroyer(expEffect, 1));
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Surface") && !gamemanager.levelFinished)
        {
            transform.DOKill();
            move.AnimPlay("Run");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            onGround = false;
        }
    }

    IEnumerator ObjectDestroyer(GameObject destroyObj, float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(destroyObj);
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
