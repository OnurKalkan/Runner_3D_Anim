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
    public bool isShieldActive, isMagnetActive;
    public GameObject shield;
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
            if (other.GetComponent<Collectible>().cType != Collectible.CollectibleType.Coin)
                Destroy(other.gameObject);
            if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Star)
                scoremanager.UpdateStarScore();
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.DoubleXP)
            {
                starSound.Play();
                StartCoroutine(scoremanager.DoubleXP());
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Coin)
            {              
                if(isMagnetActive == false && other.GetComponent<Collectible>().magneting == false)
                {
                    Destroy(other.gameObject);
                }
                coinSound.Play();
                scoremanager.levelCoin++;
                uimanager.coinCountText.text = scoremanager.levelCoin.ToString();
            }
            else if (other.GetComponent<Collectible>().cType == Collectible.CollectibleType.Magnet)
            {
                isMagnetActive = true;
                StartCoroutine(MagnetActive());
                //StartCoroutine(scoremanager.DoubleXP());
            }
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
            wallHitSound.Play();
            gamemanager.levelFinished = true;
            move.AnimPlay("Die");
            move.speed = 0;
            Invoke(nameof(FailCondition), uiOpeningTime);
            collision.gameObject.transform.Find("WallCrack").gameObject.SetActive(true);
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
            wallHitSound.Play();
            //Destroy(collision.gameObject);            
            shieldCounter--;
            uimanager.shieldCountText.text = shieldCounter.ToString();
            if(shieldCounter == 0)
            {
                uimanager.shiledIcon.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                uimanager.shieldCountText.DOColor(new Color(1, 1, 1, 0.3f), 0.25f);
                shield.SetActive(false);
                isShieldActive = false;
            }
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            //collision.transform.Find("ExplosionEffect").gameObject.SetActive(true);
            collision.transform.Find("ExplosionEffect").GetComponent<ParticleSystem>().Play();
            //collision.transform.Find("ExplosionEffect").GetComponent<ParticleSystem>().Stop();
            //collision.transform.Find("Explosion").gameObject.SetActive(true);
            //GameObject parentoftheCubes = collision.transform.Find("Explosion").gameObject;
            //for (int i = 0; i < parentoftheCubes.transform.childCount; i++)
            //{
            //    parentoftheCubes.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(new Vector3(1,20,50));
            //}
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
