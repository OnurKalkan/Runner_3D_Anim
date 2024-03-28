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

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.gameObject.CompareTag("Obstacle"))
    //    {
    //        print("die collision");
    //        AnimPlay("Die");
    //        speed = 0;
    //    }
    //}

    void PlayerOriginalCollider()
    {
        tinyHeroBody.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.75f, 0);
        tinyHeroBody.GetComponent<CapsuleCollider>().height = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.levelFinished)
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
            //GetComponent<CharacterController>().Move(new Vector3(0, 0, 1) * Time.deltaTime * speed);

            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    AnimPlay("Run");
            //    speed = 10;
            //}
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerAnim.SetTrigger("Jump");
                //transform.DOJump(new Vector3(transform.position.x,2,transform.position.z), 1, 1, 0.5f);
                transform.DOMoveY(3, 0.5f);
                transform.DOMoveY(0.5f, 1.0f).SetDelay(0.5f);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                //playerAnim.SetTrigger("Jump");
                //transform.DOJump(new Vector3(transform.position.x,2,transform.position.z), 1, 1, 0.5f);
                tinyHeroBody.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.5f, 0);
                tinyHeroBody.GetComponent<CapsuleCollider>().height = 1;
                tinyHeroBody.transform.DOLocalRotate(new Vector3(-75, 0, 0), 0.75f);
                tinyHeroBody.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.75f).SetDelay(0.75f).OnComplete(PlayerOriginalCollider);
            }
            if (Input.GetKeyDown(KeyCode.A) && onLeft == false && mid == true)
            {
                onLeft = true;
                mid = false;
                transform.DOMoveX(leftBorder, transSpeed);
                playerAnim.SetTrigger("MoveLeft");
                //transform.position = new Vector3(-2, height, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.A) && mid == false && onRight == true)
            {
                onRight = false;
                mid = true;
                transform.DOMoveX(0, transSpeed);
                playerAnim.SetTrigger("MoveLeft");
                //transform.position = new Vector3(0, height, transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.D) && onRight == false && mid == true)
            {
                onRight = true;
                mid = false;
                transform.DOMoveX(rightBorder, transSpeed);
                playerAnim.SetTrigger("MoveRight");
                //transform.position = new Vector3(2, height, transform.position.z);
            }
            else if (Input.GetKeyDown(KeyCode.D) && onLeft == true && mid == false)
            {
                onLeft = false;
                mid = true;
                transform.DOMoveX(0, transSpeed);
                playerAnim.SetTrigger("MoveRight");
                //transform.position = new Vector3(0, height, transform.position.z);
            }
        }        
    }    
}
