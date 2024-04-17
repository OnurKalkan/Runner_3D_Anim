using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    public int speed = 1;
    public float leftBorder = -2.5f, rightBorder = 2.5f;
    float transSpeed = 0.25f;
    public bool onLeft = false, mid = true, onRight = false;
    Animator playerAnim;
    GameObject tinyHeroBody;
    GameManager gameManager;
    public float jumpHeight = 2;

    private void Awake()
    {
        playerAnim = transform.Find("Player").GetComponent<Animator>();
        tinyHeroBody = transform.Find("Player").gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void AnimPlay(string animName)
    {
        playerAnim.SetBool("Run", false);
        playerAnim.SetBool("Idle", false);
        playerAnim.SetBool("Die", false);
        playerAnim.SetBool(animName, true);
    }    

    void PlayerOriginalCollider()
    {
        tinyHeroBody.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.75f, 0);
        tinyHeroBody.GetComponent<CapsuleCollider>().height = 1.5f;
    }

    //IEnumerator Jump()
    //{
    //    tinyHeroBody.transform.DOLocalMoveY(tinyHeroBody.transform.localPosition.y + jumpHeight, 0.5f).SetEase(Ease.OutFlash);
    //    yield return new WaitForSeconds(0.5f);
    //    tinyHeroBody.transform.DOLocalMoveY(tinyHeroBody.transform.localPosition.y - jumpHeight, 0.75f).SetEase(Ease.InFlash);
    //}

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.levelFinished)
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
            if (Input.GetKeyDown(KeyCode.X))
            {
                tinyHeroBody.transform.DOKill();
                tinyHeroBody.transform.DOMoveY(0.5f, 0.2f).SetEase(Ease.InFlash);
            }
            if (Input.GetKeyDown(KeyCode.W) && tinyHeroBody.GetComponent<Player>().onGround)
            {
                playerAnim.SetTrigger("Jump");
                tinyHeroBody.GetComponent<Player>().jumpSound.Play();
                //StartCoroutine(Jump());
                tinyHeroBody.transform.DOLocalMoveY(tinyHeroBody.transform.localPosition.y + jumpHeight, 0.5f).SetEase(Ease.OutFlash);
                tinyHeroBody.transform.DOLocalMoveY(tinyHeroBody.transform.localPosition.y, 0.75f).SetDelay(0.5f).SetEase(Ease.InFlash);
            }
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    tinyHeroBody.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.5f, 0);
            //    tinyHeroBody.GetComponent<CapsuleCollider>().height = 1;
            //    //tinyHeroBody.transform.DOLocalRotate(new Vector3(-75, 0, 0), 0.75f);
            //    tinyHeroBody.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.75f).SetDelay(0.75f).OnComplete(PlayerOriginalCollider);
            //}
            if (Input.GetKeyDown(KeyCode.A) && onLeft == false && mid == true)
            {
                tinyHeroBody.GetComponent<Player>().jumpSound.Play();
                tinyHeroBody.GetComponent<Player>().jumpSound.Play();
                onLeft = true;
                mid = false;
                transform.DOMoveX(leftBorder, transSpeed);
                playerAnim.SetTrigger("MoveLeft");
            }
            else if (Input.GetKeyDown(KeyCode.A) && mid == false && onRight == true)
            {
                tinyHeroBody.GetComponent<Player>().jumpSound.Play();
                onRight = false;
                mid = true;
                transform.DOMoveX(0, transSpeed);
                playerAnim.SetTrigger("MoveLeft");
            }
            if (Input.GetKeyDown(KeyCode.D) && onRight == false && mid == true)
            {
                tinyHeroBody.GetComponent<Player>().jumpSound.Play();
                onRight = true;
                mid = false;
                transform.DOMoveX(rightBorder, transSpeed);
                playerAnim.SetTrigger("MoveRight");
            }
            else if (Input.GetKeyDown(KeyCode.D) && onLeft == true && mid == false)
            {
                tinyHeroBody.GetComponent<Player>().jumpSound.Play();
                onLeft = false;
                mid = true;
                transform.DOMoveX(0, transSpeed);
                playerAnim.SetTrigger("MoveRight");
            }
        }        
    }    
}
